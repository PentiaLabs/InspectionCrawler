namespace InspectionCrawler.Application.Model
{
    public class LargePageDetectorSettings
    {
        public LargePageDetectorSettings()
        {
            IsEnabled = false;
        }

        public bool IsEnabled { get; set; }

        public int ByteSize { get; set; }
    }
}
