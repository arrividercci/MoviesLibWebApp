using DocumentFormat.OpenXml.Bibliography;

namespace MoviesWebAspNetMVC.ViewModels
{
    public class RecensionViewModel
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public string UserName { get; set; }
        public int RateId { get; set; }
        public string Comment { get; set; }
    }
}
