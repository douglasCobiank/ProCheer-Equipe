using Equipe.Infrastructure.Data;
using Equipe.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Equipe.Infrastructure.Repositories
{
    public class EquipeRepository(EquipeDbContext context) : IEquipeRepository
    {
        protected readonly EquipeDbContext _context = context;
        protected readonly DbSet<EquipeData> _dbSet = context.Set<EquipeData>();
        public async Task AddAsync(EquipeData equipeData)
        {
            // ✅ GINÁSIO EXISTENTE
            if (equipeData.Ginasio?.Id > 0)
            {
                _context.Attach(equipeData.Ginasio);
            }

            // ✅ ENDEREÇO EXISTENTE
            if (equipeData.Ginasio?.Endereco?.Id > 0)
            {
                _context.Attach(equipeData.Ginasio.Endereco);
            }

            // Anexa cada relação atleta-ginásio, se existir
            if (equipeData.EquipeAtletas != null && equipeData.EquipeAtletas.Any())
            {
                // ✅ ATLETAS EXISTENTES
                foreach (var ea in equipeData.EquipeAtletas)
                {
                    if (ea.Atleta?.Id > 0)
                    {
                        _context.Attach(ea.Atleta);

                        // ✅ DOCUMENTO EXISTENTE
                        if (ea.Atleta.Documento?.Id > 0)
                            _context.Attach(ea.Atleta.Documento);
                    }
                }
            }

            await _dbSet.AddAsync(equipeData);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(EquipeData equipeData)
        {
            if (_context.Entry(equipeData).State == EntityState.Detached)
                _dbSet.Attach(equipeData);

            _dbSet.Remove(equipeData);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync(EquipeData equipeData)
        {
            _context.Entry(equipeData).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<List<EquipeData>> GetAllWithIncludeAsync()
        {
            return await _dbSet.Include(d => d.Ginasio)
                .ThenInclude(d => d.Endereco)
                .Include(d => d.EquipeAtletas).ToListAsync();
        }

        public async Task<EquipeData?> GetByIdAsync(int id)
        {
            return await _dbSet.Include(d => d.Ginasio).Include(d => d.EquipeAtletas).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<EquipeData?> GetEquipePorNomeAsync(string nome)
        {
            return await _dbSet.Include(d => d.Ginasio)
                .ThenInclude(d => d.Endereco)
                .Include(d => d.EquipeAtletas).FirstOrDefaultAsync(a => a.NomeEquipe == nome);
        }

        public void AttachEntity<T>(T entity) where T : class
        {
            if (entity == null)
                return;

            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
                _context.Attach(entity);
        }
    }
}