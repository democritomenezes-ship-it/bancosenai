namespace BancoSENAIAPI.Models
{
    public class Cliente
    {
        public int CodigoCLiente { get; set; }
        public string NomeCliente { get; set; }
        public string CPF { get; set; }
        public string Sexo { get; set; }
        public string Endereco { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public int Saldo { get; set; }
        public int NumeroAgencia { get; set; }


    }
}