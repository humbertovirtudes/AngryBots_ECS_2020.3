using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

[UpdateAfter(typeof(MoveForwardSystem))]
public class TimedDestroySystem : JobComponentSystem {

  protected override JobHandle OnUpdate(JobHandle inputDeps) {
    float deltaTime = Time.DeltaTime;
    EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);

    Entities
     .WithBurst()
     .ForEach((Entity entity, ref TimeToLive timeToLive) => {
       timeToLive.Value -= deltaTime;
       if (timeToLive.Value <= 0f)
         commandBuffer.DestroyEntity(entity);
     }).Run();

    commandBuffer.Playback(EntityManager);
    commandBuffer.Dispose();

    return default;
  }
}

