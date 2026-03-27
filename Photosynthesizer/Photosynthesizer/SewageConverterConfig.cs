using System;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000463 RID: 1123
public class SewageConverterConfig : IBuildingConfig
{
    // Token: 0x06001781 RID: 6017 RVA: 0x0008580C File Offset: 0x00083A0C
    public override BuildingDef CreateBuildingDef()
    {
        string id = "SewageConverter";
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
        return buildingDef;
    }

    // Token: 0x06001782 RID: 6018 RVA: 0x00085910 File Offset: 0x00083B10
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
            new ElementConverter.ConsumedElement(SimHashes.Water.CreateTag(), 0.1f, true)
        };
        elementConverter.outputElements = new ElementConverter.OutputElement[]
        {
            new ElementConverter.OutputElement(0.1f, SimHashes.DirtyWater, 0f, false, true, 0f, 0.5f, 0.75f, byte.MaxValue, 0, true),
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
        conduitDispenser.invertElementFilter = false;
        conduitDispenser.elementFilter = new SimHashes[]
        {
            SimHashes.DirtyWater
        };
    }

    // Token: 0x06001783 RID: 6019 RVA: 0x00085AF3 File Offset: 0x00083CF3
    public override void DoPostConfigureComplete(GameObject go)
    {
        go.AddOrGet<LogicOperationalController>();
        go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits, false);
    }
    public const string ID = "SewageConverter";
}
