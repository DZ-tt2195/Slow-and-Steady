using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityNEW : MonoBehaviour
{
    CanvasGroup group;
    Button button;
    [SerializeField] int cost;
    [SerializeField] float cooldown;
    [SerializeField] protected AudioClip sound;
    WaitForSeconds timer;

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
        button = GetComponent<Button>();
        button.onClick.AddListener(UseEffect);
        timer = new(cooldown);
    }

    private void Start()
    {
        StartCoroutine(OnCooldown());
    }

    IEnumerator OnCooldown()
    {
        group.alpha = 0.4f;
        button.interactable = false;

        yield return timer;

        group.alpha = 1f;
        button.interactable = true;
    }

    void UseEffect()
    {
        if (PlayerNEW.instance.energy >= cost)
        {
            PlayerNEW.instance.energy -= cost;
            GeneratorNEW.instance.abilitiesUsed++;
            StartCoroutine(AbilityEffect());
            StartCoroutine(OnCooldown());
        }
    }

    protected virtual IEnumerator AbilityEffect()
    {
        yield return null;
    }
}
