using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitLayerAdjust : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] _sprites;
    private int[] _layerOrders;
    private int _numLayers;
    // Start is called before the first frame update
    void Start()
    {
        _numLayers = _sprites.Length;
        _layerOrders = new int[_numLayers];
        for (int i = 0; i < _numLayers; i++)
        {
            _layerOrders[i] = _sprites[i].sortingOrder;
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        float currentY = transform.position.y;
        for (int i = 0; i < _numLayers; i++)
        {


            _sprites[i].sortingOrder =  _layerOrders[i]- (int)(currentY * 100);
        }
    }
}
