using System;
using System.Collections.Generic;
using CommandLine;

namespace InspectionCrawler.IcRunner
{
    internal class Options
    {
        #region Crawler options

        [Option('u', "url", Required = true, HelpText = "Url to start the crawl from")]
        public Uri Url { get; set; }

        [Option('f', "log-file", Required = false, HelpText = "Write logs to this file")]
        public string LogFile { get; set; }

        #endregion

        #region Slow page detector options

        [Option("slow-page-detector-enabled", Required = false, HelpText = "Enable the 'Slow page detector'")]
        public bool SlowPageDetectorEnabled { get; set; }

        [Option("slow-page-detector-milliseconds", Required = false, HelpText = "Threshold in milliseconds")]
        public int SlowPageDetectorMilliseconds { get; set; }

        #endregion

        #region Large page detector options

        [Option("large-page-detector-enabled", Required = false, HelpText = "Enable the 'Large page detector'")]
        public bool LargePageDetectorEnabled { get; set; }

        [Option("large-page-detector-byte-size", Required = false, HelpText = "Threshold in number of bytes")]
        public int LargePageDetectorByteSizes { get; set; }

        #endregion

        #region Error detector options

        [Option("error-detector-enabled", Required = false, HelpText = "Enable the 'Error detector'")]
        public bool ErrorDetectorEnabled { get; set; }

        [Option("error-detector-check-external-links", Required = false, HelpText = "Enable to check external links")]
        public bool ErrorDetectorCheckExternalLinks { get; set; }

        [Option("error-detector-treat-redirects-as-errors", Required = false, HelpText = "Enable to log redirects as errors")]
        public bool ErrorDetectorTreatRedirectsAsErrors { get; set; }

        #endregion

        #region All references collector options

        [Option("all-references-collector-enabled", Required = false, HelpText = "Enable the 'All references collector'")]
        public bool AllReferencesCollectorEnabled { get; set; }

        [Option("all-references-collector-targets", Required = false, HelpText = "Specify the uris to collector for")]
        public IEnumerable<Uri> AllReferencesCollectorTargets { get; set; }

        #endregion

        #region Scheme collector options

        [Option("scheme-collector-enabled", Required = false, HelpText = "Enable the 'Scheme collector'")]
        public bool SchemeCollectorEnabled { get; set; }

        [Option("scheme-collector-schemes", Required = false, HelpText = "Specify the schemes to collect for")]
        public IEnumerable<string> SchemeCollectorSchemes { get; set; }

        #endregion

        #region External links collector options

        [Option("external-links-collector-enabled", Required = false, HelpText = "Enable the 'External links collector'")]
        public bool ExternalLinksCollectorEnabled { get; set; }

        [Option("external-links-collector-schemes", Required = false, HelpText = "Specify the uris to ignore")]
        public IEnumerable<Uri> ExternalLinksCollectorIgnore { get; set; }

        #endregion
    }
}
