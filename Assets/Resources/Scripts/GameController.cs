using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public GameStage currentStage;
    private GameStage actionStage;
    public int turn = 0;
    [SerializeField]
    Camera camera1;
    [SerializeField]
    Camera camera2;

    private void Awake() {
        instance = this;
    }
    void Start()
    {
        currentStage = GameStage.Deploy;
        camera1.gameObject.SetActive(true);
        camera2.gameObject.SetActive(false);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(actionStage == currentStage)
            return;
        
        switch (actionStage)
        {
            case GameStage.PlayerAttackEnemyMap:
               TransitionToPlayerAttackEnemyMap();
               break;
            
            case GameStage.PlayerAttackCinematic:
                TransitionToPlayerAttackCinematic();
                break;
            
            case GameStage.IAAttackPlayerMap:
                TransitionToIAAttack();
                break;
            default:
                break;
        }
    }

    public void TransitionToPlayerAttackEnemyMap()
    {
        if(currentStage == GameStage.Deploy)
        {
            EndDeployStage();
        }
        currentStage = GameStage.PlayerAttackEnemyMap;
        camera1.gameObject.SetActive(false);
        camera2.gameObject.SetActive(true);
        turn++;
    }

    public void TransitionToPlayerAttackCinematic()
    {
        currentStage = GameStage.PlayerAttackCinematic;
        if(EnemyMapController.instance.CheckEndOfGame())
        {
            actionStage = GameStage.EndOfGame;
            EndOfGame("Player");
        }
        else
        {
            actionStage = GameStage.IAAttackPlayerMap;
        }
    }

    public void TransitionToIAAttack()
    {
        currentStage = GameStage.IAAttackPlayerMap;
        camera2.gameObject.SetActive(false);
        camera1.gameObject.SetActive(true);
        EnemyMapController.instance.IAEnemyShot();
        if(PlayerController.instance.CheckEndOfGame())
        {
            actionStage = GameStage.EndOfGame;
            EndOfGame("IA");
        }
        else
            actionStage = GameStage.PlayerAttackEnemyMap;
    }

    public void EndDeployStage()
    {
        Debug.Log("End Deploy Stage");
        List<Tile> playerTiles = MapController.instance.AllTiles;
        EnemyMapController.instance.GenerateEnemyMap(playerTiles);
        List<Ship> ships = PlayerController.instance.ships;
        GameObject.Find("InitialSetupCanvas").SetActive(false);
        EnemyMapController.instance.GenerateEnemyShips(ships);
    }

    public void EndOfGame(string winner)
    {
        currentStage = GameStage.EndOfGame;
        camera1.gameObject.SetActive(false);
        camera2.gameObject.SetActive(false);
        Debug.Log("End of Game: " + winner + " wins!");
        // Show end game panel
        GameObject endGamePanel = GameObject.Find("EndGamePanel");
        if(endGamePanel == null)
        {
            Debug.Log("Error: EndGamePanel not found, check EndOfGame on GameController");
            return;
        }
        endGamePanel.SetActive(true);
        endGamePanel.transform.Find("Winner").GetComponent<UnityEngine.UI.Text>().text = winner;
    }

    public void UpdateStage(GameStage nextStage)
    {
        actionStage = nextStage;
    }
}


/// <summary>
/// 07-03-2025 : Game will be
/// Deploy
/// LOOP START ( turn + 1 )
/// PlayerAttackEnemyMap
/// PlayerAttackCinematic
/// IAAttackPlayerMap
/// LOOP END
/// EndOfGame
/// </summary>
public enum GameStage
{
    Deploy = 0,
    PlayerAttackEnemyMap = 1,
    PlayerAttackCinematic = 2,
    IAAttackPlayerMap = 3,
    EndOfGame = 99
}

public enum HitResult 
{
    Miss = 0 ,
    Hit = 1 ,
    Sunk = 2
}
