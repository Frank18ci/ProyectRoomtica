using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoomticaFrontEnd.Models;
using RoomticaGrpcServiceBackEnd;
using Google.Protobuf.WellKnownTypes;
namespace RoomticaFrontEnd.Controllers
{
    public class TipoHabitacionController : Controller
    {
        private TipoHabitacionService.TipoHabitacionServiceClient? tipoHabitacionService;
        public async Task<ActionResult> Listar()
        {
            var chanal = GrpcChannel.ForAddress("http://localhost:5225");
            tipoHabitacionService = new TipoHabitacionService.TipoHabitacionServiceClient(chanal);
            var request = new Empty();
            var mensaje = await tipoHabitacionService.GetAllAsync(request);
            List<TipoHabitacionModel> tipoHabitacionModels = new List<TipoHabitacionModel>();
            foreach(var item in mensaje.TipoHabitaciones_)
            {
                tipoHabitacionModels.Add(new TipoHabitacionModel()
                {
                    Id = item.Id,
                    Tipo = item.Tipo,
                    descripccion = item.Descripccion
                });
            }
            return View(tipoHabitacionModels);
        }
        // GET: TipoHabitacionController
        public ActionResult Index()
        {
            return View();
        }

        // GET: TipoHabitacionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TipoHabitacionController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipoHabitacionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TipoHabitacionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TipoHabitacionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: TipoHabitacionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TipoHabitacionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
