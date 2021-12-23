using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Level", order = 51)] 
public class Level : ScriptableObject
{

    [SerializeField]
    protected List<UnitData.EnemyData> enemies = new List<UnitData.EnemyData>();
    
    [SerializeField]
    protected List<int> levels = new List<int>();
    
    [SerializeField]
    protected List<float> change = new List<float>();
    
    [SerializeField]
    protected float spawnTime;
    
    
    [SerializeField]
    protected int increaseLevel;
    
    [SerializeField]
    protected float multipleLevel;
    
    public List<UnitData.EnemyData> Enemies => enemies;
    public List<int> Levels => levels;
    public int IncreaseLevel => increaseLevel;
    public float MultipleLevel => multipleLevel;
    public List<float> Change => change;

    public float SpawnTime => spawnTime;
}
