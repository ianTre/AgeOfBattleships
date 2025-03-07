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
    public Dictionary<ShipType,string> panelForShip;
    
    // Start is called before the first frame update

    void Awake ()
    {
        instance = this;
    }
    void Start()
    {
        ships = new List<Ship>();
        panelForShip = new Dictionary<ShipType, string>();
        AddConstantReferences();
    }

    private void AddConstantReferences()
    {
        panelForShip.Add(ShipType.Battleship,"Ship1Panel");
        panelForShip.Add(ShipType.Crusier,"Ship2Panel");
        panelForShip.Add(ShipType.Corvette,"Ship4Panel");
        panelForShip.Add(ShipType.Destroyer,"Ship3Panel");
        panelForShip.Add(ShipType.Frigate,"Ship5Panel");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            leftCtrlPressed=!leftCtrlPressed;
        }

        /*if(Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("M was pressed");
            cameraOnMain=!cameraOnMain;
            UpdateCameraPosition();
            SetAtackMode(true);
        }*/
    }

    /*private void SetAtackMode(bool v)
    {
        AtackRadarCanvasController.instance.CreateGrid(MapController.instance.rowSize,MapController.instance.columSize);
    }*/

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
        }

        CameraAnchor.transform.position = new Vector3(newPosition.x,newPosition.y,newPosition.z);
    }

    public void AddShip(Ship ship)
    {
        this.ships.Add(ship);
    }

    public void RemoveShip(Ship ship)
    {
        if(!ships.Contains(ship))
            return;
        
        ships.Remove(ship);
        string panelName = panelForShip[ship.shipType];
        GameObject shipPanel = GameObject.Find(panelName);
        if(shipPanel == null)
        {
            Debug.Log("Error: panel not found, check RemoveShip on PlayerController. Panel name is :" + panelName);
            return;
        }
        shipPanel.transform.Find("Inner").GetComponent<DragUIShips>().EnablePanel();

    }

    public void EndDeployStage()
    {
        GameController.instance.UpdateStage(GameStage.PlayerAttackEnemyMap);
    }



    public bool CanShipBeDeployed(Ship ship,int quantity)
    {
        return ships.Where(x => x.shipType == ship.shipType).Count() < quantity;
    }

    public HitResult ProcessEnemyHit(int z,int x)
    {
        foreach (Ship ship in ships)
        {
            Tile hitTile = ship.ocuppiedTiles.Find(tile => tile.ZCoord == z && tile.XCoord == x);
            if(hitTile != null)
            {
                ship.TakeHit(hitTile);
                if(ship.isSunk)
                    return HitResult.Sunk;
                return HitResult.Hit;
            }
        }
        return HitResult.Miss;
    }

}
    
    
