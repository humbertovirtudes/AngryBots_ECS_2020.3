using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

[UpdateBefore(typeof(CollisionSystem))]
public class PlayerTransformUpdateSystem : JobComponentSystem {

  protected override JobHandle OnUpdate(JobHandle inputDeps) {
    Entities
      .WithoutBurst()
      .WithAll<PlayerTag>()
      .ForEach((ref Translation pos) => {
        if (Settings.IsPlayerDead())
          return;
        pos.Value = Settings.PlayerPosition;
      }).Run();

    return default;
  }
}
