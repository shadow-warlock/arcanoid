using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public GameObject[] blocksSets;
    
    // Start is called before the first frame update
    private void Start()
    {
        RespawnBlocks();
    }

    private void RespawnBlocks()
    {
        if (transform.childCount != 0 && transform.GetChild(0).childCount == 0)
        {
            Destroy(transform.GetChild(0).gameObject);
            transform.GetChild(0).SetParent(null);
        }

        if (transform.childCount == 0)
        {
            int setIndex = Random.Range(0, blocksSets.Length);
            GameObject blocksSet = Instantiate(blocksSets[setIndex], transform.position, Quaternion.identity);
            blocksSet.transform.SetParent(transform);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        RespawnBlocks();
    }
}
