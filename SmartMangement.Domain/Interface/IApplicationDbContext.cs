using Microsoft.EntityFrameworkCore;
using SmartMangement.Domain.Models;

namespace SmartMangement.Domain.Interface
{
    public interface IApplicationDbContext
    {
        DbSet<UserEnity> Users { get; set; }
    }
}
