﻿namespace SkySwordKill.NextModEditor.Event;

public interface IEventArgs
{
    object Sender { get; set; }
}

public class EventArgs : IEventArgs
{
    public object Sender { get; set; }
}