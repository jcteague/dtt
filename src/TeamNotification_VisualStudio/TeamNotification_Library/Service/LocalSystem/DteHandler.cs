using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EnvDTE;

namespace TeamNotification_Library.Service.LocalSystem
{
    public class DteHandler : IHandleDte
    {
        private readonly IStoreDTE dteStore;
        private const string vsViewKindCode = "{7651A701-06E5-11D1-8EBD-00A0C90F26EA}";

        public bool IsValidSolution { get { return (CurrentSolution.FileName != ""); } }
        public bool HasTextOnLine { get; private set; }
        public IWrapProject[] Projects { get { return dteStore.Solution.Projects; }  }
        public IWrapSolution CurrentSolution { get { return dteStore.Solution; } }
        
        public DteHandler(IStoreDTE dteStore)
        {
            this.dteStore = dteStore;
            this.HasTextOnLine = false;
        }

        public IWrapDocument OpenFile(string projectName, string fileName)
        {
            if (!IsValidSolution) return null;

            try
            {
                return new DocumentWrapper(Container
                    .GetInstance<IFindVisualStudioItems>()
                    .FindDocument(projectName, fileName)
                    .SelectMany(x =>
                                {
                                    var w = x.Open(vsViewKindCode);
                                    w.Visible = true;
                                    w.Activate();
                                    return x.Document;
                                }));
            }
            catch
            {
                return null;
            }
        }

        public EditPoint GetEditPoint(IWrapDocument document, int line)
        {
            var editDoc = document.TextDocument;
            var objEditPt = editDoc.CreateEditPoint();
            objEditPt.StartOfDocument();
            document.ReadOnly = false;

            var i = 1;
            while (!objEditPt.AtEndOfDocument && i < line)
            {
                objEditPt.LineDown();
                ++i;
            }
            HasTextOnLine = !(objEditPt.AtEndOfDocument);
            return objEditPt;
         }

        public void PasteCode(EditPoint objEditPt, string code, PasteOptions pasteOption)
        {
            switch (pasteOption)
            {
                case PasteOptions.Overwrite:
                    objEditPt.Delete(code.Length);
                    break;
                case PasteOptions.Append:
                    objEditPt.EndOfDocument();
                    break;
            }
            objEditPt.Insert(code);
        }
    }
}