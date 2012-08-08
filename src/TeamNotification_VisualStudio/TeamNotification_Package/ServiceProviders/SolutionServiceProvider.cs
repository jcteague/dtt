using System;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace AvenidaSoftware.TeamNotification_Package.ServiceProviders
{
    public class SolutionServiceProvider : IServiceProvider
    {
        public object GetService(Type serviceType)
        {
            return (IVsSolution)Package.GetGlobalService(typeof(SVsSolution));
        }
    }
}