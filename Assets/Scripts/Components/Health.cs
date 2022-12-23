using Unity.Entities;
using Unity.Rendering;

[MaterialProperty("_Health")]
public struct Health : IComponentData
{
    public float health;
}
