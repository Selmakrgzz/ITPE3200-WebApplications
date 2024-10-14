using MyShop.Models;

namespace MyShop.DAL;

public interface IItemRepository
{
    //En metode som returnerer en task som inneholder en 
    //samling (IEnumerable) av alle Item-objektene. Den henter alle varer
	Task<IEnumerable<Item>?> GetAll();
    //En metode som returnerer en task som inneholder et enkelt Item, 
    //eller null hvis varen med den angitte id ikke finnes. Den henter en vare basert på ID
    Task<Item?> GetItemById(int id);
    //En metode som returnerer en task og oppretter et nytt Item-objekt i databasen
	Task<bool> Create(Item item);
    //En metode som returnerer en task og oppdaterer et eksisterende Item-objekt i databasen
    Task<bool> Update(Item item);
    //En metode som returnerer en task som inneholder en boolsk verdi (true eller false) 
    //basert på om slettingen av et Item med gitt id var vellykket
    Task<bool> Delete(int id);
}
