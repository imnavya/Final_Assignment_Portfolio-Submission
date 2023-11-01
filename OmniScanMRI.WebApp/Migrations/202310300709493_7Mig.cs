namespace OmniScanMRI.WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _7Mig : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AppointmentsAvailabilities",
                c => new
                    {
                        TimeSlotID = c.String(nullable: false, maxLength: 128),
                        DoctorID = c.String(nullable: false, maxLength: 128),
                        AvailableDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TimeSlotID)
                .ForeignKey("dbo.Doctors", t => t.DoctorID, cascadeDelete: true)
                .Index(t => t.DoctorID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AppointmentsAvailabilities", "DoctorID", "dbo.Doctors");
            DropIndex("dbo.AppointmentsAvailabilities", new[] { "DoctorID" });
            DropTable("dbo.AppointmentsAvailabilities");
        }
    }
}
