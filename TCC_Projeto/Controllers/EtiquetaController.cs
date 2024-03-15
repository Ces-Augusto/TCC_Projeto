using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using TCC_Projeto.Models;
using TCC_Projeto.Models.Etiqueta;
using CsvHelper;
using System.Data.Entity;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Image;


namespace TCC_Projeto.Controllers
{
    public class EtiquetaController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly Conexao _context;

        public EtiquetaController(IWebHostEnvironment hostingEnvironment, Conexao context)
        {
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }

        public IActionResult Etiqueta()
        {
            return View();
        }

        //Método para lidar com o envio do formulário via AJAX
        [HttpPost]
        public async Task<IActionResult> EnviarEtiqueta(EtiquetaViewModel etiqueta)
        {
            if (ModelState.IsValid)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await etiqueta.Imagem.CopyToAsync(memoryStream);
                    var etiquetaModel = new Etiquetas
                    {
                        Nome = etiqueta.Nome,
                        Descricao = etiqueta.Descricao,
                        Imagem = memoryStream.ToArray()
                    };

                    _context.Etiquetas.Add(etiquetaModel);
                    await _context.SaveChangesAsync();
                }

                return Json(new { success = true, message = "Dados enviados com sucesso!" });
            }

            return Json(new { success = false, message = "Erro ao enviar os dados." });
        }

        [HttpPost]
        public async Task<IActionResult> CriarPDF(PDFViewModel request)
        {
            if (ModelState.IsValid && request.PDF != null)
            {
                try
                {
                    // Caminho para a pasta de uploads
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Caminho para o arquivo PDF
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + request.PDF.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Salvar o arquivo PDF
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await request.PDF.CopyToAsync(stream);
                    }

                    // Criar um novo MemoryStream para armazenar o PDF
                    MemoryStream memoryStream = new MemoryStream();

                    // Criar um novo documento PDF
                    PdfWriter writer = new PdfWriter(memoryStream);
                    PdfDocument pdfDoc = new PdfDocument(writer);
                    Document document = new Document(pdfDoc);

                    // Ler o arquivo CSV
                    using (var reader = new StreamReader(request.PDF.OpenReadStream()))
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        while (csv.Read())
                        {
                            string nomeDaEtiqueta = csv.GetField<string>(0);

                            // Buscar imagem correspondente na tabela Etiqueta
                            var etiqueta = _context.Etiquetas.FirstOrDefault(e => e.Nome == nomeDaEtiqueta);
                            if (etiqueta != null)
                            {
                                // Ler a imagem da etiqueta
                                byte[] imageBytes = etiqueta.Imagem;

                                // Adicionar a imagem ao documento PDF
                                using (var imageStream = new MemoryStream(imageBytes))
                                {
                                    iText.Layout.Element.Image pdfImage = new iText.Layout.Element.Image(ImageDataFactory.Create(imageBytes));
                                    document.Add(pdfImage);
                                }
                            }
                        }
                    }

                    // Fechar o documento
                    document.Close();

                    // Salvar o PDF no banco de dados
                    PDF pdf = new PDF
                    {
                        NPedido = request.NPedido,
                        Data = request.Data,
                        CaminhoPDF = uniqueFileName // Salvando apenas o nome do arquivo para o exemplo
                    };

                    _context.PDF.Add(pdf);
                    await _context.SaveChangesAsync();

                    // Configurar o tipo de conteúdo da resposta
                    Response.ContentType = "application/pdf";

                    // Definir o nome do arquivo para download
                    Response.Headers.Append("Content-Disposition", "attachment; filename=arquivo.pdf");

                    // Escrever o conteúdo do PDF no corpo da resposta
                    byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                    await Response.Body.WriteAsync(fileBytes, 0, fileBytes.Length);

                    // Fechar o MemoryStream
                    memoryStream.Close();

                    // Retornar uma resposta vazia
                    return new EmptyResult();
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Erro ao enviar o PDF: " + ex.Message });
                }
            }

            return Json(new { success = false, message = "Erro ao enviar os dados." });
        }




        [HttpPost]
        public async Task<IActionResult> ExcluirEtiqueta(int id)
        {
            try
            {
                // Encontrar a etiqueta pelo ID
                var etiqueta = await _context.Etiquetas.FindAsync(id);

                // Verificar se a etiqueta foi encontrada
                if (etiqueta != null)
                {
                    // Remover a etiqueta do contexto
                    _context.Etiquetas.Remove(etiqueta);

                    // Salvar as alterações no banco de dados
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "Etiqueta excluída com sucesso!" });
                }
                else
                {
                    return Json(new { success = false, message = "Etiqueta não encontrada." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public byte[]? ObterImagemDaEtiqueta(int id)
        {
            var etiqueta = _context.Etiquetas.FirstOrDefault(e => e.Id == id);

            if (etiqueta != null)
            {
                return etiqueta.Imagem;
            }

            return null;
        }

        [HttpGet]
        public IActionResult ObterTodasEtiquetas()
        {
            // Consultar todas as etiquetas do banco de dados
            var etiquetas = _context.Etiquetas.ToList();

            // Mapear os dados das etiquetas para um novo objeto com a imagem convertida para base64
            var etiquetasComImagemBase64 = etiquetas.Select(e => new
            {
                Id = e.Id,
                Nome = e.Nome,
                Descricao = e.Descricao,
                Imagem = Convert.ToBase64String(e.Imagem) // Convertendo a imagem para uma string base64
            }).ToList();

            return Json(etiquetasComImagemBase64);
        }
    }
}
