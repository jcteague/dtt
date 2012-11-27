using System.Linq;
using EnvDTE;
using TeamNotification_Library.Configuration;

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
                     return GlobalConstants.ProgrammingLanguages.CSharp;

                 case "vb":
                     return GlobalConstants.ProgrammingLanguages.VisualBasic;

                 case "js":
                     return GlobalConstants.ProgrammingLanguages.JavaScript;
                 case "cpp":
                 case "h":
                     return GlobalConstants.ProgrammingLanguages.Cpp;

                 default:
                     return GlobalConstants.ProgrammingLanguages.Unknown;
             }
         }
    }
}