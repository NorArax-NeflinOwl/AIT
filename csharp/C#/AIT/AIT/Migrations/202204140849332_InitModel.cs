namespace AIT.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AitPersons",
                c => new
                    {
                        prnID = c.Int(nullable: false, identity: true),
                        prnLogin = c.String(nullable: false),
                        prnEmail = c.String(),
                        prnPassword = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.prnID);
            
            CreateTable(
                "dbo.AitPersonsDetail",
                c => new
                    {
                        prdPersonID = c.Int(nullable: false),
                        prdFName = c.String(),
                        prdLName = c.String(),
                        prdBDate = c.DateTime(nullable: false),
                        prdPesel = c.String(maxLength: 11),
                    })
                .PrimaryKey(t => t.prdPersonID)
                .ForeignKey("dbo.AitPersons", t => t.prdPersonID)
                .Index(t => t.prdPersonID);
            
            CreateTable(
                "dbo.AitQuickNote",
                c => new
                    {
                        qknID = c.Int(nullable: false, identity: true),
                        qknPersonID = c.Int(nullable: false),
                        qknTitle = c.String(nullable: false),
                        qknNote = c.String(nullable: false),
                        qknCDate = c.DateTime(nullable: false),
                        qknPassword = c.String(),
                    })
                .PrimaryKey(t => new { t.qknID, t.qknPersonID })
                .ForeignKey("dbo.AitPersons", t => t.qknPersonID, cascadeDelete: true)
                .Index(t => t.qknPersonID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AitQuickNote", "qknPersonID", "dbo.AitPersons");
            DropForeignKey("dbo.AitPersonsDetail", "prdPersonID", "dbo.AitPersons");
            DropIndex("dbo.AitQuickNote", new[] { "qknPersonID" });
            DropIndex("dbo.AitPersonsDetail", new[] { "prdPersonID" });
            DropTable("dbo.AitQuickNote");
            DropTable("dbo.AitPersonsDetail");
            DropTable("dbo.AitPersons");
        }
    }
}
