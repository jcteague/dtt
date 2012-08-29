using System.Linq;
using EnvDTE;
using Globals = TeamNotification_Library.Configuration.Globals;

namespace TeamNotification_Library.Extensions
{
    public static class DTEExtensions
    {
         public static int GetProgrammingLanguage(this Document document)
         {
             var extension = document.FullName.Split('\\').Last().Split('.').Last().ToLower();

             switch (extension)
             {
                 case "cs":
                     return Globals.ProgrammingLanguages.CSharp;

                 case "vb":
                     return Globals.ProgrammingLanguages.VisualBasic;

                 case "js":
                     return Globals.ProgrammingLanguages.JavaScript;

                 default:
                     return Globals.ProgrammingLanguages.Unknown;
             }
         }
    }
}