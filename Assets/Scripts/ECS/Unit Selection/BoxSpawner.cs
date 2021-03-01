using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public struct Spawner_FromEntity : IComponentData
{
    public int CountX;
    public int CountY;
    public Entity Prefab;
}

public struct Box : IComponentData
{
}

public class BoxSpawner : MonoBehaviour
{
    public GameObject Prefab;
    public int CountX = 100;
    public int CountY = 100;

    private BlobAssetStore _blobAssetStore;

    private void Start()
    {
        _blobAssetStore = new BlobAssetStore();
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, _blobAssetStore);
        var prefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(Prefab, settings);
        var entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        for (var x = 0; x < CountX; x++)
        {
            for (var y = 0; y < CountY; y++)
            {
                var instance = entityManager.Instantiate(prefab);

                var position = transform.TransformPoint(new float3(x, y, 0));
                entityManager.SetComponentData(instance, new Translation { Value = position });
            }
        }
    }

    private void OnDestroy()
    {
        _blobAssetStore.Dispose();
    }
}
