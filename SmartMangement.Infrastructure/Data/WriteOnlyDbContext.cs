using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SmartMangement.Domain.Configuration;
using SmartMangement.Domain.Interface;
using SmartMangement.Domain.Models;

namespace SmartManagement.Infrastructure.Data
{
    public class WriteOnlyDbContext: DbContext, IApplicationDbContext
    {
        private readonly DbContextOptions<SmartDbContext> _options;
        public virtual DbSet<UserEnity> Users { get; set; }
        public WriteOnlyDbContext(DbContextOptions<SmartDbContext> options) : base(options)
        {
            _options = options;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
            modelBuilder.AddFunctionsToBuilder();

        }
        public DbContextOptions<SmartDbContext> GetDbContextOptions()
        {
            return _options;
        }
        public EntityEntry<T> Update<T>(T entity, List<string> properties) where T : class
        {
            Entry(entity).State = EntityState.Unchanged;
            foreach (var property in properties)
            {
                Entry(entity).Property(property).IsModified = true;
            }
            return base.Update(entity);
        }
        public sealed override async Task<int> SaveChangesAsync(CancellationToken token)
        {
            int entries = await base.SaveChangesAsync(token);
            ChangeTracker.Clear();
            return entries;
        }
        public sealed override int SaveChanges()
        {
            int result = base.SaveChanges();
            ChangeTracker.Clear();
            return result;
        }
    }
}
