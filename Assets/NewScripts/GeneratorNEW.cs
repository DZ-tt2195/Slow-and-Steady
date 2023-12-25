using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using UnityEngine.UI;
using System;
using TMPro;

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
    [ReadOnly] public int missedBullets = 0;

    [SerializeField] List<WaveNEW> listOfWaves = new();
    [SerializeField] Slider waveBar;
    [SerializeField] TMP_Text waveText;

    private void Awake()
    {
        instance = this;
        enemyTracker = GameObject.Find("Enemy Tracker").transform;
    }

    private void Start()
    {
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
        catch (IndexOutOfRangeException)
        {
            yield break;
        }

        float time = 15f;
        while (time > 0)
        {
            time -= Time.deltaTime;
            if (!gameOn)
            {
                Debug.Log("game on disabled");
                yield break;
            }
            if (enemyTracker.childCount == 0)
            {
                Debug.Log("wave triggered faster");
                break;
            }
            yield return null;
        }

        StartCoroutine(Gameplay(waveNumber + 1));
    }

    internal void GameOver()
    {
        Debug.Log("you lost");
    }
}
