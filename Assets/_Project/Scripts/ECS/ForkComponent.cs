using Unity.Entities;
using Unity.Mathematics;

public struct ForkComponent : IComponentData
{
    public Entity Owner;
    public float MoveSpeed;
    public float RotationSpeed;
    public float3 Offset;
    public float3 BaseRotation;
    public float3 LeverOffset;
}
