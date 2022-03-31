﻿using HarmonyLib;
using UnityEngine;
using WWC.Extensions;
using WWC.MonoBehaviours;
using Sonigon;
using UnboundLib;

namespace WWC.Patches
{
    [HarmonyPatch(typeof(CharacterStatModifiers))] 
    class CharacterStatModifiers_Patch
    {
        [HarmonyPostfix]
        [HarmonyPatch("ConfigureMassAndSize")]
        static void MassAdjustment(CharacterStatModifiers __instance, CharacterData ___data)
        {
            if (__instance.GetAdditionalData().MassModifier != 1f)
            {
                float massCurr = (float)___data.playerVel.GetFieldValue("mass");
                float massMod = __instance.GetAdditionalData().MassModifier;
                float massTarg = massCurr * massMod;
                ___data.playerVel.SetFieldValue("mass", massTarg);
            }
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