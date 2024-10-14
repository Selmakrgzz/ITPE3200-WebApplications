using Microsoft.EntityFrameworkCore;
using MyShop.Models;

namespace MyShop.DAL;

public class ItemRepository : IItemRepository
{
    //Deklarerer et felt _db av typen ItemDbContext, som brukes til å kommunisere med databasen
    private readonly ItemDbContext _db;

    private readonly ILogger<ItemRepository> _logger;

    //Konstruktør som initialiserer _db med en instans av ItemDbContext, injisert via Dependency Injection
    public ItemRepository(ItemDbContext db, ILogger<ItemRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    //GetAll: Henter alle Item-objekter fra databasen som en asynkron operasjon og returnerer dem som en liste
    public async Task<IEnumerable<Item>?> GetAll()
    {
        try
        {
            return await _db.Items.ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[ItemRepository] items ToListAsync() failed when GetAll(), error message: {e}", e.Message);
            return null;
        }
    }

    //GetItemById: Henter et enkelt Item fra databasen basert på en gitt ID som en asynkron operasjon. 
    //Returnerer null hvis elementet ikke finnes
    public async Task<Item?> GetItemById(int id)
    {
        try
        {
            return await _db.Items.FindAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError("[ItemRepository] item FindAsync(id) failed when GetItemById for ItemId {ItemId:0000}, error message: {e}", id, e.Message);
            return null;
        }
    }

    //Create: Legger til et nytt Item i databasen og lagrer endringene asynkront
    public async Task<bool> Create(Item item)
    {
        try
        {
            _db.Items.Add(item);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[ItemRepository] item creation failed for item {@item}, error message: {e}", item, e.Message);
            return false;
        }
    }
    
    //Update: Oppdaterer et eksisterende Item i databasen og lagrer endringene asynkront
    public async Task<bool> Update(Item item)
    {
        try
        {
            _db.Items.Update(item);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[ItemRepository] item FindAsync(id) failed when updating ItemId {ItemId:0000}, error message: {e}", item, e.Message);
            return false;
        }
    }

    //Delete: Henter et Item med en gitt ID fra databasen. 
    //Hvis varen finnes, fjernes den fra databasen, og endringene lagres asynkront. 
    //Returnerer true hvis slettingen lykkes, ellers false hvis varen ikke finnes
    public async Task<bool> Delete(int id)
    {
        try
        {
            var item = await _db.Items.FindAsync(id);
            if (item == null)
            {
                return false;
            }

            _db.Items.Remove(item);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[ItemRepository] item deletion failed for the ItemId {ItemId:0000}, error message: {e}", id, e.Message);
            return false;
        }
    }
}
