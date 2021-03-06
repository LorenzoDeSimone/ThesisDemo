﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LordOfTheDeadDeath : LordOfTheDeadsState
{
    private float elapsedTime;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Initialization(animator);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= myStatus.timeToDisappearAfterDeath)
        {
            myStatus.DeathFade.SetActive(true);
            myStatus.DeathFade.transform.parent = null;
            Destroy(myStatus.gameObject);
        }
        //gameObject.GetComponent<MeshRenderer>().material = myMaterial;
    }

    protected override void CheckTransitions() { }
}
