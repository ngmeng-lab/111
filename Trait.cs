using HarmonyLib;
using Database;
using Klei.AI;
using TUNING;
using UnityEngine;
using System.Linq;

namespace PhotosynthesisTrait
{
    public class PhotosynthesisTrait
    {
        private const string TRAIT_ID = "PhotosynthesisFirstVersion";
        [HarmonyPatch(typeof(Db), "Initialize")]
        public class Db_Initialize_Patch
        {
            public static void Postfix()
            {
                RegisterTrait();
            }
        }
        public static void RegisterTrait()
        {
            if (Db.Get().traits.resources.Any(t => t.Id == TRAIT_ID)) return;

            Trait trait = new Trait(
                TRAIT_ID,
                "光合作用",
                "通过光合作用获取足够使用的氧气......确实足够，对自己而言。同时提升力量和植物亲和。",
                5f,
                true,
                null,
                true,
                true
            );

            trait.Add(new AttributeModifier(
                Db.Get().Attributes.AirConsumptionRate.Id,
                -DUPLICANTSTATS.STANDARD.BaseStats.OXYGEN_USED_PER_SECOND,
                TRAIT_ID
            ));

            trait.Add(new AttributeModifier(
                Db.Get().Attributes.Strength.Id,
                16,
                TRAIT_ID,
                true
            ));

            trait.Add(new AttributeModifier(
                Db.Get().Attributes.Botanist.Id,
                16,
                TRAIT_ID,
                true 
            ));

            Db.Get().traits.Add(trait);

            if (!DUPLICANTSTATS.GOODTRAITS.Any(t => t.id == TRAIT_ID))
            {
                DUPLICANTSTATS.GOODTRAITS.Add(new DUPLICANTSTATS.TraitVal
                {
                    id = TRAIT_ID,
                    rarity = DUPLICANTSTATS.RARITY_COMMON,
                    statBonus = 5,
                    doNotGenerateTrait = false
                });
            }
        }

        public static void ApplyTraitToDuplicant(MinionStartingStats duplicant)
        {
            if (duplicant.Traits.Any(t => t.Id == TRAIT_ID)) return;

            Trait trait = Db.Get().traits.TryGet(TRAIT_ID);
            if (!trait.IsNullOrDestroyed())
            {
                duplicant.Traits.Add(trait);
            }
        }

        [HarmonyPatch(typeof(MinionStartingStats), "GenerateTraits")]
        public class GenerateTraits_Postfix
        {
            public static void Postfix(MinionStartingStats __instance)
            {
                ApplyTraitToDuplicant(__instance);
            }
        }
    }
}