using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FirePowerController : MonoBehaviour
{
    public List<GameObject> cannons;
    private Ship ship;
    public ShipSoundController shipSounds;
    void Start()
    {
        ship = GetComponent<Ship>();
        shipSounds = FindAnyObjectByType<ShipSoundController>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FireCannons()
    {
        CameraRotator.instance.StartRotation(transform.position);
        StartCoroutine(FireSubRoutine());
    }

    private IEnumerator FireSubRoutine()
    {
        foreach (var cannon in cannons)
        {
            var explosion = cannon.GetComponentInChildren<ParticleSystem>();
            if (explosion == null)
            {
                Debug.Log("Explosion not found in cannon: " + cannon.name);
                continue;
            }
            //Vector3 cannonPosition = new Vector3(cannon.transform.position.x, cannon.transform.position.y, cannon.transform.position.z + 15);
            //var explosion = Instantiate(explosionPrefab, cannonPosition , cannon.transform.rotation);
            explosion.Play();
            shipSounds.PlayExplosionSound(ship);
            MoveCannonBackAndforward(cannon.transform);
            yield return new WaitForSeconds(1.5f); // Adjust the delay as needed

        }
        CameraRotator.instance.StopRotation();
    }

    private void MoveCannonBackAndforward(Transform cannon)
    {
       Vector3 startPosition = new Vector3(cannon.transform.localPosition.x,cannon.transform.localPosition.y,cannon.transform.localPosition.z);
       Vector3 finalPosition = new Vector3(cannon.transform.localPosition.x,cannon.transform.localPosition.y,cannon.transform.localPosition.z - 2);
       StartCoroutine(Lerper(startPosition, finalPosition, 0.3f, cannon));
    }

    IEnumerator Lerper(Vector3 start , Vector3 end, float duration, Transform cannon)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            cannon.transform.localPosition = Vector3.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        cannon.transform.localPosition = end;
        elapsed = 0f;
        while (elapsed < duration)
        {
            cannon.transform.localPosition = Vector3.Lerp(end,start, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        cannon.transform.localPosition = start;
    }

}
