using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace UrbanBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly string _storagBasePath;
        private readonly long _maxFileSizeBytes;
        private readonly ITenantService _tenantService;

        // Magic bytes para validar contenido real del archivo
        private static readonly Dictionary<string, byte[][]> MagicBytes = new()
        {
            { ".jpg",  new[] { new byte[] { 0xFF, 0xD8, 0xFF } } },
            { ".jpeg", new[] { new byte[] { 0xFF, 0xD8, 0xFF } } },
            { ".png",  new[] { new byte[] { 0x89, 0x50, 0x4E, 0x47 } } },
            { ".gif",  new[] { new byte[] { 0x47, 0x49, 0x46, 0x38 } } },
            { ".webp", new[] { new byte[] { 0x52, 0x49, 0x46, 0x46 } } }, // RIFF
            { ".mp4",  new[] { new byte[] { 0x00, 0x00, 0x00 }, new byte[] { 0x66, 0x74, 0x79, 0x70 } } }, // ftyp (offset 4)
            { ".webm", new[] { new byte[] { 0x1A, 0x45, 0xDF, 0xA3 } } },
        };

        public FilesController(IConfiguration configuration, ITenantService tenantService)
        {
            _storagBasePath = configuration["FileStorage:BasePath"]
                ?? throw new InvalidOperationException("FileStorage:BasePath is not configured");
            var maxSizeMb = configuration.GetValue<int>("FileStorage:MaxFileSizeMB", 10);
            _maxFileSizeBytes = maxSizeMb * 1024L * 1024L;
            _tenantService = tenantService;
        }

        /// <summary>
        /// Obtiene la carpeta base del tenant actual: {BasePath}/{tenantSlug}
        /// Si no hay tenant (admin), usa _shared
        /// </summary>
        private string GetTenantStoragePath()
        {
            var slug = _tenantService.GetCurrentTenantSlug();
            var tenantFolder = string.IsNullOrWhiteSpace(slug) ? "_shared" : slug;
            return Path.Combine(_storagBasePath, tenantFolder);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file, [FromForm] string folder = "images", [FromForm] string? fileName = null, [FromForm] string? imageProfile = null)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest(new { message = "No file provided" });
                }

                // Validar tamaño máximo en backend
                if (file.Length > _maxFileSizeBytes)
                {
                    return BadRequest(new { message = $"File size exceeds the maximum allowed ({_maxFileSizeBytes / (1024 * 1024)}MB)" });
                }

                var imageExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var videoExtensions = new[] { ".mp4", ".webm" };
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                var isVideo = imageProfile == "video" && videoExtensions.Contains(fileExtension);

                if (!imageExtensions.Contains(fileExtension) && !isVideo)
                {
                    return BadRequest(new { message = imageProfile == "video"
                        ? "Only image or video files (.mp4, .webm) are allowed"
                        : "Only image files are allowed" });
                }

                // Validar magic bytes del archivo
                if (!await ValidateMagicBytesAsync(file, fileExtension))
                {
                    return BadRequest(new { message = "File content does not match the expected format" });
                }

                var safeFolder = SanitizeFolder(folder);
                if (string.IsNullOrWhiteSpace(safeFolder))
                {
                    return BadRequest(new { message = "Invalid folder name" });
                }

                var tenantPath = GetTenantStoragePath();
                var uploadsFolder = Path.Combine(tenantPath, safeFolder);

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Path traversal prevention
                var fullUploadsPath = Path.GetFullPath(uploadsFolder);
                var fullBasePath = Path.GetFullPath(_storagBasePath);
                if (!fullUploadsPath.StartsWith(fullBasePath))
                {
                    return Forbid();
                }

                var baseFileName = string.IsNullOrWhiteSpace(fileName)
                    ? Path.GetFileNameWithoutExtension(file.FileName)
                    : Path.GetFileNameWithoutExtension(fileName);

                // GUID parcial para nombres no predecibles
                var uniqueId = Guid.NewGuid().ToString("N")[..12];

                string uniqueFileName;
                string filePath;

                if (isVideo)
                {
                    uniqueFileName = $"{baseFileName}_{uniqueId}{fileExtension}";
                    filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using var stream = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(stream);
                }
                else
                {
                    uniqueFileName = $"{baseFileName}_{uniqueId}.webp";
                    filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    await ProcessAndSaveImageAsync(file, filePath, imageProfile);
                }

                return Ok(new
                {
                    message = "File uploaded successfully",
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

                if (string.IsNullOrWhiteSpace(safeFolder) || string.IsNullOrWhiteSpace(safeFileName))
                {
                    return BadRequest(new { message = "Invalid file or folder name" });
                }

                var allowedExtensions = new[] { ".webp", ".jpg", ".jpeg", ".png", ".gif", ".mp4", ".webm" };
                var fileExtension = Path.GetExtension(safeFileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest(new { message = "Invalid file type" });
                }

                var tenantPath = GetTenantStoragePath();
                var filePath = Path.Combine(tenantPath, safeFolder, safeFileName);

                // Path traversal prevention
                var fullPath = Path.GetFullPath(filePath);
                var allowedPath = Path.GetFullPath(Path.Combine(tenantPath, safeFolder));

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
                ".mp4" => "video/mp4",
                ".webm" => "video/webm",
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

                var tenantPath = GetTenantStoragePath();
                var filePath = Path.Combine(tenantPath, safeFolder, safeFileName);

                // Path traversal prevention
                var fullPath = Path.GetFullPath(filePath);
                var allowedPath = Path.GetFullPath(Path.Combine(tenantPath, safeFolder));

                if (!fullPath.StartsWith(allowedPath))
                {
                    return Forbid();
                }

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

                if (!System.Text.RegularExpressions.Regex.IsMatch(pattern, @"^[a-zA-Z0-9_-]+$"))
                {
                    return BadRequest(new { message = "Invalid pattern format" });
                }

                var tenantPath = GetTenantStoragePath();
                var uploadsFolder = Path.Combine(tenantPath, safeFolder);

                if (!Directory.Exists(uploadsFolder))
                {
                    return Ok(new { message = "Folder not found, no files to delete", deletedCount = 0 });
                }

                // Path traversal prevention
                var fullUploadsPath = Path.GetFullPath(uploadsFolder);
                var fullBasePath = Path.GetFullPath(_storagBasePath);

                if (!fullUploadsPath.StartsWith(fullBasePath))
                {
                    return Forbid();
                }

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

        /// <summary>
        /// Valida que los primeros bytes del archivo coincidan con los magic bytes esperados
        /// </summary>
        private static async Task<bool> ValidateMagicBytesAsync(IFormFile file, string extension)
        {
            if (!MagicBytes.TryGetValue(extension, out var signatures))
                return true; // Extensión sin magic bytes definidos, pasar

            using var stream = file.OpenReadStream();
            var headerBuffer = new byte[12];
            var bytesRead = await stream.ReadAsync(headerBuffer.AsMemory(0, headerBuffer.Length));

            if (bytesRead < 3)
                return false;

            // MP4 tiene el magic byte "ftyp" en offset 4
            if (extension == ".mp4")
            {
                var ftypSignature = new byte[] { 0x66, 0x74, 0x79, 0x70 };
                if (bytesRead >= 8)
                {
                    bool match = true;
                    for (int i = 0; i < ftypSignature.Length; i++)
                    {
                        if (headerBuffer[4 + i] != ftypSignature[i])
                        {
                            match = false;
                            break;
                        }
                    }
                    return match;
                }
                return false;
            }

            // Para otros formatos, verificar que al menos una firma coincida
            foreach (var signature in signatures)
            {
                if (bytesRead >= signature.Length)
                {
                    bool match = true;
                    for (int i = 0; i < signature.Length; i++)
                    {
                        if (headerBuffer[i] != signature[i])
                        {
                            match = false;
                            break;
                        }
                    }
                    if (match) return true;
                }
            }

            return false;
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

        private (int maxWidth, int maxHeight, int quality, bool pad) GetProfileSettings(string? profile)
        {
            return profile switch
            {
                "hero" => (1920, 1080, 80, false),
                "gallery" => (800, 600, 80, false),
                "icon" => (128, 128, 80, false),
                _ => (256, 256, 75, true), // avatar default
            };
        }

        private async Task ProcessAndSaveImageAsync(IFormFile file, string filePath, string? imageProfile = null)
        {
            var (maxWidth, maxHeight, quality, pad) = GetProfileSettings(imageProfile);

            using (var stream = file.OpenReadStream())
            using (var image = await Image.LoadAsync(stream))
            {
                var (newWidth, newHeight) = CalculateDimensions(image.Width, image.Height, maxWidth, maxHeight);

                image.Mutate(x => x.Resize(newWidth, newHeight, KnownResamplers.Lanczos3));

                if (pad && (newWidth < maxWidth || newHeight < maxHeight))
                {
                    image.Mutate(x => x.Pad(maxWidth, maxHeight, SixLabors.ImageSharp.Color.White));
                }

                var encoder = new WebpEncoder { Quality = quality };
                await image.SaveAsync(filePath, encoder);
            }
        }

        private (int width, int height) CalculateDimensions(int originalWidth, int originalHeight, int maxWidth, int maxHeight)
        {
            if (originalWidth <= maxWidth && originalHeight <= maxHeight)
            {
                return (originalWidth, originalHeight);
            }

            var ratioW = (double)maxWidth / originalWidth;
            var ratioH = (double)maxHeight / originalHeight;
            var ratio = Math.Min(ratioW, ratioH);

            return ((int)(originalWidth * ratio), (int)(originalHeight * ratio));
        }
    }
}
