using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class ResourceOverTime : Resource
{
    [EnumToggleButtons]
    public UsageType usageType;
    [MinValue(1)]
    [HideIf("usageType", UsageType.Requires)]
    public int perSecond = 1;
}