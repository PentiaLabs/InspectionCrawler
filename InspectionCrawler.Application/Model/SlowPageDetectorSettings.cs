namespace InspectionCrawler.Application.Model
{
    public class SlowPageDetectorSettings
    {
        public SlowPageDetectorSettings()
        {
            IsEnabled = false;
        }

        public bool IsEnabled { get; set; }

        public int Milliseconds { get; set; }
    }
}
