namespace InspectionCrawler.Application.Model
{
    public class ErrorDetectorSettings
    {
        public ErrorDetectorSettings()
        {
            IsEnabled = false;
            CheckExternalLinks = false;
            TreatRedirectsAsErrors = false;
        }

        public bool IsEnabled { get; set; }

        public bool CheckExternalLinks { get; set; }

        public bool TreatRedirectsAsErrors { get; set; }
    }
}
