using Microsoft.AspNetCore.Mvc;
using PrestamosWeb.Models;
using PrestamosWeb.Repositories;

namespace PrestamosWeb.Controllers
{
    public class PrestamosController : Controller
    {
        private readonly PrestamoRepository _repo;

        public PrestamosController(IConfiguration configuration)
        {
            _repo = new PrestamoRepository(configuration);
        }

        public IActionResult Index()
        {
            var lista = _repo.Listar();
            return View(lista);
        }


        [HttpPost]
        public IActionResult Create(Prestamo model)
        {
            _repo.Insertar(model);

            _repo.RegistrarHistorico(0, "INSERT",
                "Se creó un nuevo préstamo");

            TempData["success"] = "Préstamo guardado correctamente";

            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            ViewBag.Empleados = _repo.ListarEmpleados();
            ViewBag.Autoriza = _repo.ListarEmpleados();
            ViewBag.TiposPago = _repo.ListarTiposPago();

            return View();
        }
        public IActionResult Edit(int id)
        {
            var prestamo = _repo.Buscar(id);

            ViewBag.Empleados = _repo.ListarEmpleados();
            ViewBag.Autoriza = _repo.ListarEmpleados();
            ViewBag.TiposPago = _repo.ListarTiposPago();

            return View(prestamo);
        }


        [HttpPost]
        public IActionResult Edit(Prestamo model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Empleados = _repo.ListarEmpleados();
                ViewBag.Autoriza = _repo.ListarEmpleados();
                ViewBag.TiposPago = _repo.ListarTiposPago();
                return View(model);
            }

            // 🔥 obtener estado anterior (opcional si quieres detalle)
            var anterior = _repo.Buscar(model.IdPrestamo);

            _repo.Actualizar(model);

            _repo.RegistrarHistorico(model.IdPrestamo,"UPDATE",$"Se actualizó el préstamo. Saldo: {anterior.Saldo} → {model.Saldo}");
            TempData["success"] = "Préstamo actualizado correctamente";

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            _repo.Eliminar(id);

            _repo.RegistrarHistorico(
                id,
                "DELETE",
                "Se eliminó el préstamo"
            );

            TempData["success"] = "Registro eliminado correctamente";

            return RedirectToAction("Index");
        }
        public IActionResult Details(int id)
        {
            var prestamo = _repo.Buscar(id);
            var historial = _repo.ListarHistorico(id);

            ViewBag.Historial = historial;

            return View(prestamo);
        }

        public IActionResult Dashboard()
        {
            var dashboard = _repo.ObtenerDashboard();
            return View(dashboard);
        }
        
    }
}