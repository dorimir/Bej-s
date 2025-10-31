using System;
using System.Collections;
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
        StartCoroutine(SpawnFish());
    }
}
