using MarketPOS.Application.Services.InterfacesServices.FileStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace MarketPOS.Infrastructure.Services.FileStorage;

public class FileService : IFileService
{
    private readonly IHostEnvironment _env;

    private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png" };
    private const long _maxFileSize = 2 * 1024 * 1024; // 2MB

    public FileService(IHostEnvironment env)
    {
        _env = env;
    }

    /// <summary>
    /// حفظ صورة المستخدم في فولدر مخصص له (حسب نوع المستخدم)
    /// </summary>
    public async Task<string> SaveUserImageAsync(
        Guid userId, string PersonFullName ,IFormFile file, string folderName, CancellationToken cancellationToken = default)
    {
        var shortId = userId.ToString().Substring(0, 8);

        if (String.IsNullOrEmpty(shortId))
            throw new InvalidOperationException("User ID cannot be empty.");

        if (String.IsNullOrEmpty(PersonFullName))
            throw new InvalidOperationException("User name cannot be empty.");

        if (file == null || file.Length == 0)
            throw new InvalidOperationException("No file uploaded.");

        if (file.Length > _maxFileSize)
            throw new InvalidOperationException("File size cannot exceed 2 MB.");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!_allowedExtensions.Contains(extension))
            throw new InvalidOperationException("Only .jpg, .jpeg, and .png are allowed.");

        // 📂 المسار: wwwroot/images/{folderName}/{userId}
        var userFolder = Path.Combine(_env.ContentRootPath, "wwwroot", "images", folderName, $"{PersonFullName}_{shortId}");
        if (!Directory.Exists(userFolder))
            Directory.CreateDirectory(userFolder);

        // 🗑️ احذف أي صورة قديمة لنفس المستخدم
        foreach (var oldFile in Directory.GetFiles(userFolder))
        {
            File.Delete(oldFile);
        }

        // 📌 اسم جديد للصورة
        var fileName = $"{PersonFullName}_{shortId}_{file.FileName}";
        var filePath = Path.Combine(userFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        // ✅ URL نسبي يخزن في الـ DB
        return $"/images/{folderName}/{PersonFullName}_{shortId}/{fileName}";
    }

    /// <summary>
    /// حذف صورة المستخدم بالكامل (مثلاً عند تغييرها أو إزالة الحساب)
    /// </summary>
    public void DeleteUserImage(Guid userId, string userName, string folderName)
    {
        var userFolder = Path.Combine(
            _env.ContentRootPath,
            "wwwroot",
            "images",
            folderName,
            $"{userName}_{userId}"
        );

        if (Directory.Exists(userFolder))
        {
            Directory.Delete(userFolder, true); // يحذف الفولدر والصور اللي فيه
        }
    }
}

