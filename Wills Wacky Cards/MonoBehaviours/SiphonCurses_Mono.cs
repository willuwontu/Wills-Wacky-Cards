﻿using UnityEngine;
using WWC.Extensions;
using UnboundLib.GameModes;
using UnboundLib;
using System.Collections.Generic;
using System;
using System.Linq;

namespace WWC.MonoBehaviours
{
    public class SiphonCurses_Mono : Hooked_Mono
    {
        public CardInfo hiltlessBlade;

        private float multiplier = 1f;

        private CharacterData data;
        private Player player;
        private Block block;
        private HealthHandler health;
        private CharacterStatModifiers stats;
        private Gun gun;
        private GunAmmo gunAmmo;
        private Gravity gravity;
        private WeaponHandler weaponHandler;

        private void Start()
        {
            HookedMonoManager.instance.hookedMonos.Add(this);
            data = GetComponentInParent<CharacterData>();
        }

        private void Update()
        {
            if (!player)
            {
                if (!(data is null))
                {
                    player = data.player;
                    weaponHandler = data.weaponHandler;
                    gun = weaponHandler.gun;
                    gunAmmo = gun.GetComponentInChildren<GunAmmo>();
                    block = data.block;
                    stats = data.stats;
                    health = data.healthHandler;
                    gravity = player.GetComponent<Gravity>();
                }

            }
        }

        public override void OnPointStart()
        {
            multiplier = 1f;
            foreach (var card in player.data.currentCards.Where((cardInfo) => cardInfo.cardName.ToLower() == "Siphon Curses".ToLower()))
            {
                multiplier *= 1.8f;
            }
            stats.movementSpeed *= multiplier;
        }

        public override void OnPointEnd()
        {
            stats.movementSpeed /= multiplier;
        }

        public override void OnGameStart()
        {
            UnityEngine.GameObject.Destroy(this);
        }


        private void OnDestroy()
        {
            HookedMonoManager.instance.hookedMonos.Remove(this);
        }

        public void Destroy()
        {
            UnityEngine.Object.Destroy(this);
        }
    }
}