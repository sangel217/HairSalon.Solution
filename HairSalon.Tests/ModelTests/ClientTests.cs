using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using HairSalon.Models;

namespace HairSalon.Tests
{
  [TestClass]
  public class ClientTest : IDisposable
  {
    public void Dispose()
    {
      Client.ClearAll();
    }
    public ClientTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=sarah_angel_test;";
    }

    [TestMethod]
    public void ClientConstructor_CreatesInstanceOfClient_Client()
    {
      Client newClient = new Client("Test", 1);
      Assert.AreEqual(typeof(Client), newClient.GetType());
    }

    [TestMethod]
    public void GetClientName_ReturnsClientName_String()
    {
      string clientName = "sarah";
      Client newClient = new Client(clientName, 1);
      string result = newClient.GetClientName();
      Assert.AreEqual(clientName, result);
    }

    [TestMethod]
    public void GetId_ClientsInstantiateWithAnIdAndGetterReturns_Int()
    {
        string clientName = "sarah";
        Client newClient = new Client(clientName, 1);
        int result = newClient.GetId();
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void GetAll_ReturnsEmptyListFromDatabase_ClientList()
    {
      List<Client> newList = new List<Client> { };
      List<Client> result = Client.GetAll();
      CollectionAssert.AreEqual(newList, result);
    }

    [TestMethod]
    public void Equals_ReturnsTrueIfClientNamesAreTheSame_Client()
    {
      Client firstClient = new Client("sarah", 1);
      Client secondClient = new Client("sarah", 1);
      Assert.AreEqual(firstClient, secondClient);
    }

    [TestMethod]
    public void Save_SavesToDatabase_ClientList()
    {
      Client testClient = new Client("sarah", 0);
      testClient.Save();
      List<Client> result = Client.GetAll();
      List<Client> testList = new List<Client>{testClient};
      CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void GetAll_ReturnsClients_ClientList()
    {
      string clientName01 = "sarah";
      string clientName02 = "becky";
      Client newClient1 = new Client(clientName01, 0);
      Client newClient2 = new Client(clientName02, 0);
      newClient1.Save();
      newClient2.Save();
      List<Client> newList = new List<Client> { newClient1, newClient2 };
      List<Client> result = Client.GetAll();
      CollectionAssert.AreEqual(newList, result);
    }

    [TestMethod]
    public void Save_AssignsIdToObject_Id()
    {
      Client testClient = new Client("sheryl", 1);
      testClient.Save();
      Client savedClient = Client.GetAll()[0];
      int result = savedClient.GetId();
      int testId = testClient.GetId();
      Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void Find_ReturnsCorrectClientFromDatabase_Client()
    {
      Client testClient = new Client("sarah", 0);
      testClient.Save();
      Client foundClient = Client.Find(testClient.GetId());
      Assert.AreEqual(testClient, foundClient);
    }

    [TestMethod]
    public void Edit_UpdatesClientInDatabase_String()
    {
      Client testClient = new Client("Test Client", 1);
      testClient.Save();
      string secondClient = "Updated Client";

      testClient.Edit(secondClient);
      string result = Client.Find(testClient.GetId()).GetClientName();

      Assert.AreEqual(secondClient, result);
    }

    [TestMethod]
    public void Delete_DeletesClientFromDatabase_Client()
    {
      //Arrange
      string testClientName = "Test Client";
      Client testClient = new Client(testClientName, 1);
      testClient.Save();

      //Act
      Client foundClient = Client.Find(testClient.GetId());
      foundClient.Delete();
      List<Client> newList = new List<Client>{};
      List<Client> testList = Client.GetAll();

      //Assert
      CollectionAssert.AreEqual(newList, testList);
    }

  }
}
