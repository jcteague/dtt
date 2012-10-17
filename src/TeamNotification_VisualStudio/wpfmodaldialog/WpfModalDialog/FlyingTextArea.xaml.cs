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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace S2Snext.GUI.Dialogs
{
    /// <summary>
    /// Interaction logic for FlyingTextArea.xaml
    /// </summary>
    public partial class FlyingTextArea : UserControl
    {
        //public FlyingTextArea()
        //{
        //    InitializeComponent();
        //}

        public FlyingTextArea(string text)
        {
            InitializeComponent();
            this.tbxInsertedText.Text = text;
        }
        public String GetText()
        {
            return tbxInsertedText.Text;
        }
    }
}
