using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HairSalon.Controllers;
using HairSalon.Models;

namespace HairSalon.Tests
{
  [TestClass]
  public class ClientsControllerTests
  {
    [TestMethod]
    public void New_ReturnsCorrectView_True()
    {
      ClientsController controller = new ClientsController();
      ActionResult newView = controller.New(4);
      Assert.IsInstanceOfType(newView, typeof (ViewResult));

    }

    [TestMethod]
    public void Show_ReturnsCorrectView_True()
    {
      ClientsController controller = new ClientsController();
      ActionResult showView = controller.Show(1, 1);
      Assert.IsInstanceOfType(showView, typeof(ViewResult));
    }
  }
}
