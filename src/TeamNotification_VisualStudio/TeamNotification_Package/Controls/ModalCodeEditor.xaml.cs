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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ICSharpCode.AvalonEdit.Highlighting;
using TeamNotification_Library.Service;
using TeamNotification_Library.Service.Async;
using TeamNotification_Library.Service.Async.Models;
using TeamNotification_Library.Service.Controls;
using TeamNotification_Library.Service.Highlighters;

namespace AvenidaSoftware.TeamNotification_Package.Controls
{
    /// <summary>
    /// Interaction logic for ModalCodeEditor.xaml
    /// </summary>
    public partial class ModalCodeEditor : Window, IShowCode
    {
        private IProvideSyntaxHighlighter<IHighlightingDefinition> syntaxHighlighter;
        private IHandleMixedEditorEvents mixedEditorEvents;

        public ModalCodeEditor()
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
            syntaxHighlighter = new AvalonSyntaxHighlighterProvider();
            mixedEditorEvents = Container.GetInstance<IHandleMixedEditorEvents>();
        }

        public Panel RefControl { get; set; }

        public string SyntaxHighlighting { get; set; }

        public string Show(string code, int programmingLanguageIdentifier)
        {
            var mce = new ModalCodeEditor
                          {
                              RefControl = RefControl,
                              rectShadowingArea = {Height = RefControl.Height*0.8, Width = RefControl.Width*0.8},
                              Visibility = Visibility.Visible
                          };
            mce.tbxInsertedText.SyntaxHighlighting = syntaxHighlighter.GetFor(programmingLanguageIdentifier);
            mce.tbxInsertedText.Text = code;
            return mce.ShowDialog() == true ? mce.tbxInsertedText.Text : "";
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
//            mixedEditorEvents.OnCodeAppended(this, new CodeWasAppended());
        }
    }
}
