using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace HairSalon.Models
{
  public class Stylist
  {
    private string _stylistName;
    private int _id;

    public Stylist(string stylistName, int id = 0)
    {
      _stylistName = stylistName;
      _id = id;
    }

    public string GetStylistName()
    {
      return _stylistName;
    }

    public int GetId()
    {
      return _id;
    }

    public override int GetHashCode()
    {
        return this.GetId().GetHashCode();
    }

    public static List<Stylist> GetAll()
    {
      List<Stylist> allStylists = new List<Stylist> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM stylists;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int StylistId = rdr.GetInt32(0);
        string StylistName = rdr.GetString(1);
        Stylist newStylist = new Stylist(StylistName, StylistId);
        allStylists.Add(newStylist);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allStylists;
    }

    public override bool Equals(System.Object otherStylist)
    {
      if (!(otherStylist is Stylist))
      {
        return false;
      }
      else
      {
        Stylist newStylist = (Stylist) otherStylist;
        bool idEquality = this.GetId() == newStylist.GetId();
        bool stylistNameEquality = this.GetStylistName() == newStylist.GetStylistName();
        return (idEquality && stylistNameEquality);
      }
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO stylists (stylistName) VALUES (@stylistName);";
      MySqlParameter stylistName = new MySqlParameter();
      stylistName.ParameterName = "@stylistName";
      stylistName.Value = this._stylistName;
      cmd.Parameters.Add(stylistName);
      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static void ClearAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM stylists;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static Stylist Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM stylists WHERE id = (@searchId);";
      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int stylistId = 0;
      string stylistName = "";
      while(rdr.Read())
      {
        stylistId = rdr.GetInt32(0);
        stylistName = rdr.GetString(1);
      }
      Stylist newClient = new Stylist(stylistName, stylistId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newClient;
    }

    public List<Client> GetClients()
    {
      List<Client> allStylistClients = new List<Client> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM clients WHERE stylist_id = @stylistId;";
      MySqlParameter stylistId = new MySqlParameter();
      stylistId.ParameterName = "@stylistId";
      stylistId.Value = this._id;
      cmd.Parameters.Add(stylistId);
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int clientId = rdr.GetInt32(0);
        string clientName = rdr.GetString(1);
        int clientStylistId = rdr.GetInt32(2);
        Client newClient = new Client(clientName, clientStylistId, clientId);
        allStylistClients.Add(newClient);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allStylistClients;
    }

    public void Edit(string newStylist)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE stylists SET stylistName = @newStylist WHERE id = @searchId;";
      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);
      MySqlParameter stylistName = new MySqlParameter();
      stylistName.ParameterName = "@newStylist";
      stylistName.Value = newStylist;
      cmd.Parameters.Add(stylistName);
      cmd.ExecuteNonQuery();
      _stylistName = newStylist;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = new MySqlCommand("DELETE FROM stylists WHERE id = @stylistId; DELETE FROM clients WHERE id = @stylistId;", conn);
      MySqlParameter stylistIdParameter = new MySqlParameter();
      stylistIdParameter.ParameterName = "@stylistId";
      stylistIdParameter.Value = this.GetId();
      cmd.Parameters.Add(stylistIdParameter);
      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }

    public List<Specialty> GetSpecialties()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT specialtyId FROM stylists_specialties WHERE stylistId = @stylistId;";
      MySqlParameter stylistIdParameter = new MySqlParameter();
      stylistIdParameter.ParameterName = "@stylistId";
      stylistIdParameter.Value = _id;
      cmd.Parameters.Add(stylistIdParameter);
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<int> specialtyIds = new List<int> {};
      while(rdr.Read())
      {
        int specialtyId = rdr.GetInt32(0);
        specialtyIds.Add(specialtyId);
      }
      rdr.Dispose();
      List<Specialty> specialties = new List<Specialty> {};
      foreach (int specialtyId in specialtyIds)
      {
        var specialtyQuery = conn.CreateCommand() as MySqlCommand;
        specialtyQuery.CommandText = @"SELECT * FROM specialties WHERE id = @SpecialtyId;";
        MySqlParameter specialtyIdParameter = new MySqlParameter();
        specialtyIdParameter.ParameterName = "@SpecialtyId";
        specialtyIdParameter.Value = specialtyId;
        specialtyQuery.Parameters.Add(specialtyIdParameter);
        var specialtyQueryRdr = specialtyQuery.ExecuteReader() as MySqlDataReader;
        while(specialtyQueryRdr.Read())
        {
          int thisSpecialtyId = specialtyQueryRdr.GetInt32(0);
          string description = specialtyQueryRdr.GetString(1);
          Specialty foundSpecialty = new Specialty(description, thisSpecialtyId);
          specialties.Add(foundSpecialty);
        }
        specialtyQueryRdr.Dispose();
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return specialties;
    }

    public void AddSpecialty(Specialty newSpecialty)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO stylists_specialties (stylistId, specialtyId) VALUES (@StylistId, @SpecialtyId);";
      MySqlParameter stylistId = new MySqlParameter();
      stylistId.ParameterName = "@StylistId";
      stylistId.Value = _id;
      cmd.Parameters.Add(stylistId);
      MySqlParameter specialtyId = new MySqlParameter();
      specialtyId.ParameterName = "@SpecialtyId";
      specialtyId.Value = newSpecialty.GetId();
      cmd.Parameters.Add(specialtyId);
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
  }
}
