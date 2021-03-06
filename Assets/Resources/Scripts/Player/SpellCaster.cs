﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellCaster : MonoBehaviour
{
    [HideInInspector] public bool coolDownReady = true;
    [SerializeField] private float cooldownSeconds = 10f;
    [SerializeField] private int spellDamage = 2;
    [SerializeField] private InteractionManager interactionManager;
    [SerializeField] private GameObject cooldownBar;
    [SerializeField] private Image cooldownBarImage;
    [SerializeField] private AudioClip fireSpellSound;

    private PlayerController playerController;

	// Use this for initialization
	void Start ()
    {
        if(!PlayerChoices.Instance().CanUseExplosionSpell)
        {
            cooldownBar.SetActive(false);
            enabled = false;
        }

        playerController = GetComponent<PlayerController>();	
	}

    public void SpellEnabled()
    {
        PlayerChoices.Instance().CanUseExplosionSpell = true;
        cooldownBar.SetActive(true);
        enabled = true;
    }

	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("B") && Time.timeScale > 0 && IsExplosionSpellReady())
            ExplosionSpell();
    }

    private bool IsExplosionSpellReady()
    {
        return PlayerChoices.Instance().CanUseExplosionSpell && coolDownReady;
    }

    public void ExplosionSpell()
    {
        AudioManager.Instance().PlayClipOneShot(fireSpellSound);
        coolDownReady = false;
        Hittable hittable;
        UnityEngine.Object spawnEffect = Resources.Load("Prefabs/NPCs/Skeleton/SpawnEffectRed");
        GameObject spawnEffectGO = (GameObject)Instantiate(spawnEffect);
        spawnEffectGO.transform.position = transform.position;

        foreach (Transform t in interactionManager.NearTargets)
        {
            if (t)
            {
                hittable = t.GetComponent<Hittable>();
                if (hittable && !t.gameObject.layer.Equals("Ally"))
                    hittable.UpdateHealth(-spellDamage);
            }
        }

        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        float elapsedTime = 0f;

        while (elapsedTime < cooldownSeconds)
        {
            elapsedTime += Time.deltaTime;
            cooldownBarImage.fillAmount = elapsedTime / cooldownSeconds;
            yield return null;
        }

        coolDownReady = true;
        cooldownBarImage.fillAmount = 1f;
    }
}
