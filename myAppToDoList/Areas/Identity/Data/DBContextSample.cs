using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using myAppToDoList.Areas.Identity.Data;
using System.Reflection.Emit;

public class DBContextSample : IdentityDbContext<SampleUser>
{
    public DBContextSample(DbContextOptions<DBContextSample> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Product>().HasKey(p => p.Id);

        //builder.Entity<Product>()
        //    .HasOne(p => p.User) // User property'sinin olduğu yerdeki ismi kontrol ederek güncelleyin
        //    .WithMany()
        //    .HasForeignKey(p => p.Id); // Product sınıfınızdaki UserId property'sinin adı ne ise onu buraya yazın
    }
}