namespace OmniScanMRI.WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _8Mig : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Appointments", "AppointmentDttm", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Appointments", "AppointmentDttm", c => c.DateTime(nullable: false));
        }
    }
}
