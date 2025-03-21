using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSoundController : MonoBehaviour
{
    
    // Start is called before the first frame updateusing System;

    [SerializeField]
    AudioClip shipDeploySound;
    
    [SerializeField]
    AudioClip shipShootingSound; 
   
    [SerializeField]
    AudioClip waterSplashSound; 


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     
    }

    public void ReproduceShipDeploySound(Ship ship)
    {
        AudioSource audio = ship.GetComponent<AudioSource>();
        audio.clip = shipDeploySound;
        audio.Play();
    }

    /*public void ReproduceShootingSound(Ship ship)
    {
        shipDeploySound.Play();
    }

    public void ReproduceWaterSplashSound(Ship ship)
    {
        shipDeploySound.Play();
    }


*/
}

