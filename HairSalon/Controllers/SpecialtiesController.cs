using Microsoft.AspNetCore.Mvc;
using HairSalon.Models;
using System.Collections.Generic;

namespace HairSalon.Controllers
{
  public class SpecialtiesController : Controller
  {

    [HttpGet("/specialties")]
    public ActionResult Index()
    {
      List<Specialty> allSpecialties = Specialty.GetAll();
      return View(allSpecialties);
    }

    [HttpGet("/specialties/new")]
    public ActionResult New()
    {
      return View();
    }

    [HttpPost("/specialties")]
    public ActionResult Create(string description)
    {
      Specialty newSpecialty = new Specialty(description);
      newSpecialty.Save();
      List<Specialty> allSpecialties = Specialty.GetAll();
      return View("Index", allSpecialties);
    }

    [HttpPost("/specialties/delete")]
    public ActionResult DeleteAll()
    {
      Specialty.ClearAll();
      return View();
    }

    [HttpGet("/specialties/{id}")]
    public ActionResult Show(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Specialty selectedSpecialty = Specialty.Find(id);
      List<Stylist> specialtyStylists = selectedSpecialty.GetStylists();
      List<Stylist> allStylists = Stylist.GetAll();
      model.Add("selectedSpecialty", selectedSpecialty);
      model.Add("specialtyStylists", specialtyStylists);
      model.Add("allStylists", allStylists);
      return View(model);
    }


    [HttpGet("/stylists/{stylistId}/specialties/{specialtyId}/edit")]
    public ActionResult Edit(int stylistId, int specialtyId)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Stylist stylist = Stylist.Find(stylistId);
      model.Add("stylist", stylist);
      Specialty specialty = Specialty.Find(specialtyId);
      model.Add("specialty", specialty);
      return View(model);
    }

    [HttpPost("/stylists/{stylistId}/specialties/{specialtyId}")]
    public ActionResult Update(int stylistId, int specialtyId, string newDescription)
    {
      Specialty specialty = Specialty.Find(specialtyId);
      specialty.Edit(newDescription);
      Dictionary<string, object> model = new Dictionary<string, object>();
      Stylist stylist = Stylist.Find(stylistId);
      model.Add("stylist", stylist);
      model.Add("specialty", specialty);
      return View("Show", model);
    }

    [HttpPost("/specialties/{specialtyId}/delete")]
    public ActionResult Delete(int specialtyId)
    {
      Specialty specialty = Specialty.Find(specialtyId);
      specialty.Delete();
      return RedirectToAction("Index");
    }

    [HttpPost("/specialties/{specialtyId}/stylists/new")]
    public ActionResult AddStylists(int specialtyId, int stylistId)
    {
      Specialty specialty = Specialty.Find(specialtyId);
      Stylist stylist = Stylist.Find(stylistId);
      specialty.AddStylists(stylist);
      return RedirectToAction("Show",  new { id = specialtyId });
    }
  }
}
