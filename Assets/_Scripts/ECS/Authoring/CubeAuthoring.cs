using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class CubeAuthoring : MonoBehaviour
{ }

public class CubeBaker : Baker<CubeAuthoring>
{
    public override void Bake(CubeAuthoring authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
        AddComponent(entity, new CubeCompoment
        { });
    }
}
