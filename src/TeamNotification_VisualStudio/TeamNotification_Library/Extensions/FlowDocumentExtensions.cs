using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace TeamNotification_Library.Extensions
{
    public static class FlowDocumentExtensions
    {
        public static string GetDocumentText<T>(this T content) where T : FlowDocument
        {
            return new TextRange(content.ContentStart, content.ContentEnd).Text;
        }
    }
}
