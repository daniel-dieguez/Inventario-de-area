using Inventario.models;
using Microsoft.EntityFrameworkCore;

namespace Inventario.context;


public class ApplicationDbContext : DbContext
{

    public ApplicationDbContext(DbContextOptions <ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Productos> Productos { get; set; } = null!;
}