

using Sirenix.OdinInspector;

[System.Serializable]
public class Resource
{

    [EnumPaging]
    public ResourceType type;
    public int amount;
}