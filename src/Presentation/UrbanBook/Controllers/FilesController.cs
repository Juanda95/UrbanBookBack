using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace UrbanBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FilesController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromForm] string folder = "images", [FromForm] string? fileName = null)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest(new { message = "No file provided" });
                }

                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest(new { message = "Only image files are allowed" });
                }

                var safeFolder = SanitizeFolder(folder);
                if (string.IsNullOrWhiteSpace(safeFolder))
                {
                    return BadRequest(new { message = "Invalid folder name" });
                }

                var webRoot = _webHostEnvironment.WebRootPath;
                if (string.IsNullOrWhiteSpace(webRoot))
                {
                    webRoot = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot");
                }

                var uploadsFolder = Path.Combine(webRoot, "uploads", safeFolder);

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var baseFileName = string.IsNullOrWhiteSpace(fileName)
                    ? Path.GetFileNameWithoutExtension(file.FileName)
                    : Path.GetFileNameWithoutExtension(fileName);

                var uniqueFileName = $"{baseFileName}_{DateTime.UtcNow.Ticks}.webp";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Procesar imagen: redimensionar, comprimir y convertir a WebP
                await ProcessAndSaveImageAsync(file, filePath);

                // Construir URL absoluta del servidor
                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                var fileUrl = $"{baseUrl}/uploads/{safeFolder}/{uniqueFileName}";

                return Ok(new
                {
                    message = "File uploaded successfully",
                    fileUrl,
                    fileName = uniqueFileName
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error uploading file", error = ex.Message });
            }
        }

        [HttpGet("get/{fileName}")]
        public IActionResult GetFile(string fileName, [FromQuery] string folder = "images")
        {
            try
            {
                var safeFolder = SanitizeFolder(folder);
                var safeFileName = Path.GetFileName(fileName);

                // Validaciones de seguridad
                if (string.IsNullOrWhiteSpace(safeFolder) || string.IsNullOrWhiteSpace(safeFileName))
                {
                    return BadRequest(new { message = "Invalid file or folder name" });
                }

                // Solo permitir acceso a archivos con extensiones seguras
                var allowedExtensions = new[] { ".webp", ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(safeFileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest(new { message = "Invalid file type" });
                }

                var webRoot = _webHostEnvironment.WebRootPath;
                if (string.IsNullOrWhiteSpace(webRoot))
                {
                    webRoot = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot");
                }

                var filePath = Path.Combine(webRoot, "uploads", safeFolder, safeFileName);

                // Validar que el archivo está dentro del directorio permitido (prevenir path traversal)
                var fullPath = Path.GetFullPath(filePath);
                var allowedPath = Path.GetFullPath(Path.Combine(webRoot, "uploads", safeFolder));

                if (!fullPath.StartsWith(allowedPath))
                {
                    return Forbid();
                }

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound(new { message = "File not found" });
                }

                var fileStream = System.IO.File.OpenRead(filePath);
                var contentType = GetContentType(fileExtension);

                return File(fileStream, contentType, safeFileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving file", error = ex.Message });
            }
        }

        private string GetContentType(string fileExtension)
        {
            return fileExtension switch
            {
                ".webp" => "image/webp",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };
        }

        [HttpDelete("delete")]
        public IActionResult Delete([FromQuery] string fileName, [FromQuery] string folder = "images")
        {
            try
            {
                var safeFolder = SanitizeFolder(folder);
                var safeFileName = Path.GetFileName(fileName);

                if (string.IsNullOrWhiteSpace(safeFolder) || string.IsNullOrWhiteSpace(safeFileName))
                {
                    return BadRequest(new { message = "Invalid file or folder name" });
                }

                var webRoot = _webHostEnvironment.WebRootPath;
                if (string.IsNullOrWhiteSpace(webRoot))
                {
                    webRoot = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot");
                }

                var filePath = Path.Combine(webRoot, "uploads", safeFolder, safeFileName);

                if (!System.IO.File.Exists(filePath))
                {
                    return NotFound(new { message = "File not found" });
                }

                System.IO.File.Delete(filePath);

                return Ok(new { message = "File deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting file", error = ex.Message });
            }
        }

        [HttpDelete("delete-by-pattern")]
        public IActionResult DeleteByPattern([FromQuery] string pattern, [FromQuery] string folder = "images")
        {
            try
            {
                var safeFolder = SanitizeFolder(folder);
                if (string.IsNullOrWhiteSpace(safeFolder))
                {
                    return BadRequest(new { message = "Invalid folder name" });
                }

                // Validar que el patrón solo contiene caracteres seguros (alfanuméricos, guiones, guiones bajos)
                if (!System.Text.RegularExpressions.Regex.IsMatch(pattern, @"^[a-zA-Z0-9_-]+$"))
                {
                    return BadRequest(new { message = "Invalid pattern format" });
                }

                var webRoot = _webHostEnvironment.WebRootPath;
                if (string.IsNullOrWhiteSpace(webRoot))
                {
                    webRoot = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot");
                }

                var uploadsFolder = Path.Combine(webRoot, "uploads", safeFolder);

                if (!Directory.Exists(uploadsFolder))
                {
                    return Ok(new { message = "Folder not found, no files to delete", deletedCount = 0 });
                }

                // Validar que uploadsFolder está dentro del webroot (prevenir path traversal)
                var fullUploadsPath = Path.GetFullPath(uploadsFolder);
                var fullWebRoot = Path.GetFullPath(webRoot);

                if (!fullUploadsPath.StartsWith(fullWebRoot))
                {
                    return Forbid();
                }

                // Buscar y eliminar archivos que coincidan con el patrón
                // Solo eliminar archivos que terminan con números y extensión (formato: cliente_123_9999999999.webp)
                var files = Directory.GetFiles(uploadsFolder, $"{pattern}_*.webp");
                int deletedCount = 0;

                foreach (var file in files)
                {
                    try
                    {
                        System.IO.File.Delete(file);
                        deletedCount++;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error deleting file {file}: {ex.Message}");
                    }
                }

                return Ok(new { message = $"{deletedCount} file(s) deleted successfully", deletedCount });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting files", error = ex.Message });
            }
        }

        private static string SanitizeFolder(string folder)
        {
            var trimmed = folder?.Trim() ?? string.Empty;
            if (trimmed.Contains("..") || trimmed.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            {
                return string.Empty;
            }

            return trimmed.Replace("\\", string.Empty).Replace("/", string.Empty);
        }

        /// <summary>
        /// Procesa la imagen: redimensiona para avatar (256x256), comprime y convierte a WebP
        /// </summary>
        private async Task ProcessAndSaveImageAsync(IFormFile file, string filePath)
        {
            using (var stream = file.OpenReadStream())
            using (var image = await Image.LoadAsync(stream))
            {
                // Redimensionar para avatares (256x256) manteniendo aspect ratio
                const int avatarSize = 256;
                
                // Calcular dimensiones manteniendo aspect ratio
                var (newWidth, newHeight) = CalculateDimensions(image.Width, image.Height, avatarSize);
                
                // Redimensionar la imagen
                image.Mutate(x => x.Resize(newWidth, newHeight, KnownResamplers.Lanczos3));
                
                // Si la imagen es más pequeña que el tamaño del avatar, añadir padding
                if (newWidth < avatarSize || newHeight < avatarSize)
                {
                    image.Mutate(x => x.Pad(avatarSize, avatarSize, SixLabors.ImageSharp.Color.White));
                }

                // Guardar como WebP con compresión
                var encoder = new WebpEncoder { Quality = 75 };
                await image.SaveAsync(filePath, encoder);
            }
        }

        /// <summary>
        /// Calcula las nuevas dimensiones manteniendo aspect ratio
        /// </summary>
        private (int width, int height) CalculateDimensions(int originalWidth, int originalHeight, int maxSize)
        {
            if (originalWidth <= maxSize && originalHeight <= maxSize)
            {
                return (originalWidth, originalHeight);
            }

            if (originalWidth > originalHeight)
            {
                var newWidth = maxSize;
                var newHeight = (int)((double)originalHeight * maxSize / originalWidth);
                return (newWidth, newHeight);
            }
            else
            {
                var newHeight = maxSize;
                var newWidth = (int)((double)originalWidth * maxSize / originalHeight);
                return (newWidth, newHeight);
            }
        }
    }
}
