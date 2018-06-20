namespace Catalogue.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class accept_updates : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Divisions",
                c => new
                    {
                        DivisionId = c.Int(nullable: false, identity: true),
                        DivisionName = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.DivisionId);
            
            AddColumn("dbo.Administrations", "DivisionId", c => c.Int(nullable: false));
            AddColumn("dbo.Employees", "EmployeeRoom", c => c.String(nullable: false, maxLength: 10));
            AddColumn("dbo.Employees", "EmployeePhoto", c => c.String());
            AddColumn("dbo.Employees", "DateAdoption", c => c.DateTime(nullable: false));
            AddColumn("dbo.Employees", "DateDismissal", c => c.DateTime());
            AlterColumn("dbo.Administrations", "AdministrationName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Administrations", "AdministrationAddress", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Administrations", "AdministrationFax", c => c.String(maxLength: 12));
            AlterColumn("dbo.Administrations", "AdministrationEMail", c => c.String(maxLength: 50));
            AlterColumn("dbo.Administrations", "AdministrationSkype", c => c.String(maxLength: 20));
            AlterColumn("dbo.Departments", "DepartmentName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Employees", "EmployeeFullName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Employees", "EmployeeAddress", c => c.String(maxLength: 100));
            AlterColumn("dbo.Employees", "EmployeePhone", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Employees", "EmployeePersonalPhone", c => c.String(maxLength: 100));
            AlterColumn("dbo.Employees", "EmployeeEmail", c => c.String(maxLength: 50));
            AlterColumn("dbo.Employees", "EmployeeSkype", c => c.String(maxLength: 30));
            AlterColumn("dbo.Positions", "PositionName", c => c.String(nullable: false, maxLength: 100));
            CreateIndex("dbo.Administrations", "DivisionId");
            AddForeignKey("dbo.Administrations", "DivisionId", "dbo.Divisions", "DivisionId", cascadeDelete: true);
            DropColumn("dbo.Employees", "EployeeRoom");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Employees", "EployeeRoom", c => c.String());
            DropForeignKey("dbo.Administrations", "DivisionId", "dbo.Divisions");
            DropIndex("dbo.Administrations", new[] { "DivisionId" });
            AlterColumn("dbo.Positions", "PositionName", c => c.String());
            AlterColumn("dbo.Employees", "EmployeeSkype", c => c.String());
            AlterColumn("dbo.Employees", "EmployeeEmail", c => c.String());
            AlterColumn("dbo.Employees", "EmployeePersonalPhone", c => c.String());
            AlterColumn("dbo.Employees", "EmployeePhone", c => c.String());
            AlterColumn("dbo.Employees", "EmployeeAddress", c => c.String());
            AlterColumn("dbo.Employees", "EmployeeFullName", c => c.String());
            AlterColumn("dbo.Departments", "DepartmentName", c => c.String());
            AlterColumn("dbo.Administrations", "AdministrationSkype", c => c.String());
            AlterColumn("dbo.Administrations", "AdministrationEMail", c => c.String());
            AlterColumn("dbo.Administrations", "AdministrationFax", c => c.String());
            AlterColumn("dbo.Administrations", "AdministrationAddress", c => c.String());
            AlterColumn("dbo.Administrations", "AdministrationName", c => c.String());
            DropColumn("dbo.Employees", "DateDismissal");
            DropColumn("dbo.Employees", "DateAdoption");
            DropColumn("dbo.Employees", "EmployeePhoto");
            DropColumn("dbo.Employees", "EmployeeRoom");
            DropColumn("dbo.Administrations", "DivisionId");
            DropTable("dbo.Divisions");
        }
    }
}
