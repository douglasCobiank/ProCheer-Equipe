using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Equipe.Infrastructure.Data
{
    public class EquipeDbContextFactory: IDesignTimeDbContextFactory<EquipeDbContext>
    {
        public EquipeDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EquipeDbContext>();

            optionsBuilder.UseNpgsql("Host=db.tugjkfhjgfzqttuexicr.supabase.co;Database=postgres;Username=postgres;Password=procheer2025;SSL Mode=Require;Trust Server Certificate=true");

            return new EquipeDbContext(optionsBuilder.Options);
        }
    }
}