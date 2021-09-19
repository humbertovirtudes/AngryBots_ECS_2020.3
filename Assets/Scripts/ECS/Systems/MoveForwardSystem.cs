using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Unity.Transforms {
  public class MoveForwardSystem : JobComponentSystem {

    protected override JobHandle OnUpdate(JobHandle inputDeps) {

      float dt = Time.DeltaTime;

      JobHandle jobHandle = Entities
        .WithAll<MoveForward>()
        .WithBurst()
        .ForEach((ref Translation pos, in Rotation rot, in MoveSpeed speed) => {
          pos.Value = pos.Value + (dt * speed.Value * math.forward(rot.Value));
        }).Schedule(inputDeps);

      return jobHandle;
    }
  }
}
