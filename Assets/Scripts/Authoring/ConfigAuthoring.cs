using Unity.Entities;
using UnityEngine;

class ConfigAuthoring : MonoBehaviour
{
    public GameObject playerPrefab;
    public int playerCount;
}

class ConfigBaker : Baker<ConfigAuthoring>
{
    public override void Bake(ConfigAuthoring authoring)
    {
        AddComponent(new Config
        {
            playerPrefab = GetEntity(authoring.playerPrefab),
            playerCount = authoring.playerCount
        });
    }
}