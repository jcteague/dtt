﻿using TeamNotification_Library.Service.Async.Models;

namespace TeamNotification_Library.Service.Async
{
    public interface IHandleLoginEvents
    {
        event CustomEventHandler<UserAttemptedLogin> UserHasLogged;
        event CustomEventHandler<UserAttemptedLogin> UserCouldNotLogIn;

        void OnLoginSuccess(object source);
        void OnLoginFail(object source);
    }
}