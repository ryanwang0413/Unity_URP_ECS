using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class CubeAuthoring : MonoBehaviour
{
    public float emissionValue;
}

public class CubeBaker : Baker<CubeAuthoring>
{
    public override void Bake(CubeAuthoring authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
        AddComponent(entity, new CubeCompoment
        { });
        AddComponent(entity, new EmissionComponent
        {
            emission = authoring.emissionValue
        });
    }
}
