namespace OmniScanMRI.WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1Mig : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Administrators",
                c => new
                    {
                        AdminID = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        ContactNumber = c.String(),
                        Email = c.String(),
                        Role = c.String(),
                    })
                .PrimaryKey(t => t.AdminID)
                .ForeignKey("dbo.AspNetUsers", t => t.AdminID)
                .Index(t => t.AdminID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        AppointmentID = c.String(nullable: false, maxLength: 128),
                        PatientID = c.String(nullable: false, maxLength: 128),
                        DoctorID = c.String(nullable: false, maxLength: 128),
                        AdminID = c.String(maxLength: 128),
                        AppointmentDttm = c.DateTime(nullable: false),
                        Notes = c.String(maxLength: 500),
                        Status = c.String(maxLength: 50),
                        Doctors_DoctorID = c.String(maxLength: 128),
                        Patient_PatientID = c.String(maxLength: 128),
                        Administrators_AdminID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.AppointmentID)
                .ForeignKey("dbo.Administrators", t => t.AdminID)
                .ForeignKey("dbo.Doctors", t => t.Doctors_DoctorID)
                .ForeignKey("dbo.Doctors", t => t.DoctorID)
                .ForeignKey("dbo.Patients", t => t.Patient_PatientID)
                .ForeignKey("dbo.Patients", t => t.PatientID)
                .ForeignKey("dbo.Administrators", t => t.Administrators_AdminID)
                .Index(t => t.PatientID)
                .Index(t => t.DoctorID)
                .Index(t => t.AdminID)
                .Index(t => t.Doctors_DoctorID)
                .Index(t => t.Patient_PatientID)
                .Index(t => t.Administrators_AdminID);
            
            CreateTable(
                "dbo.Doctors",
                c => new
                    {
                        DoctorID = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Specialization = c.String(),
                        LicenseNumber = c.String(),
                        ContactNumber = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.DoctorID)
                .ForeignKey("dbo.AspNetUsers", t => t.DoctorID)
                .Index(t => t.DoctorID);
            
            CreateTable(
                "dbo.Patients",
                c => new
                    {
                        PatientID = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        DateOfBirth = c.DateTime(nullable: false),
                        Gender = c.String(),
                        Address = c.String(),
                        ContactNumber = c.String(),
                        Email = c.String(),
                        MedicalHistory = c.String(),
                    })
                .PrimaryKey(t => t.PatientID)
                .ForeignKey("dbo.AspNetUsers", t => t.PatientID)
                .Index(t => t.PatientID);
            
            CreateTable(
                "dbo.ScanDetails",
                c => new
                    {
                        ScanID = c.String(nullable: false, maxLength: 128),
                        FilePath = c.String(nullable: false, maxLength: 255),
                        FileName = c.String(nullable: false, maxLength: 100),
                        DateTaken = c.DateTime(nullable: false),
                        UploadBy_UserId = c.String(nullable: false, maxLength: 128),
                        Patient_PatientID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ScanID)
                .ForeignKey("dbo.AspNetUsers", t => t.UploadBy_UserId, cascadeDelete: true)
                .ForeignKey("dbo.Patients", t => t.Patient_PatientID)
                .Index(t => t.UploadBy_UserId)
                .Index(t => t.Patient_PatientID);
            
            CreateTable(
                "dbo.TrackEmails",
                c => new
                    {
                        TrackID = c.String(nullable: false, maxLength: 128),
                        ToEmail = c.String(nullable: false),
                        Subject = c.String(),
                        Content = c.String(),
                        AdminID = c.String(nullable: false, maxLength: 128),
                        SentDate = c.DateTime(nullable: false),
                        CCEmail = c.String(),
                    })
                .PrimaryKey(t => t.TrackID)
                .ForeignKey("dbo.Administrators", t => t.AdminID, cascadeDelete: true)
                .Index(t => t.AdminID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.TrackEmails", "AdminID", "dbo.Administrators");
            DropForeignKey("dbo.Appointments", "Administrators_AdminID", "dbo.Administrators");
            DropForeignKey("dbo.Appointments", "PatientID", "dbo.Patients");
            DropForeignKey("dbo.ScanDetails", "Patient_PatientID", "dbo.Patients");
            DropForeignKey("dbo.ScanDetails", "UploadBy_UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Appointments", "Patient_PatientID", "dbo.Patients");
            DropForeignKey("dbo.Patients", "PatientID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Appointments", "DoctorID", "dbo.Doctors");
            DropForeignKey("dbo.Appointments", "Doctors_DoctorID", "dbo.Doctors");
            DropForeignKey("dbo.Doctors", "DoctorID", "dbo.AspNetUsers");
            DropForeignKey("dbo.Appointments", "AdminID", "dbo.Administrators");
            DropForeignKey("dbo.Administrators", "AdminID", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.TrackEmails", new[] { "AdminID" });
            DropIndex("dbo.ScanDetails", new[] { "Patient_PatientID" });
            DropIndex("dbo.ScanDetails", new[] { "UploadBy_UserId" });
            DropIndex("dbo.Patients", new[] { "PatientID" });
            DropIndex("dbo.Doctors", new[] { "DoctorID" });
            DropIndex("dbo.Appointments", new[] { "Administrators_AdminID" });
            DropIndex("dbo.Appointments", new[] { "Patient_PatientID" });
            DropIndex("dbo.Appointments", new[] { "Doctors_DoctorID" });
            DropIndex("dbo.Appointments", new[] { "AdminID" });
            DropIndex("dbo.Appointments", new[] { "DoctorID" });
            DropIndex("dbo.Appointments", new[] { "PatientID" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Administrators", new[] { "AdminID" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.TrackEmails");
            DropTable("dbo.ScanDetails");
            DropTable("dbo.Patients");
            DropTable("dbo.Doctors");
            DropTable("dbo.Appointments");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Administrators");
        }
    }
}
