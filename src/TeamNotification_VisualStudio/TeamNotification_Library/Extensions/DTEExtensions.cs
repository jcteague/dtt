using System.Linq;
using EnvDTE;
using Globals = TeamNotification_Library.Configuration.Globals;

namespace TeamNotification_Library.Extensions
{
    public static class DTEExtensions
    {
         public static int Getprogramminglanguage(this Document document)
         {
             var extension = document.FullName.Split('\\').Last().Split('.').Last().ToLower();

             switch (extension)
             {
                 case "cs":
                     return Globals.programminglanguages.CSharp;

                 case "vb":
                     return Globals.programminglanguages.VisualBasic;

                 case "js":
                     return Globals.programminglanguages.JavaScript;

                 default:
                     return Globals.programminglanguages.Unknown;
             }
         }
    }
}