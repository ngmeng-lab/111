using HarmonyLib;
using KMod;
using UnityEngine;

namespace ONI_TraitMod
{
    public class Mod : UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            harmony.PatchAll();
        }
    }
}