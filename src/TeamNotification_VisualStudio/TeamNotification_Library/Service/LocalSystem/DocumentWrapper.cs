using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;

namespace TeamNotification_Library.Service.LocalSystem
{
    public class DocumentWrapper : IWrapDocument
    {
        private Document Document { get; set; }
        public DocumentWrapper(Document document)
        {
            Document = document; 
            Selection = new SelectionWrapper(TextDocument);
        }
        public bool ReadOnly { get { return Document.ReadOnly; } set { Document.ReadOnly = value; } }
        public IWrapSelection Selection { get; private set; }
        public IWrapTextDocument TextDocument { get { return new TextDocumentWrapper((TextDocument)Document.Object("TextDocument")); } }
        public int Length { get { var objEditPt = TextDocument.CreateEditPoint(); objEditPt.StartOfDocument(); return objEditPt.GetText(TextDocument.EndPoint).Length; } }
        public int Lines { 
            get 
            { 
                var objEditPt = TextDocument.CreateEditPoint(); 
                objEditPt.StartOfDocument(); 
                var textParts = objEditPt.GetText(TextDocument.EndPoint).Split('\n'); 
                return textParts.Where(line => line != "").Count();
            } 
        }

        public void Paste(int line, string text, PasteOptions option)
        {
            var objEditPt = TextDocument.CreateEditPoint();
            objEditPt.MoveToLineAndOffset(line, 1);
            objEditPt.Insert(text);
            switch (option)
            {
                case PasteOptions.Overwrite:
                    var textLines = text.Split('\n').Count(textline => textline != "");
                    var textSelection = TextDocument.Selection;
                    textSelection.MoveTo(line + textLines, 1);
                    textSelection.LineDown(true, textLines);
                    textSelection.Delete();
                    break;
            }
        }
        public void Paste(int line, PasteOptions option)
        {
            Paste(line, Selection.Text, option);
        }

        public void Undo() { this.Document.Undo(); }
    }

}
