using System.Collections.Generic;

namespace InspectionCrawler.Application.Model
{
    public class SchemeCollectorSettings
    {
        public SchemeCollectorSettings()
        {
            IsEnabled = false;
            Schemes = new List<string>();
        }

        public bool IsEnabled { get; set; }

        public List<string> Schemes { get; }
    }
}
