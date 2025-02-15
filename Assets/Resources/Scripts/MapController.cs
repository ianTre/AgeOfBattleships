using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapController : MonoBehaviour
{
    GameObject map;
    List<Tile> AllTiles = new List<Tile>();
    public int mapSize = 0;
    public int rowSize;
    public int columSize;
    
    //IMPORTANT: WE WILL ALWAYS FOLLOW THE PATTERN (ROW , COLUMN) , so it will be (Z , X) .

    void Start()
    {
        map = this.gameObject;
        MapAllTiles();
        this.rowSize = AllTiles.OrderBy(tile => tile.ZCoord).Last().ZCoord;
        this.columSize = AllTiles.OrderBy(tile => tile.XCoord).Last().XCoord;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
        private void MapAllTiles()
    {
        GameObject infoText = GameObject.Find("InfoText");
        foreach (Transform child in map.transform)
        {
            Debug.Log("Tile");
            Tile tile =child.GetComponent<Tile>();

            tile.Xpos = tile.transform.position.x;
            tile.Ypos = tile.transform.position.y;
            tile.Zpos = tile.transform.position.z;
            tile.Name = tile.gameObject.name;
            AllTiles.Add(tile);
            GameObject duplicateInfoText = GameObject.Instantiate(infoText);
            duplicateInfoText.transform.position = new Vector3(tile.Xpos - 0.25f ,tile.Ypos,tile.Zpos + 0.15f);
            duplicateInfoText.transform.parent = tile.transform;
        }
        var orderedTiles = AllTiles.OrderByDescending(tile => tile.Zpos).ThenBy(tile => tile.Xpos);
        
        int x = -1;
        int z = 0;
        float lastRowPos = orderedTiles.First().Zpos;
        foreach (Tile tile in orderedTiles)
        {
            mapSize++;
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
            tile.Id = mapSize;
        }
    }

    public List<Tile> getNeighborhoods(Tile tile)
    {
        int lowerX,higherX;
        int lowerZ,higherZ;
        lowerX= tile.XCoord-1;
        higherX= tile.XCoord+1;
        lowerZ= tile.ZCoord-1;
        higherZ= tile.ZCoord+1;   

        //Secuence will be 1 |  2   | 3
        //                 4 | tile | 6
        //                 7 |  8   | 9
        
        List<Tile> neighborhoods = new List<Tile>();
        AddIfExists(neighborhoods,lowerZ,lowerX);       // 1
        AddIfExists(neighborhoods,lowerZ,tile.XCoord);  // 2
        AddIfExists(neighborhoods,lowerZ,higherX);      // 3 

        AddIfExists(neighborhoods,tile.ZCoord,lowerX);  // 4
                                                        // Tile
        AddIfExists(neighborhoods,tile.ZCoord,higherX); // 6

        AddIfExists(neighborhoods,higherZ,lowerX);      // 7
        AddIfExists(neighborhoods,higherZ,tile.XCoord); // 8
        AddIfExists(neighborhoods,higherZ,higherX);     // 9

        return neighborhoods;

    }

    private void AddIfExists(List<Tile> neighborhoods, int z, int x)
    {
        Tile tile = FindTileByCoord(z,x);
        if(tile != null)
            neighborhoods.Add(tile);
    }

    public Tile FindTileByCoord(int z,int x)
    {
        return AllTiles.FirstOrDefault(tile => tile.XCoord == x && tile.ZCoord == z);
    }
}
