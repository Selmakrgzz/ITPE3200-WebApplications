//Importerer grunnleggende systemfunksjoner
//using System;
//Gir tilgang til generiske samlinger, som for eksempel 'List'
//using System.Collections.Generic;
//Gir LINQ funksjonalitet, som gjør det enklere å jobbe med data
//using System.Linq;
//Gir funksjoner for asynkron programmering
//Altså det gjør det mulig for programmet å utføre andr eoppgaver mens det
//venter på at en langsom operasjon som I/O skal utføres
//using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
//Importerer funskjoner for å opprette kontrollere og jobbe med HTTP-forespørsler
using Microsoft.AspNetCore.Mvc;
//Importerer modeller som antas å være definert i 'MyShop.Models' navnerommet
using MyShop.Models;
using MyShop.ViewModels;
/*using Microsoft.EntityFrameworkCore; is no longer needed because the direct
database access is not done in the Controllers but in the repository.
All direct database access via _itemDbContext is now removed. Instead, ItemController.cs
uses the methods defined in ItemRepository to access the database.*/
//using Microsoft.EntityFrameworkCore;
using MyShop.DAL;

//Definerer et navnrom som organiserer koden under MyShop.Controllers
namespace MyShop.Controllers;

//Definerer en kontroller kalt 'ItemController', som arver fra 'Controller',
//en baseklasse i ASP.NET Core MVC
public class ItemController : Controller 
{
    private readonly IItemRepository _itemRepository;
    private readonly ILogger<ItemController> _logger;

    //It is called when an instance of ItemController is created, typically during the handling of an incomingøHTTP request
    public ItemController(IItemRepository itemRepository, ILogger<ItemController> logger)
    {
        _itemRepository = itemRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Table()
    {
        var items = await _itemRepository.GetAll();
        if (items == null)
        {
            _logger.LogError("[ItemController] Item list not found while executing _itemRepository.GetAll()");
            return NotFound("Item list not found"); 
        }
        var itemsViewModel = new ItemsViewModel(items, "Table");
        return View(itemsViewModel);
    }

    public async Task<IActionResult> Grid()
    {
        var items = await _itemRepository.GetAll();
        if (items == null)
        {
            _logger.LogError("[ItemController] Item list not found while executing _itemRepository.GetAll()");
            return NotFound("Item list not found"); 
        }
        var itemsViewModel = new ItemsViewModel(items, "Grid");
        return View(itemsViewModel);
    }
    
    public async Task<IActionResult> Details(int id)
    {
        var item = await _itemRepository.GetItemById(id);
        //Hvis varen ikke finnes, returner en 404
        if (item == null)
        {
            _logger.LogError("[ItemController] Item not found for the ItemId {ItemId:0000}", id);
            return NotFound("Item not found for the ItemId");
        }
        //Returner visningen med detaljene for varen
        return View(item);
    }

    //Når det skal opprettes et nytt element, så er det en GET-forespørsel
    //The HttpGet version of Create() does not communicate with the database and does not need the async method
    [HttpGet]
    public IActionResult Create() //Returnerer IActionResult
    {
        //Returnerer et view (HTML-side), der brukeren kan fylle ut informasjon for det nye elementet
        return View();
    }

    //Når brukeren sender inn skjemaet med dataene(eks nytt element) vil POST-metoden ta seg av innsending
    [HttpPost]
    public async Task<IActionResult> Create(Item item) //Tar inn et Item-objekt
    {
        //Denne linjen sjekker om den sendte modellen (i dette tilfellet item) er gyldig i 
        //henhold til valideringsreglene som er definert i Item-modellen.
        if (ModelState.IsValid)
        {
            bool returnOk = await _itemRepository.Create(item);
            if (returnOk)
            {
                //Etter at det nye elementet er lagt til i databasen, blir brukeren omdirigert til en annen 
                //handling (action), i dette tilfellet en metode kalt Table
                return RedirectToAction(nameof(Table));
            }
        }
        _logger.LogWarning("[ItemController] Item creation failed {@item}", item);
        //Hvis ModelState.IsValid er false (f.eks. hvis skjemaet inneholder feil), vises skjemaet igjen 
        //med de inndataene brukeren allerede har fylt ut, slik at brukeren kan rette opp feilene.
        return View(item);
    }        

    //Denne metoden håndterer en GET-forespørsel for å vise et skjema som lar brukeren oppdatere et eksisterende element
    [HttpGet]
    public async Task<IActionResult> Update(int id) //Parameteren id representerer elementets ID i databasen som skal oppdateres.
    {
        var item = await _itemRepository.GetItemById(id);
        //Sjekker om elementet finnes i databasen
        if (item == null)
        {
            _logger.LogError("[ItemController] Item not found when updating the ItemId {ItemId:0000}", id);
            return BadRequest("Item not found for the ItemId");
        }
        //Hvis elementet finnes, returneres en visning som viser skjemaet for å oppdatere elementet. 
        //item blir sendt til View slik at det kan forfylles med eksisterende data.
        return View(item);
    }

    //Metoden mottar det oppdaterte Item-objektet via skjemaet
    [HttpPost]
    public async Task<IActionResult> Update(Item item)
    {
        //Sjekker om det innsendte objektet oppfyller valideringsreglene
        if (ModelState.IsValid)
        {
            bool returnOk = await _itemRepository.Update(item);
            if (returnOk)
            {
                //Etter en vellykket oppdatering omdirigeres brukeren til en liste (Table) som viser alle elementene
                return RedirectToAction(nameof(Table));
            }
        }
        _logger.LogWarning("[ItemController] Item update failed {@item}", item);
        //Hvis dataene er ugyldige (ModelState er ikke valid), vises skjemaet på nytt med de 
        //opprinnelige innsendte dataene og valideringsfeil
        return View(item);
    }

    //Denne metoden håndterer en GET-forespørsel for å vise en bekreftelsesside for sletting
    [HttpGet]
    public async Task<IActionResult> Delete(int id) //Parameteren id representerer elementets ID som skal slettes
    {
        var item = await _itemRepository.GetItemById(id);
        //Sjekker om elementet finnes i databasen
        if (item == null)
        {
            _logger.LogError("[ItemController] Item not found for the ItemId {ItemId:0000}", id);
            return BadRequest("Item not dound for the ItemId");
        }
        //Hvis elementet finnes, vises en bekreftelsesside 
        //med informasjon om elementet slik at brukeren kan bekrefte slettingen
        return View(item);
    }

    //Denne metoden håndterer POST-forespørselen som sendes inn når brukeren bekrefter slettingen. 
    //Den utfører selve slettingen i databasen
    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id) //Metoden mottar ID-en til elementet som skal slettes
    {
        bool returnOk = await _itemRepository.Delete(id);
        if (!returnOk)
        {
            _logger.LogError("[ItemController] Item deletion failed for the ItemId {ItemId:0000}", id);
            return BadRequest("Item deletion failed");
        }
        return RedirectToAction(nameof(Table));
    }
    
}

   