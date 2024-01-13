using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuildingScriptableObject", menuName = "ScriptableObjects/Building")]
public class BuildingScriptableObject : ScriptableObject
{
    public int CoinCost;
    public int WoodCost;
    public int StoneCost;
    public int IronCost;

    public BuildingScriptableObject nextLevel = null;


}
