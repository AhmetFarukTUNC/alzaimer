using Microsoft.AspNetCore.Mvc;
using AlzheimerApp.Data;
using AlzheimerApp.Services;
using AlzaimerApp.Data.Models;

namespace AlzheimerApp.WebUI.Controllers
{
    public class PredictController : Controller
    {
        private readonly FlaskApiService _flaskService;
        private readonly AlzheimerContext _context;

        public PredictController(FlaskApiService flaskService, AlzheimerContext context)
        {
            _flaskService = flaskService;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Kullanıcı giriş yapmamışsa
            if (!HttpContext.Session.TryGetValue("UserId", out _))
            {
                TempData["Error"] = "Tahmin yapabilmek için giriş yapmalısınız.";
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.Error = "Lütfen bir dosya seçin.";
                return View();
            }

            // Görseli byte dizisine çevir
            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            var bytes = ms.ToArray();
            string base64Image = Convert.ToBase64String(bytes);

            // Flask API'den tahmin sonucu al
            var (predClass, confidence) = await _flaskService.PredictAsync(bytes);

            // Session’daki UserId
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["Error"] = "Kullanıcı bilgisi alınamadı. Lütfen tekrar giriş yapın.";
                return RedirectToAction("Login", "Account");
            }

            // Tahmini veritabanına kaydet
            var prediction = new Prediction
            {
                FileName = file.FileName,
                ImageData = bytes,
                PredictedClass = predClass,
                Confidence = (float)confidence,
                UserId = userId.Value,
                CreatedAt = DateTime.Now
            };

            _context.Predictions.Add(prediction);
            await _context.SaveChangesAsync();

            // Aynı sayfada görüntülemek için ViewBag
            ViewBag.UploadedImage = base64Image;
            ViewBag.Result = predClass;
            ViewBag.Confidence = $"{confidence * 100:F2}%";

            return View();
        }

        [HttpGet]
        public IActionResult Result()
        {
            // Session’dan UserId al
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                TempData["Error"] = "Kullanıcı bilgisi alınamadı. Lütfen tekrar giriş yapın.";
                return RedirectToAction("Login", "Account");
            }

            // Kullanıcının tüm tahminlerini çek
            var predictions = _context.Predictions
                .Where(p => p.UserId == userId.Value)
                .OrderByDescending(p => p.CreatedAt)
                .ToList();

            return View(predictions);
        }


    }
}
