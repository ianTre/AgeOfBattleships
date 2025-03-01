using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public List<Ship> ships;
    public static PlayerController instance;
    public bool leftCtrlPressed = false;
    public bool cameraOnMain = true;
    
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

        if(Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("M was pressed");
            cameraOnMain=!cameraOnMain;
            UpdateCameraPosition();
            SetAtackMode(true);
        }
    }

    private void SetAtackMode(bool v)
    {
        AtackRadarCanvasController.instance.CreateGrid(10,10);
    }

    private void UpdateCameraPosition()
    {
        var CameraAnchor = GameObject.Find("CameraAnchor");
        if(CameraAnchor==null)
        {
            Debug.Log("CameraAnchor GameObject is null , check UpdateCameraPosition on PlayerController");
            return;
        }
        Vector3 newPosition;
        if(cameraOnMain)
            newPosition = MapController.instance.AllTiles.First().transform.position;
        else
        {
            var tile = EnemyMapController.instance.allTiles.FirstOrDefault();
            if(tile==null)
                return;
            newPosition = tile.transform.position;
            Debug.Log("newPos" + newPosition);
        }

        CameraAnchor.transform.position = new Vector3(newPosition.x,newPosition.y,newPosition.z);
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

    public void EndDeployStage()
    {
        Debug.Log("End Deploy Stage");
        List<Tile> playerTiles = MapController.instance.AllTiles;
        EnemyMapController.instance.GenerateEnemyMap(playerTiles);
        EnemyMapController.instance.GenerateEnemyShips(ships);

    }

    
}
