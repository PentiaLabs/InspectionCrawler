using System;
using System.Collections.Generic;

namespace InspectionCrawler.Application.Model
{
    public class AllReferencesCollectorSettings
    {
        public AllReferencesCollectorSettings()
        {
            IsEnabled = false;
            Targets = new List<Uri>();
        }

        public bool IsEnabled { get; set; }

        public List<Uri> Targets { get; }
    }
}
