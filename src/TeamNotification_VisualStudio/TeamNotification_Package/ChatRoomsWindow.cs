﻿using System;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using AvenidaSoftware.TeamNotification_Package.Controls;
using Microsoft.VisualStudio.Shell;
using Container = TeamNotification_Library.Service.Container;

namespace AvenidaSoftware.TeamNotification_Package
{
    [Guid(GuidList.guidLoginWindowPersistanceString)]
    public class ChatRoomsWindow : ToolWindowPane
    {
        private string href = "http://dtt.local:3000/registration?&userName=Raymi&userMessage=hellothere";

        /// <summary>
        /// Standard constructor for the tool window.
        /// </summary>
        public ChatRoomsWindow() :
            base(null)
        {           
            // Set the window title reading it from the resources.
            this.Caption = Resources.ToolWindowTitle;
            // Set the image that will appear on the tab of the window frame
            // when docked with an other window
            // The resource ID correspond to the one defined in the resx file
            // while the Index is the offset in the bitmap strip. Each image in
            // the strip being 16x16.
            this.BitmapResourceID = 301;
            this.BitmapIndex = 1;

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on 
            // the object returned by the Content property.
//            base.Content = Container.GetInstance<IBuildDynamicControls>().GetContentFrom(href);
            base.Content = Container.GetInstance<IBuildDynamicControls>().GetContentFrom(href);
        }       
    }
}