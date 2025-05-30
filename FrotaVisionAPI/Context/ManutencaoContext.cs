﻿using FrotaVisionAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FrotaVisionAPI.Context
{
    public class ManutencaoContext : DbContext
    {
        public ManutencaoContext(DbContextOptions<ManutencaoContext> options)
            : base(options)
        {
        }
        public DbSet<Manutencao> Manutencao{ get; set; } = null!;
    }
    
}

