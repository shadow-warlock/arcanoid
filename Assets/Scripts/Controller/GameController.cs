using System;
using Unit;
using UnitUI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverModal;
    
    [SerializeField]
    private Wizard wizard;
    
    [SerializeField]
    private SpawnEnemy enemySpawner;
    
    [SerializeField]
    private WizardUIListener wizardUIListener;
    
    [SerializeField]
    private EnemyUIListener enemyUIListener;
    
    [SerializeField]
    private SummonUIListener summonUIListener;

    private void Start()
    {
        wizardUIListener.Unit = wizard;
        enemySpawner.OnSpawn = OnEnemySpawn;
        wizard.OnSummon = OnSummon;
    }

    private void OnEnemySpawn(Enemy enemy)
    {
        enemyUIListener.Unit = enemy;
    }
    
    private void OnSummon(Summon summon)
    {
        summonUIListener.Unit = summon;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneTransition.SwitchScene("Menu");
    }
    
    public void GameOver(String text)
    {
        Time.timeScale = 0;
        gameOverModal.SetActive(true);
        gameOverModal.transform.GetChild(1).GetComponent<Text>().text = text;
    }
}
