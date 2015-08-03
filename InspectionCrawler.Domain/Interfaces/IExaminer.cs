using System;
using System.Collections.Generic;

namespace InspectionCrawler.Domain.Interfaces
{
    public interface IExaminer
    {
        string GetText(string query);
        string GetAttribute(string query, string name);
        IEnumerable<Uri> Links { get; }
    }
}
