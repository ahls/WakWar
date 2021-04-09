using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitLayerAdjust : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] sprites;
    private int[] layerOrders;
    private int numLayers;
    // Start is called before the first frame update
    void Start()
    {
        numLayers = sprites.Length;
        layerOrders = new int[numLayers];
        for (int i = 0; i < numLayers; i++)
        {
            layerOrders[i] = sprites[i].sortingOrder;
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        float currentY = transform.position.y;
        for (int i = 0; i < numLayers; i++)
        {


            sprites[i].sortingOrder = (int)(currentY * 100 )+ layerOrders[i];
        }
    }
}
