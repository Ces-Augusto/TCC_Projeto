namespace TCC_Projeto.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class EtiquetaMigrate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Etiquetas",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Nome = c.String(nullable: false, maxLength: 50),
                    Descricao = c.String(nullable: false, maxLength: 150),
                    Imagem = c.Binary(nullable: false),
                })
                .PrimaryKey(t => t.Id);
        }

        public override void Down()
        {
            DropTable("dbo.Etiquetas");
        }
    }
}
