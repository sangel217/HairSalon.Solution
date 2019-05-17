using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using HairSalon.Models;

namespace HairSalon.Tests
{
    [TestClass]
    public class StylistTest : IDisposable
    {
      public void Dispose()
      {
        Stylist.ClearAll();
      }
      public StylistTest()
      {
        DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=sarah_angel_test;";
      }

      [TestMethod]
      public void StylistConstructor_CreatesInstanceOfStylist_Stylist()
      {
        string stylistName = "Test Stylist";
        Stylist newStylist = new Stylist(stylistName);
        Assert.AreEqual(typeof(Stylist), newStylist.GetType());
      }

      [TestMethod]
      public void GetStylistName_ReturnsStylistName_String()
      {
        string stylistName = "Test Stylist";
        Stylist newStylist = new Stylist(stylistName);
        string actualResult = newStylist.GetStylistName();
        Assert.AreEqual(stylistName, actualResult);
      }

      [TestMethod]
      public void GetId_ReturnsStylistId_Int()
      {
        string name = "Test Stylist";
        Stylist newStylist = new Stylist(name);
        int actualResult = newStylist.GetId();
        Assert.AreEqual(0, actualResult);
      }

      [TestMethod]
      public void GetAll_ReturnsEmptyListFromDatabase_StylistList()
      {
        List<Stylist> newList = new List<Stylist> { };
        List<Stylist> result = Stylist.GetAll();
        CollectionAssert.AreEqual(newList, result);
      }

      [TestMethod]
      public void Equals_ReturnsTrueIfStylistNamesAreTheSame_Stylist()
      {
        Stylist firstStylist = new Stylist("sarah", 1);
        Stylist secondStylist = new Stylist("sarah", 1);
        Assert.AreEqual(firstStylist, secondStylist);
      }

      [TestMethod]
      public void Save_SavesToDatabase_StylistList()
      {
        Stylist testStylist = new Stylist("sarah", 1);
        testStylist.Save();
        List<Stylist> result = Stylist.GetAll();
        List<Stylist> testList = new List<Stylist>{testStylist};
        CollectionAssert.AreEqual(testList, result);
      }

      [TestMethod]
      public void GetAll_ReturnsStylists_StylistList()
      {
        string stylistName01 = "sarah";
        string stylistName02 = "becky";
        Stylist newStylist1 = new Stylist(stylistName01, 1);
        Stylist newStylist2 = new Stylist(stylistName02, 1);
        newStylist1.Save();
        newStylist2.Save();
        List<Stylist> newList = new List<Stylist> { newStylist1, newStylist2 };
        List<Stylist> result = Stylist.GetAll();
        CollectionAssert.AreEqual(newList, result);
      }

      [TestMethod]
      public void Save_AssignsIdToObject_Id()
      {
        Stylist testStylist = new Stylist("sheryl", 1);
        testStylist.Save();
        Stylist savedStylist = Stylist.GetAll()[0];
        int result = savedStylist.GetId();
        int testId = testStylist.GetId();
        Assert.AreEqual(testId, result);
      }

      [TestMethod]
      public void Find_ReturnsCorrectStylistFromDatabase_Stylist()
      {
        Stylist testStylist = new Stylist("sarah", 1);
        testStylist.Save();
        Stylist foundStylist = Stylist.Find(testStylist.GetId());
        Assert.AreEqual(testStylist, foundStylist);
      }

      [TestMethod]
      public void Edit_UpdatesStylistInDatabase_String()
      {
        Stylist testStylist = new Stylist("Test Stylist");
        testStylist.Save();
        string secondStylist = "Updated Stylist";

        testStylist.Edit(secondStylist);
        string result = Stylist.Find(testStylist.GetId()).GetStylistName();

        Assert.AreEqual(secondStylist, result);
      }

      [TestMethod]
      public void Delete_DeletesStylistFromDatabase_Stylist()
      {
        //Arrange
        string testStylistName = "Test Stylist";
        Stylist testStylist = new Stylist(testStylistName);
        testStylist.Save();

        //Act
        Stylist foundStylist = Stylist.Find(testStylist.GetId());
        foundStylist.Delete();
        List<Stylist> newList = new List<Stylist>{};
        List<Stylist> testList = Stylist.GetAll();
        // Stylist resultStylists = new Stylist("");

        //Assert
        CollectionAssert.AreEqual(newList, testList);
      }


    }
}
