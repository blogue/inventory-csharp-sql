using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Inventory
{
  public class InventoryTest : IDisposable
  {
    public InventoryTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=inventory_test;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      Item.DeleteAll();
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      int result = Item.GetAll().Count;

      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfDescriptionsAreTheSame()
    {
      Item firstItem = new Item("Coffee mug", "dishes");
      Item secondItem = new Item("Coffee mug", "dishes");

      Assert.Equal(firstItem, secondItem);
    }

    [Fact]
    public void Test_Save_SavesToDatabase()
    {
      Item testItem = new Item("Coffee mug", "cups");

      testItem.Save();
      List<Item> result = Item.GetAll();
      List<Item> testList = new List<Item>{testItem};

      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToObject()
    {
      Item testItem = new Item("Coffee mug", "cup");

      testItem.Save();
      Item savedItem = Item.GetAll()[0];

      int result = savedItem.GetId();
      int testId = testItem.GetId();

      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsItemInDatabase()
    {
      Item testItem = new Item("Coffee mug", "cup");
      testItem.Save();

      Item foundItem = Item.Find(testItem.GetId());

      Assert.Equal(testItem, foundItem);
    }

    [Fact]
    public void Test_FindByName_FindItemInDatabase()
    {
      Item testItem = new Item("Coffee mug", "cup");
      testItem.Save();

      Item foundItem = Item.FindByName(testItem.GetName());

      Assert.Equal(testItem, foundItem);
    }

    [Fact]
    public void Test_SearchItems_FindItemInDatabase()
    {
      Item testItem = new Item("Coffee mug", "cup");
      testItem.Save();

      List<Item> foundItems = Item.SearchItems("coffee");

      Assert.Equal(testItem, foundItems[0]);
    }
  }
}
