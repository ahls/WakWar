using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class UnitSelector : MonoBehaviour
{
    #region 변수
    public static UnitSelector instance;
    List<GameObject> UnitList = new List<GameObject>();
    private Transform selectionBoxTransform;
    private float3 startLocation;
    private const float clickThreshold = 0.1f;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        selectionBoxTransform = transform.Find("selectionBox");
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {///좌클릭 누름
            startLocation = cursorLocation();
            selectionBoxTransform.gameObject.SetActive(true);
            selectionBoxTransform.position = startLocation;

        }
        if(Input.GetMouseButton(0))
        {//좌클릭 눌린상태
            selectionBoxTransform.localScale = cursorLocation() - startLocation;
        }
        if(Input.GetMouseButtonUp(0))
        {//좌클릭 뗌


            //쉬프트 안누르고 있으면 선택 해제 안함
            if(!Input.GetKey(KeyCode.LeftShift))
            {
                foreach (GameObject selectedUnit in UnitList)
                {
                    selectedUnit.GetComponent<UnitStats>().setSelectionCircleState(false);
                }
                UnitList.Clear();//리스트 비움
            }

            selectionBoxTransform.gameObject.SetActive(false);
            float3 endLocation = cursorLocation();

            //오버랩박스 크기 만듬
            float2 centerOfBox = new float2 ( (endLocation.x + startLocation.x) / 2f, (endLocation.y + startLocation.y) / 2f);
            float2 sizeOfBox;

            if (math.distance(endLocation,startLocation) <= clickThreshold) 
            {// 클릭싸이즈면 0.2*0.2 크기 박스로 생성
                Debug.Log(math.distance(endLocation, startLocation));
                sizeOfBox = new float2(0.2f, 0.2f);
            }
            else
            {// 클릭이 아닐경우 평범하게 생성
                sizeOfBox = new float2(math.abs(endLocation.x - startLocation.x), math.abs(endLocation.y - startLocation.y));
            }


            Collider2D [] thingsInSelcetion = Physics2D.OverlapBoxAll(centerOfBox, sizeOfBox, 0f);
            foreach (Collider2D thing in thingsInSelcetion)
            {
                UnitStats tempStats = thing.GetComponent<UnitStats>();
                if(tempStats != null)
                {
                    tempStats.setSelectionCircleState(true);
                    UnitList.Add(thing.gameObject);
                }
            }


        }
    }


    private float3 cursorLocation()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
