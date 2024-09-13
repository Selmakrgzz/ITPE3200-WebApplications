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
    public IActionResult Table()
    {
        var items = GetItems();
        var itemsViewModel = new ItemsViewModel(items, "Table");
        return View(itemsViewModel);
    }

    public IActionResult Grid()
    {
        var items = GetItems();
        var itemsViewModel = new ItemsViewModel(items, "Grid");
        return View(itemsViewModel);
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
    }*/
    
    public IActionResult Details(int id)
    {
        //Henter listen over varer i GetItems-metoden
        var items = GetItems();
        //Finner varen med den spesifikke ID-en
        var item = items.FirstOrDefault(i => i.ItemId == id);
        //Hvis varen ikke finnes, returner en 404
        if (item == null)
            return NotFound();
            //Returner visningen med detaljene for varen
        return View(item);
    }

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
    }
}