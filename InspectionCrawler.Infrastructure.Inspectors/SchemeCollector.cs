﻿using System.Collections.Generic;
using System.Linq;
using InspectionCrawler.Domain.Interfaces;
using InspectionCrawler.Domain.Model;

namespace InspectionCrawler.Infrastructure.Inspectors
{
    public class SchemeCollector : Inspector
    {
        private readonly List<string> _schemes;

        public SchemeCollector(IInspectorLog log, IEnumerable<string> schemes) : base(log)
        {
            _schemes = schemes.ToList();
        }

        public override string Name => "Scheme collector";

        public override void InspectPage(Page page)
        {
            foreach (var link in page.Examiner.Links)
            {
                if (_schemes.Exists(s => s == link.Scheme) && IsInfoEnabled)
                    Info($"{link} found on {page.Uri}");
            }
        }
    }
}
