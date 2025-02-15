using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragUIShips : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    [SerializeField] GameObject PrefabToInstantiate;
    [SerializeField] RectTransform UIDragElement;
    [SerializeField] RectTransform Canvas;

    private Vector2 mOriginalLocalPointerPosition;
    private Vector3 mOriginalPanelLocalPosition;
    private Vector2 mOriginalPosition;
    
    // Start is called before the first frame update
    void Start() {
        mOriginalPosition = UIDragElement.localPosition;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        mOriginalPanelLocalPosition = UIDragElement.localPosition; 
        RectTransformUtility.ScreenPointToLocalPointInRectangle(Canvas,eventData.position,eventData.pressEventCamera,out mOriginalLocalPointerPosition);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPointerPosition;
        if(RectTransformUtility.ScreenPointToLocalPointInRectangle(
            Canvas,
            eventData.position,
            eventData.pressEventCamera,
            out localPointerPosition))
            {
                Vector3 offsetToOriginal = localPointerPosition - mOriginalLocalPointerPosition;
                UIDragElement.localPosition = mOriginalPanelLocalPosition + offsetToOriginal;
            }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        StartCoroutine(Coroutine_MoveUIElement(UIDragElement, mOriginalPosition , 0.5f));
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, 1000.0f)) 
        { 
            Vector3 worldPoint = hit.point;
            CreateObject(worldPoint);
        }
    }

    private void CreateObject(Vector3 position)
    {
        if(PrefabToInstantiate == null)
        {
            Debug.Log("No prefab to instatiate");
            return;
        }

        GameObject obj = Instantiate(PrefabToInstantiate, position, Quaternion.identity);
    }

    IEnumerator Coroutine_MoveUIElement(RectTransform r , Vector2 targetPosition , float duration = 0.1f)
    {
        float elapsedTime = 0;
        Vector2 startIntPos = r.localPosition;

        while (elapsedTime < duration)
        {
            r.localPosition = Vector2.Lerp(startIntPos , targetPosition , (elapsedTime/duration));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
    }
}
