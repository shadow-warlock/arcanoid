using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{

    public GameObject enemyPrefab;
    public GameObject wizard;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (transform.childCount == 0)
        {
            GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            enemy.transform.SetParent(transform);
            enemy.transform.Rotate(0, 180, 0);
            enemy.GetComponent<Enemy>().wizard = wizard;
        }
    }
}
