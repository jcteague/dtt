using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;

namespace TeamNotification_Library.Service.LocalSystem
{

    public class TextDocumentWrapper : IWrapTextDocument
    {
        private readonly TextDocument textDocument;
        public TextDocumentWrapper(TextDocument textDocument) { this.textDocument = textDocument; }
        public TextSelection Selection{ get { return textDocument.Selection; } }
        public EditPoint CreateEditPoint() { return textDocument.CreateEditPoint(); }
        public TextPoint EndPoint { get { return textDocument.EndPoint; } }

        public TextPoint StartPoint
        {
            get { return textDocument.StartPoint; }
        }
    }
}
