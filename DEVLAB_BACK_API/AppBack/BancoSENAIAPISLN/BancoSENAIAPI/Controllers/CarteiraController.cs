using BancoSENAIAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace BancoSENAIAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CarteiraController : ControllerBase
    {
        private List<Carteira> _carteira = new List<Carteira>
        {
            new Carteira { NumeroCarteira = 1, NomeCarteira = "Agro", ApetiteCarteira = 10000 },
            new Carteira { NumeroCarteira = 2, NomeCarteira = "Tecnologia", ApetiteCarteira = 50000 },
            new Carteira { NumeroCarteira = 3, NomeCarteira = "Renda Fixa", ApetiteCarteira = 25000 }
        };

        [HttpGet]
        public IActionResult ListarTodas()
        {
            return Ok(_carteira);
        }

        [HttpPost]
        public IActionResult Cadastrar([FromBody] Carteira novaCarteira)
        {

            if (_carteira.Any(a => a.NumeroCarteira == novaCarteira.NumeroCarteira))
                return BadRequest(new { message = "Este número de agência já existe." });

            if (string.IsNullOrWhiteSpace(novaCarteira.NomeCarteira))
            {
                return BadRequest(new { message = "O nome da carteira não pode ser vazio." });
            }

            if (novaCarteira.ApetiteCarteira == 0)
            {
                novaCarteira.ApetiteCarteira = 10000;
            }

            _carteira.Add(novaCarteira);
            return Created("", novaCarteira);
        }

        [HttpGet("{codigo}")]
        public IActionResult ConsultarPorCodigo(int codigo)
        {
            var carteira = _carteira.FirstOrDefault(a => a.NumeroCarteira == codigo);

            if (carteira == null)
                return NotFound(new { message = "Agência não encontrada." });

            return Ok(carteira);
        }

        [HttpPut("{codigo}")]
        public IActionResult Alterar(int codigo, [FromBody] Carteira carteiraAtualizada)
        {
            var carteiraExistente = _carteira.FirstOrDefault(a => a.NumeroCarteira == codigo);

            if (carteiraExistente == null) return NotFound();

            carteiraExistente.ApetiteCarteira = carteiraAtualizada.ApetiteCarteira;
            carteiraExistente.NomeCarteira = carteiraAtualizada.NomeCarteira;

            return NoContent();
        }

        [HttpDelete("{codigo}")]
        public IActionResult Excluir(int codigo)
        {
            var carteira = _carteira.FirstOrDefault(a => a.NumeroCarteira == codigo);

            if (carteira == null) return NotFound();

            _carteira.Remove(carteira);
            return Ok(new { message = "Agência excluída com sucesso." });
        }
    }
}