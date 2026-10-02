using BancoSENAIAPI.Data;
using BancoSENAIAPI.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BancoSENAIAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DocumentController : Controller
    {
        private AppDbContext _context;
        public DocumentController(AppDbContext context)
        {
            _context = context;
        }

        private readonly string _caminhoRaiz = Path.Combine(Directory.GetCurrentDirectory(), "ClienteArquivo");


        [HttpPost("upload/{codigoCliente}")]
        public async Task<IActionResult> PostArquivo(int codigoCliente, IFormFile arquivo)
        {
            double tamanhoMax = 2 * 1024 * 1024;
            if (arquivo == null || arquivo.Length == 0) return BadRequest("Nenhum arquivo encontrado");

            if (arquivo.Length > tamanhoMax)
            {
                return BadRequest($"Tamanho do arquivo passou de 2MB, então não pode ser enviado");
            }
            string pastaCliente = Path.Combine(_caminhoRaiz, codigoCliente.ToString());

            if (!Directory.Exists(pastaCliente))
            {
                Directory.CreateDirectory(pastaCliente);
            }

            if (!await _context.Cliente.AnyAsync(e => e.CodigoCLiente == codigoCliente))
            {
                return BadRequest("Nenhum cliente com dados encontrado");
            }
            string extensao = Path.GetExtension(arquivo.FileName);
            if (extensao != ".png" && extensao != ".jpg" && extensao != ".pdf")
            {
                return BadRequest("Somente pode ser enviados arquivos do tipo");
            }
            string nomeOriginal = Path.GetFileNameWithoutExtension(arquivo.FileName);
            string novoNome = $"{codigoCliente}_{nomeOriginal}_{Guid.NewGuid()}{extensao}";
            string caminhoFinal = Path.Combine(pastaCliente, novoNome);

            using (var stream = new FileStream(caminhoFinal, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }
            var documentos = new DocumentoMetadado
            {
                Name = nomeOriginal,
                Extensao = extensao,
                Caminho = caminhoFinal,
                CodigoCliente = codigoCliente
            };
            Console.WriteLine(documentos.Caminho);
            _context.Documento.Add(documentos);
            await _context.SaveChangesAsync();
            return Ok(new { mensagem = "Documento criado com suscesso" }); ;
        }

        [HttpGet("listagem/{codigoCliente}")]
        public async Task<IActionResult> ListarDocumentos([FromRoute] int codigoCliente)
        {
            if (!await _context.Documento.AnyAsync(e => e.CodigoCliente == codigoCliente))
            {
                return NotFound("Nenhum cliente");
            }
            var documentos = await _context.Documento.Where(d => d.CodigoCliente == codigoCliente).ToListAsync();
            return Ok(documentos);
        }

        [HttpGet("cliente/{codigoCliente}/dowload/{id}")]
        public async Task<IActionResult> DowloadArquivo([FromRoute] int id, [FromRoute] int codigoCliente)
        {
            if (!await _context.Documento.AnyAsync(e => e.Id == id))
            {
                return NotFound("Nenhum arquivo encontrado");
            }

            var documento = await _context.Documento.FirstOrDefaultAsync(e => e.Id == id);
            if (documento.CodigoCliente != codigoCliente)
            {
                return BadRequest("Você não pode ver esse arquivo");
            }

            var fileByts = await System.IO.File.ReadAllBytesAsync(documento.Caminho);//Lista de bytes que forma  imagem

            return File(fileByts, "aplication/octet-stream", documento.Name);
        }

        [HttpDelete("cliente/{codigoCliente}/excluir/{id}")]
        public async Task<IActionResult> DeleteDocument([FromRoute] int id, [FromRoute] int codigoCliente)
        {
            if (!await _context.Documento.AnyAsync(e => e.Id == id))
            {
                return NotFound("Nenhum arquivo encontrado");
            }

            var documento = await _context.Documento.FirstOrDefaultAsync(e => e.Id == id);
            if (documento.CodigoCliente != codigoCliente)
            {
                return BadRequest("Você não pode ver esse arquivo");
            }

            _context.Documento.Remove(documento);
            await _context.SaveChangesAsync();
            System.IO.File.Delete(documento.Caminho);

            return NoContent();
        }
    }
}