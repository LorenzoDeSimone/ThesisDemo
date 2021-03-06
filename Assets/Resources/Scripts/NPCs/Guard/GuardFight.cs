﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardFight : GuardState
{
    private float timefromLastAttack;
    private float elapsedTime;
    private float timeToWaitBeforeAttack;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Initialization(animator);
        myGuardStatus.MovingStatus = CharacterStatus.movingIdleValue;
        navMeshAgent.speed = myGuardStatus.runSpeed;
        if (myGuardStatus.Target)
            navMeshAgent.destination = myGuardStatus.Target.position;
        myFSM.SetBool("fighting", true);

        timeToWaitBeforeAttack = Random.Range(0f, 2f);
        elapsedTime = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (myGuardStatus.Target == null)
        {
            myGuardStatus.MovingStatus = CharacterStatus.movingIdleValue;
            myFSM.SetBool("fighting", false);
            return;
        }

        float distance = Vector3.Distance(myFSM.transform.position, myGuardStatus.Target.position);
        //Actual fighting case
        if (distance <= navMeshAgent.stoppingDistance && !myGuardStatus.DeathStatus)
        {
            elapsedTime += Time.deltaTime;
            navMeshAgent.isStopped = true;
            RotateTowards(myGuardStatus.Target.position);
            myGuardStatus.MovingStatus = CharacterStatus.movingIdleValue;
            if (elapsedTime > timeToWaitBeforeAttack)
            {
                elapsedTime = 0f;
                myGuardStatus.RequestAttack();
                myFSM.SetTrigger("newCombo");
            }
        }
        else
        {
            elapsedTime = 0f;
            navMeshAgent.destination = myGuardStatus.Target.position;
            navMeshAgent.isStopped = false;
            myGuardStatus.MovingStatus = CharacterStatus.movingRunValue;
        }

        CheckTransitions();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}f

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    protected override void CheckTransitions()
    {
        base.CheckTransitions();
        
        float distance = Vector3.Distance(myFSM.transform.position, myGuardStatus.Target.position);

        if (distance > myGuardStatus.attackRadius)
        {
            myFSM.SetBool("fighting", false);
        } 
        /*if (IsTargetInSight(myGuardStatus.chaseViewRadius))
        {
            myGuardStatus.lastTargetPosition = myGuardStatus.target.position;
            myFSM.SetInteger("targetInSight", GuardState.targetInSight);
        }
        else
        {
            myFSM.SetInteger("targetInSight", GuardState.targetNotSeen);
        }*/
    }
}
