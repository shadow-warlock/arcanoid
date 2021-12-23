using UnityEngine;

public abstract class CountStatusData : StatusData
{
    [SerializeField] 
    private int count;

    public int Count => count;
}