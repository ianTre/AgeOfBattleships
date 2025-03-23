using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEditor.Rendering;
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

    public void SelectionLightOn(Tile tile, Ship ship, bool hasfocus)
    { 
     
      if(ship.hasfocus)
      {
        GameObject selectionlight = GameObject.Find("SelectionLight");
        shipSounds.PlayShipSelectionSound(ship);       

            switch (tile.ZCoord)
            {
                case 0:
                selectionlight.transform.position = new Vector3(tile.Xpos,31, 90);
                    break;
                case 1:
                selectionlight.transform.position = new Vector3(tile.Xpos,31, 60);
                    break;
                case 2:
                selectionlight.transform.position = new Vector3(tile.Xpos,31, 30);
                    break;
                case 3:
                selectionlight.transform.position = new Vector3(tile.Xpos,31, 0);
                    break;
                case 4:
                selectionlight.transform.position = new Vector3(tile.Xpos,31, -30);
                    break;
                case 5:
                selectionlight.transform.position = new Vector3(tile.Xpos,31, -60);
                    break;
                case 6:
                selectionlight.transform.position = new Vector3(tile.Xpos,31, -90);
                    break;
                case 7:
                selectionlight.transform.position = new Vector3(tile.Xpos,31, -120);
                    break;
                case 8:
                selectionlight.transform.position = new Vector3(tile.Xpos,31, -150);
                    break;
                case 9:
                selectionlight.transform.position = new Vector3(tile.Xpos,31, -180);
                    break;

                    
            }
        selectionlight.GetComponent<Light>().enabled = true;
        }
              else
              {
                ship.hasfocus = false;
        

              }
      }
     
     

    
    public void SelectionLightOff(Ship ship)
    {
     shipSounds.PlayShipSelectionSoundOff(ship);
     GameObject selectionlight = GameObject.Find("SelectionLight");
     selectionlight.GetComponent<Light>().enabled = false;
     
    }

}
