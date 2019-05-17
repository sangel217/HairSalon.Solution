using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using HairSalon.Models;

namespace HairSalon.Tests
{
  [TestClass]
  public class SpecialtyTest : IDisposable
  {
    public SpecialtyTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=sarah_angel_test;";
    }

    public void Dispose()
    {
      Specialty.ClearAll();
    }

    [TestMethod]
    public void SpecialtyConstructor_CreatesInstanceOfSpecialty_Specialty()
    {
      string description = "Test Specialty";
      Specialty newSpecialty = new Specialty(description);
      Assert.AreEqual(typeof(Specialty), newSpecialty.GetType());
    }

    [TestMethod]
    public void GetDescription_ReturnsDescription_String()
    {
      string description = "Test Specialty";
      Specialty newSpecialty = new Specialty(description);
      string actualResult = newSpecialty.GetDescription();
      Assert.AreEqual(description, actualResult);
    }

    [TestMethod]
    public void GetId_ReturnsSpecialtyId_Int()
    {
      string description = "Test Specialty";
      Specialty newSpecialty = new Specialty(description);
      int actualResult = newSpecialty.GetId();
      Assert.AreEqual(0, actualResult);
    }

    [TestMethod]
    public void Equals_ReturnsTrueIfDescriptionsAreTheSame_Specialty()
    {
      //Arrange, Act
      Specialty firstSpecialty = new Specialty("Test Specialty");
      Specialty secondSpecialty = new Specialty("Test Specialty");

      //Assert
      Assert.AreEqual(firstSpecialty, secondSpecialty);
    }

    [TestMethod]
    public void Save_SavesSpecialtyToDatabase_SpecialtyList()
    {
      //Arrange
      Specialty testSpecialty = new Specialty("Test Specialty");
      testSpecialty.Save();

      //Act
      List<Specialty> result = Specialty.GetAll();
      List<Specialty> testList = new List<Specialty>{testSpecialty};

      //Assert
      CollectionAssert.AreEqual(testList, result);
    }
    [TestMethod]
    public void Save_DatabaseAssignsIdToSpecialty_Id()
    {
      //Arrange
      Specialty testSpecialty = new Specialty("Test specialty");
      testSpecialty.Save();

      //Act
      Specialty savedSpecialty = Specialty.GetAll()[0];

      int result = savedSpecialty.GetId();
      int testId = testSpecialty.GetId();

      //Assert
      Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void GetAll_SpecialtysEmptyAtFirst_List()
    {
      //Arrange, Act
      int result = Specialty.GetAll().Count;

      //Assert
      Assert.AreEqual(0, result);
    }

    [TestMethod]
     public void GetAll_ReturnsAllSpecialtyObjects_SpecialtyList()
     {
         string description01 = "Book";
         string description02 = "Book2";
         Specialty newSpecialty1 = new Specialty(description01);
         newSpecialty1.Save();
         Specialty newSpecialty2 = new Specialty(description02);
         newSpecialty2.Save();
         List<Specialty> newList = new List<Specialty> { newSpecialty1, newSpecialty2 };

         List<Specialty> actualResult = Specialty.GetAll();
         CollectionAssert.AreEqual(newList, actualResult);
     }

     [TestMethod]
     public void Find_ReturnsSpecialtyInDatabase_Specialty()
     {
       //Arrange
       Specialty testSpecialty = new Specialty("Test Specialty");
       testSpecialty.Save();

       //Act
       Specialty foundSpecialty = Specialty.Find(testSpecialty.GetId());

       //Assert
       Assert.AreEqual(testSpecialty, foundSpecialty);
     }

     [TestMethod]
     public void Delete_DeletesSpecialtyFromDatabase_Specialty()
     {
       //Arrange
       string testDescription = "Test Specialty";
       Specialty testSpecialty = new Specialty(testDescription);
       testSpecialty.Save();

       //Act
       Specialty foundSpecialty = Specialty.Find(testSpecialty.GetId());
       foundSpecialty.Delete();
       List<Specialty> newList = new List<Specialty>{};
       List<Specialty> testList = Specialty.GetAll();
       //Assert
       CollectionAssert.AreEqual(newList, testList);
     }

     [TestMethod]
     public void Edit_UpdatesSpecialtyInDatabase_String()
     {
       Specialty testSpecialty = new Specialty("Test Specialty");
       testSpecialty.Save();
       string secondDescription = "Updated Specialty";

       testSpecialty.Edit(secondDescription);
       string result = Specialty.Find(testSpecialty.GetId()).GetDescription();

       Assert.AreEqual(secondDescription, result);
     }
  }
}
