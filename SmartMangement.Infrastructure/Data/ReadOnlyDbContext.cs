using Microsoft.EntityFrameworkCore;
using SmartManagement.Infrastructure.Extensions;
using SmartMangement.Domain.Configuration;
using SmartMangement.Domain.Interface;
using SmartMangement.Domain.Models;
using System.Reflection;

namespace SmartManagement.Infrastructure.Data
{
    public class ReadOnlyDbContext: DbContext, IApplicationDbContext
    {
        private readonly DbContextOptions<SmartDbContext> _options;
        public virtual DbSet<UserEnity> Users { get; set; }
        public ReadOnlyDbContext(DbContextOptions<SmartDbContext> options) : base(options)
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
    }
    public static class  ModelBuilderExtensions
    {
        public static void AddFunctionsToBuilder(this ModelBuilder modelBuilder)
        {
            MethodInfo method = typeof(JsonToSqlFunction).GetMethod("JsonValue");
            if (method == null)
            {
                throw new ArgumentNullException("jsonName");
            }
            modelBuilder.HasDbFunction(typeof(JsonToSqlFunction).GetMethod("jsonValue")).HasName("JSON_VALUE").IsBuiltIn()
                    .HasSchema(null);

        }
    }
}
