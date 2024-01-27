using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class DependentResource : Resource
{
    [EnumToggleButtons]
    public UsageType usageType;

    [HideIf("usageType", UsageType.ConstantRequirement)]
    public bool VisitorDependent = false;

    [MinValue(1)]
    [ShowIf("@!VisitorDependent && usageType != UsageType.ConstantRequirement")]
    public int perSecond = 1;
    [MinValue(1)]
    [ShowIf("@VisitorDependent && usageType != UsageType.ConstantRequirement")]
    public int perVisitor = 1;
}