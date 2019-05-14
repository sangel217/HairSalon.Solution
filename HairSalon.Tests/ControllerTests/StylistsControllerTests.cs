using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HairSalon.Controllers;
using HairSalon.Models;

namespace HairSalon.Tests
{
  [TestClass]
  public class StylistControllerTest
  {
    [TestMethod]
    public void Index_HasCorrectModelType_StylistList()
    {
        //Arrange
        StylistsController controller = new StylistsController();
        ViewResult indexView = controller.Index() as ViewResult;

        //Act
        var result = indexView.ViewData.Model;

        //Assert
        Assert.IsInstanceOfType(result, typeof(List<Stylist>));
    }

    [TestMethod]
    public void Create_ReturnsCorrectActionType_RedirectToActionResult()
    {
      StylistsController controller = new StylistsController();
      IActionResult view = controller.Create("sarah");
      Assert.IsInstanceOfType(view, typeof(RedirectToActionResult));
    }

  }
}
