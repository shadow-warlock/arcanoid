using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

    public GameObject balls;
    
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            other.gameObject.transform.SetParent(null);
            Destroy(other.gameObject);
            if (balls.transform.childCount == 0)
            {
                GameObject player = GameObject.FindWithTag("PlayerTag");
                if (player == null)
                {
                    print("player not found");
                }
                else
                {
                    Platform platformScript = player.GetComponent<Platform>();
                    if (platformScript == null)
                    {
                        print("playerScript not found");
                    }
                    else
                    {
                        platformScript.TakeDamage(1);
                    }
                }
            }
        }
    }
}
