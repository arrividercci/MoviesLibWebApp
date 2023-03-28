namespace MoviesWebAspNetMVC.Services
{
    public interface IFileSaverService
    {
        public Task<string> SaveAsync(IFormFile file, string fileDirectory);
    }
}
