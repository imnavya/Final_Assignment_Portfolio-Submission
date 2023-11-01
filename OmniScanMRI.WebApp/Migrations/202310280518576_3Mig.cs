namespace OmniScanMRI.WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3Mig : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ScanDetails", "DateTaken", c => c.DateTime(nullable: false, storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ScanDetails", "DateTaken", c => c.DateTime(nullable: false));
        }
    }
}
