namespace OmniScanMRI.WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _6Mig : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Ratings", "RatedDoctorID", "dbo.Doctors");
            DropIndex("dbo.Ratings", new[] { "RatedDoctorID" });
            AddColumn("dbo.Ratings", "RatedAppointmentID", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.Ratings", "RatedAppointmentID");
            AddForeignKey("dbo.Ratings", "RatedAppointmentID", "dbo.Appointments", "AppointmentID", cascadeDelete: true);
            DropColumn("dbo.Ratings", "RatedDoctorID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ratings", "RatedDoctorID", c => c.String(nullable: false, maxLength: 128));
            DropForeignKey("dbo.Ratings", "RatedAppointmentID", "dbo.Appointments");
            DropIndex("dbo.Ratings", new[] { "RatedAppointmentID" });
            DropColumn("dbo.Ratings", "RatedAppointmentID");
            CreateIndex("dbo.Ratings", "RatedDoctorID");
            AddForeignKey("dbo.Ratings", "RatedDoctorID", "dbo.Doctors", "DoctorID", cascadeDelete: true);
        }
    }
}
