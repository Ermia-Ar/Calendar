﻿using System;

namespace Core.Application.Common.Event;

public class OnlineClientEventArgs : EventArgs
{
    public IOnlineClient Client { get; }

    public OnlineClientEventArgs(IOnlineClient client)
    {
        Client = client;
    }
}
