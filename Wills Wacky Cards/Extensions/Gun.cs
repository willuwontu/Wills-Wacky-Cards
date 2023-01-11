﻿using System;
using System.Runtime.CompilerServices;
using HarmonyLib;
using UnityEngine;
using WWC.MonoBehaviours;

namespace WWC.Extensions
{
    // ADD FIELDS TO GUN
    [Serializable]
    public class GunAdditionalData
    {
        public bool useForcedAttackSpeed;
        public float forcedReloadSpeed;
        public bool useForcedReloadSpeed;
        public bool useAttacksPerAttack;
        public int attacksPerAttack;
        public bool shotgun;
        public float minimumAttackSpeed;
        public float minimumReloadSpeed;
        public float speedDamageMultiplier;
        public bool overHeated;
        public Minigun_Mono minigun;
        public bool useMinimumReloadSpeed;
        public bool useMinimumAttackSpeed;
        public float frequency;
        public float amplitude;

        public GunAdditionalData()
        {
            useForcedAttackSpeed = false;
            forcedReloadSpeed = 0.0f;
            useForcedReloadSpeed = false;
            attacksPerAttack = 1;
            useAttacksPerAttack = false;
            shotgun = false;
            speedDamageMultiplier = 1f;
            overHeated = false;
            minigun = null;
            useMinimumAttackSpeed = false;
            useMinimumReloadSpeed = false;
            minimumAttackSpeed = 1f;
            minimumReloadSpeed = 1f;
            frequency = 0.5f;
            amplitude = 0.5f;
        }
    }
    public static class GunExtension
    {
        public static readonly ConditionalWeakTable<Gun, GunAdditionalData> data =
            new ConditionalWeakTable<Gun, GunAdditionalData>();

        public static GunAdditionalData GetAdditionalData(this Gun gun)
        {
            return data.GetOrCreateValue(gun);
        }

        public static void AddData(this Gun gun, GunAdditionalData value)
        {
            try
            {
                data.Add(gun, value);
            }
            catch (Exception) { }
        }

        public static float BulletsPerSecond(this Gun gun, GunAmmo gunAmmo = null)
        {
            int proj = gun.numberOfProjectiles;
            int bursts = gun.bursts > 0 ? gun.bursts : 1;
            int bulletsPerAttack = proj * bursts;
            float timeForAttack = (bursts - 1) * gun.timeBetweenBullets;

            float reloadTime = 0f;
            GunAmmo ammo = gunAmmo != null ? gunAmmo : (GunAmmo)Traverse.Create(gun).Field("gunAmmo").GetValue();
            if (ammo != null)
            {
                reloadTime = (ammo.reloadTime + ammo.reloadTimeAdd) * ammo.reloadTimeMultiplier;
            }

            float bulletsPerSecond = bulletsPerAttack / Time.deltaTime;

            if (gun.attackSpeed == 0f && reloadTime == 0f && gun.timeBetweenBullets == 0f)
            {
                return bulletsPerSecond;
            }
            else if (gun.timeBetweenBullets > reloadTime || gun.attackSpeed > reloadTime)
            {
                bulletsPerSecond = Mathf.Min(bulletsPerAttack / (timeForAttack + gun.attackSpeed), bulletsPerSecond);
            }
            else 
            {
                bulletsPerSecond = Mathf.Min(bulletsPerAttack / (timeForAttack + gun.attackSpeed + reloadTime), bulletsPerSecond);
            }

            return bulletsPerSecond;
        }
    }
    // reset extra gun attributes when resetstats is called
    [HarmonyPatch(typeof(Gun), "ResetStats")]
    class GunPatchResetStats
    {
        private static void Prefix(Gun __instance)
        {
            __instance.GetAdditionalData().useForcedAttackSpeed = false;
            __instance.GetAdditionalData().useForcedReloadSpeed = false;
            __instance.GetAdditionalData().forcedReloadSpeed = 0.0f;
            __instance.GetAdditionalData().useAttacksPerAttack = false;
            __instance.GetAdditionalData().attacksPerAttack = 1;
            __instance.GetAdditionalData().shotgun = false;
            __instance.GetAdditionalData().speedDamageMultiplier = 1f;
            __instance.GetAdditionalData().overHeated = false;
            __instance.GetAdditionalData().minigun = null;
            __instance.GetAdditionalData().useMinimumReloadSpeed = false;
            __instance.GetAdditionalData().useMinimumAttackSpeed = false;
            __instance.GetAdditionalData().minimumAttackSpeed = 1f;
            __instance.GetAdditionalData().minimumReloadSpeed = 1f;
            __instance.GetAdditionalData().frequency = 0.5f;
            __instance.GetAdditionalData().amplitude = 0.5f;
        }
    }
}
