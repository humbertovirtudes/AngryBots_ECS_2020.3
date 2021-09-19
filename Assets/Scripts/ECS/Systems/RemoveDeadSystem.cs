using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

[UpdateInGroup(typeof(InitializationSystemGroup))]
[AlwaysSynchronizeSystem]
public class RemoveDeadSystem : SystemBase {

  protected override void OnUpdate() {
    EntityCommandBuffer commandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.TempJob);

    Entities
      .WithAny<PlayerTag>()
      .WithoutBurst()
      .ForEach((Entity entity, in Health health) => {
        if (health.Value <= 0) {
          Settings.PlayerDied();
        }
      }).Run();

    Entities
      .WithAny<EnemyTag>()
      .WithoutBurst()
      .ForEach((Entity entity, in Health health, in Translation pos) => {
        if (health.Value <= 0) {
          commandBuffer.DestroyEntity(entity);
          BulletImpactPool.PlayBulletImpact(pos.Value);
        }
      }).Run();

    commandBuffer.Playback(EntityManager);
    commandBuffer.Dispose();
  }
}
