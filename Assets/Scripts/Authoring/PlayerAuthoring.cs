using Unity.Entities;
using UnityEngine;

class PlayerAuthoring : MonoBehaviour
{
    public float health;
}

class PlayerBaker : Baker<PlayerAuthoring>
{
    public override void Bake(PlayerAuthoring authoring)
    {
        AddComponent(new Player
        {
            health = authoring.health
        });
    }
}
