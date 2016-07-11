using Nancy;
using System;
using System.Collections.Generic;

namespace Inventory
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => View["index.cshtml"];

      Post["/add_item"] = _ =>
      {
        string itemName = Request.Form["item-name"];
        string itemType = Request.Form["item-type"];
        Item newItem = new Item(itemName, itemType);
        newItem.Save();
        return View["index.cshtml"];
      };

      Get["/add_item"] = _ => View["/add_item.cshtml"];

      Get["/view_all"] = _ =>
      {
        List<Item> allItems = Item.GetAll();
        return View["view_all.cshtml", allItems];
      };

      Get["/search"] = _ =>
      {
        Item foundItem = Item.FindByName(Request.Query["search-term"]);
        return View["view_item.cshtml", foundItem];
      };

      Get["/view_item/{id}"] = parameters =>
      {
        Item foundItem = Item.Find(parameters.id);
        return View["view_item.cshtml", foundItem];
      };
    }
  }

}
