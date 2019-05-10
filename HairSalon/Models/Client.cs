using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace HairSalon.Models
{
  public class Client
  {
    private string _clientName;
    private int _id;

    public Client(string clientName, int id = 0)
    {
      _clientName = clientName;
      _id = id;
    }

    public string GetClientName()
    {
      return _clientName;
    }

    public void SetClientName(string newClientName)
    {
      _clientName = newClientName;
    }

    public int GetId()
    {
      return _id;
    }

    public static void ClearAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM clients;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Client> GetAll()
    {
        List<Client> allClients = new List<Client> {};
        MySqlConnection conn = DB.Connection();
        conn.Open();
        MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM clients;";
        MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
        while(rdr.Read())
        {
          int clientId = rdr.GetInt32(0);
          string clientName = rdr.GetString(1);
          Client newClient = new Client(clientName, clientId);
          allClients.Add(newClient);
        }
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
        return allClients;
    }

    public override bool Equals(System.Object otherClient)
    {
      if (!(otherClient is Client))
      {
        return false;
      }
      else
      {
        Client newClient = (Client) otherClient;
        bool idEquality = this.GetId() == newClient.GetId();
        bool clientNameEquality = this.GetClientName() == newClient.GetClientName();
        return (idEquality && clientNameEquality);
      }
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO clients (clientName) VALUES (@clientName);";
      MySqlParameter clientName = new MySqlParameter();
      clientName.ParameterName = "@clientName";
      clientName.Value = this._clientName;
      cmd.Parameters.Add(clientName);
      // MySqlParameter categoryId = new MySqlParameter();
      // categoryId.ParameterName = "@category_id";
      // categoryId.Value = this._categoryId;
      // cmd.Parameters.Add(categoryId);
      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static Client Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM clients WHERE id = (@searchId);";
      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int clientId = 0;
      string clientName = "";
      //int clientCategoryId = 0;
      while(rdr.Read())
      {
        clientId = rdr.GetInt32(0);
        clientName = rdr.GetString(1);
        //clientCategoryId = rdr.GetInt32(2);
      }
      Client newClient = new Client(clientName, /*clientCategoryId,*/ clientId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newClient;
    }
  }
}
