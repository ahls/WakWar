using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
//이 이후부터는 임시로 만드는 엔티티를 위해서 넣음
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Rendering;
public class SelectableAuthor : MonoBehaviour
{
    #region 변수
    public static SelectableAuthor singleton;
    public Transform selectionBoxTransform;
    private EntityManager EM;

    [SerializeField] private Material sampleMaterial;
    [SerializeField] private Mesh sampleMesh;
    #endregion
    private void Awake()
    {
        singleton = this;
        EM = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    // Start is called before the first frame update
    void Start()
    {
        EntityArchetype testArchType = EM.CreateArchetype
            (
            typeof(SelectableComponent),
            typeof(Translation),
            typeof(RenderMesh),
            typeof(RenderBounds),
            typeof(LocalToWorld)
            );

        for (int i = 0; i < 5; i++)
        {


            Entity entity = EM.CreateEntity(testArchType);

            float3 newLocation = new float3(i * 3f, 0f, 0f);
            EM.SetComponentData(entity, new Translation { Value = newLocation });
            EM.SetSharedComponentData(entity, new RenderMesh { material = sampleMaterial, mesh = sampleMesh });

        }
    }

    
}
