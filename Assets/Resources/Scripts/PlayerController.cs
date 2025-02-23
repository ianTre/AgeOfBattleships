using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public List<Ship> ships;
    public static PlayerController instance;
    public bool leftCtrlPressed = false;
    
    // Start is called before the first frame update

    void Awake ()
    {
        instance = this;
    }
    void Start()
    {
        
        ships = new List<Ship>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            Debug.Log("Ctrl pressed");
            leftCtrlPressed=!leftCtrlPressed;
        }
    }

    public void AddShip(Ship ship)
    {
        this.ships.Add(ship);
    }

    public void RemoveShip(Ship ship)
    {
        if(ships.Contains(ship))
            ships.Remove(ship);
    }

    
}
