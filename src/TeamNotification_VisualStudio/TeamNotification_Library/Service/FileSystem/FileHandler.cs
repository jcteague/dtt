using System;
using System.IO;
using System.Linq;
using TeamNotification_Library.Configuration;
using TeamNotification_Library.Models;

namespace TeamNotification_Library.Service.FileSystem
{
    public class FileHandler : IHandleFiles
    {
        private readonly IHandleEncoding base64Encoder;

        public FileHandler(IHandleEncoding base64Encoder)
        {
            this.base64Encoder = base64Encoder;
        }

        public void Write(LoginResponse response)
        {
            File.WriteAllLines(GetResourcePath(GlobalConstants.Paths.UserResource), response.SerializeForFile().Select(x => base64Encoder.Encode(x)));
        }

        public LoginResponse Read()
        {
            var userResource = GetResourcePath(GlobalConstants.Paths.UserResource);
            if (!File.Exists(userResource))
                return null;

            var lines = File.ReadAllLines(userResource).Select(x => base64Encoder.Decode(x)).ToArray();
            return new LoginResponse
                       {
                           user = new User
                                      {
                                          id = Convert.ToInt32(lines[0]),
                                          email = lines[1],
                                          password = lines[2]
                                      },
                           success = true,
                           redis = new Collection.RedisConfig
                                    {
                                        host = lines[3],
                                        port = lines[4]
                                    }
                       };
        }

        public void Delete()
        {
            File.Delete(GetResourcePath(GlobalConstants.Paths.UserResource));
        }

        private string GetResourcePath(string resource)
        {
            return Path.Combine(Path.GetTempPath(), resource);
        }
    }
}