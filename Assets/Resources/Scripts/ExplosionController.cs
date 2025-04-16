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
}

