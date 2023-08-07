using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public bool playing = true;
    Generator gen;
    public Transform bulletcounter;
    public Bullet bullet;
    Image sr;

    public float energy = 15;
    public int shootcost = 1;
    public float rechargetime = 1f;

    Slider energySlider;
    TMP_Text energyText;

    public int startingbullets = 150;
    int remainingbullets;
    public Slider bulletSlider;
    public TMP_Text bulletText;

    public Image[] abilitydisplay = new Image[3];
    public TMP_Text[] textdisplay = new TMP_Text[3];

    public bool[] online = new bool[3];
    public int[] cost = new int[3];
    public int[] cooldown = new int[3];

    public AudioSource audiosource;
    public AudioClip[] poweraudio = new AudioClip[5];

    public int bulletsshot = 0;
    public int powerupsused = 0;

    // Start is called before the first frame update
    void Start()
    {
        gen = FindObjectOfType<Generator>().GetComponent<Generator>();
        energySlider = GameObject.Find("Energy Bar").GetComponent<Slider>();
        energyText = energySlider.transform.GetChild(2).GetComponent<TMP_Text>();

        remainingbullets = startingbullets;
        StartCoroutine(FreeEnergy());
        sr = this.GetComponent<Image>();
        audiosource = this.GetComponent<AudioSource>();
        bullet.gameObject.SetActive(false);

        for (int i = 0; i < 3; i++)
            StartCoroutine(Cooldown(i));
    }

    public IEnumerator FreeEnergy()
    {
        yield return new WaitForSeconds(rechargetime);
        energy++;
        if (energy >= 50)
            energy = 50;
        if (playing)
            StartCoroutine(FreeEnergy());
    }

    public IEnumerator Cooldown(int n)
    {
        online[n] = false;
        yield return new WaitForSeconds(cooldown[n]);
        online[n] = true;
    }

    public void CreateBullet(Vector2 target)
    {
        Bullet newBullet = Instantiate(bullet, bulletcounter);
        newBullet.gameObject.SetActive(true);

        newBullet.speed = 15f;
        newBullet.transform.localPosition = new Vector2(0, 0);
        newBullet.targetPosition = target.normalized * 10000f;
    }

    void AbilityOne()
    {
        if (online[0] && energy >= cost[0] && Input.GetKeyDown(KeyCode.Alpha1))
        {
            energy -= cost[0];
            powerupsused++;
            StartCoroutine(Cooldown(0));
            audiosource.PlayOneShot(poweraudio[0]);

            Enemy[] listofenemies = FindObjectsOfType<Enemy>();
            for (int i = 0; i < listofenemies.Length; i++)
                StartCoroutine(Freeze(listofenemies[i]));
        }
    }

    IEnumerator Freeze(Enemy enemy)
    {
        enemy.frozen = true;
        float originalspeed = enemy.speed;
        enemy.speed = 0;

        yield return new WaitForSeconds(5f);
        enemy.speed = originalspeed;
        enemy.frozen = false;
    }

    IEnumerator AbilityTwo()
    {
        if (online[1] && energy >= cost[1] && Input.GetKeyDown(KeyCode.Alpha2))
        {
            energy -= cost[1];
            powerupsused++;
            StartCoroutine(Cooldown(1));

            for (int i = 0; i<2; i++)
            {
                Enemy[] listofenemies = FindObjectsOfType<Enemy>();
                for (int j = listofenemies.Length - 1; j >= 0; j--)
                {
                    if (listofenemies[j] != null)
                    {
                        audiosource.PlayOneShot(poweraudio[1]);
                        StartCoroutine(listofenemies[j].TakeDamage());
                    }
                    yield return new WaitForSeconds(0.1f);
                }
                yield return new WaitForSeconds(1f);
            }
        }
    }

    void AbilityThree()
    {
        if (online[2] && energy >= cost[2] && Input.GetKeyDown(KeyCode.Alpha3))
        {
            energy -= cost[2];
            StartCoroutine(Cooldown(2));
            audiosource.PlayOneShot(poweraudio[2]);
            powerupsused++;

            energy += 25;
            if (energy > 50)
                energy = 50;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //this rotating code is from this video: https://www.youtube.com/watch?v=-bkmPm_Besk
        Vector3 rotation = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.localPosition;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotZ);

        if (playing)
        {
            if (remainingbullets > 0 && Input.GetMouseButtonDown(0) && energy >= shootcost)
            {
                energy -= shootcost;
                CreateBullet(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                bulletsshot++;
                remainingbullets--;
                audiosource.PlayOneShot(poweraudio[3]);
            }

            for (int i = 0; i < 3; i++)
            {
                if (online[i])
                    textdisplay[i].text = $"{i + 1}. " + textdisplay[i].name + $"\n{cost[i]} Energy";
                else
                    textdisplay[i].text = $"Recharging...\n{cost[i]} Energy";

                if (online[i] && energy >= cost[i])
                    abilitydisplay[i].color = new Color(1, 1, 1, 1);
                else
                    abilitydisplay[i].color = new Color(1, 1, 1, 0.25f);
            }

            AbilityOne();
            StartCoroutine(AbilityTwo());
            AbilityThree();

            energySlider.value = (float)(energy / 50);
            energyText.text = $"Energy: {(int)energy}";

            bulletSlider.value = ((float)remainingbullets / startingbullets);
            bulletText.text = $"Bullets: {remainingbullets}";
        }
    }
}
