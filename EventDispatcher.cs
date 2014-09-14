/************************************************************************************************************
Class Name:     EventDispatcher
Namespace:      Com.EpixCode.Util.Events.EpixEvents
Type:           Util
Definition:
                The [EventDispatcher] manage all [EventObject]. It his the internal facade
                to communicate with the event system.
************************************************************************************************************/


using System;
using System.Collections.Generic;
    public static class EventDispatcher
    {
        private static WeakDictionary<object, Dictionary<string, EventObject>> _eventObjDict;
        private static WeakDictionary<object, List<EventObject>> _listenerDict;
        
        static EventDispatcher()
        {
            _eventObjDict = new WeakDictionary<object, Dictionary<string, EventObject>>();
            _listenerDict = new WeakDictionary<object, List<EventObject>>();
        }

        /*************************************************************************************
         * PUBLIC: Listener logic
         *************************************************************************************/
        public static void AddEventListener(string aEvent, object aDispatcher, object aListener, Action<EventObject> aMethod, int aPriority = 1)
        {
            if (aDispatcher == null || aListener == null)
            {
                return;
            }

            if (!_eventObjDict.ContainsKey(aDispatcher)) //If the dispatcher has never been registered, create a entry for it.
            {
                RegistertDispatcher(aDispatcher);
            }

            if (!IsDispatcherEventRelationRegistered(aDispatcher, aEvent)) //If this event has never been registered for that dispatcher create a new EventObject to manage it.
            {
                RegistertDispatcherEventRelation(aDispatcher, aEvent);
            }

            EventObject eventObj = _eventObjDict[aDispatcher][aEvent];
            eventObj.RegisterEventListener(aListener, aMethod, aPriority);
            AddListenerToCache(aListener, eventObj);
        }

        public static void RemoveEventListener(string aEvent, object aDispatcher, object aListener, Action<EventObject> aMethod)
        {
            if (aDispatcher == null || aListener == null)
            {
                return;
            }

            if (!_eventObjDict.ContainsKey(aDispatcher))
            {
                return;
            }

            if (!IsDispatcherEventRelationRegistered(aDispatcher, aEvent))
            {
                return;
            }

            EventObject eventObj = _eventObjDict[aDispatcher][aEvent];
            RemoveListenerFromCache(aListener, eventObj);
            eventObj.UnregisterEventListener(aListener, aMethod);
        }

        private static void AddListenerToCache(object aListener, EventObject aEvent)
        {
            if (!_listenerDict.ContainsKey(aListener))
            {
                _listenerDict.Add(aListener, new List<EventObject>());
            }

            if (!_listenerDict[aListener].Contains(aEvent))
            {
                _listenerDict[aListener].Add(aEvent);
            }
        }

        private static void RemoveListenerFromCache(object aListener, EventObject aEvent)
        {
            if (_listenerDict.ContainsKey(aListener))
            {
                if (_listenerDict[aListener].Contains(aEvent))
                {
                    _listenerDict[aListener].Remove(aEvent);
                }
            }
        }

        public static void RemoveAllEventListner(object aListener)
        {
            if (_listenerDict.ContainsKey(aListener))
            {
                List<EventObject> eventList = _listenerDict[aListener];
                for (int i = eventList.Count - 1; i >= 0; i--)
                {
                    eventList[i].UnregisterEventListener(aListener);
                    RemoveListenerFromCache(aListener, eventList[i]);
                }
            }
        }

        /*************************************************************************************
         * PUBLIC: Dispatcher logic
         *************************************************************************************/
        public static void RegistertDispatcher(object aDispatcher)
        {
            _eventObjDict.Add(aDispatcher, new Dictionary<string, EventObject>());
        }

        public static void RegistertDispatcherEventRelation(object aDispatcher, string aEvent)
        {
            EventObject eventObject = new EventObject(aEvent, aDispatcher);
            _eventObjDict[aDispatcher].Add(aEvent, eventObject);
            eventObject.RegisterDestroyCallback(OnEventObjectDestroy);
        }

        public static void DispatchEvent(string aEvent, object aDispatcher, object[] aParams)
        {
            //Check if someone is listening, if not, there is no point to disptach
            if (!_eventObjDict.ContainsKey(aDispatcher))
            {
                return;
            }

            if (!IsDispatcherEventRelationRegistered(aDispatcher, aEvent))
            {
                return;
            }

            Dispatch(aDispatcher, aEvent, aParams);
        }

        public static void DispatchEvent(string aEvent, object aDispatcher)
        {
            if (!_eventObjDict.ContainsKey(aDispatcher))
            {
                return;
            }
            if (!IsDispatcherEventRelationRegistered(aDispatcher, aEvent))
            {
                return;
            }

            Dispatch(aDispatcher, aEvent);
        }

        private static void Dispatch(object aDispatcher, string aEvent, object[] aParams = null)
        {
            if (aParams == null)
            {
                _eventObjDict[aDispatcher][aEvent].Dispatch();
            }
            else
            {
                _eventObjDict[aDispatcher][aEvent].Dispatch(aParams);
            }

            _eventObjDict.Clean();
	}       


        /*************************************************************************************
         * EVENT
         *************************************************************************************/
        private static void OnEventObjectDestroy(EventObject aEventObject)
        {
            if (aEventObject == null)
            {
                return;
            }

            object dispatcher = aEventObject.Dispatcher;

            if (!_eventObjDict.ContainsKey(dispatcher))
            {
                return;
            }

            Dictionary<string, EventObject> eventObjectDictionary = _eventObjDict[dispatcher];

            eventObjectDictionary.Remove(aEventObject.EventName);
            
            if (eventObjectDictionary.Count <= 0)
            {
                _eventObjDict.Remove(dispatcher);
            }
        }

        /*************************************************************************************
         * UTIL
         *************************************************************************************/
        private static bool IsDispatcherEventRelationRegistered(object aDispatcher, string aEvent)
        {
            if (!_eventObjDict.ContainsKey(aDispatcher))
            {
                return false;
            }

            return _eventObjDict[aDispatcher].ContainsKey(aEvent);
        }
    }
