using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace HairSalon.Models
{
  public class Specialty
  {
    private string _description;
    private int _id;

    public Specialty(string description, int id = 0)
    {
      _description = description;
      _id = id;
    }

    public string GetDescription()
    {
      return _description;
    }

    public int GetId()
    {
      return _id;
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
      cmd.CommandText = @"DELETE FROM specialties;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Specialty> GetAll()
    {
      List<Specialty> allSpecialties = new List<Specialty> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM specialties;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int specialtyId = rdr.GetInt32(0);
        string description = rdr.GetString(1);
        Specialty newSpecialty = new Specialty(description, specialtyId);
        allSpecialties.Add(newSpecialty);
      }
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return allSpecialties;
    }

    public override bool Equals(System.Object otherSpecialty)
    {
      if (!(otherSpecialty is Specialty))
      {
        return false;
      }
      else
      {
        Specialty newSpecialty = (Specialty) otherSpecialty;
        bool idEquality = this.GetId() == newSpecialty.GetId();
        bool descriptionEquality = this.GetDescription() == newSpecialty.GetDescription();
        return (idEquality && descriptionEquality);
      }
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO specialties (description) VALUES (@description);";
      MySqlParameter description = new MySqlParameter();
      description.ParameterName = "@description";
      description.Value = this._description;
      cmd.Parameters.Add(description);
      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static Specialty Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM specialties WHERE id = (@searchId);";
      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int specialtyId = 0;
      string description = "";
      while(rdr.Read())
      {
        specialtyId = rdr.GetInt32(0);
        description = rdr.GetString(1);
      }
      Specialty newSpecialty = new Specialty(description, specialtyId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newSpecialty;
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = new MySqlCommand("DELETE FROM specialties WHERE id = @descriptionId;", conn);
      MySqlParameter descriptionIdParameter = new MySqlParameter();
      descriptionIdParameter.ParameterName = "@descriptionId";
      descriptionIdParameter.Value = this.GetId();
      cmd.Parameters.Add(descriptionIdParameter);
      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }

    public void Edit(string newDescription)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE specialties SET description = @newDescription WHERE id = @searchId;";
      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);
      MySqlParameter description = new MySqlParameter();
      description.ParameterName = "@newDescription";
      description.Value = newDescription;
      cmd.Parameters.Add(description);
      cmd.ExecuteNonQuery();
      _description = newDescription;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public List<Stylist> GetStylists()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT stylistId FROM stylists_specialties WHERE specialtyId = @specialtyId;";
      MySqlParameter specialtyIdParameter = new MySqlParameter();
      specialtyIdParameter.ParameterName = "@specialtyId";
      specialtyIdParameter.Value = _id;
      cmd.Parameters.Add(specialtyIdParameter);
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<int> stylistIds = new List<int> {};
      while(rdr.Read())
      {
        int stylistId = rdr.GetInt32(0);
        stylistIds.Add(stylistId);
      }
      rdr.Dispose();
      List<Stylist> stylists = new List<Stylist> {};
      foreach (int stylistId in stylistIds)
      {
        var stylistQuery = conn.CreateCommand() as MySqlCommand;
        stylistQuery.CommandText = @"SELECT * FROM stylists WHERE id = @StylistId;";
        MySqlParameter stylistIdParameter = new MySqlParameter();
        stylistIdParameter.ParameterName = "@StylistId";
        stylistIdParameter.Value = stylistId;
        stylistQuery.Parameters.Add(stylistIdParameter);
        var stylistQueryRdr = stylistQuery.ExecuteReader() as MySqlDataReader;
        while(stylistQueryRdr.Read())
        {
          int thisStylistId = stylistQueryRdr.GetInt32(0);
          string stylistName = stylistQueryRdr.GetString(1);
          Stylist foundStylist = new Stylist(stylistName, thisStylistId);
          stylists.Add(foundStylist);
        }
        stylistQueryRdr.Dispose();
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return stylists;
    }

    public void AddStylist(Stylist newStylist)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      //this adds an entry to the categories_items join table containg IDs for relevant Specialty and Stylist objects
      cmd.CommandText = @"INSERT INTO stylists_specialties (stylistId, specialtyId) VALUES (@StylistId, @SpecialtyId);";
      MySqlParameter stylistId = new MySqlParameter();
      stylistId.ParameterName = "@StylistId";
      stylistId.Value = newStylist.GetId();
      cmd.Parameters.Add(stylistId);
      MySqlParameter specialtyId = new MySqlParameter();
      specialtyId.ParameterName = "@SpecialtyId";
      specialtyId.Value = _id;
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
