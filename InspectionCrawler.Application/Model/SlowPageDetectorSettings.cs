namespace InspectionCrawler.Application.Model
{
    public class SlowPageDetectorSettings
    {
        public SlowPageDetectorSettings()
        {
            IsEnabled = false;
            Milliseconds = 500;
        }

        public bool IsEnabled { get; set; }

        public int Milliseconds { get; set; }
    }
}
