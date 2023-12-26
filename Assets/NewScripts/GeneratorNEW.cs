using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using UnityEngine.UI;
using System;
using TMPro;
using System.Diagnostics;
using UnityEngine.InputSystem;

[System.Serializable]
public class WaveNEW
{
    public List<EnemyNEW> listOfEnemies = new List<EnemyNEW>();
}

public class GeneratorNEW : MonoBehaviour
{
    public static GeneratorNEW instance;
    [ReadOnly] public bool gameOn = true;

    Transform enemyTracker;

    [SerializeField] List<WaveNEW> listOfWaves = new();
    [SerializeField] Slider waveBar;
    [SerializeField] TMP_Text waveText;

    [SerializeField] TMP_Text endText;
    [SerializeField] TMP_Text statsText;

    [ReadOnly] public int missedBullets = 0;
    [ReadOnly] public int abilitiesUsed = 0;
    Stopwatch stopwatch;

    private void Awake()
    {
        instance = this;
        enemyTracker = GameObject.Find("Enemy Tracker").transform;
    }

    private void Start()
    {
        stopwatch = new Stopwatch();
        stopwatch.Start();
        waveBar.maxValue = listOfWaves.Count;
        StartCoroutine(Gameplay(0));
    }

    IEnumerator Gameplay(int waveNumber)
    {
        try
        {
            foreach (EnemyNEW enemy in listOfWaves[waveNumber].listOfEnemies)
            {
                EnemyNEW next = Instantiate(enemy, enemyTracker);
                next.transform.localPosition = new Vector2(10, UnityEngine.Random.Range(-3.5f, 3.5f));
            }
            waveBar.value = waveNumber + 1;
            waveText.text = $"Wave: {waveNumber + 1}/{listOfWaves.Count}";
        }
        catch (ArgumentOutOfRangeException)
        {
            StartCoroutine(WaitForEnding());
        }

        float time = 15f;
        while (time > 0)
        {
            time -= Time.deltaTime;
            if (!gameOn)
                yield break;
            if (enemyTracker.childCount == 0)
                break;
            yield return null;
        }

        StartCoroutine(Gameplay(waveNumber + 1));
    }

    IEnumerator WaitForEnding()
    {
        while (enemyTracker.childCount > 0)
        {
            yield return null;
        }

        GameOver("You Won!");
    }

    string CalculateTime()
    {
        stopwatch.Stop();
        TimeSpan x = stopwatch.Elapsed;
        string part = x.Seconds < 10 ? $"0{x.Seconds}" : $"{x.Seconds}";
        return $"{x.Minutes}:" + part + $".{x.Milliseconds}";
    }

    string CalculatePercentage()
    {
        if (PlayerNEW.instance.remainingBullets == 100)
            return "0%";
        else
        {
            float answer = (float)(PlayerNEW.instance.remainingBullets - missedBullets) / (PlayerNEW.instance.remainingBullets-missedBullets);
            return (answer * 100f).ToString("F1") + "%";
        }
    }

    internal void GameOver(string text)
    {
        gameOn = false;
        PlayerNEW.instance.joystick.transform.parent.gameObject.SetActive(false);
        PlayerNEW.instance.gameObject.SetActive(false);
        endText.text = text;
        endText.transform.parent.gameObject.SetActive(true);

        statsText.text =
            $"Bullet Accuracy: {CalculatePercentage()}" +
            $"\nAbilities Used: {abilitiesUsed}" +
            $"\nTime Taken: {CalculateTime()}";
    }
}
