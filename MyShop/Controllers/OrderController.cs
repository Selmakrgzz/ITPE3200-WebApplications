using Microsoft.AspNetCore.Mvc;
using MyShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MyShop.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyShop.DAL;

namespace MyShop.Controllers;

public class OrderController : Controller
{
    /*Et felt i kontrolleren som brukes for å få tilgang til databasen via 
    ItemDbContext. readonly betyr at dette feltet kun kan settes i konstruktøren 
    og kan ikke endres etterpå*/
    private readonly ItemDbContext _itemDbContext;

    public OrderController(ItemDbContext itemDbContext)
    {
        _itemDbContext = itemDbContext;
    }

    /*Table er en metode (ofte referert til som en "action") som vil 
    bli kalt når brukeren gjør en HTTP-forespørsel til en bestemt rute.
    async tillater metoden å kjøre asynkront for å forbedre ytelsen*/
    public async Task<IActionResult> Table()
    {
        /*Denne linjen henter alle rader fra Orders-tabellen i databasen ved 
        hjelp av Entity Framework, og resultatet blir en liste med Order-objekter*/
        List<Order> orders = await _itemDbContext.Orders.ToListAsync();
        return View(orders);
    }

    //For displaying the view defined in CreateOrderItem.cshtml
    [HttpGet]
    public async Task<IActionResult> CreateOrderItem()
    {
        //retrieves the Items and Orders from the database and convert them to lists
        var items = await _itemDbContext.Items.ToListAsync();
        var orders = await _itemDbContext.Orders.ToListAsync();
        //creates a new CreateOrderItemViewModel, which has three member variables:
        //OrderItem, ItemSelectList, and OrderSelectList
        var createOrderItemViewModel = new CreateOrderItemViewModel
        {
            //creates an empty OrderItem
            OrderItem = new OrderItem(),

            //wraps the information of ItemIds and Item Names into the ItemSelectList
            ItemSelectList = items.Select(item => new SelectListItem
            {
                Value = item.ItemId.ToString(),
                Text = item.ItemId.ToString() + ": " + item.Name
            }).ToList(),

            //wraps the information of OrderIds, OrderDates, and Customer Names into the OrderSelectList
            OrderSelectList = orders.Select(order => new SelectListItem
            {
                Value = order.OrderId.ToString(),
                Text = "Order" + order.OrderId.ToString() + ", Date: " + order.OrderDate + ", Customer: " + order.Customer.Name
            }).ToList(),
        };
        return View(createOrderItemViewModel);
    }


    //For creating an OrderItem
    //If user input ItemId or OrderId that does not exist in the database,
    //then the OrderItem cannot be created successfully and has to be handled
    [HttpPost]
    public async Task<IActionResult> CreateOrderItem(OrderItem orderItem)
    {
        try
        {
            //We try to find the given OrderId and the ItemId in the database
            var newItem = _itemDbContext.Items.Find(orderItem.ItemId);
            var newOrder = _itemDbContext.Orders.Find(orderItem.OrderId);

            //If one of them cannot be found => throw a cannot find message
            if (newItem == null || newOrder == null)
            {
                return BadRequest("Item or Order not found.");
            }

            //If the Ids are found, we'll create a new OrderItem, calculate its
            //price and then add it to the database
            var newOrderItem = new OrderItem
            {
                ItemId = orderItem.ItemId,
                Item = newItem,
                Quantity = orderItem.Quantity,
                OrderId = orderItem.OrderId,
                Order = newOrder,
            };
            newOrderItem.OrderItemPrice = orderItem.Quantity * newOrderItem.Item.Price;

            _itemDbContext.OrderItems.Add(newOrderItem);
            await _itemDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Table));
        }
        catch
        {
            return BadRequest("OrderItem creation failed.");
        }
    }
}

