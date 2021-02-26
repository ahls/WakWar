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
    public Material selectionMaterial;
    public Mesh selectionMesh;
    #endregion

    private void Awake()
    {
        singleton = this;
        EM = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    // Start is called before the first frame update
    void Start()
    {

        //실험용 유닛 소환 *5
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


        selectionMesh = CreateMesh(2, 1);
    }

    public static Mesh CreateMesh(float meshWidth, float meshHeight)
    {
        Vector3[] vertices = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] triangles = new int[6];

        float meshWidthHalf = meshWidth / 2f;
        float meshHeightHalf = meshHeight / 2f;

        vertices[0] = new Vector3(-meshWidthHalf, meshHeightHalf);
        vertices[1] = new Vector3(meshWidthHalf, meshHeightHalf);
        vertices[2] = new Vector3(-meshWidthHalf, -meshHeightHalf);
        vertices[3] = new Vector3(meshWidthHalf, -meshHeightHalf);

        uv[0] = new Vector2(0, 1);
        uv[1] = new Vector2(1, 1);
        uv[2] = new Vector2(0, 0);
        uv[3] = new Vector2(1, 0);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;
        triangles[3] = 2;
        triangles[4] = 1;
        triangles[5] = 3;
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        return mesh;
    }
}
