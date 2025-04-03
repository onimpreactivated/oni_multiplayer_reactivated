namespace MultiplayerMod.Events;

/// <summary>
/// MultiplayerMod's event handler with a <see cref="EventArgs"/> parameter.
/// Called when an event is with the specified <see cref="EventArgs"/> triggered .
/// </summary>
/// <typeparam name="TEventArgs">The type of the <see cref="EventArgs"/> of the event.</typeparam>
public delegate void OniEventHandlerTEventArgs<in TEventArgs>(TEventArgs ev)
    where TEventArgs : EventArgs;
