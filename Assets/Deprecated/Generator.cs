using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using System;

public class Generator : MonoBehaviour
{
    Slider waveSlider;
    TMP_Text waveText;

    Transform enemycounter;
    Player player;
    GameObject EndScreen;

    public float timerperwave = 15f;
    public int missedbullets = 0;
    float usedtimer;
    List<Wave> waves = new List<Wave>();
    Stopwatch stopwatch;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
            Application.Quit();
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Start is called before the first frame update
    void Start()
    {
        stopwatch = new Stopwatch();
        stopwatch.Start();

        Transform listOfWaves = GameObject.Find("ListOfWaves").transform;
        for (int i = 0; i<listOfWaves.childCount; i++)
            waves.Add(listOfWaves.GetChild(i).GetComponent<Wave>());

        waveSlider = GameObject.Find("Wave bar").GetComponent<Slider>();
        waveText = waveSlider.transform.GetChild(2).GetComponent<TMP_Text>();
        player = FindObjectOfType<Player>().GetComponent<Player>();
        enemycounter = GameObject.Find("Enemy Counter").transform;
        EndScreen = GameObject.Find("EndScreen");

        EndScreen.SetActive(false);
        StartCoroutine(TimerDecrease());
        StartCoroutine(Gameplay());
    }

    public IEnumerator TimerDecrease()
    {
        yield return new WaitForSeconds(1f);
        usedtimer--;
        StartCoroutine(TimerDecrease());
    }

    public IEnumerator Gameplay()
    {
        for (int i = 0; i<waves.Count; i++)
        {
            waveSlider.value = ((float)(i+1) / 10);
            waveText.text = $"Wave {i+1}/10";

            usedtimer = timerperwave;
            List<Enemy> nextWave = waves[i].list;
            for (int j = 0; j < nextWave.Count; j++)
            {
                Enemy nextEnemy = Instantiate(nextWave[j], enemycounter);
                nextEnemy.StartingPosition(new Vector2(1050f, UnityEngine.Random.Range(-350f, 350f)));
            }

            while (true)
            {
                if (enemycounter.transform.childCount == 0)
                    break;
                else if (usedtimer < 0)
                    break;
                yield return null;
            }
        }

        while (true)
        {
            if (enemycounter.transform.childCount == 0)
                break;
            yield return null;
        }

        Victory();
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
        if (player.bulletsshot == 0)
            return "0%";
        else
        {
            float answer = ((float)(player.bulletsshot - missedbullets) / (float)(player.bulletsshot));
            return (answer*100f).ToString("F1") + "%";
        }
    }

    public void Victory()
    {
        StopAllCoroutines();
        player.playing = false;
        EndScreen.gameObject.SetActive(true);
        EndScreen.transform.GetChild(0).GetComponent<TMP_Text>().text = "You won!";
        EndScreen.transform.GetChild(1).GetComponent<TMP_Text>().text =
                $"\nBullet Accuracy: " + CalculatePercentage() +
                $"\nAbilities Used: {player.powerupsused}" +
                $"\nTime Taken: " + CalculateTime();
    }

    public void GameOver()
    {
        StopAllCoroutines();
        player.gameObject.SetActive(false);
        EndScreen.gameObject.SetActive(true);

        EndScreen.transform.GetChild(0).GetComponent<TMP_Text>().text = "You lost.";
        EndScreen.transform.GetChild(1).GetComponent<TMP_Text>().text =  
            $"\nBullet Accuracy: " + CalculatePercentage() +
            $"\nAbilities Used: {player.powerupsused}" +
            $"\nTime Taken: " + CalculateTime();
    }
}
