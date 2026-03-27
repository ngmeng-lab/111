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

            // 纯净水转换器
            Strings.Add(new string[]
            {
                "STRINGS.BUILDINGS.PREFABS.CLEANWATERCONVERTER.NAME",
                "纯净水转换器"
            });
            Strings.Add(new string[]
            {
                "STRINGS.BUILDINGS.PREFABS.CLEANWATERCONVERTER.EFFECT",
                "使用纯净水然后产出等量的污水。"
            });
            Strings.Add(new string[]
            {
                "STRINGS.BUILDINGS.PREFABS.CLEANWATERCONVERTER.DESC",
                "“可是我们为什么要把干净的水变脏呢？”"
            });
            ModUtil.AddBuildingToPlanScreen("Plumbing", "CleanWaterConverter");


            // 污水转换器
            Strings.Add(new string[]
            {
                "STRINGS.BUILDINGS.PREFABS.SEWAGECONVERTER.NAME",
                "污水转换器"
            });
            Strings.Add(new string[]
            {
                "STRINGS.BUILDINGS.PREFABS.SEWAGECONVERTER.EFFECT",
                "可以把日常使用的污水变干净并且超额产出。"
            });
            Strings.Add(new string[]
            {
                "STRINGS.BUILDINGS.PREFABS.SEWAGECONVERTER.DESC",
                "“嗯，这玩意儿是不是有点不太对劲？”"
            });
            ModUtil.AddBuildingToPlanScreen("Plumbing", "SewageConverter");

            // 水基发电机
            Strings.Add(new string[]
            {
                "STRINGS.BUILDINGS.PREFABS.WATERBASEDGENERATOR.NAME",
                "水基发电机"
            });
            Strings.Add(new string[]
            {
                "STRINGS.BUILDINGS.PREFABS.WATERBASEDGENERATOR.EFFECT",
                "消耗纯净水并产生电力，同时排出部分污染水。"
            });
            Strings.Add(new string[]
            {
                "STRINGS.BUILDINGS.PREFABS.WATERBASEDGENERATOR.DESC",
                "“水电，但是核动力版。”"
            });
            ModUtil.AddBuildingToPlanScreen("Power", "waterbasedgenerator");
        }
    }
}