using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AnimationController : MonoBehaviour
{
    [SerializeField]
    GameObject missExplosion;
    [SerializeField]
    GameObject sunkExplosion;
    [SerializeField]
    GameObject HitExplosion;
    [SerializeField]
    GameObject HitFire;
    public static AnimationController instance;
    public Vector3 currentPosition;
    public HitResult currentHitResult;
    [SerializeField]
    AudioClip BombFalling;
    [SerializeField]
    AudioClip WaterSplash;
    [SerializeField]
    AudioClip BombExplosion;
    [SerializeField]
    GameObject cameraRotatorFull;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PlayExplosion()
    {
        cameraRotatorFull.GetComponent<CameraRotator>().StartRotation(currentPosition);
        cameraRotatorFull.GetComponentInChildren<AudioSource>().clip = BombFalling;
        cameraRotatorFull.GetComponentInChildren<AudioSource>().Play();
        switch (currentHitResult)
        {
            case HitResult.Miss:
                PlayMissExplosion(currentPosition);
                break;
            /*case HitResult.Sunk:
                PlaySunkExplosion(currentPosition);
                break;*/
            case HitResult.Hit:
                PlayHitExplosion(currentPosition);
                break;
            default:
                break;
        }
    }

    public void SetNextExplosion(Vector3 nextPos , HitResult nextHitResult)
    {
        currentPosition = nextPos;
        currentHitResult = nextHitResult;
    }

    public void PlayMissExplosion(Vector3 position)
    {
        position.y += 10f;
        StartCoroutine(ShowMissExplosion(3f,position));
    }

        public void PlayHitExplosion(Vector3 position)
    {
        position.y += 12f;
        StartCoroutine(ShowHitAnimation(3f, position));
    }

    public IEnumerator ShowMissExplosion(float delay, Vector3 position)
    {
        float elapsed = 0f;
        while (elapsed < delay)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        cameraRotatorFull.GetComponentInChildren<AudioSource>().clip = WaterSplash;
        cameraRotatorFull.GetComponentInChildren<AudioSource>().Play();
        GameObject explosion = Instantiate(missExplosion, position, Quaternion.identity);
        elapsed = 0f;
        while (elapsed < 5f)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        Destroy(explosion, 5f);
        cameraRotatorFull.GetComponent<CameraRotator>().StopRotation();
        GameController.instance.UpdateStage(GameStage.PlayerAttackEnemyMap);
    }


    public void PlaySunkExplosion(Vector3 position)
    {
        //GameObject explosion = Instantiate(sunkExplosion, position, Quaternion.identity);
        //Destroy(explosion, 5f);
        cameraRotatorFull.GetComponent<CameraRotator>().StopRotation();
        GameController.instance.UpdateStage(GameStage.PlayerAttackEnemyMap);
    }



    public IEnumerator ShowHitAnimation(float delay ,Vector3 position)
    {
        float elapsed = 0f;
        int numberOfRepets = 2;
        while(numberOfRepets > 0)
        {
            numberOfRepets--;
            elapsed = 0f;
            while (elapsed < delay)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
            GameObject explosion = Instantiate(HitExplosion, position, Quaternion.identity);
            explosion.transform.localScale = new Vector3(explosion.transform.localScale.x / 2, explosion.transform.localScale.y / 2, explosion.transform.localScale.z / 2);
            explosion.GetComponent<ParticleSystem>().Play();
            cameraRotatorFull.GetComponentInChildren<AudioSource>().clip = BombExplosion;    
            cameraRotatorFull.GetComponentInChildren<AudioSource>().Play();
            Destroy(explosion, 5f);   
        }
        Ship ship = PlayerController.instance.GetShipByPosition(position);
        int totalHits = ship.shipTiles.Where(x => x.hitted == true).Count();
        ship.gameObject.GetComponent<FirePowerController>().TakeDamage(totalHits , ship.shipTiles.Count);
        elapsed = 0f;
        while (elapsed < delay)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        cameraRotatorFull.GetComponent<CameraRotator>().StopRotation();
        GameController.instance.UpdateStage(GameStage.PlayerAttackEnemyMap);
    }
}
