using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

public partial struct ForkSystem : ISystem
{
    InputComponent inputComponent;

    public void OnStart(ref SystemState system)
    {
        foreach (var (transform, fork) in SystemAPI.Query<RefRW<
            LocalTransform>,
            RefRO<ForkComponent>>())
        {
            transform.ValueRW.Rotation = quaternion.Euler(math.radians(fork.ValueRO.BaseRotation));
        }
    }

    public void OnUpdate(ref SystemState system)
    {
        EntityManager entityManager = system.EntityManager;

        if(!SystemAPI.TryGetSingleton(out inputComponent))
        {
            return;
        }

        float deltaTime = SystemAPI.Time.DeltaTime;

        foreach (var (transform, mass, velocity, fork) in SystemAPI.Query<RefRW<
            LocalTransform>,
            RefRW<PhysicsMass>,
            RefRW<PhysicsVelocity>,
            RefRW<ForkComponent>>())
        {
            if (!SystemAPI.Exists(fork.ValueRO.Owner))
                continue;

            var ownerTransform = SystemAPI.GetComponent<LocalTransform>(fork.ValueRO.Owner);
            float3 offset = fork.ValueRO.Offset;

            // mass.ValueRW.IsKinematic = true;
            mass.ValueRW.InverseInertia = float3.zero;
            // velocity.ValueRW.Linear = float3.zero;
            // velocity.ValueRW.Angular = float3.zero;

            //transform.ValueRW.Position = ownerTransform.Position + offset + fork.ValueRO.LeverOffset;
                //+ math.rotate(ownerTransform.Rotation, offset);
            // transform.ValueRW.Rotation = quaternion.Euler(math.radians(fork.ValueRO.BaseRotation));
            //fork.ValueRW.LeverOffset += new float3(0, inputComponent.forkLever * fork.ValueRO.MoveSpeed * deltaTime, 0);
            //transform.ValueRW = transform.ValueRO.RotateY(math.radians(inputComponent.forkDirection) * fork.ValueRO.RotationSpeed * deltaTime);

            fork.ValueRW.LeverOffset +=
                new float3(0,
                inputComponent.forkLever * fork.ValueRO.MoveSpeed * deltaTime,
                0);

            float3 target =
                ownerTransform.Position +
                offset +
                fork.ValueRO.LeverOffset;

            velocity.ValueRW.Linear =
                (target - transform.ValueRO.Position) / deltaTime;

            float3 movement = new float3(0, inputComponent.forkLever * fork.ValueRO.MoveSpeed * deltaTime, 0);
            fork.ValueRW.LeverOffset += movement;

            velocity.ValueRW.Angular.y = inputComponent.forkDirection * fork.ValueRO.RotationSpeed;


        }

    }
}
