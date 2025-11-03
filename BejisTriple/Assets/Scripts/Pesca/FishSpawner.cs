using System;
using System.Collections;
using System.Numerics;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public GameObject carpa, trucha, siluro, barbo;

    /*
    
    Cada segundo aparece un nuevo pez.
    9/20 veces aparece una trucha (45%)
    7/20 veces aparece una carpa (35%)
    3/20 veces aparece un siluro (15%)
    1/20 veces aparece un barbo dorado (5%)
    
    */
    void Start()
    {
        StartCoroutine(SpawnFish());
    }

    private IEnumerator SpawnFish()
    {
        yield return new WaitForSeconds(1f);
        var rand = new System.Random();
        double randomNumber = rand.NextDouble();
        GameObject spawnedFish;
        float spawnPointY = (float)((rand.NextDouble() * 6) + 6);
        double Xchooser = rand.NextDouble();
        float spawnPointX;
        if (Xchooser >= 0.5) spawnPointX = 10;
        else spawnPointX = -10;
        if (randomNumber <= 0.05)
        {
            //Spawn barbo dorado
            spawnedFish = Instantiate(barbo, new UnityEngine.Vector3(spawnPointX, spawnPointY, 2), UnityEngine.Quaternion.identity);
        }
        else if (randomNumber <= 0.15)
        {
            //Spawn siluro
            spawnedFish = Instantiate(siluro, new UnityEngine.Vector3(spawnPointX, spawnPointY, 2), UnityEngine.Quaternion.identity);
        }
        else if (randomNumber <= 0.35)
        {
            //Spawn carpa
            spawnedFish = Instantiate(carpa, new UnityEngine.Vector3(spawnPointX, spawnPointY, 2), UnityEngine.Quaternion.identity);
        }
        else
        {
            spawnedFish = Instantiate(trucha, new UnityEngine.Vector3(spawnPointX, spawnPointY, 2), UnityEngine.Quaternion.identity);
        }
        spawnedFish.GetComponent<Rigidbody>().linearVelocity = new UnityEngine.Vector2(7 * (-spawnPointX/10), spawnedFish.GetComponent<Rigidbody>().linearVelocity.y);
        StartCoroutine(DeleteFish(spawnedFish));
        StartCoroutine(SpawnFish());
    }

    private IEnumerator DeleteFish(GameObject fish)
    {
        yield return new WaitForSeconds(10f);
        fish.SetActive(false);
    }
}
