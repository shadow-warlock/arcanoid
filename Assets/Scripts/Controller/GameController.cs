using System;
using Component.Coins;
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
    private Platform platform;
    
    [SerializeField]
    private SpawnEnemy enemySpawner;
    
    [SerializeField]
    private WizardUIListener wizardUIListener;
    
    [SerializeField]
    private EnemyUIListener enemyUIListener;
    
    [SerializeField]
    private SummonUIListener summonUIListener;

    private static GameController _instance;
    
    public Wizard Wizard => wizard;

    public Platform Platform => platform;

    private void Start()
    {
        _instance = this;
        wizardUIListener.Unit = wizard;
        wizard.AfterDie += unit => GameOver("Персонаж погиб");
        platform.OnDie += () => GameOver("Закончились светлячки");
        enemySpawner.OnSpawn = OnEnemySpawn;
        wizard.OnSummon = OnSummon;
    }

    public static GameController GetInstance()
    {
        if (_instance == null)
        {
            throw new Exception("Game controller is null");
        }

        return _instance;
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
    
    private void GameOver(String text)
    {
        Time.timeScale = 0;
        gameOverModal.SetActive(true);
        gameOverModal.transform.GetChild(1).GetComponent<Text>().text = text;
        CoinsStore.GetInstance().AddCoins(wizard.GetCoins());
    }
}
