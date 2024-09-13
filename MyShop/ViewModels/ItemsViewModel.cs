using MyShop.Models;

namespace MyShop.ViewModels
{
    //Denne ViewModel brukes for å transportere data mellom controller og view (visning)
    public class ItemsViewModel
    {
        //En liste av 'Item'-objekter som representerer elementene som skal vises i visningen.
        public IEnumerable<Item> Items;
        //Denne brukes for å vise dynamisk informasjon om hvilken visning som er aktiv (f.eks. Grid eller Table View).
        public string? CurrentViewName;

        // Konstruktør for ItemsViewModel som initialiserer 'Items' og 'CurrentViewName'.
        // Konstruktøren krever en liste av 'Item'-objekter og navnet på den aktive visningen.
        public ItemsViewModel(IEnumerable<Item> items, string? currentViewName)
        {
            //Setter feltet 'Items' til verdien som blir passert inn i konstruktøren.
            Items = items;
            //Setter feltet 'CurrentViewName' til navnet på den aktive visningen som blir passert inn.
            CurrentViewName = currentViewName;
        }
    }
}
