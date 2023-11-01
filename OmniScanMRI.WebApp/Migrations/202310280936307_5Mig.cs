namespace OmniScanMRI.WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _5Mig : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        RatingID = c.String(nullable: false, maxLength: 128),
                        RatedByPatientID = c.String(nullable: false, maxLength: 128),
                        RatedDoctorID = c.String(nullable: false, maxLength: 128),
                        RatingValue = c.Int(nullable: false),
                        Comments = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.RatingID)
                .ForeignKey("dbo.Patients", t => t.RatedByPatientID, cascadeDelete: true)
                .ForeignKey("dbo.Doctors", t => t.RatedDoctorID, cascadeDelete: true)
                .Index(t => t.RatedByPatientID)
                .Index(t => t.RatedDoctorID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Ratings", "RatedDoctorID", "dbo.Doctors");
            DropForeignKey("dbo.Ratings", "RatedByPatientID", "dbo.Patients");
            DropIndex("dbo.Ratings", new[] { "RatedDoctorID" });
            DropIndex("dbo.Ratings", new[] { "RatedByPatientID" });
            DropTable("dbo.Ratings");
        }
    }
}
