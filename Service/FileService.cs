using Microsoft.AspNetCore.Http;
using ServiceAbstraction;
using Shared.CommonResult;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class FileService : IFileService
    {
        private readonly string[] allowedExtensions = { ".jpg", ".png", ".jpeg" };
        private readonly IWebHostEnvironment _environment;

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<Result<string>> SaveFileAsync(IFormFile file, string folderName = "Images")
        {
            if (file == null || file.Length == 0)
                return Error.Validation("File.Invalid", "الملف غير صالح أو تالف، يرجى محاولة رفع ملف آخر.");

            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
                return Result<string>.Fail(
                    Error.Validation("File.InvalidExtension", "عذراً، الملحقات المسموح بها فقط هي .jpg و .png")
                );

            var fileName = $"{Guid.NewGuid()}{extension}";

            var folderPath = Path.Combine(_environment.WebRootPath, folderName);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            // إرجاع المسار بصيغة الويب المناسبة
            return $"/{folderName}/{fileName}";
        }

        public Task<Result> DeleteAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return Task.FromResult(Result.Fail(Error.NotFound("File", "عذراً، الملف المطلوب غير موجود أو تم حذفه مسبقاً.")));

            // استخدام DirectorySeparatorChar لضمان توافق الحذف مع Windows و Linux
            var normalizedPath = filePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var fullPath = Path.Combine(_environment.WebRootPath, normalizedPath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            return Task.FromResult(Result.Ok());
        }

        public async Task<Result<string>> UpdateAsync(string oldFilePath, IFormFile newFile, string folderName = "Images")
        {
            // التحقق من وجود مسار قديم قبل محاولة حذفه
            if (!string.IsNullOrEmpty(oldFilePath))
            {
                await DeleteAsync(oldFilePath);
            }

            return await SaveFileAsync(newFile, folderName);
        }
    }
}
