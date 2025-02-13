using Microsoft.EntityFrameworkCore;
using Actuli.Api.Models;

namespace Actuli.Api.DbContext;

public class ApplicationUserDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public ApplicationUserDbContext(DbContextOptions<ApplicationUserDbContext> options) : base(options)
    {
    }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
}