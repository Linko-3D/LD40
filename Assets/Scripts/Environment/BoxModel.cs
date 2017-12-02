using System;

public class BoxModel : IWeightableModel {

    [Serializable]
    public class Settings {
        public int Weight = 5;
    }

    private Logger _logger;
    private Settings _settings;

    public BoxModel(Settings settings, PrincessCakeModel.Settings princessCakeSettings) {
        _logger = Game.Instance.LoggerFactory("Box");
        _settings = settings;

        _logger.Assert(_settings.Weight <= princessCakeSettings.MaxWeight, "WeightRequiredToMove should be less than or equal to pricessCake.MaxWeight");
        _logger.Assert(_settings.Weight >= princessCakeSettings.MinWeight, "WeightRequiredToMove should be greater than or equal to pricessCake.MinWeight");
    }

    public int Weight { get { return _settings.Weight; } }

    public bool CanMoveBy(PrincessCakeModel princessCake) {
        return princessCake.CanMove(this);
    }

    public bool CanMoveBy(GroundButtonModel groundButton) {
        return groundButton.CanBePressedBy(this);
    }
}