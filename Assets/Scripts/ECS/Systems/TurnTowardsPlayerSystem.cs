using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateBefore(typeof(MoveForwardSystem))]
public class TurnTowardsPlayerSystem : JobComponentSystem {

  protected override JobHandle OnUpdate(JobHandle inputDeps) {
    if (Settings.IsPlayerDead())
      return inputDeps;

    float3 playerPosition = new float3(Settings.PlayerPosition);

    Entities
      .WithAll<EnemyTag>()
      .WithBurst()
      .ForEach((ref Translation pos, ref Rotation rot) => {
        float3 heading = playerPosition - pos.Value;
        heading.y = 0f;
        rot.Value = quaternion.LookRotation(heading, math.up());
      }).Run();

    return default;
  }
}

