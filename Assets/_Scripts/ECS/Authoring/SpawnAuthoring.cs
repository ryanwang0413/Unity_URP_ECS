using UnityEngine;
using Unity.Entities;

public class SpawnAuthoring : MonoBehaviour
{
    public int grid_rows;
    public int grid_columns;
    public float padding;
    public GameObject singlePrefab;
    public GameObject[] prefabs;
}

public class SpawnBaker : Baker<SpawnAuthoring>
{
    public override void Bake(SpawnAuthoring authoring)
    {
        Entity entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
        //Entity entity = GetEntity(authoring, TransformUsageFlags.None);
        Entity[] entities = new Entity[authoring.prefabs.Length];
        for (int i = 0; i < entities.Length; i++)
        {
            entities[i] = GetEntity(authoring.prefabs[i], TransformUsageFlags.Dynamic);
        }

        AddComponent(entity, new SpawnComponent
        {
            grid_rows = authoring.grid_rows,
            grid_columns = authoring.grid_columns,
            padding = authoring.padding,
            singlePrefab = GetEntity(authoring.singlePrefab, TransformUsageFlags.Dynamic),
            prefabArray = new PrefabArray(entities)
        });
    }
}

public struct PrefabArray
{
    public Entity prefab0;
    public Entity prefab1;
    public Entity prefab2;

    public PrefabArray(Entity[] entities)
    {
        prefab0 = entities[0];
        prefab1 = entities[1];
        prefab2 = entities[2];
    }

    public Entity GetPrefab(int index)
    {
        switch (index)
        {
            case 0: return prefab0;
            case 1: return prefab1;
            case 2: return prefab2;
            default: return prefab0;
        }
    }
}
