using Dio.DevWeekPottencial.Api.Src.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace Dio.DevWeekPottencial.Api.Src.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> opt) : base(opt)
        {

        }

        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Contrato> Contratos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Pessoa>(p => {
                p.HasKey(p => p.Id);
                p.HasMany(e => e.ListaContratos).WithOne().HasForeignKey(c => c.PessoaId);

            });

            builder.Entity<Contrato>(c => {
                c.HasKey(c => c.Id);
            });
        }
    }
}



        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    //1 para 1
        //    builder.Entity<Endereco>()
        //        .HasOne(endereco => endereco.Cinema)
        //        .WithOne(cinema => cinema.Endereco)
        //        .HasForeignKey<Cinema>(cinema => cinema.EnderecoId);

        //    //1 para muitos
        //    builder.Entity<Cinema>()
        //        .HasOne(cinema => cinema.Gerente)
        //        .WithMany(gerente => gerente.Cinemas)
        //        .HasForeignKey(cinema => cinema.GerenteId);
        //    //.HasForeignKey(cinema => cinema.GerenteId).IsRequired(false); //Permitir que a chave estrangeira serja nulla
        //    //.OnDelete(DeleteBehavior.Restrict) //Por padrão a deleção será como Cascade, colocando como restrict, é necessário excluir a dependencia primeiro.

        //    //n para n
        //    builder.Entity<Sessao>()
        //        .HasOne(sessao => sessao.Filme)
        //        .WithMany(filme => filme.Sessoes)
        //        .HasForeignKey(sessao => sessao.FilmeId);
        //    builder.Entity<Sessao>()
        //        .HasOne(sessao => sessao.Cinema)
        //        .WithMany(cinema => cinema.Sessoes)
        //        .HasForeignKey(sessao => sessao.CinemaId);

        //}

        //public DbSet<Filme> Filmes { get; set; }
        //public DbSet<Cinema> Cinemas { get; set; }
        //public DbSet<Endereco> Enderecos { get; set; }
        //public DbSet<Gerente> Gerentes { get; set; }
        //public DbSet<Sessao> Sessoes { get; set; }
