using ASP_NET_PZ_03.Models;
using System.IO;

namespace ASP_NET_PZ_03.Services
{
    public class LocalUploadedFileStorage
    {
        private readonly string _rootDir;
        public LocalUploadedFileStorage(string rootDir)
        {
            _rootDir = rootDir;
        }

        protected string GetFullFilePath(string filename)
        {
            var dir1 = filename[0].ToString();
            var dir2 = filename[1].ToString();
            var directory = Path.Combine(_rootDir, dir1, dir2);
            var fullFileName = Path.Combine(directory, filename);
            return fullFileName;
        }

        public async Task<ImageFile> SaveUploadedFileAsync(IFormFile formFile)
        {
            var filename = Guid.NewGuid().ToString().ToLower() + Path.GetExtension(formFile.FileName);


            var fullFileName = GetFullFilePath(filename);
            Directory.CreateDirectory(Path.GetDirectoryName(fullFileName));

            using (var file = File.OpenWrite(fullFileName))
            {
                await formFile.CopyToAsync(file);
            }
            var random = new Random();
            return new ImageFile
            {
                Id = random.Next(1,5000),
                Filename = filename,
                OriginalFilename = formFile.FileName,
            };
        }
        public void DeleteUploadedFile(ImageFile file)
        {
            var fullPath = GetFullFilePath(file.Filename);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}
