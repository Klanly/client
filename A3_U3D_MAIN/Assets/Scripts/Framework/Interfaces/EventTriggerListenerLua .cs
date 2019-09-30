using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using SimpleFramework.Manager;
using LuaInterface;
namespace SimpleFramework
{
    public class EventTriggerListenerLua : UnityEngine.EventSystems.EventTrigger
    {
        //public delegate void VoidDelegate(GameObject go);
        //public delegate void VectorDelegate(GameObject go, Vector2 delta);
        public LuaFunction onClick;
        public LuaFunction onPointClick;
        public LuaFunction onDown;
        public LuaFunction onEnter;
        public LuaFunction onExit;
        public LuaFunction onUp;
        public LuaFunction onMove;
        public LuaFunction onSelect;
        public LuaFunction onUpdateSelect;
        public LuaFunction onDragIn;
        public LuaFunction onDrag;
        public LuaFunction onDragOut;
        public LuaFunction onDragEnd;
        public LuaFunction onInPoDrag;


      



        static public EventTriggerListenerLua Get(GameObject go)
        {
            if (go == null)
            {
                Debug.LogError("EventTriggerListener_go_is_NULL");
                return null;
            }
            else
            {
                EventTriggerListenerLua listener = go.GetComponent<EventTriggerListenerLua>();
                if (listener == null) listener = go.AddComponent<EventTriggerListenerLua>();
                return listener;
            }
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (onDragIn != null) onDragIn.Call(gameObject);
        }

        //public override void OnScroll(PointerEventData eventData)
        //{
        //    base.OnScroll(eventData);
        //}

        public override void OnInitializePotentialDrag(PointerEventData eventData)
        {
            if (onInPoDrag != null) onInPoDrag.Call(gameObject, eventData.position);
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (onDrag != null) onDrag.Call(gameObject, eventData.delta);
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            if (onDragOut != null) onDragOut.Call(gameObject);
            if (onDragEnd != null) onDragEnd.Call(gameObject, eventData.position);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            if (onClick != null) onClick.Call(gameObject);
            if (onPointClick != null) onPointClick.Call(gameObject, eventData.position);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (onDown != null) onDown.Call(gameObject);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (onEnter != null) onEnter.Call(gameObject);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            if (onExit != null) onExit.Call(gameObject);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            if (onUp != null) onUp.Call(gameObject);
        }

        public override void OnSelect(BaseEventData eventData)
        {
            if (onSelect != null) onSelect.Call(gameObject);
        }

        public override void OnUpdateSelected(BaseEventData eventData)
        {
            if (onUpdateSelect != null) onUpdateSelect.Call(gameObject);
        }

        public override void OnMove(AxisEventData eventData)
        {
            if (onMove != null) onMove.Call(gameObject, eventData.moveVector);
        }

        public void clearAllListener()
        {
            onClick = null;
            onDown = null;
            onEnter = null;
            onExit = null;
            onUp = null;
            onSelect = null;
            onUpdateSelect = null;
            onDrag = null;
            onDragOut = null;
            onDragIn = null;
            onMove = null;
            onInPoDrag = null;
            onDragEnd = null;
            Destroy(gameObject.GetComponent<EventTriggerListener>());
        }
    }
}
