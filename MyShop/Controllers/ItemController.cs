//Importerer grunnleggende systemfunksjoner
using System;
//Gir tilgang til generiske samlinger, som for eksempel 'List'
using System.Collections.Generic;
//Gir LINQ funksjonalitet, som gjør det enklere å jobbe med data
using System.Linq;
//Gir funksjoner for asynkron programmering
//Altså det gjør det mulig for programmet å utføre andr eoppgaver mens det
//venter på at en langsom operasjon som I/O skal utføres
using System.Threading.Tasks;
//Importerer funskjoner for å opprette kontrollere og jobbe med HTTP-forespørsler
using Microsoft.AspNetCore.Mvc;
//Importerer modeller som antas å være definert i 'MyShop.Models' navnerommet
using MyShop.Models;
using MyShop.ViewModels;

//Definerer et navnrom som organiserer koden under MyShop.Controllers
namespace MyShop.Controllers;

//Definerer en kontroller kalt 'ItemController', som arver fra 'Controller',
//en baseklasse i ASP.NET Core MVC
public class ItemController : Controller 
{
    //Declares a private read-only field for storing an instance of ItemDbContext
    private readonly ItemDbContext _itemDbContext;

    //It is called when an instance of ItemController is created, typically during the handling of an incomingøHTTP request
    public ItemController(ItemDbContext ItemDbContext)
    {
        _itemDbContext = ItemDbContext;
    }

    public IActionResult Table()
    {
        List<Item> items = _itemDbContext.Items.ToList();
        var itemsViewModel = new ItemsViewModel(items, "Table");
        return View(itemsViewModel);
    }

    public IActionResult Grid()
    {
        List<Item> items = _itemDbContext.Items.ToList();
        var itemsViewModel = new ItemsViewModel(items, "Grid");
        return View(itemsViewModel);
    }
    
    public IActionResult Details(int id)
    {
        //Henter listen over varer i GetItems-metoden
        List<Item> items = _itemDbContext.Items.ToList();
        //Finner varen med den spesifikke ID-en
        var item = items.FirstOrDefault(i => i.ItemId == id);
        //Hvis varen ikke finnes, returner en 404
        if (item == null)
            return NotFound();
            //Returner visningen med detaljene for varen
        return View(item);
    }

    //Når det skal opprettes et nytt element, så er det en GET-forespørsel
    [HttpGet]
    public IActionResult Create() //Returnerer IActionResult
    {
        //Returnerer et view (HTML-side), der brukeren kan fylle ut informasjon for det nye elementet
        return View();
    }

    //Når brukeren sender inn skjemaet med dataene(eks nytt element) vil POST-metoden ta seg av innsending
    [HttpPost]
    public IActionResult Create(Item item) //Tar inn et Item-objekt
    {
        //Denne linjen sjekker om den sendte modellen (i dette tilfellet item) er gyldig i 
        //henhold til valideringsreglene som er definert i Item-modellen.
        if (ModelState.IsValid)
        {
            //Dette legger det nye Item-objektet til i DbSet Items, som representerer 
            //tabellen i databasen. Dette betyr at elementet legges til i databasen
            _itemDbContext.Items.Add(item);
            //Denne metoden sparer alle de endringene som er gjort på DbContext 
            //(i dette tilfellet å legge til et nytt element). Det er her elementet faktisk lagres i databasen.
            _itemDbContext.SaveChanges();
            //Etter at det nye elementet er lagt til i databasen, blir brukeren omdirigert til en annen 
            //handling (action), i dette tilfellet en metode kalt Table
            return RedirectToAction(nameof(Table));
        }
        //Hvis ModelState.IsValid er false (f.eks. hvis skjemaet inneholder feil), vises skjemaet igjen 
        //med de inndataene brukeren allerede har fylt ut, slik at brukeren kan rette opp feilene.
        return View(item);
    }        

    //Denne metoden håndterer en GET-forespørsel for å vise et skjema som lar brukeren oppdatere et eksisterende element
    [HttpGet]
    public IActionResult Update(int id) //Parameteren id representerer elementets ID i databasen som skal oppdateres.
    {
        //Søker i databasen etter et element med den spesifikke ID-en. Find(id) henter det aktuelle elementet fra databasen.
        var item = _itemDbContext.Items.Find(id);
        //Sjekker om elementet finnes i databasen
        if (item == null)
        {
            return NotFound();
        }
        //Hvis elementet finnes, returneres en visning som viser skjemaet for å oppdatere elementet. 
        //item blir sendt til View slik at det kan forfylles med eksisterende data.
        return View(item);
    }

    //Metoden mottar det oppdaterte Item-objektet via skjemaet
    [HttpPost]
    public IActionResult Update(Item item)
    {
        //Sjekker om det innsendte objektet oppfyller valideringsreglene
        if (ModelState.IsValid)
        {
            //Oppdaterer det eksisterende elementet i databasen med nye verdier fra item
            _itemDbContext.Items.Update(item);
            //Lagre endringene i databasen
            _itemDbContext.SaveChanges();
            //Etter en vellykket oppdatering omdirigeres brukeren til en liste (Table) som viser alle elementene
            return RedirectToAction(nameof(Table));
        }
        //Hvis dataene er ugyldige (ModelState er ikke valid), vises skjemaet på nytt med de 
        //opprinnelige innsendte dataene og valideringsfeil
        return View(item);
    }

