using BancoSENAIAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BancoSENAIAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ClienteController : ControllerBase
    {
        private List<Cliente> _clientes = new List<Cliente>
        {
            new Cliente { CodigoCLiente = 1, NomeCliente = "João Pedro Silva", CPF = "12345678901", Sexo = "Masculino", Endereco = "Rua das Flores, 120", Cidade = "Aracaju", Estado = "SE", Saldo = 2500, NumeroAgencia = 1 },
            new Cliente { CodigoCLiente = 2, NomeCliente = "Maria Eduarda Santos", CPF = "23456789012", Sexo = "Feminino", Endereco = "Avenida Beira Mar, 450", Cidade = "Aracaju", Estado = "SE", Saldo = 5800, NumeroAgencia = 2 },
            new Cliente { CodigoCLiente = 3, NomeCliente = "Lucas Almeida Costa", CPF = "34567890123", Sexo = "Masculino", Endereco = "Rua São José, 85", Cidade = "Itabaiana", Estado = "SE", Saldo = 1200, NumeroAgencia = 3 }
        };
        private int novoCodigoCliente = 0;
        [HttpGet]
        public IActionResult ListarTodas()
        {
            return Ok(_clientes);
        }

        [HttpGet("{codigo}")]
        public IActionResult ConsultarPorCodigo(int codigo)
        {
            var carteira = _clientes.FirstOrDefault(a => a.CodigoCLiente == codigo);

            if (carteira == null)
                return NotFound(new { message = "Nenhum dado encontrado." });

            return Ok(carteira);
        }

        [HttpPost]
        public IActionResult Cadastrar([FromBody] Cliente novoCliente)
        {
            novoCodigoCliente++;
            Cliente createCliente = new Cliente { CodigoCLiente = novoCodigoCliente, NomeCliente = novoCliente.NomeCliente, CPF = novoCliente.CPF, Sexo = novoCliente.Sexo, Endereco = novoCliente.Endereco, Cidade = novoCliente.Cidade, Estado = novoCliente.Estado, Saldo = novoCliente.Saldo, NumeroAgencia = novoCliente.NumeroAgencia };
            _clientes.Add(createCliente);

            return Created("", createCliente);
        }

        [HttpPut("{codigo}")]
        public IActionResult Alterar(int codigo, [FromBody] Cliente clienteAtualizar)
        {
            var clienteExiste = _clientes.FirstOrDefault(a => a.CodigoCLiente == codigo);

            if (clienteExiste == null) return NotFound();

            clienteExiste.NomeCliente = clienteAtualizar.NomeCliente;
            clienteExiste.CPF = clienteAtualizar.CPF;
            clienteExiste.Sexo = clienteAtualizar.Sexo;
            clienteExiste.Endereco = clienteAtualizar.Endereco;
            clienteExiste.Cidade = clienteAtualizar.Cidade;
            clienteExiste.Estado = clienteAtualizar.Estado;
            clienteExiste.Saldo = clienteAtualizar.Saldo;
            clienteExiste.NumeroAgencia = clienteAtualizar.NumeroAgencia;

            return NoContent();
        }

        [HttpDelete("{codigo}")]
        public IActionResult Excluir(int codigo)
        {
            var cliente = _clientes.FirstOrDefault(a => a.CodigoCLiente == codigo);

            if (cliente == null) return NotFound();

            _clientes.Remove(cliente);
            return Ok(new { message = "Dados excluidos com sucesso." });

        }
    }
}