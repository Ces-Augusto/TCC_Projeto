using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text;
using TCC_Projeto.Models;
using System.Security.Cryptography;
using TCC_Projeto.Models.Usuario;

namespace TCC_Projeto.Controllers
{
    public class HomeController : Controller
    {
        private readonly Conexao _context;

        public HomeController(Conexao context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CadastroUsuario()
        {
            return View();

        }

        public IActionResult Signout()
        {
            Response.Cookies.Delete("UserId");
            Response.Cookies.Delete("Username");

            return Redirect("~/");
        }

        public async Task<IActionResult> Cadastrar(CadastroViewModel request)
        {
            if (ModelState.IsValid)
            {
                using (var memoryStream = new MemoryStream())
                {
                    if (request.Password == request.Password2)
                    {
                        string senhaCriptografada = CriptografarSenha(request.Password);

                        var CadastroModel = new Usuarios
                        {
                            Username = request.Username,
                            Password = senhaCriptografada,
                            Nome = request.Nome,
                            Sobrenome = request.Sobrenome,
                            Email = request.Email,
                            DataCriacao = DateTime.Now,
                            Ativo = true
                        };

                        _context.Usuarios.Add(CadastroModel);
                        await _context.SaveChangesAsync();

                        return Redirect("~/");
                    }
                    else
                    {
                        return View();
                    }
                }
            }
            else
            {
                return View();
            }
        }

        public IActionResult Entrar(LoginViewModel request)
        {
            if (HttpContext.Request.Cookies.ContainsKey("UserId"))
            {
                return View();
            }
            if (ModelState.IsValid)
            {
                // Verificar se o usuário e a senha correspondem a um registro no banco de dados
                var usuario = _context.Usuarios.FirstOrDefault(u => u.Username == request.Username);

                if (usuario != null && VerificarSenha(request.Password, usuario.Password))
                {
                    HttpContext.Response.Cookies.Append("UserId", usuario.Id.ToString());
                    HttpContext.Response.Cookies.Append("Username", usuario.Username);
                    return View();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Credenciais inválidas. Por favor, tente novamente.");
                    TempData["ErrorMessage"] = "Credenciais inválidas. Por favor, tente novamente.";
                }
            }
            return Redirect("~/");
        }

        public IActionResult CriarImpressao()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Função para criptografar a senha usando SHA-256
        private static string CriptografarSenha(string senha)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(senha));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static bool VerificarSenha(string senhaDigitada, string senhaCriptografada)
        {
            string senhaDigitadaCriptografada = CriptografarSenha(senhaDigitada);
            return senhaDigitadaCriptografada == senhaCriptografada;
        }
    }
}
