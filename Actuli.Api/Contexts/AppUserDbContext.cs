using Microsoft.EntityFrameworkCore;
using Actuli.Api.Models;

namespace Actuli.Api.DbContext;

public class AppUserDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public AppUserDbContext(DbContextOptions<AppUserDbContext> options) : base(options)
    {
    }

    public DbSet<AppUser> ApplicationUsers { get; set; }
}