using System.Drawing;

namespace Dio.DevWeekPottencial.Api.Src.Models
{
    public class Contrato
    {
        public int Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public string TokenId { get; set; }
        public double Valor { get; set; }
        public bool Pago { get; set; }
        public int PessoaId { get; set; }

        public Contrato()
        {
            DataCriacao = DateTime.Now;
            Valor = 0;
            TokenId = "000000";
            Pago = false;
        }

        public Contrato(string tokenId, double valor)
        {
            DataCriacao = DateTime.Now;
            Valor = valor;
            TokenId = tokenId;
            Pago = false;
        }
    }
}
