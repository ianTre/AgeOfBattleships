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

    public List<Tile> ocuppiedTiles;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
