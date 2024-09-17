using Microsoft.AspNetCore.Mvc.Rendering;
using MyShop.Models;

namespace MyShop.ViewModels;

//Dette er en ViewModel som brukes til å overføre data mellom kontrolleren og visningen
public class CreateOrderItemViewModel
{
    //Definerer en public OrderItem, som representerer et enkelt ordre-element (av typen OrderItem).
    //default! setter standardverdien, men her indikeres at verdien vil bli satt senere
    public OrderItem OrderItem { get; set; } = default!;
    //Definerer en public ItemSelectList, som er en liste over SelectListItem.
    //Dette brukes for å lage en nedtrekksmeny av Items
    public List<SelectListItem> ItemSelectList { get; set; } = default!;
    //Definerer en public OrderSelectList, som er en liste over SelectListItem.
    //Dette brukes for å lage en nedtrekksmeny av Order
    public List<SelectListItem> OrderSelectList { get; set; } = default!;
}
