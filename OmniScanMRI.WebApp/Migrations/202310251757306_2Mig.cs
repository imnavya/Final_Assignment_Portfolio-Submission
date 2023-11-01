namespace OmniScanMRI.WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2Mig : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Patients", "DateOfBirth", c => c.DateTime(nullable: false, storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Patients", "DateOfBirth", c => c.DateTime(nullable: false));
        }
    }
}
