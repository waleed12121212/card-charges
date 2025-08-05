using Microsoft.AspNetCore.Components.Forms;

namespace BlazingPizza.Shared.Interfaces;

public interface IImageService
{
    Task<string> UploadImageAsync(IBrowserFile file, string folder = "carrier");
    Task<bool> DeleteImageAsync(string fileName, string folder = "carrier");
    Task<string> GetImagePathAsync(string fileName, string folder = "carrier");
    bool ImageExists(string fileName, string folder = "carrier");
} 