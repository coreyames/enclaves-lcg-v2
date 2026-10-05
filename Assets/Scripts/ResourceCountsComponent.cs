using UnityEngine;

public class ResourceCountsComponent : MonoBehaviour {
    public enum PREFAB_PATH {
        FULL,
        COMMIT
    }
    
    public PREFAB_PATH PrefabType { get; set; } = PREFAB_PATH.FULL;
    private readonly string FullPrefabPath = "Prefabs/ResourceCountsPanelFull";
    private readonly string CommitPrefabPath = "Prefabs/ResourceCountsPanelFull";
     
    public ValuePanel Personnel   { get; set; }
    public ValuePanel Survivalist { get; set; }
    public ValuePanel Mechanic    { get; set; }
    public ValuePanel Biologist   { get; set; }
    public ValuePanel Analyst     { get; set; }
    public ValuePanel Steward     { get; set; }
    public ValuePanel Ancillary   { get; set; }
    public ValuePanel EnergyReady { get; set; }
    public ValuePanel EnergyCap   { get; set; }
    public ValuePanel Food        { get; set; }
    public ValuePanel Water       { get; set; }
    public ValuePanel Component   { get; set; }
    public ValuePanel Stability   { get; set; }
    public ValuePanel Despair     { get; set; }
         
    public string GetNeededPrefabPath(PREFAB_PATH choice) {
        return choice switch {
            PREFAB_PATH.FULL => FullPrefabPath,
            PREFAB_PATH.COMMIT => CommitPrefabPath,
            _ => "",
        };
    }
    
    public void Start() { 
        Personnel   = GameObject.Find("PersonnelCountPanel")  .GetComponent<ValuePanel>();
        Survivalist = GameObject.Find("SurvivalistCountPanel").GetComponent<ValuePanel>();
        Mechanic    = GameObject.Find("MechanicCountPanel")   .GetComponent<ValuePanel>();
        Biologist   = GameObject.Find("BiologistCountPanel")  .GetComponent<ValuePanel>();
        Personnel   = GameObject.Find("AnalystCountPanel")    .GetComponent<ValuePanel>();
        Steward     = GameObject.Find("StewardCountPanel")    .GetComponent<ValuePanel>();
        Food        = GameObject.Find("FoodCountPanel")       .GetComponent<ValuePanel>();
        Water       = GameObject.Find("WaterCountPanel")      .GetComponent<ValuePanel>();
        Component   = GameObject.Find("ComponentCountPanel")  .GetComponent<ValuePanel>();
        EnergyReady = GameObject.Find("EnergyReadyCountPanel").GetComponent<ValuePanel>();
         
        if (PrefabType == PREFAB_PATH.FULL) {
            Ancillary = GameObject.Find("AncillaryCountPanel").GetComponent<ValuePanel>();
            EnergyCap = GameObject.Find("EnergyCapCountPanel").GetComponent<ValuePanel>();         
            Stability = GameObject.Find("StabilityCountPanel").GetComponent<ValuePanel>();
            Despair   = GameObject.Find("DespairCountPanel")  .GetComponent<ValuePanel>();
        }
    }
}