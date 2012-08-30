using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using EnvDTE;
using TeamNotification_Library.Service.LocalSystem;
using Window = System.Windows.Window;

namespace AvenidaSoftware.TeamNotification_Package.Controls
{
    /// <summary>
    /// Interaction logic for AskingPaste.xaml
    /// </summary>
    public partial class AskingPaste : Window
    {
        private readonly IWrapDocument document;
        private string originalText;
        private string textToPaste;
        private int lines;
        public AskingPaste()
        {
            InitializeComponent();
            btnPaste.Click +=HandleClick;
            btnCancel.Click += CloseWindow;
        }

        public AskingPaste(IWrapDocument document, string originalText, string textToPaste)
        {
            InitializeComponent();
            btnPaste.Click += HandleClick;
            iudLine.ValueChanged += DudLine_OnValueChanged;
            chkOverwrite.Checked += HandleCheck;
            chkOverwrite.Unchecked += HandleCheck;
            this.document = document;
            this.originalText = originalText;
            this.textToPaste = textToPaste;
        }

        public static PastingResponse Show(IWrapDocument document, string originalText, string textToPaste, int line)
        {
            textToPaste = AppendNewLine(textToPaste);
            var textToPasteLines = CountLines(textToPaste);
            document.Paste(1, textToPaste, PasteOptions.Insert);
            document.Selection.Select(1, textToPasteLines);
            
            var ap = new AskingPaste(document, originalText, textToPaste)
            {
                lines = textToPasteLines,
                iudLine = { Maximum = (document.Lines - document.Selection.Lines) + 1, Value = line }
            };

            ap.iudLine.Focus();
            ap.ShowDialog();

            if (ap.DialogResult.HasValue && ap.DialogResult.Value && ap.iudLine.Value != null)
                return new PastingResponse { line = (int)ap.iudLine.Value, pasteOption = ap.chkOverwrite.IsChecked != null && (ap.chkOverwrite.IsChecked.Value) ? PasteOptions.Overwrite : PasteOptions.Append };
            
            return new PastingResponse { line = -1, pasteOption = PasteOptions.Abort };
        }

        private static string AppendNewLine(string textToPaste)
        {
            if (textToPaste[textToPaste.Length - 1] != '\n') textToPaste += '\n';
            return textToPaste;
        }
        private static int CountLines(string text)
        {
            return text.Split('\n').Count(textLine => textLine != "");
        }

        private void RewriteFile()
        {
            var textDocument = document.TextDocument;
            var editPoint = textDocument.CreateEditPoint();
            textDocument.Selection.SelectAll();
            textDocument.Selection.Delete();
            editPoint.Insert(originalText);
        }


        private void DudLine_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (iudLine.Value != null)
                MoveCode(lines, (int)iudLine.Value);
        }

        private void HandleCheck(object sender, RoutedEventArgs e)
        {
            if (iudLine.Value == null) return;
            RewriteFile();
            MoveCode(lines, (int)iudLine.Value);
        }

        private void MoveCode(int lines, int lineTo)
        {
            var opt = PasteOptions.Insert;
            var isChecked = this.chkOverwrite.IsChecked;
            if ((bool)isChecked)
            {
                opt = PasteOptions.Overwrite;
                RewriteFile();
                var editPoint = document.TextDocument.CreateEditPoint();
                editPoint.MoveToLineAndOffset(1, 1);
                editPoint.Insert(textToPaste);
                document.Selection.Select(1, lines);
            }
            document.Selection.Cut();
            document.Paste(lineTo, opt);
            document.Selection.Select(lineTo, lines);
        }

        private void HandleClick(object sender, EventArgs args) { DialogResult = true; }
        private void CloseWindow(object sender, EventArgs args) { Close(); }
    }
}