﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatistics : MonoBehaviour
{
    //These are the real values that represent the player statistics
    //every method that wants to change character's equip permamently
    //should update through this class
    private static PlayerStatistics instance;
    private static PlayerStatistics rollbackInstance;

    [SerializeField] private WeaponInfo defaultWeapon;
    [SerializeField] private HeadInfo defaultHead;
    [SerializeField] private ArmourInfo defaultArmour;
    [SerializeField] private ShieldInfo defaultShield;
    [SerializeField] private int defaultMaxHealth, defaultVelocity;

    [SerializeField] private WeaponInfo weapon;
    [SerializeField] private HeadInfo head;
    [SerializeField] private ArmourInfo armour;
    [SerializeField] private ShieldInfo shield;

    [SerializeField] private int maxHealth;
    [SerializeField] private float velocity;

    private GameObject player;
    private EquipSlots playerEquipSlots;

    public static PlayerStatistics Instance()
    {
        if (instance == null)
            instance = FindObjectOfType<PlayerStatistics>();

        return instance;
    }

    public void Reset()
    {
        Instance().maxHealth = defaultMaxHealth;
        ApplyPlayerMaxHealth();
        Instance().velocity = defaultVelocity;
        ApplyPlayerVelocity();
        ChangePlayerHead(defaultHead);
        ChangePlayerArmour(defaultArmour);
        ChangePlayerShield(defaultShield);
        ChangePlayerWeapon(defaultWeapon);
    }

    public WeaponInfo PlayerWeaponInfo
    {
        set
        {
            Instance().weapon = value;
            ApplyPlayerWeapon();
        }

        get { return Instance().weapon; }
    }

    public HeadInfo PlayerHeadInfo
    {
        set
        {
            Instance().head = value;
            ApplyPlayerHead();
        }

        get { return Instance().head; }
    }

    public ArmourInfo PlayerArmourInfo
    {
        set
        {
            Instance().armour = value;
            ApplyPlayerArmour();
        }

        get { return Instance().armour; }
    }

    public ShieldInfo PlayerShieldInfo
    {
        set
        {
            Instance().shield = value;
            ApplyPlayerShield();
        }

        get { return Instance().shield; }
    }

    //Equips players with items he/she gathered so far
    void Awake()
    {
        Instance().player = FindObjectOfType<PlayerController>().gameObject;
        Instance().playerEquipSlots = Instance().player.GetComponent<EquipSlots>();
        LoadPlayerStatistics();

        rollbackInstance = (PlayerStatistics)instance.MemberwiseClone();
    }

    public void Rollback()
    {
        instance = rollbackInstance;
    }

    private void LoadPlayerStatistics()
    {
        ApplyPlayerMaxHealth();
        ApplyPlayerVelocity();
        ApplyPlayerHead();
        ApplyPlayerArmour();
        ApplyPlayerShield();
        ApplyPlayerWeapon();
    }

    public void ApplyPlayerMaxHealth()
    {
        Instance().player.GetComponent<Hittable>().MaxHealth = Instance().maxHealth;
    }

    private void ApplyPlayerVelocity()
    {
        Instance().player.GetComponent<PlayerController>().velocity = Instance().velocity;
    }

    private void ApplyPlayerWeapon()
    {
        Instance().playerEquipSlots.weapon.GetComponent<MeshFilter>().mesh = Instance().weapon.mesh;
        Instance().playerEquipSlots.weapon.damage = Instance().weapon.damage;
    }

    public void ChangePlayerWeapon(WeaponInfo weapon)
    {
        PlayerWeaponInfo = weapon;
        ApplyPlayerWeapon();
    }

    private void ApplyPlayerHead()
    {
        Instance().playerEquipSlots.head.GetComponent<MeshFilter>().mesh = Instance().head.mesh;
    }

    public void ChangePlayerHead(HeadInfo head)
    {
        PlayerHeadInfo = head;
        ApplyPlayerHead();
    }

    private void ApplyPlayerArmour()
    {
        Instance().playerEquipSlots.armour.GetComponent<SkinnedMeshRenderer>().sharedMesh = Instance().armour.mesh;
    }


    public void ChangePlayerShield(ShieldInfo shield)
    {
        PlayerShieldInfo = shield;
        ApplyPlayerShield();
    }

    private void ApplyPlayerShield()
    {
        Instance().playerEquipSlots.shield.GetComponent<MeshFilter>().mesh = Instance().shield.mesh;
    }

    public void ChangePlayerArmour(ArmourInfo armour)
    {
        PlayerArmourInfo = armour;
        ApplyPlayerArmour();
    }

    public void GiveHealthBonus(int bonus)
    {
        Instance().maxHealth += bonus;
        ApplyPlayerMaxHealth();
        Instance().player.GetComponent<Hittable>().CurrentHealth = Instance().maxHealth;
    }

    public void GiveVelocityBonus(float bonus)
    {
        Instance().velocity += bonus;
        ApplyPlayerVelocity();
    }
}
