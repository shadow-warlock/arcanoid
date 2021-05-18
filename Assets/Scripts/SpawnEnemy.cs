using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{

    public GameObject enemyPrefab;
    public GameObject enemyHpBar;
    public GameObject damageUIContainer;
    public GameObject damagePrefab;
    public GameObject coinPrefab;
    private bool _spawned = false;
    
    // Start is called before the first frame update
    void Start()
    {
    }

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
        GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        enemy.transform.SetParent(transform);
        enemy.transform.Rotate(0, 180, 0);
        enemy.GetComponent<Enemy>().hpBar = enemyHpBar;
        enemy.GetComponent<Enemy>().damagePrefab = damagePrefab;
        enemy.GetComponent<Enemy>().damageUIContainer = damageUIContainer;
        _spawned = false;
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
