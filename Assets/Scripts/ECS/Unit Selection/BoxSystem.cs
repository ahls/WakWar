using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class BoxSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.WithAll<Box_Component>().ForEach((Entity entity, ref Translation translation) =>
        {
            translation.Value = new float3(translation.Value.x, translation.Value.y, 0f);
        }).Schedule();
    }
}
