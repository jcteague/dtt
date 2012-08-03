using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.FileSystem
{
    public class FileHandler : IHandleFiles
    {
        public void Write(IEnumerable<CollectionData> items)
        {
            var lines = items.Select(x => x.name + "=" + x.value).ToArray();
            Debug.WriteLine(Directory.GetCurrentDirectory());
            File.WriteAllLines(Globals.Paths.UserResource, lines);
        }

        public User Read()
        {
            if (!File.Exists(Globals.Paths.UserResource))
                return null;

            var lines = File.ReadAllLines(Globals.Paths.UserResource);
            return new User
                       {
                           id = Convert.ToInt32(GetValue(lines, 0)),
                           email = GetValue(lines, 1),
                           password = GetValue(lines, 2)
                       };
        }

        private string GetValue(string[] lines, int index)
        {
            return lines[index].Split('=')[1];
        }
    }
}