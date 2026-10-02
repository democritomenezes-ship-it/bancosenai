using System.ComponentModel.DataAnnotations;

namespace BancoSENAIAPI.Models
{
    public class Cliente
    {
        [Key]
        public int CodigoCLiente { get; set; }
        public string NomeCliente { get; set; }
        [Required]
        public string CPF { get; set; }
        [Required]
        public string Sexo { get; set; }
        [Required]
        public string Endereco { get; set; }
        [Required] 
        public string Cidade { get; set; }
        [Required]
        public string Estado { get; set; }
        [Required]
        public int Saldo { get; set; }
        [Required]
        public int NumeroAgencia { get; set; }


    }
}