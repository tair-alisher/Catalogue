namespace Catalogue.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Administrations",
                c => new
                    {
                        AdministrationId = c.Int(nullable: false, identity: true),
                        AdministrationName = c.String(),
                        AdministrationPost = c.Int(nullable: false),
                        AdministrationAddress = c.String(),
                        AdministrationFax = c.String(),
                        AdministrationEMail = c.String(),
                        AdministrationSkype = c.String(),
                        AdministrationCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AdministrationId);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentId = c.Int(nullable: false, identity: true),
                        DepartmentName = c.String(),
                        AdministrationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DepartmentId)
                .ForeignKey("dbo.Administrations", t => t.AdministrationId, cascadeDelete: true)
                .Index(t => t.AdministrationId);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        EmployeeId = c.Int(nullable: false, identity: true),
                        EmployeeFullName = c.String(),
                        EployeeRoom = c.String(),
                        EmployeeAddress = c.String(),
                        EmployeePhone = c.String(),
                        EmployeePersonalPhone = c.String(),
                        EmployeeEmail = c.String(),
                        EmployeeSkype = c.String(),
                        PositionId = c.Int(nullable: false),
                        DepartmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EmployeeId)
                .ForeignKey("dbo.Positions", t => t.PositionId, cascadeDelete: true)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: true)
                .Index(t => t.PositionId)
                .Index(t => t.DepartmentId);
            
            CreateTable(
                "dbo.Positions",
                c => new
                    {
                        PositionId = c.Int(nullable: false, identity: true),
                        PositionName = c.String(),
                    })
                .PrimaryKey(t => t.PositionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Departments", "AdministrationId", "dbo.Administrations");
            DropForeignKey("dbo.Employees", "DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.Employees", "PositionId", "dbo.Positions");
            DropIndex("dbo.Employees", new[] { "DepartmentId" });
            DropIndex("dbo.Employees", new[] { "PositionId" });
            DropIndex("dbo.Departments", new[] { "AdministrationId" });
            DropTable("dbo.Positions");
            DropTable("dbo.Employees");
            DropTable("dbo.Departments");
            DropTable("dbo.Administrations");
        }
    }
}
