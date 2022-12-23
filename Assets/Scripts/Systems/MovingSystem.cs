using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[BurstCompile]
partial struct MovingJob : IJobEntity
{
    public EntityCommandBuffer.ParallelWriter ECB;
    public int StartFrame;
    public int EndFrame;

    void Execute([ChunkIndexInQuery] int chunkIndex, ref PlayerAspect player)
    {
        for (int frame = StartFrame; frame < EndFrame; ++frame)
        {
            if (player.Direction.Equals(float2.zero) || frame % 1000 == 0) // generate new direction
            {
                var seed = (uint)(frame + player.Index);
                if (seed == 0) ++seed;
                Unity.Mathematics.Random random = new Unity.Mathematics.Random(seed);
                random.NextDouble();
                player.Direction = random.NextFloat2Direction();
            }

            float3 pos = player.Position;
            pos.x = Mathf.Min(Mathf.Max(-100, pos.x + player.Direction.x * (float)0.1), 100);
            pos.z = Mathf.Min(Mathf.Max(-50, pos.z + player.Direction.y * (float)0.1), 150);
            if (pos.x == -100 || pos.x == 100 || pos.z == -50 || pos.z == 150)
                player.Direction = -player.Direction;
            player.Position = pos;

            player.Health = Mathf.Min(Mathf.Max(0, player.Health + (float)0.001 * (pos.x < 0 ? 1 : -1)), 1);
        }
    }
}

[BurstCompile]
partial struct MovingSystem : ISystem
{
    private int frameCount;
    private float timeCount;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        frameCount = 0;
        timeCount = 0;
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        var movingJob = new MovingJob
        {
            ECB = ecb.AsParallelWriter(),
            StartFrame = frameCount
        };
        timeCount += SystemAPI.Time.DeltaTime * 1000;
        while (frameCount < timeCount) ++frameCount;
        movingJob.EndFrame = frameCount;
        movingJob.ScheduleParallel();
    }
}