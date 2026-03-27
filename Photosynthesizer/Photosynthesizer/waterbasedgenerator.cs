using System;
using STRINGS;
using TUNING;
using UnityEngine;

public class WaterGeneratorConfig : IBuildingConfig
{
    public override BuildingDef CreateBuildingDef()
    {
        string id = "waterbasedgenerator";
        int width = 3;
        int height = 3;
        string anim = "waterbasedgenerator_kanim";
        int hitpoints = 100;
        float construction_time = 480f;
        string[] array = new string[]
        {
            "Metal"
        };
        float[] construction_mass = new float[]
        {
            TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER5[0]
        };
        string[] construction_materials = array;
        float melting_point = 2400f;
        BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
        EffectorValues tier = NOISE_POLLUTION.NOISY.TIER5;
        BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, tier, 0.2f);
        buildingDef.GeneratorWattageRating = 400f;
        buildingDef.GeneratorBaseCapacity = buildingDef.GeneratorWattageRating;
        buildingDef.ExhaustKilowattsWhenActive = 0f;
        buildingDef.SelfHeatKilowattsWhenActive = 1f;
        buildingDef.ViewMode = OverlayModes.Power.ID;
        buildingDef.AudioCategory = "Metal";
        buildingDef.UtilityInputOffset = new CellOffset(-1, 2);
        buildingDef.UtilityOutputOffset = new CellOffset(1, 0);
        buildingDef.RequiresPowerOutput = true;
        buildingDef.PowerOutputOffset = new CellOffset(1, 0);
        buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
        buildingDef.InputConduitType = ConduitType.Liquid;
        buildingDef.OutputConduitType = ConduitType.Liquid;
        buildingDef.AddSearchTerms(SEARCH_TERMS.POWER);
        buildingDef.AddSearchTerms(SEARCH_TERMS.GENERATOR);
        return buildingDef;
    }

    public override void DoPostConfigureComplete(GameObject go)
    {
        go.AddOrGet<LogicOperationalController>();
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.PowerBuilding, false);
        go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.GeneratorType, false);
        go.AddOrGet<LoopingSounds>();
        Storage storage = go.AddOrGet<Storage>();
        storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
        BuildingDef def = go.GetComponent<Building>().Def;
        float capacity = 10f;
        go.AddOrGet<LoopingSounds>();

        ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
        conduitConsumer.conduitType = def.InputConduitType;
        conduitConsumer.consumptionRate = 1f;
        conduitConsumer.capacityTag = GameTags.Water;
        conduitConsumer.capacityKG = capacity;
        conduitConsumer.forceAlwaysSatisfied = true;
        conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;

        ConduitDispenser conduitDispenser = go.AddOrGet<ConduitDispenser>();
        conduitDispenser.conduitType = def.OutputConduitType;
        conduitDispenser.elementFilter = new SimHashes[] { SimHashes.DirtyWater };
        conduitDispenser.storage = storage;
        conduitDispenser.alwaysDispense = true;

        EnergyGenerator energyGenerator = go.AddOrGet<EnergyGenerator>();
        energyGenerator.powerDistributionOrder = 8;
        energyGenerator.ignoreBatteryRefillPercent = true;
        energyGenerator.hasMeter = true;
        energyGenerator.formula = new EnergyGenerator.Formula
        {
            inputs = new EnergyGenerator.InputItem[]
            {
                new EnergyGenerator.InputItem(GameTags.Water, 1f, capacity)
			},
            outputs = new EnergyGenerator.OutputItem[]
            {
                new EnergyGenerator.OutputItem(SimHashes.DirtyWater, 0.5f, false, new CellOffset(1, 0), 303.15f)
			}
        };
        Tinkerable.MakePowerTinkerable(go);
        go.AddOrGetDef<PoweredActiveController.Def>();
    }

    public const string ID = "waterbasedgenerator";
    public const float CONSUMPTION_RATE = 1f;
    public const float EXHAUST_LIQUID_RATE = 0.5f;
    private const int WIDTH = 3;
    private const int HEIGHT = 3;
}