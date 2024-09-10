using ImageUploadApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class ImageUploadController : Controller
{
    private readonly string[] _allowedExtensions = new[] { ".jpeg", ".jpg", ".png", ".dicom" };
    private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

    public ImageUploadController()
    {
        // Ensure that the uploads directory exists
        if (!Directory.Exists(_uploadPath))
        {
            Directory.CreateDirectory(_uploadPath);
        }
    }

    [HttpGet]
    public IActionResult Index()
    {
        var model = Directory.GetFiles(_uploadPath).Select(filePath => new ImageModel
        {
            ImageName = Path.GetFileName(filePath),
            ImagePath = Path.Combine("/uploads", Path.GetFileName(filePath)),
            ImageType = Path.GetExtension(filePath)
        }).ToList();

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile[] files)
    {
        foreach (var file in files)
        {
            if (file != null && file.Length > 0)
            {
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!_allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("File", "Unsupported file format");
                    return View("Index");
                }

                var fileName = Path.GetFileName(file.FileName);
                var filePath = Path.Combine(_uploadPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
        }

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult ViewImage(string fileName)
    {
        var filePath = Path.Combine(_uploadPath, fileName);
        if (System.IO.File.Exists(filePath))
        {
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/octet-stream", fileName);
        }

        return NotFound();
    }
}

