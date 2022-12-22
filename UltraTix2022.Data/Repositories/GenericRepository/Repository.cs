using Microsoft.EntityFrameworkCore;
using UltraTix2022.API.UltraTix2022.Data.Models.Entities;

namespace UltraTix2022.Data.Repositories.GenericRepository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly UltraTixDBContext context;
        private DbSet<T> _entities;

        public Repository(UltraTixDBContext context)
        {
            this.context = context;
            _entities = context.Set<T>();
        }

        public async Task<T?> Get(Guid id)
        {
            return await _entities.FindAsync(id);
        }

        public async Task<Guid> Insert(T entity)
        {
            await _entities.AddAsync(entity);
            await Update();
            return (Guid)entity.GetType().GetProperty("Id").GetValue(entity);
        }

        public async Task Update()
        {
            await context.SaveChangesAsync();
        }

    }
}
