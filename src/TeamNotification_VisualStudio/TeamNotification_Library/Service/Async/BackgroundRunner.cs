using System;
using System.ComponentModel;

namespace TeamNotification_Library.Service.Async
{
    public class BackgroundRunner : IRunInBackgroundWorker
    {
        public void Run(Action action)
        {
            var backgroundWorker = new BackgroundWorker();
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.DoWork += (o, args) => 
                                           {
                                               action();
                                               args.Cancel = true;
                                           };
            backgroundWorker.RunWorkerAsync();                        
        }
    }
}