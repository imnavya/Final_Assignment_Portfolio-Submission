namespace OmniScanMRI.WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _9Mig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ScanDetails", "DoctorsNotes", c => c.String(maxLength: 1000));
            AddColumn("dbo.ScanDetails", "MedicalHistory", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ScanDetails", "MedicalHistory");
            DropColumn("dbo.ScanDetails", "DoctorsNotes");
        }
    }
}
