using UnityEngine;
using Unity.Entities;

public class BlockIDAuthoring : MonoBehaviour
{
    public float blockID;
}

public class BlcokIDBaker : Baker<BlockIDAuthoring>
{
    public override void Bake(BlockIDAuthoring authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
        AddComponent(entity, new BlockIDComponent
        {
            blockID = authoring.blockID
        });
    }
}