using BancoSENAIAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BancoSENAIAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Agencia> Agencia => Set<Agencia>();
        public DbSet<Carteira> Carteira => Set<Carteira>();
        public DbSet<DocumentoMetadado> Documento => Set<DocumentoMetadado>();
        public DbSet<Cliente> Cliente => Set<Cliente>();
    }
}