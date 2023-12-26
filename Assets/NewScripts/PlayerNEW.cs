using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PlayerNEW : MonoBehaviour
{

#region Variables

    public static PlayerNEW instance;

    [Foldout("Misc", true)]
        [SerializeField] BulletNEW bulletPrefab;
        Transform bulletTracker;
        Camera mainCam;
        [ReadOnly] public int energy;

    [Foldout("UI", true)]
        [SerializeField] Slider energyBar;
        [SerializeField] TMP_Text energyText;
        WaitForSeconds rechargeEnergy = new(1f);

        internal int remainingBullets = 100;
        [SerializeField] Slider bulletBar;
        [SerializeField] TMP_Text bulletText;

    [Foldout("Audio", true)]
        [SerializeField] AudioClip shoot;    

    #endregion

#region Setup

    private void Awake()
    {
        instance = this;
        mainCam = Camera.main;
        bulletTracker = GameObject.Find("Bullet Tracker").transform;
    }

    private void OnEnable()
    {
        Application.targetFrameRate = 60;
        Debug.Log($"Screensize: {Screen.width}, {Screen.height}");

        if (Application.isMobilePlatform)
        {
            MobileInput.instance.OnStartTouch += CreateBullet;
            Debug.Log("playing on phone");
        }
        else
        {
            Debug.Log("playing on computer");
        }
    }

    private void OnDisable()
    {
        if (Application.isMobilePlatform)
        {
            MobileInput.instance.OnStartTouch -= CreateBullet;
        }
    }

    private void Start()
    {
        energy = 15;
        StartCoroutine(FreeEnergy());
    }

    #endregion

#region Gameplay

    IEnumerator FreeEnergy()
    {
        energy++;
        if (energy >= 50)
            energy = 50;

        yield return rechargeEnergy;

        if (GeneratorNEW.instance.gameOn)
            StartCoroutine(FreeEnergy());
    }

    private void Update()
    {
        energyBar.value = energy;
        energyText.text = $"Energy: {energy}";

        if (!Application.isMobilePlatform)
        {
            Vector2 screenPosition = GetWorldCoordinates(Input.mousePosition);
            float rotZ = Mathf.Atan2(screenPosition.y, screenPosition.x) * Mathf.Rad2Deg;
            transform.localEulerAngles = new Vector3(0, 0, rotZ);

            if (Input.GetMouseButtonDown(0))
                CreateBullet(Input.mousePosition);
        }
    }

    Vector2 GetWorldCoordinates(Vector2 screenPos)
    {
        Vector2 screenCoord = new(screenPos.x, screenPos.y);
        Vector2 worldCoord = mainCam.ScreenToWorldPoint(screenCoord);
        return worldCoord;
    }

    void CreateBullet(Vector2 screenPosition)
    {
        screenPosition = GetWorldCoordinates(screenPosition);

        if (GeneratorNEW.instance.gameOn && remainingBullets > 0 && energy > 0)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                AudioManager.instance.PlaySound(shoot, 0.5f);

                BulletNEW newBullet = Instantiate(bulletPrefab, bulletTracker);
                newBullet.transform.localPosition = new Vector2(0, 0);
                newBullet.targetPosition = screenPosition.normalized * 10000f;

                remainingBullets--;
                bulletBar.value = remainingBullets;
                bulletText.text = $"Bullets: {remainingBullets}";

                energy--;
                energyBar.value = energy;
                energyText.text = $"Energy: {energy}";
            }
        }
    }

    #endregion

}
