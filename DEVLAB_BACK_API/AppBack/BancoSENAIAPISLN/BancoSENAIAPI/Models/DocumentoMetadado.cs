namespace BancoSENAIAPI.Models
{
    public class DocumentoMetadado
    {
        public int id { get; set; }
        public string name { get; set; }    
        public string Extensao { get; set;}
        public string Caminho { get; set;}
        public int CodigoCliente { get; set;}
    }
}
