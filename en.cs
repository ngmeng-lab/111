using HarmonyLib;

namespace PhotosynthesisTrait
{
    public class Patches
    {
        [HarmonyPatch(typeof(Db))]
        [HarmonyPatch("Initialize")]
        public class Db_Initialize_Patch
        {
            private static bool _done = false;

            public static void Postfix()
            {
                if (_done) return;
                _done = true;

                var tech = Db.Get().Techs.Get("ImprovedOxygen");
                if (tech != null && !tech.unlockedItemIDs.Contains("WaterPump"))
                {
                    tech.unlockedItemIDs.Add("WaterPump");
                }
            }
        }
    }
}