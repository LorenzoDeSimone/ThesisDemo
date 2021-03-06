﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCounter : MonoBehaviour
{
    private bool attackStarted = false;
    private int nEnemiesToKill, nEnemiesKilled;

    private Transform[] barriers;

    public void NotifyKill()
    {
        if (attackStarted)
        {
            nEnemiesKilled++;
            if(nEnemiesKilled >= nEnemiesToKill)
            {
                foreach (Transform t in barriers)
                {
                    if(t)
                        t.gameObject.SetActive(false);
                }
            }
        }
    }

    public void StartAttack(Transform[] barriers, Transform[] enemiesToDisable, Transform[] enemiesToActivate)
    {
        if (!attackStarted)
        {
            this.barriers = barriers;
            attackStarted = true;
            nEnemiesToKill = enemiesToActivate.Length;
            nEnemiesKilled = 0;

            foreach (Transform t in barriers)
                t.gameObject.SetActive(true);

            foreach (Transform t in enemiesToActivate)
            {
                t.gameObject.SetActive(true);
                t.GetComponent<EnemyStatus>().AIManager.enabled = true;
            }

            foreach (Transform t in enemiesToDisable)
            {
                t.GetComponent<EnemyStatus>().healthBarGO.SetActive(false);
                t.gameObject.SetActive(false);
            }
        }
    }
}
