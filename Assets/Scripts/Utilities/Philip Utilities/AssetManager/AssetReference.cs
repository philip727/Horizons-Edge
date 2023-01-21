using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Philip.Utilities {
    [System.Serializable]
    public class AssetReference<TAsset> where TAsset : Object
    {
        [field:SerializeField] public string ReferenceName { private set; get; }
        [field:SerializeField] public TAsset Asset { private set; get; }
    }
}

