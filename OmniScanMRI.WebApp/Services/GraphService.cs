using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using System.Text.RegularExpressions;
using System.Net.Http.Headers;
using Microsoft.Kiota.Abstractions.Authentication;
using System.Net.Http;
using System.Threading;
using Microsoft.Graph.Models;
using Microsoft.Kiota.Abstractions;
using Microsoft.Graph;


namespace OmniScanMRI.WebApp.Services
{
    public class GraphService
    {
        private IConfidentialClientApplication clientApp;
        private Microsoft.Graph.GraphServiceClient graphClient;

        public GraphService()
        {
            clientApp = ConfidentialClientApplicationBuilder.Create("0f671ebd-715c-4e8d-80d5-47d02b657025")
                .WithClientSecret("plO8Q~31xZ7qcHj.BfRVwrqBMTcXKH6HwckQvcJp")
                .WithAuthority(new Uri("https://login.microsoftonline.com/4777f9d3-9153-4a66-955c-4374eb645adf"))
                .Build();

            graphClient = new GraphServiceClient(new CustomAuthenticationProvider(this.GetToken));
        }

        public async Task RegisterUser(string userEmail, string userPassword, string role)
        {
            string username = userEmail.Split('@')[0];
            var user = new User
            {
                AccountEnabled = true,
                DisplayName = username, 
                MailNickname = username,
                UserPrincipalName = userEmail,
                PasswordProfile = new PasswordProfile
                {
                    ForceChangePasswordNextSignIn = true,
                    Password = userPassword
                }
            };

            // Create the user
            var newUser = await graphClient.Users.PostAsync(user);

            var groupId = await GetGroupIdForRoleName(role);
            
            // Assign the user to the correct group
            var requestBody = new Microsoft.Graph.Models.ReferenceCreate
            {
                OdataId = $"https://graph.microsoft.com/v1.0/directoryObjects/{newUser.Id}",
            };
            await graphClient.Groups[groupId].Members.Ref.PostAsync(requestBody);

            // Assign the actual app role to the user in the enterprise application configuration
            await AssignRoleToUser(newUser.Id, role);
            
        }
        private async Task AssignRoleToUser(string userId, string roleName)
        {
            var appId = "0f671ebd-715c-4e8d-80d5-47d02b657025"; 

            var result = await graphClient.ServicePrincipals.GetAsync((requestConfiguration) =>
            {
                requestConfiguration.QueryParameters.Filter = $"appId eq '{appId}'";
                requestConfiguration.QueryParameters.Top = 1;
            });

            var servicePrincipal = result.Value.FirstOrDefault();  // Retrieves the service principal based on appId.

            // Get the count, set Top to 1, the count will be either 0 or 1 based on if there's a match.
            int count = servicePrincipal != null ? 1 : 0;

            if (servicePrincipal == null)
            {
                throw new Exception("Service Principal not found for the given AppId");
            }


            // Find the role definition
            var appRole = servicePrincipal.AppRoles.FirstOrDefault(r => string.Equals(r.Value, roleName, StringComparison.OrdinalIgnoreCase));

            if (appRole == null)
            {
                throw new Exception($"Role {roleName} not found in the enterprise application");
            }

            // Create a role assignment
            var appRoleAssignment = new AppRoleAssignment
            {
                PrincipalId = Guid.Parse(userId),
                ResourceId = Guid.Parse(servicePrincipal.Id),
                AppRoleId = appRole.Id
            };

            await graphClient.Users[userId].AppRoleAssignments.PostAsync(appRoleAssignment);
        }

        private string SanitizeRoleName(string roleName)
        {
            return Regex.Replace(roleName, @"[^\w\s]", string.Empty).Trim();
        }

        private async Task<string> GetGroupIdForRoleName(string roleName)
        {
            var sanitizedRoleName = SanitizeRoleName(roleName);
            var groupPage = await graphClient.Groups.GetAsync((requestConfiguration) =>
            {
                requestConfiguration.QueryParameters.Filter = $"displayName eq '{sanitizedRoleName}'";
            });

            var group = groupPage.Value.FirstOrDefault();

            if (group != null)
            {
                return group.Id;
            }

            return null;
        }

        private async Task<string> GetToken()
        {
            var scopes = new string[] { "https://graph.microsoft.com/.default" };
            AuthenticationResult authResult = await clientApp.AcquireTokenForClient(scopes).ExecuteAsync();
            return authResult.AccessToken;
        }
    }

    public class CustomAuthenticationProvider : IAuthenticationProvider
    {
        private Func<Task<string>> _getToken;

        public CustomAuthenticationProvider(Func<Task<string>> getToken)
        {
            _getToken = getToken;
        }

        public async Task AuthenticateRequestAsync(HttpRequestMessage request)
        {
            string token = await _getToken();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task AuthenticateRequestAsync(RequestInformation requestInfo, Dictionary<string, object> authData, CancellationToken cancellationToken)
        {
            string token = await _getToken();
            requestInfo.Headers.Add("Authorization", $"Bearer {token}");
        }
    }
}
