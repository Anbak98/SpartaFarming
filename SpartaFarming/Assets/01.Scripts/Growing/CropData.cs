using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CropData", menuName = "CropData")]
public class CropData : ScriptableObject
{
    [SerializeField]
    private Sprite[] sprites;
    private CropGrowingInfoLoader growingInfoLoader;

    public int CropMaxLevel => _cropMaxLevel;

    public Sprite GetSprite(int cropKey, int cropLevel)
    {
        return sprites[cropKey * _cropKeyOffset + cropLevel];
    }

    public CropGrowingInfo GetGrowingInfo(int cropKey)
    {
        return growingInfoLoader.GetByKey(cropKey);
    }

    #region PRIVATE
    private const int _cropKeyOffset = 7;
    private const int _cropMaxLevel = 5;

    private void OnEnable()
    {
        growingInfoLoader = new();
    }
    #endregion
}
