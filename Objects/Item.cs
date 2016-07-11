using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Inventory
{
  public class Item
  {
    private int _id;
    private string _name;
    private string _type;

    public Item(string name, string type, int Id = 0)
    {
      _id = Id;
      _name = name;
      _type = type;
    }

    public int GetId()
    {
      return _id;
    }

    public string GetName()
    {
      return _name;
    }

    public string GetItemType()
    {
      return _type;
    }

    public static List<Item> GetAll()
    {
      List<Item> allItems = new List<Item>{};

      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM items;", conn);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int itemId = rdr.GetInt32(0);
        string itemName = rdr.GetString(1);
        string itemType = rdr.GetString(2);
        Item newItem = new Item(itemName, itemType, itemId);
        allItems.Add(newItem);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allItems;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM items;", conn);
      cmd.ExecuteNonQuery();
    }

    public override bool Equals(System.Object otherItem)
    {
      if (!(otherItem is Item))
      {
        return false;
      }
      else
      {
        Item newItem = (Item) otherItem;
        bool idEquality = (this.GetId() == newItem.GetId());
        bool nameEquality = (this.GetName() == newItem.GetName());
        bool typeEquality = (this.GetItemType() == newItem.GetItemType());
        return (idEquality && nameEquality && typeEquality);
      }
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO items (name, type) OUTPUT INSERTED.id VALUES (@ItemName, @ItemType);", conn);

      SqlParameter nameParameter = new SqlParameter();
      SqlParameter typeParameter = new SqlParameter();
      nameParameter.ParameterName = "@ItemName";
      typeParameter.ParameterName = "@ItemType";
      nameParameter.Value = this.GetName();
      typeParameter.Value = this.GetItemType();
      cmd.Parameters.Add(nameParameter);
      cmd.Parameters.Add(typeParameter);
      rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }


  }
}
