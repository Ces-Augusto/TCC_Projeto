namespace TCC_Projeto.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UsuariosModel",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Nome = c.String(maxLength: 50),
                    Sobrenome = c.String(maxLength: 50),
                    Username = c.String(maxLength: 50),
                    Password = c.String(maxLength: 64),
                    Email = c.String(maxLength: 50),
                    DataCriacao = c.DateTime(nullable: false),
                    Ativo = c.Boolean(nullable: false),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("dbo.UsuariosModel");
        }
    }
}
