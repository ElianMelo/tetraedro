using Unity.Entities;
using UnityEngine;

public class ForkAuthoring : MonoBehaviour
{
    public GameObject Owner;
    public float RotationSpeed;
    public Vector3 Offset;
    public Vector3 BaseRotation;
    public float MoveSpeed;

    public class ForkAuthoringBaker : Baker<ForkAuthoring>
    {
        public override void Bake(ForkAuthoring authoring)
        {
            DependsOn(authoring.GetComponent<Collider>());
            Entity forkAuthoring = GetEntity(TransformUsageFlags.Dynamic);
            Entity owner = GetEntity(authoring.Owner,  TransformUsageFlags.Dynamic);
            ForkComponent forkComponent = new ForkComponent { 
                Owner = owner, 
                RotationSpeed = authoring.RotationSpeed, 
                Offset = authoring.Offset, 
                BaseRotation = authoring.BaseRotation,
                MoveSpeed = authoring.MoveSpeed
            };

            AddComponent(forkAuthoring, forkComponent);
        }
    }
}
