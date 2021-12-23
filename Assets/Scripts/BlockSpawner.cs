using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject rightWall;
    [SerializeField] private GameObject topWall;
    [SerializeField] private GameObject bottomWall;

    // Start is called before the first frame update


    private void RespawnBlocks()
    {
        if (transform.childCount == 0)
        {
            GenerateBlocks();
        }
    }

    private void GenerateBlocks()
    {
        float top = topWall.transform.position.y;
        float bottom = bottomWall.transform.position.y;
        bottom += +(top - bottom) / 1.8f;
        float left = leftWall.transform.position.x;
        float right = rightWall.transform.position.x;
        Vector3 blockSize = blockPrefab.GetComponent<BoxCollider2D>().size *
                            blockPrefab.GetComponent<Transform>().localScale;
        for (float i = left + blockSize.x; i < right - blockSize.x; i += blockSize.x)
        {
            for (float j = bottom + blockSize.y; j < top - blockSize.y * 2; j += blockSize.y)
            {
                if (Random.Range(0.0f, 1.0f) >= 0.3)
                {
                    Vector3 offset = new Vector3(i, j) + new Vector3((left - right) / 2, -(top - bottom) / 2);
                    GameObject block = Instantiate(blockPrefab, transform.position + offset, Quaternion.identity);
                    block.transform.SetParent(transform);
                }
            }
        }
    }

    // Update is called once per frame
    private void Update()
    {
        RespawnBlocks();
    }
}