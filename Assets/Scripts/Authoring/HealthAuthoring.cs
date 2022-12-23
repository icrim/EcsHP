using Unity.Entities;
using UnityEngine;

class HealthAuthoring : MonoBehaviour
{
    [HideInInspector]public float health = 1;
}

class HealthBaker : Baker<HealthAuthoring>
{
    public override void Bake(HealthAuthoring authoring)
    {
        AddComponent(new Health
        {
            health = authoring.health
        });
    }
}
