using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    
    public void DamageAnimation()
    {
        GetComponent<Animator>().SetTrigger("Damage");
    }

}
