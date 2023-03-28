namespace MoviesWebAspNetMVC.Services
{
    public class ImageSaverService : IFileSaverService
    {
        public async Task<string> SaveAsync(IFormFile file, string fileDirectory)
        {
            string fileName = null;
            if (file != null)
            {
                fileName = file.FileName;
                var filePath = Path.Combine(fileDirectory, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            return fileName;
        }
    }
}
