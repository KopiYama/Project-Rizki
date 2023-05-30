using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class DragAndDropManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Canvas canvas;
    public Transform workspaceParent;
    private GameObject draggedObject;

    public List<GameObject> commandElements;

    private void Start()
    {
        foreach (GameObject commandElement in commandElements)
        {
            EventTrigger eventTrigger = commandElement.AddComponent<EventTrigger>();

            AddEventTriggerEntry(eventTrigger, EventTriggerType.BeginDrag, OnBeginDrag);
            AddEventTriggerEntry(eventTrigger, EventTriggerType.Drag, OnDrag);
            AddEventTriggerEntry(eventTrigger, EventTriggerType.EndDrag, OnEndDrag);
        }
    }

private void AddEventTriggerEntry(EventTrigger eventTrigger, EventTriggerType triggerType, UnityAction<PointerEventData> callback)
{
    EventTrigger.Entry entry = new EventTrigger.Entry { eventID = triggerType };
    entry.callback.AddListener((eventData) => callback((PointerEventData)eventData));
    eventTrigger.triggers.Add(entry);
}


    // Add other methods here
    public void OnBeginDrag(PointerEventData eventData)
    {
        GameObject commandElement = eventData.pointerDrag;
        if (commandElement != null)
        {
            GameObject instance = Instantiate(commandElement, commandElement.transform.position, Quaternion.identity, workspaceParent);
            draggedObject = instance;
            Graphic graphic = instance.GetComponent<Graphic>();
            if (graphic != null)
            {
                graphic.raycastTarget = false;
            }
        }
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (draggedObject != null)
        {
            Vector3 pointerPositionInWorld = Camera.main.ScreenToWorldPoint(eventData.position);
            pointerPositionInWorld.z = 0;
            draggedObject.transform.position = pointerPositionInWorld;
        }
    }


    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggedObject != null)
        {
            // You can add logic here to align the object in the sequence
            Graphic graphic = draggedObject.GetComponent<Graphic>();
            if (graphic != null)
            {
                graphic.raycastTarget = true;
            }
            draggedObject = null;
        }
    }

}
