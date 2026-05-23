using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/uploads")]
[ApiController]
public class UploadController : ControllerBase
{
    private readonly IWebHostEnvironment _env;

    public UploadController(IWebHostEnvironment env)
    {
        _env = env;
    }
    [Authorize(Policy = "Admin")]
    [HttpPost]
    public async Task<IActionResult> PostParrotImage(IFormFile file)
    {
        if (file == null || file.Length == 0) return BadRequest("No file selected.");

        var folderPath = Path.Combine(_env.WebRootPath, "images");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        var fileName = $"{Guid.NewGuid()}_{file.FileName}";
        var fullPath = Path.Combine(folderPath, fileName);

        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        var relativePath = $"/{fileName}";
        return Ok(new { imageUrl = relativePath });
    }
}