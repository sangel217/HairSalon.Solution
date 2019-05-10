using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HairSalon.Controllers;
using HairSalon.Models;

namespace HairSalon.Tests
{
    [TestClass]
    public class HomeControllerTest
    {

      [TestMethod]
      public void Index_ReturnsCorrectView_True ()
      {
        HomeController controller = new HomeController ();
        ActionResult indexView = controller.Index ();
        Assert.IsInstanceOfType (indexView, typeof (ViewResult));
      }
    }
}
