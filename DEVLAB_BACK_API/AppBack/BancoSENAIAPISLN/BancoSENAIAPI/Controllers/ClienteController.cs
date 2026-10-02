using BancoSENAIAPI.Data;
using BancoSENAIAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BancoSENAIAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ClienteController : ControllerBase
    {
        private AppDbContext _context;
        public ClienteController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ListarTodas()
        {
            var clientes = await _context.Cliente.ToListAsync();
            return Ok(clientes);
        }

        [HttpGet("{codigo}")]
        public async Task<IActionResult> ConsultarPorCodigo(int codigo)
        {
            var carteira = await _context.Cliente.FirstOrDefaultAsync(a => a.CodigoCLiente == codigo);

            if (carteira == null)
                return NotFound(new { message = "Cliente não encontrada." }); // Status 404 [6, 7]

            return Ok(carteira); // Status 200 OK [6, 7]
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] Cliente novoCliente)
        {

            if (_context.Cliente.Any(c => c.CPF == novoCliente.CPF))
            {
                return BadRequest(new { message = "CPF já cadastrado." }); // Status 400 Bad Request [6]
            }

            _context.Cliente.Add(novoCliente);
            await _context.SaveChangesAsync();
            // Retorna Status 201 Created conforme boas práticas REST [6, 8]
            return Created("", novoCliente);
        }

        [HttpPut("{codigo}")]
        public async Task<IActionResult> Alterar(int codigo, [FromBody] Cliente clienteAtualizar)
        {
            var clienteExiste = await _context.Cliente.FirstOrDefaultAsync(a => a.CodigoCLiente == codigo);

            if (clienteExiste == null) return NotFound();

            clienteExiste.NomeCliente = clienteAtualizar.NomeCliente;
            clienteExiste.CPF = clienteAtualizar.CPF;
            clienteExiste.Sexo = clienteAtualizar.Sexo;
            clienteExiste.Endereco = clienteAtualizar.Endereco;
            clienteExiste.Cidade = clienteAtualizar.Cidade;
            clienteExiste.Estado = clienteAtualizar.Estado;
            clienteExiste.Saldo = clienteAtualizar.Saldo;
            clienteExiste.NumeroAgencia = clienteAtualizar.NumeroAgencia;

            await _context.SaveChangesAsync();
            // Retorna Status 204 No Content para atualizações bem-sucedidas [6, 9]
            return NoContent();
        }

        [HttpDelete("{codigo}")]
        public async Task<IActionResult> Excluir(int codigo)
        {
            var cliente = await _context.Cliente.FirstOrDefaultAsync(a => a.CodigoCLiente == codigo);

            if (cliente == null) return NotFound();

            _context.Cliente.Remove(cliente);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Cliente excluída com sucesso." }); // Status 200 [6]
        }
    }
}