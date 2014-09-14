/************************************************************************************************************
Class Name:     GlobalDispatcher
Namespace:      Com.EpixCode.Util.Events.EpixEvents
Type:           Util / Singleton
Definition:
                [GlobalDispatcher] is simply a singleton used by the event system to dispatch
                global informations between objects that has no direct relations.
************************************************************************************************************/

public class GlobalDispatcher
{
private static GlobalDispatcher _instance;

private GlobalDispatcher() { }

public static GlobalDispatcher Instance
{
    get
    {
        if (_instance == null)
        {
            _instance = new GlobalDispatcher();
        }
        return _instance;
    }
}
}
