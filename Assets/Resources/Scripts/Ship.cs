using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{

    public int HealtPoints;
    public int attackPoints;
    public int defencePoints;
    public int Speed;
    public Mode SelectedAction;
    public bool hasfocus = false;
    public ShipType shipType;
    public List<Tile> ocuppiedTiles;


    // Start is called before the first frame update
    void Start()
    {
        ocuppiedTiles = new List<Tile>();
    }

    public void AddOcuppiedTile(Tile tile)
    {
        if(!ocuppiedTiles.Contains(tile)) 
        { 
            ocuppiedTiles.Add(tile);
        }
    }

    public int Size()
    {
        switch (shipType)
        {
            case ShipType.Battleship:
                return 5;
            case ShipType.Corvette:
                return 2;
            case ShipType.Crusier:
                return 3;
            case ShipType.Destroyer:
                return 4;
            case ShipType.Frigate:
                return 3;
            default:
                return 1;
        }
    }

    public void RemoveOcuppiedTile(Tile tile) 
    {
        if (!ocuppiedTiles.Contains(tile))
        {
            ocuppiedTiles.Remove(tile);
        }
    }

    // Update is called once per frame
    void Update()
    {
      if(Input.GetKey(KeyCode.Delete) && hasfocus)
      {
        PlayerController.instance.RemoveShip(this);
        Destroy(this.gameObject);

      } 
    }

    public void OnMouseOver()
    {
        hasfocus = true;
    }

    public void OnMouseExit()
    {
        hasfocus = false;
    }

}

public enum Mode
{
    None = 0,
    Attack = 1,
    Deffend = 2,
    Move = 3
}

public enum ShipType
{
    Frigate = 1,
    Destroyer = 2,
    Crusier = 3,
    Corvette = 4,
    Battleship = 5
}
