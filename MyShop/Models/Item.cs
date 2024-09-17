//Gir tilgang til grunnleggende typer og funksjonalitet som er en del av .NET bibloteket
//For eksempel String og decimal
using System;
//Dette definerer et navnrom kalt MyShop.Models. Navnrom brukes for å organisere og strukturere koden
//Det hjelper med å unngå navnkonflikter mellom forskjellige klasser i større prosjekter
//Altså, at koden tilhører Models mappen
namespace MyShop.Models
{
    public class Item
    {
        //Automatisk generert property med type int, som representerer en unik id for Item-objektet.
        //get set betyr at vedien kan både leses og skrives
        public int ItemId { get; set; } 
        //Dette er en property med type string som representerer navnet på elementet "= string.Empty;"
        //Det angir at "Name" skal være en tom streng, altså "". Merk: "Name" propertyen er en obligatorisk
        //verdi. Dette betyr at den ikke kan ha verdien null. Vi setter derfor en standardverdi string.Empty
        //hvis det ikke står noe verdi i "Name" så er den bare tom ("") og ikke null
        public String Name { get; set; } = string.Empty;
        //Dette er en property med type decimal som representerer prisen på elementet
        public decimal Price { get; set; }
        //Dette er en property med type string som representerer beskrivelsen på elementet
        //? etter string indikerer at denne propertyen er nullable, altså den kan ha verdien 'null'
        public string? Description { get; set; }
        //Dette er en property med type string. I dette tilfelle så representerer navnet ImageUrl en URL
        //som peker til et bilde relatert til varen
        public string? ImageUrl { get; set; }
        //navigation property
        //A navigation property in an entity class represents a relationship between entities and 
        //allows you to navigate from one entity to related entities.
        public virtual List<OrderItem>? OrderItems { get; set; }
    }
}