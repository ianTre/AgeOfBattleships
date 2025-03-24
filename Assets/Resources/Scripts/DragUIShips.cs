using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Numerics;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class DragUIShips : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    [SerializeField] RectTransform UIDragElement;
    [SerializeField] RectTransform Canvas;
    public ShipInformationScriptableObject ScriptableObject;
    private Vector2 mOriginalLocalPointerPosition;
    private Vector3 mOriginalPanelLocalPosition;
    private Vector2 mOriginalPosition;
    private Color availableColor = new Color32(138,255,117,255);
    private Color notAvailableColor = new Color32(255,51,30,255);
    public ShipSoundController shipSounds; 
    public SelectionController selectionController;
    private Tile tiletolight;
    
    // Start is called before the first frame update
    void Start()
    {
        selectionController = FindAnyObjectByType<SelectionController>(); 
        shipSounds = FindAnyObjectByType<ShipSoundController>(); 
        mOriginalPosition = UIDragElement.localPosition;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Ship ship = ScriptableObject.PrefabToInstantiate.GetComponent<Ship>();
        if(!PlayerController.instance.CanShipBeDeployed(ship,ScriptableObject.quantity))
        {
            eventData.pointerDrag = null;
            return;
        }

        mOriginalPanelLocalPosition = UIDragElement.localPosition; 
        RectTransformUtility.ScreenPointToLocalPointInRectangle(Canvas,eventData.position,eventData.pressEventCamera,out mOriginalLocalPointerPosition);
        MapController.instance.SetDragAndDroping(true);
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

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit, 1000.0f)) 
        { 
            //Vector3 worldPoint = hit.point;
            if(hit.collider.gameObject.tag == "WaterTile")
            {
                Tile impactedTile = hit.collider.gameObject.GetComponent<Tile>();
                Ship ship = ScriptableObject.PrefabToInstantiate.GetComponent<Ship>();
                MapController.instance.CanShipBeDeployed(impactedTile,ship);
                tiletolight = impactedTile;
                
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        StartCoroutine(Coroutine_MoveUIElement(UIDragElement, mOriginalPosition , 0.5f));
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, 1000.0f)) 
        { 
            //Vector3 worldPoint = hit.point;
            if(hit.collider.gameObject.tag == "WaterTile")
            {
                Tile impactedTile = hit.collider.gameObject.GetComponent<Tile>();
                CreateObject(impactedTile);
            }
        }
        MapController.instance.SetDragAndDroping(false);
        MapController.instance.CleanTiles();
        PlayerController.instance.leftCtrlPressed=false;
    }

    private void CreateObject(Tile tile)
    {
        if(ScriptableObject.PrefabToInstantiate == null)
        {
            Debug.Log("No prefab to instatiate");
            return;
        }

        Vector3 position = new Vector3(tile.Xpos,tile.Ypos + 6, tile.Zpos);
        GameObject obj;
        
        Quaternion quaternion;
        if(PlayerController.instance.leftCtrlPressed)
            quaternion = Quaternion.Euler(new Vector3(0,90,0)); //ROTATED TO HORIZONTAL
        else
            quaternion = Quaternion.identity; // ROTATED VERTICALLY

        if(!MapController.instance.CanShipBeDeployed(tile,ScriptableObject.PrefabToInstantiate.GetComponent<Ship>()))
            return;
        obj = Instantiate(ScriptableObject.PrefabToInstantiate, position, quaternion);
        
        Ship ship = obj.GetComponent<Ship>();
        PlayerController.instance.AddShip(ship);
        shipSounds.PlayShipDeploySound(ship);

        if(!PlayerController.instance.CanShipBeDeployed(ship,ScriptableObject.quantity))
        {
            this.transform.parent.Find("Outter").GetComponent<Image>().color = notAvailableColor;
        }
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

    public void EnablePanel()
    {
        this.transform.parent.Find("Outter").GetComponent<Image>().color = availableColor;
    }
}
