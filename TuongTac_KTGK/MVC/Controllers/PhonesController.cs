using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class PhonesController : Controller
    {
        // GET: Phones
        //public ActionResult Index()
        //{
        //    return View();
        //}
        public ActionResult Index()
        {
            IEnumerable<Phone> empList;
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Phones").Result;
            empList = response.Content.ReadAsAsync<IEnumerable<Phone>>().Result;
            return View(empList);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HttpClient http = new HttpClient();
            http.BaseAddress = new Uri("http://localhost:54381/");
            var resource = http.GetAsync("api/Phones/" + id);
            var result = resource.Result;
            var read = result.Content.ReadAsAsync<Phone>();
            Phone phone = read.Result;
            return View(phone);
        }
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
                return View(new Phone());
            else
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Phones/" + id.ToString()).Result;
                return View(response.Content.ReadAsAsync<Phone>().Result);
            }
        }
        [HttpPost]
        public ActionResult AddOrEdit(Phone e)
        {
            if (e.Id == 0)
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Phones", e).Result;
                TempData["SuccessMessage"] = "Saved Successfully";
            }
            else
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("Phones/" + e.Id, e).Result;
                TempData["SuccessMessage"] = "Updated Successfully";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("Phones/" + id.ToString()).Result;
            TempData["SuccessMessage"] = "Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}