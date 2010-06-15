using System;

namespace BoxeeStarter.Utilities.Processes
{
    public interface INotifier
    {
        event EventHandler NotifyMe;
    }
}