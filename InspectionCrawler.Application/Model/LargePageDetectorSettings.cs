namespace InspectionCrawler.Application.Model
{
    public class LargePageDetectorSettings
    {
        public LargePageDetectorSettings()
        {
            IsEnabled = false;
            ByteSize = 1024*1024;
        }

        public bool IsEnabled { get; set; }

        public int ByteSize { get; set; }
    }
}
