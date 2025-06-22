using Microsoft.AspNetCore.Http;

namespace Portfolio.Core.Helper
{
    public static class FileServices
    {
        public static void DeleteFile(string FileName, string FolderName)
        {
            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName, FileName);

            if (File.Exists(FilePath))
                File.Delete(FilePath);
        }
        public static string UploadFile(IFormFile? File, string FolderName)
        {
            if (File is null) return "null";

            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FolderName);

            string FileName = $"{Guid.NewGuid()}{File.FileName.Replace(" ", "_")}";

            string FilePath = Path.Combine(FolderPath, FileName);

            using var FS = new FileStream(FilePath, FileMode.Create);
            File.CopyTo(FS);

            return FilePath;
        }
    }

}
