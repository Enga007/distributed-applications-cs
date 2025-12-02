using SafeBG.Models.Enums;

namespace SafeBG.Models.ViewModels
{
    public class ReportFilterViewModel
    {
        public string? SearchText { get; set; }
        public int? CityId { get; set; }
        public int? CategoryId { get; set; }
        public ReportStatus? Status { get; set; }

        // За страниците
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        // Dropdown данни
        public List<City>? Cities { get; set; }
        public List<Category>? Categories { get; set; }

        // Резултати
        public List<Report>? Results { get; set; }

        // За сортиране
        public string? SortBy { get; set; }
        public bool SortAsc { get; set; } = true;

        // Брой за пагинация
        public int TotalCount { get; set; }
    }
}
