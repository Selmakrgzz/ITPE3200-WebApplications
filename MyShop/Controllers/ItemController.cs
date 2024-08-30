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

//Definerer et navnrom som organiserer koden under MyShop.Controllers
namespace MyShop.Controllers;

//Definerer en kontroller kalt 'ItemController', som arver fra 'Controller',
//en baseklasse i ASP.NET Core MVC
public class ItemController : Controller 
{
    //Definerer en metode som returnerer en 'IActionResult', vanligvis en HTML-side
    //eller en JSON-data. Denne metoden vil bli kjørt når brukeren navigerer til en
    //spesifikk URL som er knyttet til denne handlingen
    public IActionResult Table()
    {
        //Her opprettes det en ny liste av typen 'Item'. 
        //'List<Item>' er en generisk samling som kan inneholde
        //objekter av typen 'Item'
        var items = new List<Item>();
        //Oppretter et nytt 'Item' objekt iom at Item er en modellklasse i MyShop.Models
        var item1 = new Item();
        //Setter egenskapene i item1
        item1.ItemId = 1;
        item1.Name = "Pizza";
        item1.Price = 60;

        //Her oppretter vi et Item objekt til, men på en litt anerledes måte
        var item2 = new Item
        {
            ItemId = 2,
            Name = "Fried Chicken Leg",
            Price = 15
        };

        //Legger til de nye Items-objektene i items-listen
        items.Add(item1);
        items.Add(item2);

        //Er et dynamisk objekt som brukes til å overføre data fra kontrolleren
        //til viewet. Her settes 'CurrentViewName' til en strengverdi
        ViewBag.CurrentViewName = "List of shop Items";
        //Returnerer viewet til klienten med 'items'-listen som modell. 
        //Viewet vil typisk presentere denne listen på en HTML-side.
        return View(items);
    }
}