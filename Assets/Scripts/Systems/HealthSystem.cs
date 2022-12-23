using Unity.Burst;
using Unity.Entities;
using Unity.Collections;

[BurstCompile]
partial struct HealthJob : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter ECB;
    [ReadOnly]public ComponentLookup<Player> players;

    void Execute([ChunkIndexInQuery] int chunkIndex, ref HealthAspect health)
    {
        health.Health = players[health.Parent].health;
    }
}

[BurstCompile]
partial struct HealthSystem : ISystem
{
    public ComponentLookup<Player> players;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        players = state.GetComponentLookup<Player>(true);
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        players.Update(ref state);
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        var healthJob = new HealthJob
        {
            ECB = ecb.AsParallelWriter(),
            players = players
        };
        healthJob.ScheduleParallel();
    }
}