using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor.Rendering;
using UnityEditor.U2D.Aseprite;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEditor.VersionControl;
using System.Numerics;

public class ExplosionController : MonoBehaviour
{
    public Tile tile;
    public ShipSoundController shipSounds;
    public GameObject explosions;

    // Start is called before the first frame update
    void Start()
    {
        explosions = GameObject.Find("Explosions");
        shipSounds = FindAnyObjectByType<ShipSoundController>(); 
    }

    // Update is called once per frame
    void Update()
    {
    }

     public void ShowExplosion(Ship ship, string key)
    {
        if(key == "down")
        {
        MoveCannonforward(ship);     
        shipSounds.PlayExplosionSound(ship);
        }
        else
        {
        MoveCannonbackward(ship);     
        }
    }

    private void MoveCannonbackward(Ship ship)
    {
       Transform cannon;
       try
       {
        cannon = ship.transform.Find("Weapons").Find("Large Turret").Find("Large Cannons");
       }
       catch
       {
        try
        {
            cannon = ship.transform.Find("Weapons").Find("Medium Turret").Find("Medium Cannons");
        }
        catch
        {
            cannon = ship.transform.Find("Weapons").Find("Small Turret").Find("Small Cannons");

        }
       }
       
  
       UnityEngine.Vector3 startPosition = new UnityEngine.Vector3(cannon.transform.position.x,cannon.transform.position.y,cannon.transform.position.z);
       UnityEngine.Vector3 finalPosition = new UnityEngine.Vector3(cannon.transform.position.x,cannon.transform.position.y,cannon.transform.position.z + 2);
       cannon.transform.position = UnityEngine.Vector3.LerpUnclamped(startPosition, finalPosition, 1f);
    }

    private void MoveCannonforward(Ship ship)
    {

       Transform cannon;
       try
       {
        cannon = ship.transform.Find("Weapons").Find("Large Turret").Find("Large Cannons");
       }
       catch
       {
        try
        {
            cannon = ship.transform.Find("Weapons").Find("Medium Turret").Find("Medium Cannons");
        }
        catch
        {
            cannon = ship.transform.Find("Weapons").Find("Small Turret").Find("Small Cannons");

        }

       }    
       
        UnityEngine.Vector3 startPosition = new UnityEngine.Vector3(cannon.transform.position.x,cannon.transform.position.y,cannon.transform.position.z);
       UnityEngine.Vector3 finalPosition = new UnityEngine.Vector3(cannon.transform.position.x,cannon.transform.position.y,cannon.transform.position.z - 2);
       cannon.transform.position = UnityEngine.Vector3.LerpUnclamped(startPosition, finalPosition, 1f);
    }
}

