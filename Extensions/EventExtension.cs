/************************************************************************************************************
Class Name:     EventExtension
Namespace:      Com.EpixCode.Util.Events.EpixEvents
Type:           Native Extension
Definition:
                [EventExtension] use C# Native Extension to inject event system functionalities
                inside any objects.
************************************************************************************************************/
using System;

public static class EventExtension
{
    //Local Event
    public static void AddEventListener(this object aObject, string aEvent, object aDispatcher, Action<EventObject> aMethod, int aPriority = 1)
    {
        EventDispatcher.AddEventListener(aEvent, aDispatcher, aObject, aMethod, aPriority);
    }

    public static void RemoveEventListener(this object aObject, string aEvent, object aDispatcher, Action<EventObject> aMethod)
    {
        EventDispatcher.RemoveEventListener(aEvent, aDispatcher, aObject, aMethod);
    }

    public static void RemoveAllEventListener(this object aObject)
    {
        EventDispatcher.RemoveAllEventListner(aObject);
    }

    public static void DispatchEvent(this object aObject, string aEvent)
    {
        EventDispatcher.DispatchEvent(aEvent, aObject);
    }

    public static void DispatchEvent(this object aObject, string aEvent, object[] aParams)
    {
        EventDispatcher.DispatchEvent(aEvent, aObject, aParams);
    }

    //Global Event
    public static void AddGlobalEventListener(this object aObject, string aEvent, Action<EventObject> aMethod, int aPriority = 1)
    {
        EventDispatcher.AddEventListener(aEvent, GlobalDispatcher.Instance, aObject, aMethod, aPriority);
    }

    public static void RemoveGlobalEventListener(this object aObject, string aEvent, Action<EventObject> aMethod)
    {
        EventDispatcher.RemoveEventListener(aEvent, GlobalDispatcher.Instance, aObject, aMethod);
    }

    public static void DispatchGlobalEvent(this object aObject, string aEvent)
    {
        EventDispatcher.DispatchEvent(aEvent, GlobalDispatcher.Instance);
    }

    public static void DispatchGlobalEvent(this object aObject, string aEvent, object[] aParams)
    {
        EventDispatcher.DispatchEvent(aEvent, GlobalDispatcher.Instance, aParams);
    }
}
