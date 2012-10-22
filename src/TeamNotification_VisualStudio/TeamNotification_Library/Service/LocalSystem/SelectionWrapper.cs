using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;

namespace TeamNotification_Library.Service.LocalSystem
{
    public class SelectionWrapper : IWrapSelection
    {
        private IWrapTextDocument TextDocument;

        public SelectionWrapper(IWrapTextDocument TextDocument)
        {
            this.TextDocument = TextDocument;
        }

        public void Copy()
        {
            TextDocument.Selection.Copy();
        }

        public void Cut()
        {
            TextDocument.Selection.Cut();
        }

        public void Select(int line, int lines = 1)
        {
            var textSelection = TextDocument.Selection;
            textSelection.MoveTo(line, 1);
            textSelection.LineDown(true, lines);
            Text = textSelection.Text;
            Lines = lines;
        }

        public string Text{ get; private set; }
        public int Lines { get; private set; }
    }
}
