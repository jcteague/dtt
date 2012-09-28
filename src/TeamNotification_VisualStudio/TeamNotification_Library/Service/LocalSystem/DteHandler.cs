using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EnvDTE;
using TeamNotification_Library.Extensions;
using TeamNotification_Library.Functional;
using TeamNotification_Library.Service.Logging;

namespace TeamNotification_Library.Service.LocalSystem
{
    public class DteHandler : IHandleDte
    {
        private readonly IStoreDTE dteStore;
        private readonly IFindVisualStudioItems visualStudioItemsFinder;


        private const string vsViewKindCode = "{7651A701-06E5-11D1-8EBD-00A0C90F26EA}";

        public bool IsValidSolution { get { return (CurrentSolution.FileName != ""); } }
        public bool HasTextOnLine { get; private set; }
        public IWrapProject[] Projects { get { return dteStore.Solution.Projects; }  }
        public IWrapSolution CurrentSolution { get { return dteStore.Solution; } }
        
        public DteHandler(IStoreDTE dteStore, IFindVisualStudioItems visualStudioItemsFinder)
        {
            this.dteStore = dteStore;
            this.visualStudioItemsFinder = visualStudioItemsFinder;
            this.HasTextOnLine = false;
        }

        public IWrapDocument OpenFile(string projectName, string fileName)
        {
            Container.GetInstance<ILog>().Write("Going to Open File: {1} in {0}".FormatUsing(projectName, fileName));
            if (!IsValidSolution) return null;

            try
            {
                return new DocumentWrapper(visualStudioItemsFinder
                    .FindDocument(projectName, fileName)
                    .SelectMany(x =>
                                {
                                    Container.GetInstance<ILog>().Write("Found this document: {0} in {1}".FormatUsing(fileName, projectName));
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