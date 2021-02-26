using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public class SelectedUnitSystem : SystemBase
{
    protected override void OnUpdate()
    {
        var mousePosition = Input.mousePosition;

        if (Input.GetMouseButtonDown(1))
        {
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            mousePosition.z = 0;

            Entities.ForEach((Entity entity, ref SelectedComponent selected, ref Translation translation) =>
            {
                selected.TargetPosition = mousePosition;
                selected.IsMove = true;

            }).ScheduleParallel();
        }

        var deltaTime = Time.DeltaTime;

        Entities.ForEach((Entity entity, ref SelectedComponent selected, ref Translation transform) =>
        {
            if (selected.IsMove)
            {
                transform.Value = math.lerp(transform.Value, selected.TargetPosition, 10.0f * deltaTime);

                var isSamePosition = transform.Value == selected.TargetPosition;

                if (isSamePosition.x && isSamePosition.y && isSamePosition.z)
                {
                    selected.IsMove = false;
                }
            }
        }).ScheduleParallel();
    }
}
