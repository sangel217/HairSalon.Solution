using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace HairSalon.Models
{
  public class Client
  {
    private string _clientName;
    private int _id;
    private int _stylistId;

    public Client(string clientName, int stylistId, int id = 0)
    {
      _clientName = clientName;
      _stylistId = stylistId;
      _id = id;
    }

    public string GetClientName()
    {
      return _clientName;
    }

    public int GetId()
    {
      return _id;
    }

    public int GetStylistId()
    {
      return _stylistId;
    }

    public override int GetHashCode()
    {
      return this.GetId().GetHashCode();
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
          int clientStylistId = rdr.GetInt32(2);
          Client newClient = new Client(clientName, clientStylistId, clientId);
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
        bool stylistEquality = this.GetStylistId() == newClient.GetStylistId();
        return (idEquality && clientNameEquality && stylistEquality);
      }
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO clients (clientName, stylist_id) VALUES (@clientName, @stylistId);";
      MySqlParameter clientName = new MySqlParameter();
      clientName.ParameterName = "@clientName";
      clientName.Value = this._clientName;
      cmd.Parameters.Add(clientName);
      MySqlParameter stylistId = new MySqlParameter();
      stylistId.ParameterName = "@stylistId";
      stylistId.Value = this._stylistId;
      cmd.Parameters.Add(stylistId);
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
      int clientStylistId = 0;
      while(rdr.Read())
      {
        clientId = rdr.GetInt32(0);
        clientName = rdr.GetString(1);
        clientStylistId = rdr.GetInt32(2);
      }
      Client newClient = new Client(clientName, clientStylistId, clientId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newClient;
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = new MySqlCommand("DELETE FROM clients WHERE id = @clientNameId;", conn);
      MySqlParameter clientNameIdParameter = new MySqlParameter();
      clientNameIdParameter.ParameterName = "@clientNameId";
      clientNameIdParameter.Value = this.GetId();
      cmd.Parameters.Add(clientNameIdParameter);
      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }

    public void Edit(string newClientName)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE clients SET clientName = @newClientName WHERE id = @searchId;";
      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);
      MySqlParameter clientName = new MySqlParameter();
      clientName.ParameterName = "@newClientName";
      clientName.Value = newClientName;
      cmd.Parameters.Add(clientName);
      cmd.ExecuteNonQuery();
      _clientName = newClientName;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

    }
  }
}
