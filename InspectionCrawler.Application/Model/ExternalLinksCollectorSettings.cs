using System;
using System.Collections.Generic;

namespace InspectionCrawler.Application.Model
{
    public class ExternalLinksCollectorSettings
    {
        public ExternalLinksCollectorSettings()
        {
            IsEnabled = false;
            Ignore = new List<Uri>();
        }

        public bool IsEnabled { get; set; }

        public List<Uri> Ignore { get; }
    }
}
