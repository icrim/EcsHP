using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

[BurstCompile]
partial struct InitPlayerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {

    }


    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var config = SystemAPI.GetSingleton<Config>();

        var ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        var entities = CollectionHelper.CreateNativeArray<Entity>(config.playerCount, Allocator.Temp);
        ecb.Instantiate(config.playerPrefab, entities);

        // run only one time
        state.Enabled = false;
    }
}