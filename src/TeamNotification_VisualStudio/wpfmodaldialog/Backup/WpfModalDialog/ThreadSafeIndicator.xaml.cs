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
	/// Interaction logic for ThreadSafeIndicator.xaml
	/// </summary>
	public partial class ThreadSafeIndicator : UserControl
	{
		public ThreadSafeIndicator()
		{
	
			InitializeComponent();
			var gifStream = this.GetType().Assembly.GetManifestResourceStream("TaskBlend.Dialogs.loader.gif");
			
			var bitmap = new BitmapImage(new Uri("Images/loader.gif",UriKind.Relative));
			GifImg.Source = bitmap;
		}
	}
}
