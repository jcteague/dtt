// Guids.cs
// MUST match guids.h
using System;
using TeamNotification_Library.Configuration;

namespace AvenidaSoftware.TeamNotification_Package
{
    static class GuidList
    {
        public const string guidTeamNotificationPkgString = "1bafd22f-6d71-4b2f-a1d9-1c30153d9f5a";
        public const string guidTeamNotificationCmdSetString = "1f254753-7849-4cf4-8c10-ccb981e8519c";
        
        public const string guidToolWindowPersistanceString = "5a2c6998-f952-4c5b-ab7f-199aeca143f0";

//        public const string guidLoginWindowPersistanceString = "052b3fa5-4a40-4588-9614-5b99dce7120d";
        public const string guidLoginWindowPersistanceString = GlobalConstants.Guids.LoginWindowPersistanceString;

        public static readonly Guid guidTeamNotificationCmdSet = new Guid(guidTeamNotificationCmdSetString);
    };
}