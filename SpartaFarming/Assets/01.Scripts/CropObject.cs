using UnityEngine;

public class CropObject : MonoBehaviour, IBuildable
{
    [Header("ScriptableObject"), SerializeField]
    private CropData cropData;
    [Header("Components"), SerializeField]
    private SpriteRenderer sprite;
    
    public void Init(int cropKey)
    {
        _cropKey = cropKey;
        _cropLevel = 0;
        _EXP = 0;

        growingInfo = cropData.GetGrowingInfo(_cropKey);
        _EXPRequirement = growingInfo.expRequirement;

        timeSystem = TimeManager.Instance.TimeSystem;
        weatherSystem = WeatherManager.Instance.WeatherSystem;

        timeSystem.On8oClock += Growing;

        UpdateCropByLevel();
    }

    /// PRIVATE
    #region PRIVATE
    private TimeSystem timeSystem;
    private WeatherSystem weatherSystem;
    private CropGrowingInfo growingInfo;
    private int _cropKey = -1;
    private int _cropLevel = -1;
    private float _EXP = -1;
    private float _EXPRequirement = -1;

    private void Growing()
    {
        if (_cropLevel == cropData.CropMaxLevel)
        {
            timeSystem.On8oClock -= Growing;
            return;
        }

        _EXP += CalculateEXP();

        while (_EXP >= _EXPRequirement)
        {   
            _EXP -= _EXPRequirement;
            ++_cropLevel;
            UpdateCropByLevel();
        }
    }

    private void UpdateCropByLevel()
    {
        sprite.sprite = cropData.GetSprite(_cropKey, _cropLevel);
    }

    private float CalculateEXP()
    {
        //float foo = 1 * EXPGrowingSpeedSeasonCor * EXPGrowingSpeedWeatherCor;
        return 10;
    }
    #endregion
}
