using System.Collections.Generic;
using UnityEngine;

namespace Philip.Utilities
{
    [System.Serializable]
    public class AssetManager<TAsset> : Singleton<AssetManager<TAsset>> where TAsset : Object
    {
        [SerializeField] List<AssetReference<TAsset>> _assets;

        public AssetManager() : base()
        {
            _assets = new List<AssetReference<TAsset>>();
        }

        public TAsset FindAsset(string searchFor)
        {
            foreach (AssetReference<TAsset> asset in _assets)
            {
                if (asset.ReferenceName == searchFor)
                {
                    return asset.Asset;
                }
            }

            return null;
        }
    }
}
