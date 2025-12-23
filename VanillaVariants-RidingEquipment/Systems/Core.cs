using AttributeRenderingLibrary;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;

namespace VanillaVariantsRidingEquipment;

// All code in this mod exists solely because of json patch conflicts between various mods that modify the same items

public class Core : ModSystem
{
    #region Behavior Properties
    private JsonObject hoovedWearablesAll;
    private JsonObject hoovedWearablesPillion;
    #endregion

    public override bool ShouldLoad(EnumAppSide forSide) => forSide.IsServer();

    public override void AssetsFinalize(ICoreAPI api)
    {
        foreach (Item item in api.World.Items)
        {
            if (item.Code == null || item.Code.Domain != "game") continue;
            if (!item.Code.PathStartsWith("hoovedwearables")) continue;

            if (item.Code.PathStartsWith("hoovedwearables-lowerbackside-pillion"))
            {
                CollectibleBehaviorShapeTexturesFromAttributes behavior = new(item);
                behavior.Initialize(hoovedWearablesPillion);
                item.CollectibleBehaviors = item.CollectibleBehaviors.Append(behavior);
            }
            else
            {
                CollectibleBehaviorShapeTexturesFromAttributes behavior = new(item);
                behavior.Initialize(hoovedWearablesAll);
                item.CollectibleBehaviors = item.CollectibleBehaviors.Append(behavior);
            }
        }
    }

    public override void AssetsLoaded(ICoreAPI api)
    {
        hoovedWearablesAll = JsonObject.FromJson(api.Assets.TryGet(AssetLocation.Create("vanvar:config/forcedpatches/hoovedwearables-all.json")).ToText());
        hoovedWearablesPillion = JsonObject.FromJson(api.Assets.TryGet(AssetLocation.Create("vanvar:config/forcedpatches/hoovedwearables-pillion.json")).ToText());
    }

    public override void Dispose()
    {
        hoovedWearablesAll = null;
        hoovedWearablesPillion = null;
    }
}