//Denne linjen importerer Entity Framework Core biblioteket. Det gir tilgang til klasser 
//og funksjoner som trengs for å kommunisere med databasen og definere datamodeller.
using Microsoft.EntityFrameworkCore;
using MyShop.Models;

namespace MyShop.DAL;

//Dette definerer klassen ItemDbContext, som arver fra klassen DbContext. 
//DbContext er en grunnleggende klasse i Entity Framework Core som gir funksjonaliteten for å jobbe med databaser.
public class ItemDbContext : DbContext
{
    /*Dette er en konstruktør for ItemDbContext-klassen. Konstruktøren tar et parameter, 
    options, som er av typen DbContextOptions<ItemDbContext>. Dette objektet brukes til 
    å konfigurere databasen, for eksempel hvilken type database (SQLite, SQL Server, osv.) 
    som skal brukes, samt tilkoblingsinformasjon.*/
	public ItemDbContext(DbContextOptions<ItemDbContext> options) : base(options)
	{
        //Denne linjen sørger for at databasen blir opprettet hvis den ikke allerede eksisterer. 
        //Database.EnsureCreated();
	}

    //Denne linjen definerer en DbSet for Item-modellen. 
    //DbSet representerer en tabell i databasen, og hver rad i tabellen vil representere en instans av Item.
    //DbSet<Item> gir deg muligheten til å utføre CRUD-operasjoner på Item-objektene i databasen.
	public DbSet<Item> Items { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    
    //By calling UseLazyLoadingProxies() in the OnConfiguring method, EF Core will generate
    //proxies for your entity classes, allowing lazy loading of related entities when accessed
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }
}
