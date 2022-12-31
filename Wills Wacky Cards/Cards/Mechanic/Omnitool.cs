﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnboundLib;
using UnboundLib.Cards;
using WWC.Extensions;
using WWC.MonoBehaviours;
using UnityEngine;
using ClassesManagerReborn.Util;

namespace WWC.Cards
{
    class Omnitool : CustomMechanicCard
    {
        public override CardInfo Card { get => card; set { if (!card) { card = value; } } }
        protected override GameObject GetAccessory()
        {
            return WillsWackyManagers.WillsWackyManagers.instance.WWMAssets.LoadAsset<GameObject>("Wrench Accessory");
        }
        public override void Callback()
        {
            gameObject.GetOrAddComponent<ClassNameMono>().className = MechanicClass.name;
        }
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            cardInfo.allowMultiple = false;
            WillsWackyCards.instance.DebugLog($"[{WillsWackyCards.ModInitials}][Card] {GetTitle()} Built");
        }
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            var upgrader = player.GetComponentInChildren<MechanicUpgrader>();

            if (upgrader)
            {
                upgrader.upgradeTimeMult *= 0.5f;
                upgrader.upgradeCooldownMult *= 0.5f;
            }

            WillsWackyCards.instance.DebugLog($"[{WillsWackyCards.ModInitials}][Card] {GetTitle()} Added to Player {player.playerID}");
        }
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            //Drives me crazy
            WillsWackyCards.instance.DebugLog($"[{WillsWackyCards.ModInitials}][Card] {GetTitle()} removed from Player {player.playerID}");
        }

        internal static CardInfo card = null;
        protected override string GetTitle()
        {
            return "Omnitool";
        }
        protected override string GetDescription()
        {
            return "The right tool, at the right time.";
        }
        protected override GameObject GetCardArt()
        {
            GameObject art;

            try
            {
                art = WillsWackyManagers.WillsWackyManagers.instance.WWMAssets.LoadAsset<GameObject>("C_Omnitool");
            }
            catch
            {
                art = null;
            }

            return art;
        }
        protected override GameObject GetCardBase()
        {
            return Mechanic.cardBase;
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return Rarities.Epic;
        }
        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
                new CardInfoStat()
                {
                    positive = true,
                    stat = "Upgrade Time",
                    amount = "-50%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                },
                new CardInfoStat()
                {
                    positive = true,
                    stat = "Upgrade Cooldown",
                    amount = "-50%",
                    simepleAmount = CardInfoStat.SimpleAmount.notAssigned
                }
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.TechWhite;
        }
        public override string GetModName()
        {
            return WillsWackyCards.ModInitials;
        }
        public override bool GetEnabled()
        {
            return true;
        }
    }
}
