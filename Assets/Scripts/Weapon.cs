﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private CharacterStatus weaponHolder;

    [SerializeField]
    private int damage = 1;

    private Animator myAnimator;
    private int lastAnimatorState = -1;


    // Use this for initialization
    void Start ()
    {
        if (weaponHolder == null)
            Debug.LogError("This weapon has no holder.");
        else
        {
            myAnimator = weaponHolder.GetComponent<Animator>();
            if (myAnimator == null)
                Debug.LogError("The weapon holder as no animator.");
        }
	}

    void OnCollisionStay(Collision collision)
    {

        //Hits only if it is the first collision in the current animator state

        //Debug.Log(weaponHolder.gameObject.name +" Hits" + collision.collider.name);

        //if (weaponHolder.gameObject.name.Equals("Guard"))
        //    Debug.Log(currentAnimatorState.fullPathHash != lastAnimatorState);

        if (weaponHolder.CanWeaponHit(collision.gameObject) && weaponHolder.AttackingStatus && !weaponHolder.gameObject.Equals(collision.collider.gameObject)) 
            //currentAnimatorState.fullPathHash != lastAnimatorState)
        {
            Shield hitShield = collision.collider.GetComponent<Shield>();
            if (hitShield && hitShield.GetShieldHolder()!=null && hitShield.GetShieldHolder().ShieldUpStatus)
            {
                if (weaponHolder.CanWeaponHit(hitShield.GetShieldHolder().gameObject))
                    hitShield.ActivateBlockEffect();
                weaponHolder.AddHitEnemy(hitShield.GetShieldHolder().gameObject, true);
                weaponHolder.AttackBlocked();
            }
            else
            {
                /*if (!weaponHolder.gameObject.Equals("Guard"))
                {
                    Debug.Log(weaponHolder.gameObject.name + " Hits" + collision.collider.name);
                }*/
                //lastAnimatorState = currentAnimatorState.fullPathHash;
                Hittable hitTarget = collision.collider.GetComponent<Hittable>();
                //Check to avoid "friendly fire"
                if (hitTarget && !hitTarget.gameObject.layer.Equals(weaponHolder.gameObject.layer))
                {
                    weaponHolder.AddHitEnemy(collision.gameObject);
                    //Debug.Log(weaponHolder.gameObject.name + " hits " + collision.gameObject);
                    hitTarget.Hit(damage);
                }
            }
        }
    }
}
