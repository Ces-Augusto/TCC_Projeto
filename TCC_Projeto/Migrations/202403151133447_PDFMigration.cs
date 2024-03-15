namespace TCC_Projeto.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class PDFMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PDFs",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    NPedido = c.String(maxLength: 50),
                    Data = c.DateTime(nullable: false),
                    CaminhoPDF = c.String(maxLength: 255),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("dbo.PDFs");
        }
    }
}
