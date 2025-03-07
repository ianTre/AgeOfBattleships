using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Input = UnityEngine.Input;
using Random = UnityEngine.Random;

public class EnemyMapController : MonoBehaviour
{
    private int offsetInMap = 3000; 
    public static EnemyMapController instance;
    public List<Tile> allTiles;
    GameObject enemyMap;
    GameObject EnemyShipsGO;
    public List<Ship> enemyShips;
    [SerializeField]
    GameObject radar;
    public List<Tile> enemyMapShootedTiles;
    public Tile selectedTile;
    [SerializeField]
    GameObject missSprite;
    [SerializeField]
    GameObject hitSprite;
    public List<Tile> PlayerMapShootedTiles;
    
    private void Awake() {
        instance = this;
    }
    // Start is called before the first frame update 
    void Start()
    {
        EnemyShipsGO = GameObject.Find("EnemyShips");
        enemyMap = GameObject.Find("EnemyMap");
        if(enemyMap==null || EnemyShipsGO == null)
            Debug.Log("Error at finding GameObjects in EnemyMapController. Check Start code");
        var enemyPos = enemyMap.transform.position;
        enemyMap.transform.position = new Vector3(enemyPos.x , enemyPos.y , enemyPos.z + offsetInMap);
        enemyMapShootedTiles = new List<Tile>();
        PlayerMapShootedTiles = new List<Tile>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown ("return"))
        {
            if(selectedTile == null)
            {
                Debug.Log("no Selected tile");
                return;
            }
            enemyMapShootedTiles.Add(selectedTile);
            GameObject prefab = CheckSpotInMap(selectedTile);
            Instantiate(prefab,selectedTile.transform);
            GameController.instance.UpdateStage(GameStage.IAAttackPlayerMap);
        }
        
    }

    public GameObject CheckSpotInMap(Tile tile)
    {
        Debug.Log("Checking " , tile);
        Debug.Log("Tile x " + tile.XCoord.ToString() + "Tile Z " + tile.ZCoord.ToString());
        foreach (Ship ship in enemyShips)
        {
            if(ship.ocuppiedTiles.Exists(sTile => sTile.XCoord == tile.XCoord && sTile.ZCoord == tile.ZCoord))
            {
                Debug.Log("Hit");
                return hitSprite;
            }
        }
        return missSprite;
    }

    public void GenerateEnemyMap(List<Tile> originalTiles)
    {
        foreach (Tile tile in originalTiles)
        {
            Vector3 newpos = new Vector3(tile.Xpos,tile.Ypos , tile.Zpos + offsetInMap);
            Instantiate(tile,newpos,Quaternion.identity,enemyMap.transform);
        }
        MapAllTiles();
        var enemyPos = enemyMap.transform.position;
        radar.transform.position = new Vector3(enemyPos.x+225.68f,20f,enemyPos.z - 44.5f);
     }

    public void MapAllTiles()
    {
        foreach (Transform child in enemyMap.transform)
        {
            Tile tile =child.GetComponent<Tile>();

            tile.Xpos = tile.transform.position.x;
            tile.Ypos = tile.transform.position.y;
            tile.Zpos = tile.transform.position.z;
            tile.Name = tile.gameObject.name;
            allTiles.Add(tile);
        }

        var orderedTiles = allTiles.OrderByDescending(tile => tile.Zpos).ThenBy(tile => tile.Xpos);
        
        int x = -1;
        int z = 0;
        float lastRowPos = orderedTiles.First().Zpos;
        foreach (Tile tile in orderedTiles)
        {
            if(tile.Zpos == lastRowPos) //Still on the same Row
                x++;    
            else // switched to a new row
            {
                x = 0;
                z++;
            }
            tile.XCoord = x;
            tile.ZCoord = z;
            var textMesh = tile.transform.GetComponentInChildren<TextMesh>();
            textMesh.text = "(" + z + "," + x + ")"; // (row,column)
            lastRowPos = tile.Zpos;
        }
    }

    public void GenerateEnemyShips(List<Ship> enemyShipsToAdd)
    {

        foreach (Ship ship in enemyShipsToAdd)
        {
            bool foundRightSpot = false;
            while(!foundRightSpot)
            {
                bool VerticalOrientation = Random.Range(0,2) == 0;
                int rowNumber = Random.Range(0,MapController.instance.rowSize);
                int columnNumber = Random.Range(0,MapController.instance.columSize);
                Tile tile = FindTileByCoord(rowNumber,columnNumber);
                if(tile==null)
                {
                    Debug.Log("Check GenerateEnemyShips on EnemyMapController , wrong tile coords were generated");
                    break;
                }

                if(CanShipBeDeployed(tile,ship,VerticalOrientation))
                {
                     Quaternion quaternion;
                    if(!VerticalOrientation)
                        quaternion = Quaternion.Euler(new Vector3(0,90,0)); //ROTATED TO HORIZONTAL
                    else
                        quaternion = Quaternion.identity; // ROTATED VERTICALLY
                    Vector3 newPosition= new Vector3(tile.Xpos,tile.Ypos + 6 ,tile.Zpos);
                    Ship NewShip = Instantiate(ship,newPosition,quaternion,EnemyShipsGO.transform);
                    enemyShips.Add(NewShip);
                    foundRightSpot = true;
                }

            }
            
        }
    }

        private void AddIfExists(List<Tile> list, int z, int x)
    {
        Tile tile = FindTileByCoord(z,x);
        if(tile != null)
            list.Add(tile);
    }

    /// <summary>
    /// Search for a tile by coordinates. Be aware that Z comes first (row) and X comes second (Column). 
    /// </summary>
    /// <param name="z"></param>
    /// <param name="x"></param>
    /// <returns>Returns the tile or null if its not found</returns>
    public Tile FindTileByCoord(int z,int x)
    {
        return allTiles.FirstOrDefault(tile => tile.XCoord == x && tile.ZCoord == z);
    }

    public void TileIsFocus(Tile tile)
    {
        if(enemyMapShootedTiles.Contains(tile))
        {
            return;
        }
        selectedTile?.DeHighlighMe();
        selectedTile = tile;
        selectedTile.HighlightMainColor();
    }




    public bool CanShipBeDeployed(Tile originalTile,Ship newShip,bool verticallyOriented)
    {
        
        List<Tile> tilesToBeOccuppied = new List<Tile>();
        int sizeToBeOcuppied = newShip.Size();

        int offset = 1; //1%2 == 1 . So we will increment  by [+1,-1,+2,-2,+3,-3]
        tilesToBeOccuppied.Add(FindTileByCoord(originalTile.ZCoord, originalTile.XCoord));
        sizeToBeOcuppied--;
        bool plus = true;
        offset++;

        while (sizeToBeOcuppied > 0)
        {
            int auxXCoord = originalTile.XCoord;
            int auxZCoord = originalTile.ZCoord;
            if (verticallyOriented)
            {
                if (plus)
                {
                    auxXCoord = originalTile.XCoord + (offset / 2);
                }
                else
                {
                    auxXCoord = originalTile.XCoord - (offset / 2);
                }
            }
            else
            {
                if (plus)
                {
                    auxZCoord = originalTile.ZCoord + (offset / 2);
                }
                else
                {
                    auxZCoord = originalTile.ZCoord - (offset / 2);
                }
            }
            offset++;
            plus = !plus;
            sizeToBeOcuppied--;
            AddIfExists(tilesToBeOccuppied, auxZCoord, auxXCoord);
        }
        //Add if exists will not add any tile outside the map. If the numbers dont match , you are trying to add a ship outside longer that the limits of the map
        if (tilesToBeOccuppied.Count != newShip.Size())
            return false;


        foreach (Ship ship in enemyShips)
        {
            if (ship.ocuppiedTiles.Exists(tile => tilesToBeOccuppied.Contains(tile)))
                return false;
        }

        return true;
    }

    public void IAEnemyShot()
    {
        bool virginSpot = false;
        int rowNumber=0;
        int columnNumber=0;
        while(!virginSpot)
        {
            rowNumber = Random.Range(0,MapController.instance.rowSize);
            columnNumber = Random.Range(0,MapController.instance.columSize);
            var tile = PlayerMapShootedTiles.Find(tile => tile.ZCoord == rowNumber && tile.XCoord == columnNumber);
            if(tile != null)
            {
                PlayerMapShootedTiles.Add(tile);
                virginSpot=true;
            }
        }

        PlayerController.instance.ProcessEnemyHit(rowNumber,columnNumber);
    }


}
