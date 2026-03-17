using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Physics;

public partial struct BallSystem : ISystem
{
    InputComponent inputComponent;

    public void OnUpdate(ref SystemState state)
    {
        EntityManager entityManager = state.EntityManager;

        if(!SystemAPI.TryGetSingleton(out inputComponent))
        {
            return;
        }

        NativeArray<Entity> entities = entityManager.GetAllEntities(Allocator.Temp);

        foreach (Entity entity in entities) {
            if (entityManager.HasComponent<BallComponent>(entity)){
                BallComponent ballComponent = entityManager.GetComponentData<BallComponent>(entity);
                RefRW<PhysicsVelocity> physicsVelocity = SystemAPI.GetComponentRW<PhysicsVelocity>(entity);
                physicsVelocity.ValueRW.Linear += new float3(inputComponent.movement.x * ballComponent.MoveSpeed * SystemAPI.Time.DeltaTime,
                    0, inputComponent.movement.y * ballComponent.MoveSpeed * SystemAPI.Time.DeltaTime);
            }
        }

        entities.Dispose();
    }
}