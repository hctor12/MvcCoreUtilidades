using Microsoft.AspNetCore.Mvc;

namespace MvcCoreUtilidades.Controllers
{
    public class UploadFilesController : Controller
    {
        private IWebHostEnvironment hostEnvironment;

        public UploadFilesController(IWebHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SubirFile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SubirFile(IFormFile fichero)
        {
            string rootFolder = this.hostEnvironment.WebRootPath;
            string fileName = fichero.FileName;
            string path = Path.Combine(rootFolder, "uploads", fileName);

            using (Stream stream = new FileStream(path, FileMode.Create))
            {
                await fichero.CopyToAsync(stream);
            }
            ViewData["MENSAJE"] = "Fichero subido a " + path;
            ViewData["FILENAME"] = fileName;
            return View();
        }
    }
}
