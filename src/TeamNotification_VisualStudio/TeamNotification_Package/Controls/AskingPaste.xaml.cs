﻿using System;
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
        private int lines = 1;
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
            dudLine.ValueChanged += DudLine_OnValueChanged;
            rbAppend.Checked += HandleCheck;
            rbOverwrite.Checked += HandleCheck;
            this.document = document;
            this.originalText = originalText;
            this.textToPaste = textToPaste;
        }

        public static PastingResponse Show(IWrapDocument document, string originalText, string textToPaste, int line)
        {
            if (textToPaste[textToPaste.Length - 1] != '\n')
                textToPaste += '\n';
            document.Paste(1, textToPaste, PasteOptions.Insert);
            var textToPasteLines = textToPaste.Split('\n').Count(messageLine => messageLine != "");
            document.Selection.Select(1, textToPasteLines);
            var ap = new AskingPaste(document, originalText, textToPaste)
            {
                rbAppend = { IsChecked = true },
                dudLine = {Maximum = (document.Lines - document.Selection.Lines) + 1, Value = line},
                lines = textToPasteLines
            };
            ap.dudLine.Focus();
            ap.ShowDialog();
            if (ap.DialogResult.HasValue && ap.DialogResult.Value && ap.dudLine.Value != null)
                return new PastingResponse { line = (int) ap.dudLine.Value, pasteOption = ap.rbAppend.IsChecked != null && (ap.rbAppend.IsChecked.Value) ? PasteOptions.Append : PasteOptions.Overwrite };
            return new PastingResponse { line = -1, pasteOption = PasteOptions.Abort };
        }

        private void RewriteFile()
        {
            var textDocument = document.TextDocument;
            var editPoint = textDocument.CreateEditPoint();
            textDocument.Selection.SelectAll();
            textDocument.Selection.Cut();
            editPoint.Insert(originalText);
        }

        private void MoveCode(int lines, int lineTo)
        {
            var opt = PasteOptions.Insert;
            var isChecked = this.rbOverwrite.IsChecked;
            if((isChecked != null && (bool)isChecked))
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
            document.Selection.Select(lineTo,lines);
        }

        private void DudLine_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (dudLine.Value != null) 
                MoveCode(lines, (int)dudLine.Value);
        }

        private void HandleCheck(object sender, RoutedEventArgs e)
        {
            if (dudLine.Value == null) return;
            RewriteFile();
            MoveCode(lines, (int) dudLine.Value);
        }

        private void HandleClick(object sender, EventArgs args) { DialogResult = true; }
        private void CloseWindow(object sender, EventArgs args) { Close(); }
    }
}