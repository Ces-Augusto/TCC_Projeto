using System.Data.Entity;
using TCC_Projeto.Models.Etiqueta;
using TCC_Projeto.Models.Usuario;
namespace TCC_Projeto.Models

{
    public class Conexao : DbContext
    {
        private const string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TCC_db;"; //dados para conectar no Banco
        public Conexao() : base(ConnectionString) 
        {
            Usuarios = Set<Usuarios>();
            Etiquetas = Set<Etiquetas>();
            PDF = Set<PDF>();
        }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Etiquetas> Etiquetas { get; set; }
        public DbSet<PDF> PDF { get; set; }
    }
}
