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
        return View["view_list.cshtml", allItems];
      };

      Get["/search"] = _ =>
      {
        List<Item> searchResults = Item.SearchItems(Request.Query["search-term"]);
        return View["view_list.cshtml", searchResults];
      };

      Get["/view_item/{id}"] = parameters =>
      {
        Item foundItem = Item.Find(parameters.id);
        return View["view_item.cshtml", foundItem];
      };

      Post["/delete_all"] = _ =>
      {
        Item.DeleteAll();
        return View["index.cshtml"];
      };
    }
  }

}
