using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AdjustSortingLayer;

public class AdjustSortingLayer : MonoBehaviour
{
    [System.Serializable]
    public class AdjustableSortingLayer
    {
        [field: SerializeField] public SpriteRenderer SpriteRenderer { private set; get; }
        public Color StartingSpriteColor { private set; get; }
        [field: SerializeField] public float PositionOffset { private set; get; }

        public AdjustableSortingLayer(SpriteRenderer spriteRenderer, float offset)
        {
            SpriteRenderer = spriteRenderer;
            StartingSpriteColor = spriteRenderer.color;
            PositionOffset = offset;
        }
    }

    [SerializeField] private List<AdjustableSortingLayer> _adjustableSortingLayers = new List<AdjustableSortingLayer>();

    private const int MAX_SORTING_LAYER = 32767;
    [SerializeField] private float _positionOffset;
    [SerializeField] private bool _alwaysUpdate = false;


    private void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer)
        {
            _adjustableSortingLayers.Add(new AdjustableSortingLayer(spriteRenderer, _positionOffset));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateSortingLayerOnPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if(_alwaysUpdate)
        {
            UpdateSortingLayerOnPosition();
        }
    }

    private void UpdateSortingLayerOnPosition()
    {
        foreach (AdjustableSortingLayer sortableLayer in _adjustableSortingLayers)
        {
            sortableLayer.SpriteRenderer.sortingOrder = (int)(MAX_SORTING_LAYER - ((transform.position.y + sortableLayer.PositionOffset) * 10f));
        }
    }
}
