using System;
using System.Collections;
using Unit;
using UnitData;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnEnemy : MonoBehaviour
{

    public Level levelData;
    public GameObject coinPrefab;
    private bool _spawned = false;
    private int _iteration = 0;
    public Action<Enemy> OnSpawn; 

    // Update is called once per frame
    private void Update()
    {
        if (transform.childCount == 0 && !_spawned)
        {
            _spawned = true;
            StartCoroutine(Spawn());
        }
    }
    
    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(2f);
        int iterationOnLevelList = _iteration % levelData.Enemies.Count;
        int levelListNumber = _iteration / levelData.Enemies.Count;
        int level = (int) (levelData.Levels[iterationOnLevelList] * (1 + levelData.MultipleLevel * levelListNumber) +
                             levelData.IncreaseLevel * levelListNumber);
        Spawn(levelData.Enemies[iterationOnLevelList], level);
        _spawned = false;
        _iteration++;
    }
    
    public void Spawn(EnemyData data, int level)
    {
        GameObject enemyObj = Instantiate(data.Prefab, transform.position, Quaternion.identity);
        enemyObj.transform.SetParent(transform);
        enemyObj.transform.Rotate(0, 180, 0);
        Enemy enemy = enemyObj.GetComponent<Enemy>();
        enemy.Level = level;
        enemy.data = data;
        _spawned = false;
        if (OnSpawn != null) OnSpawn(enemy);
    }

    public void SpawnCoins(int count)
    {
        GameObject coinContainer = GameObject.FindWithTag("CoinContainer");
        for (int i = 0; i < count; i++)
        {
            GameObject coin = Instantiate(coinPrefab, coinContainer.transform.position, Quaternion.identity);
            coin.transform.SetParent(coinContainer.transform);
            coin.GetComponent<Rigidbody2D>().AddForce(new Vector2(-3* Random.Range(1f, 2f), 2 * Random.Range(1f, 2f)), ForceMode2D.Impulse);
        }
    }
}
