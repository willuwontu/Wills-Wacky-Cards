﻿using HarmonyLib;
using UnityEngine;
using Sonigon;
using WWC.MonoBehaviours;
using WWC.Extensions;
using UnboundLib;
using UnboundLib.Networking;

namespace WWC.Patches
{
    [HarmonyPatch(typeof(CardChoice))] 
    class CardChoice_Patch
    {
        [HarmonyPostfix]
        [HarmonyPriority(Priority.First)]
        [HarmonyPatch("SpawnUniqueCard")]
        static void MomentumReplace(CardChoice __instance, ref GameObject __result, Vector3 pos, Quaternion rot)
        {
            var card = __result.GetComponent<CardInfo>();

            if ((card.sourceCard == WWC.Cards.ImmovableObject.card) || (card.sourceCard == WWC.Cards.UnstoppableForce.card))
            {
                if (PlayerManager.instance.GetPlayerWithID(__instance.pickrID).data.view.IsMine)
                {
                    UnboundLib.NetworkingManager.RPC(typeof(CardChoice_Patch), nameof(CardChoice_Patch.URPCA_IncrementMomentum));
                }

                if (card.sourceCard == WWC.Cards.ImmovableObject.card)
                {
                    var temp = __result;
                    WillsWackyCards.instance.ExecuteAfterFrames(5, () =>
                    {
                        Photon.Pun.PhotonNetwork.Destroy(temp);
                    });

                    var stacks = MomentumTracker.stacks+1;
                    var momentumCard = MomentumTracker.GetDefensecard();
                    __result = (GameObject)__instance.InvokeMethod("Spawn", new object[] { momentumCard.gameObject, pos, rot });
                    __result.GetComponent<CardInfo>().sourceCard = momentumCard;
                    __result.GetComponentInChildren<DamagableEvent>().GetComponent<Collider2D>().enabled = false;
                }

                if (card.sourceCard == WWC.Cards.UnstoppableForce.card)
                {
                    var temp = __result;
                    WillsWackyCards.instance.ExecuteAfterFrames(5, () =>
                    {
                        Photon.Pun.PhotonNetwork.Destroy(temp);
                    });

                    var stacks = MomentumTracker.stacks+1;
                    var momentumCard = MomentumTracker.GetOffensecard();
                    __result = (GameObject)__instance.InvokeMethod("Spawn", new object[] { momentumCard.gameObject, pos, rot });
                    __result.GetComponent<CardInfo>().sourceCard = momentumCard;
                    __result.GetComponentInChildren<DamagableEvent>().GetComponent<Collider2D>().enabled = false;
                }
            }
        }

        [UnboundRPC]
        internal static void URPCA_IncrementMomentum()
        {
            WWC.MonoBehaviours.MomentumTracker.stacks += 1;
            //UnityEngine.Debug.Log($"Stacks increased to {WWC.MonoBehaviours.MomentumTracker.stacks}");
        }

        //[HarmonyPrefix]
        //[HarmonyPatch("SomeMethod")]
        //static void MyMethodName()
        //{

        //}

        //[HarmonyPostfix]
        //[HarmonyPatch("SomeMethod")]
        //static void MyMethodName()
        //{

        //}
    }
}