using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProyectoWeb.Models;

namespace ProyectoWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Ayuda()
        {
            ViewBag.NombreEstudiante = "Mónica Sofia Boche Figueroa"; 
            ViewBag.Carnet = "202300379";                     
            ViewBag.LinkDoc = "https://github.com/SofiaBoche/IPC2_Proy02_202602_202300379.git"; 

            return View();
        }


        [HttpPost]
        public IActionResult InicializarSistema()
        {
            CargaController.JerarquiaCategorias = new Estructuras.ArbolCategorias();
            CargaController.LibrosGlobales = new Estructuras.ArbolAVL();

            TempData["Mensaje"] = "El sistema ha sido reiniciado a cero correctamente.";
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}