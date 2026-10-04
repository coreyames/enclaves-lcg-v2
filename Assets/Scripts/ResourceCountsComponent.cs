using UnityEngine;

public class ResourceCountsComponent : MonoBehaviour {
    
    private readonly string BasicPrefabPath = "Prefabs/ResourceCountsPanelFull";
    private readonly string FullPrefabPath = "Prefabs/ResourceCountsPanelFull";
    private readonly string EnergyPrefabPath = "Prefabs/ResourceCountsPanelFull";

    public enum PREFAB_PATH {
        BASIC,
        FULL,
        ENERGY
    }
    
    public string GetNeededPrefabPath(PREFAB_PATH choice) {
        return choice switch {
            PREFAB_PATH.BASIC => BasicPrefabPath,
            PREFAB_PATH.FULL => FullPrefabPath,
            PREFAB_PATH.ENERGY => EnergyPrefabPath,
            _ => "",
        };
    }
}