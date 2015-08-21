using System;
using System.Collections.Generic;
using System.Linq;
using InspectionCrawler.Domain.Interfaces;
using InspectionCrawler.Domain.Model;

namespace InspectionCrawler.Infrastructure.Inspectors
{
    public class AllReferencesCollector : Inspector
    {
        private readonly List<Uri> _targets;

        public AllReferencesCollector(IInspectorLog log, IEnumerable<Uri> targets) : base(log)
        {
            _targets = targets.ToList();
        }

        public override string Name => "All references collector";

        public override void InspectPage(Page page)
        {
            foreach (var link in page.Examiner.Links)
            {
                if (_targets.Exists(t => t == link) && IsInfoEnabled)
                    Info($"{link} referenced from {page.Uri}");
            }
        }
    }
}
