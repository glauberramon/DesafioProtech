using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using DesafioProtech.Domain.Entities;


namespace DesafioProtech.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Anime> Animes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurações extras (opcional)
            modelBuilder.Entity<Anime>(entity =>
            {
                entity.HasKey(a => a.Id); // Define a chave primária
                entity.Property(a => a.Nome).IsRequired().HasMaxLength(100);
                entity.Property(a => a.Diretor).HasMaxLength(50);
                entity.Property(a => a.Resumo).HasMaxLength(500);
            });
        }
    }
}