    //Denne metoden håndterer en GET-forespørsel for å vise en bekreftelsesside for sletting
    [HttpGet]
    public IActionResult Delete(int id) //Parameteren id representerer elementets ID som skal slettes
    {
        //Henter elementet som skal slettes fra databasen basert på ID
        var item = _itemDbContext.Items.Find(id);
        //Sjekker om elementet finnes i databasen
        if (item == null)
        {
            return NotFound();
        }
        //Hvis elementet finnes, vises en bekreftelsesside 
        //med informasjon om elementet slik at brukeren kan bekrefte slettingen
        return View(item);
    }

    //Denne metoden håndterer POST-forespørselen som sendes inn når brukeren bekrefter slettingen. 
    //Den utfører selve slettingen i databasen
    [HttpPost]
    public IActionResult DeleteConfirmed(int id) //Metoden mottar ID-en til elementet som skal slettes
    {
        //Henter elementet fra databasen ved hjelp av ID-en
        var item = _itemDbContext.Items.Find(id);
        //Sjekker om elementet finnes i databasen
        if (item == null)
        {
            return NotFound();
        }
        //Fjerner elementet fra databasen
        _itemDbContext.Items.Remove(item);
        //Lagre slettingen i databasen
        _itemDbContext.SaveChanges();
        //Etter at elementet er slettet, omdirigeres brukeren til en liste som viser alle elementene
        return RedirectToAction(nameof(Table));
    }
    
}

    //Definerer en metode som returnerer en 'IActionResult', vanligvis en HTML-side
    //eller en JSON-data. Denne metoden vil bli kjørt når brukeren navigerer til en
    //spesifikk URL som er knyttet til denne handlingen
    /*public IActionResult Table()
    {
        //Henter listen over varer i GetItems-metoden
        var items = GetItems();
        //Oppretter en ItemsViewModel med varene og visningstypen "Table"
        ViewBag.CurrentViewName = "Table";
        //Returner visningen med ItemViewModel
        return View(items);
    }

    public IActionResult Grid()
    {
        //Henter listen over varer i GetItems-metoden
        var items = GetItems();
        //Oppretter en ItemsViewModel med varene og visningstypen "Grid"
        ViewBag.CurrentViewName = "Grid";
        //Returner visningen med ItemViewModel
        return View(items);

        public List<Item> GetItems()
    {
        //Opprett en liste for å holde varene
        var items = new List<Item>();
        //Legg til første vare
        var item1 = new Item
        {
            ItemId = 1,
            Name = "Pizza",
            Price = 150,
            Description = "Delicious Italian dish with a thin crust topped with tomato sauce, cheese, and various toppings.",
            ImageUrl = "/images/pizza.jpg"
        };

        var item2 = new Item
        {
            ItemId = 2,
            Name = "Fried Chicken Leg",
            Price = 20,
            Description = "Crispy and succulent chicken leg that is deep-fried to perfection, often served as a popular fast food item.",
            ImageUrl = "/images/chickenleg.jpg"
        };

        var item3 = new Item
        {
            ItemId = 3,
            Name = "French Fries",
            Price = 50,
            Description = "Crispy, golden-brown potato slices seasoned with salt and often served as a popular side dish or snack.",
            ImageUrl = "/images/frenchfries.jpg"
        };

        var item4 = new Item
        {
            ItemId = 4,
            Name = "Grilled Ribs",
            Price = 250,
            Description = "Tender and flavorful ribs grilled to perfection, usually served with barbecue sauce.",
            ImageUrl = "/images/ribs.jpg"
        };

        var item5 = new Item
        {
            ItemId = 5,
            Name = "Tacos",
            Price = 150,
            Description = "Tortillas filled with various ingredients such as seasoned meat, vegetables, and salsa, folded into a delicious handheld meal.",
            ImageUrl = "/images/tacos.jpg"
        };

        var item6 = new Item
        {
            ItemId = 6,
            Name = "Fish and Chips",
            Price = 180,
            Description = "Classic British dish featuring battered and deep-fried fish served with thick-cut fried potatoes.",
            ImageUrl = "/images/fishandchips.jpg"
        };

        var item7 = new Item
        {
            ItemId = 7,
            Name = "Cider",
            Price = 50,
            Description = "Refreshing alcoholic beverage made from fermented apple juice, available in various flavors.",
            ImageUrl = "/images/cider.jpg"
        };

        var item8 = new Item
        {
            ItemId = 8,
            Name = "Coke",
            Price = 30,
            Description = "Popular carbonated soft drink known for its sweet and refreshing taste.",
            ImageUrl = "/images/coke.jpg"
        };


        items.Add(item1);
        items.Add(item2);
        items.Add(item3);
        items.Add(item4);
        items.Add(item5);
        items.Add(item6);
        items.Add(item7);
        items.Add(item8);
        return items;
    }*/