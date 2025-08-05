using BlazingPizza.Shared.Interfaces;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazingPizza.Repositories;

public class ImageService : IImageService
{
    private readonly IWebHostEnvironment _environment;

    public ImageService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> UploadImageAsync(IBrowserFile file, string folder = "carrier")
    {
        if (file == null || file.ContentType == null)
            throw new ArgumentException("Invalid file");

        // Validate file type
        var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif" };
        if (!allowedTypes.Contains(file.ContentType.ToLower()))
            throw new ArgumentException("File type not allowed. Only JPG, PNG, and GIF are supported.");

        // Validate file size (5MB max)
        if (file.Size > 5 * 1024 * 1024)
            throw new ArgumentException("File size too large. Maximum size is 5MB.");

        var fileName = $"{Guid.NewGuid()}_{file.Name}";
        var uploadsFolder = Path.Combine(_environment.WebRootPath, "img", folder);

        // Ensure directory exists
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var filePath = Path.Combine(uploadsFolder, fileName);

        using var stream = file.OpenReadStream(maxAllowedSize: 5 * 1024 * 1024);
        using var fileStream = new FileStream(filePath, FileMode.Create);
        await stream.CopyToAsync(fileStream);

        return fileName;
    }

    public async Task<bool> DeleteImageAsync(string fileName, string folder = "carrier")
    {
        if (string.IsNullOrEmpty(fileName))
            return false;

        try
        {
            var filePath = Path.Combine(_environment.WebRootPath, "img", folder, fileName);

            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
                return true;
            }

            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<string> GetImagePathAsync(string fileName, string folder = "carrier")
    {
        await Task.CompletedTask; // For async consistency
        return $"img/{folder}/{fileName}";
    }

    public bool ImageExists(string fileName, string folder = "carrier")
    {
        if (string.IsNullOrEmpty(fileName))
            return false;

        var filePath = Path.Combine(_environment.WebRootPath, "img", folder, fileName);
        return File.Exists(filePath);
    }
}