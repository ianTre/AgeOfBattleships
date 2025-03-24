using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEditor.U2D.Aseprite;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class SelectionController : MonoBehaviour 
{
    // Start is called before the first frame update
   public SelectionController selectionlight;
   public Tile tile;
   public ShipSoundController shipSounds;

    void Start()
    {
        selectionlight = FindAnyObjectByType<SelectionController>(); 
        tile = FindAnyObjectByType<Tile>();
        shipSounds = FindAnyObjectByType<ShipSoundController>(); 
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SelectionLightOn(Tile tile, Ship ship)
    {      
       GameObject selectionlight = GameObject.Find("SelectionLight");
       selectionlight.GetComponent<Light>().enabled = true;
       selectionlight.transform.position = new Vector3(tile.Xpos,31, tile.Zpos);
    }
     
    public void SelectionLightOff(Ship ship)
    {
     shipSounds.PlayShipSelectionSoundOff(ship);
     GameObject selectionlight = GameObject.Find("SelectionLight");
     selectionlight.GetComponent<Light>().enabled = false;
    }

}
