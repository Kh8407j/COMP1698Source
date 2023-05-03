using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instantiator : MonoBehaviour
{
    public GameObject enemy;

    public float minRate, maxRate, activeRate, minRand, maxRand;
    // Start is called before the first frame update
    void Start()
    {
        activeRate = minRate;
        InstantiateEnemy();
    }

    public void DelayedInstantiation()
    {
        Invoke("InstantiateEnemy", Random.Range(minRand, maxRand));
    }

    public void InstantiateEnemy()
    {
        GameObject enemyInstance = Instantiate(enemy, transform.position, transform.rotation);

        enemyInstance.GetComponent<EnemyBehaviour>().instantiator = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(activeRate < maxRate)
        {
            activeRate += 0.001f;
        }
    }
}
