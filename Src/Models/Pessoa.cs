using Microsoft.AspNetCore.Mvc;

namespace Dio.DevWeekPottencial.Api.Src.Models
{
    public class Pessoa
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Idade { get; set; }
        public string Cpf { get; set; }
        public bool Ativado { get; set; }
        public List<Contrato> ListaContratos { get; set; }

        public Pessoa()
        {
            Ativado = true;
            ListaContratos = new List<Contrato>();
        }

        public Pessoa(string nome, int idade, string cpf)
        {
            Nome = nome;
            Idade = idade;
            Cpf = cpf;
            Ativado = true;
            ListaContratos = new List<Contrato>();
        }
    }
}
