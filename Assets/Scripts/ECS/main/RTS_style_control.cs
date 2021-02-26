using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Rendering;
public class RTS_style_control : MonoBehaviour
{

    private EntityManager EM;
    // Start is called before the first frame update
    void Start()
    {
        EM = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    #region 

    private void SpawnTest(Vector3 spawnLocation)
    {
        EntityArchetype entityArchetype = EM.CreateArchetype(typeof(TestingUnit), typeof(moveTo), typeof(Translation), typeof(RenderMesh));

        Entity entity = EM.CreateEntity(entityArchetype);
        EM.SetComponentData<Translation>(entity, new Translation { Value = spawnLocation });
        EM.SetComponentData<moveTo>(entity, new moveTo { move = true, position = spawnLocation, speed = 5 });
    }

    #endregion
}

public struct TestingUnit : IComponentData { }
public struct moveTo: IComponentData
{
    public bool move;
    public float3 position;
    public float3 lastMoveDirection;
    public float speed;
}
