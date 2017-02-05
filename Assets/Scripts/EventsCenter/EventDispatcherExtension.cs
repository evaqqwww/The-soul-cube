using System;

public static class EventDispatcherExtension
{
    public static bool dispatchEvent(this EventDispatcher self, String eventName)
    {
        return self.dispatchEvent(new HEvent(eventName));
    }

    public static bool dispatchEvent(this EventDispatcher self, String eventName, params object[] args)
    {
        return self.dispatchEvent(new HEventWithParams(eventName, args));
    }
}
