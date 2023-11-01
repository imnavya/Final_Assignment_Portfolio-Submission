namespace OmniScanMRI.WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _10Mig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Appointments", "Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Appointments", "Email");
        }
    }
}
