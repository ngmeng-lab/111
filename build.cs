using HarmonyLib;

namespace PhotosynthesisTrait
{
    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
    public class GeneratedBuildings_LoadGeneratedBuildings
    {
        private static bool _done = false;

        public static void Prefix()
        {
            if (_done) return;
            _done = true;

            Strings.Add(new string[]
            {
                "STRINGS.BUILDINGS.PREFABS.WATERPUMP.NAME",
                "低压水泵"
            });
            Strings.Add(new string[]
            {
                "STRINGS.BUILDINGS.PREFABS.WATERPUMP.EFFECT",
                "这台机器只需要一点点冷却水就能够从空气中获取大量的水，足以满足日常需要甚至略有盈余。"
            });
            Strings.Add(new string[]
            {
                "STRINGS.BUILDINGS.PREFABS.WATERPUMP.DESC",
                "“等等，这个家伙用什么生产什么？”"
            });

            ModUtil.AddBuildingToPlanScreen("Plumbing", "WaterPump");
        }
    }
}