using System;
using System.Windows;
using System.Windows.Documents;
using System.Collections.ObjectModel;
using WPFTaskbarNotifier;

namespace AvenidaSoftware.TeamNotification_Package.Controls
{
    /// <summary>
    /// This is just a mock object to hold something of interest. 
    /// </summary>
    public class NotifyObject
    {
        public NotifyObject(string message, string title)
        {
            this.message = message;
            this.title = title;
        }

        private string title;
        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        private string message;
        public string Message
        {
            get { return this.message; }
            set { this.message = value; }
        }
    }

    /// <summary>
    /// This is a TaskbarNotifier that contains a list of NotifyObjects to be displayed.
    /// </summary>
    public partial class TaskbarNotifierWindow: TaskbarNotifier
    {
        private EnvDTE.DTE dte;
        public TaskbarNotifierWindow()
        {
            InitializeComponent();
        }

        public TaskbarNotifierWindow(EnvDTE.DTE dte)
        {
            InitializeComponent();
            this.MouseLeftButtonDown += Click_Handler;
            this.dte = dte;
        }

        private void Click_Handler(object sender, EventArgs e)
        {
            dte.MainWindow.Activate();
        }

        private ObservableCollection<NotifyObject> notifyContent;
        /// <summary>
        /// A collection of NotifyObjects that the main window can add to.
        /// </summary>
        public ObservableCollection<NotifyObject> NotifyContent
        {
            get
            {
                if (this.notifyContent == null)
                {
                    // Not yet created.
                    // Create it.
                    this.NotifyContent = new ObservableCollection<NotifyObject>();
                }

                return this.notifyContent;
            }
            set
            {
                this.notifyContent = value;
            }
        }

        private void Item_Click(object sender, EventArgs e)
        {
            var hyperlink = sender as Hyperlink;

            if(hyperlink == null) return;

            var notifyObject = hyperlink.Tag as NotifyObject;
            if(notifyObject != null)
            {
                dte.MainWindow.Activate();
                //MessageBox.ShowModalCodeEditor("\"" + notifyObject.Message + "\"" + " clicked!");
            }
        }

        private void HideButton_Click(object sender, EventArgs e)
        {
            this.ForceHidden();
        }
    }
}