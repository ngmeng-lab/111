using System;
using STRINGS;
using TUNING;
using UnityEngine;

public class CleanWaterConverterConfig : IBuildingConfig
{
    public override BuildingDef CreateBuildingDef()
    {
        string id = "CleanWaterConverter";
        int width = 4;
        int height = 3;
        string anim = "waterpurifier_kanim";
        int hitpoints = 100;
        float construction_time = 30f;
        float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
        string[] all_METALS = MATERIALS.ALL_METALS;
        float melting_point = 1600f;
        BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
        EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER3;

        BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(
            id, width, height, anim, hitpoints, construction_time,
            tier, all_METALS, melting_point, build_location_rule,
            TUNING.BUILDINGS.DECOR.PENALTY.TIER2, tier2, 0.2f);

        buildingDef.RequiresPowerInput = false;
        buildingDef.EnergyConsumptionWhenActive = 0f;
        buildingDef.ExhaustKilowattsWhenActive = 0f;
        buildingDef.SelfHeatKilowattsWhenActive = 4f;
        buildingDef.InputConduitType = ConduitType.Liquid;
        buildingDef.OutputConduitType = ConduitType.Liquid;
        buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(-1, 0));
        buildingDef.ViewMode = OverlayModes.LiquidConduits.ID;
        buildingDef.AudioCategory = "HollowMetal";
        buildingDef.UtilityInputOffset = new CellOffset(-1, 2);
        buildingDef.UtilityOutputOffset = new CellOffset(2, 2);
        buildingDef.PermittedRotations = PermittedRotations.FlipH;
        buildingDef.AddSearchTerms(SEARCH_TERMS.FILTER);
        buildingDef.AddSearchTerms(SEARCH_TERMS.WATER);

        //// 注册并打印调用栈
        //GeneratedBuildings.RegisterWithOverlay(OverlayScreen.LiquidVentIDs, "CleanWaterConverter");
        //Debug.Log("[WaterPump Mod] WaterPump 注册到 OverlayScreen 完成，调用栈:\n" + Environment.StackTrace);

        return buildingDef;
    }

    public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
    {
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
        Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
        storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
        go.AddOrGet<WaterPurifier>();
        Prioritizable.AddRef(go);
        ElementConverter elementConverter = go.AddOrGet<ElementConverter>();
        elementConverter.consumedElements = new ElementConverter.ConsumedElement[]
        {
            new ElementConverter.ConsumedElement(SimHashes.DirtyWater.CreateTag(), 0.1f, true)
        };
        elementConverter.outputElements = new ElementConverter.OutputElement[]
        {
            new ElementConverter.OutputElement(4.5f, SimHashes.Water, 0f, false, true, 0f, 0.5f, 0.75f, byte.MaxValue, 0, true),
        };
        ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
        conduitConsumer.conduitType = ConduitType.Liquid;
        conduitConsumer.consumptionRate = 10f;
        conduitConsumer.capacityKG = 20f;
        conduitConsumer.capacityTag = GameTags.AnyWater;
        conduitConsumer.forceAlwaysSatisfied = true;
        conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Store;
        ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
        conduitDispenser.conduitType = ConduitType.Liquid;
        conduitDispenser.invertElementFilter = true;
        conduitDispenser.elementFilter = new SimHashes[]
        {
            SimHashes.DirtyWater
        };
    }
    public override void DoPostConfigureComplete(GameObject go)
    {
        go.AddOrGet<LogicOperationalController>();
        go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits, false);
    }
    public const string ID = "CleanWaterConverter";
}
