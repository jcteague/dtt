﻿namespace TeamNotification_Library.Service.Async
{
    public interface IHandleLoginEvents
    {
        event CustomEventHandler UserHasLogged;
        event CustomEventHandler UserCouldNotLogIn;

        void OnLoginSuccess(object source);
        void OnLoginFail(object source);
    }
}