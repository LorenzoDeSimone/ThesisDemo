﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class GuardPatrol : GuardState
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Initialization(animator);
        myGuardStatus.MovingStatus = CharacterStatus.movingWalkValue;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        navMeshAgent.destination = myGuardStatus.wayPoints[myGuardStatus.nextWayPoint].position;
        navMeshAgent.isStopped = false;

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && !navMeshAgent.pathPending)
        {
            myGuardStatus.nextWayPoint = (myGuardStatus.nextWayPoint + 1) % myGuardStatus.wayPoints.Length;
        }

        CheckTransitions();
    }

    protected override void CheckTransitions()
    {
        base.CheckTransitions();

        if (IsTargetInSight(myGuardStatus.patrolViewRadius))
        {
            myGuardStatus.lastTargetPosition = myGuardStatus.target.position;
            myFSM.SetInteger("targetInSight", GuardState.targetInSight);
        }
        else
            myFSM.SetInteger("targetInSight", GuardState.targetNotSeen);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}