using UnityEngine;
using UnityEngine.Assertions.Must;

public class CropObject : MonoBehaviour, IBuildable, ICollectable
{
    [Header("ScriptableObject"), SerializeField]
    private CropData cropData;
    [Header("Components"), SerializeField]
    private SpriteRenderer spriteRender;

    public bool CanHarvested { get; private set; } = false;

    public int itemKey => _cropKey;

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

    public void Harvest()
    {
        if (_cropLevel != cropData.CropMaxLevel)
            return;

        spriteRender.sprite = cropData.GetSprite(_cropKey, cropData.CropMaxLevel + 1);
        CanHarvested = true;
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
        spriteRender.sprite = cropData.GetSprite(_cropKey, _cropLevel);
    }

    private float CalculateEXP()
    {
        //float foo = 1 * EXPGrowingSpeedSeasonCor * EXPGrowingSpeedWeatherCor;
        return 10;
    }
    #endregion
}
