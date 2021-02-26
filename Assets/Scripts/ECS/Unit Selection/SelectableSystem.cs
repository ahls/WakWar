using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;

/// <summary>
/// 셀렉터블 시스템에 사용되는 컴포넌트 입니다. 
/// 내용물이 없는 컴포넌트라 시스템과 같은 파일에서 선언했어요.
/// </summary>
public struct SelectableComponent : IComponentData{}
public struct SelectedComponent : IComponentData { }

public class SelectableSystem : ComponentSystem
{
    private float3 startPosition;
    protected override void OnUpdate()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //마우스 좌버튼 눌림
            Debug.Log("좌클릭 감지");
            startPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SelectableAuthor.singleton.selectionBoxTransform.gameObject.SetActive(true);
            SelectableAuthor.singleton.selectionBoxTransform.position = startPosition;
        }

        if(Input.GetMouseButton(0))
        {
            //마우스 좌버튼 눌린 상태
            float3 selectionBoxSize = (float3)Camera.main.ScreenToWorldPoint(Input.mousePosition) - startPosition;
            SelectableAuthor.singleton.selectionBoxTransform.localScale = selectionBoxSize;
        }
        if(Input.GetMouseButtonUp(0))
        {
            //마우스 좌버튼 뗌

            SelectableAuthor.singleton.selectionBoxTransform.gameObject.SetActive(false);

            float3 endPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            float3 bottomLeftPoint = new float3(math.min(startPosition.x, endPosition.x), math.min(startPosition.y, endPosition.y), 0);
            float3 topRightPoint = new float3(math.max(startPosition.x, endPosition.x), math.max(startPosition.y, endPosition.y), 0);

            float selectMinSize = 8;
            float selectArea = math.distance(bottomLeftPoint, topRightPoint);
            bool selectOne = false;
            int numSelected = 0;

            // 드래그가 아니라 클릭 이었을 경우, 한 유닛만 선택
            if (selectArea < selectMinSize) 
            {
                selectOne = true;
                bottomLeftPoint = new float3(-1, -1, 0) * (selectMinSize - selectArea) * 0.5f;
                topRightPoint = new float3(1, 1, 0) * (selectMinSize - selectArea) * 0.5f;
            }
            
            // 쉬프트 누르고 있지 않으면 현재 선택된 유닛이 선택 해제됨
            if (!Input.GetKey(KeyCode.LeftShift))
            {
                Entities.WithAll<SelectedComponent>().ForEach((Entity entity) =>
                    {
                        PostUpdateCommands.RemoveComponent<SelectedComponent>(entity);
                    }
                    );
            }

            //범위 안에 있는 엔티티에 선택됨 컴포넌트 추가
            Entities.ForEach((Entity entity, ref SelectableComponent selectable, ref Translation translation)=>
                {
                    if (selectOne == false || numSelected < 1)
                    {
                        float3 currentLocation = translation.Value;
                        if (
                        currentLocation.x <= topRightPoint.x &&
                        currentLocation.x >= bottomLeftPoint.x &&
                        currentLocation.y <= topRightPoint.y &&
                        currentLocation.y >= bottomLeftPoint.y)
                        {
                            PostUpdateCommands.AddComponent(entity, new SelectedComponent { });
                        }
                        numSelected++;
                    }
                });

            Debug.Log("좌하단: " + bottomLeftPoint.x + ", " + bottomLeftPoint.y + "#  우상단: " + topRightPoint.x + ", " + topRightPoint.y);
        }



    }
}
