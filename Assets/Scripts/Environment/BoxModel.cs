using System;

public class BoxModel : IWeightableModel {

    [Serializable]
    public class Settings {
        public int Weight = 5;
    }

    private Logger _logger;
    private Settings _settings;

    public string Name { get; private set; }

    public BoxModel(string name, Settings settings, PrincessCakeModel.Settings princessCakeSettings) {
        Name = name;
        _settings = settings;

        _logger = Game.Instance.LoggerFactory(Name + "::BoxModel");

        _logger.Assert(_settings.Weight == -1 || _settings.Weight <= princessCakeSettings.MaxWeight, "WeightRequiredToMove should be less than or equal to pricessCake.MaxWeight. Or -1 to not be moveable");
        _logger.Assert(_settings.Weight == -1 || _settings.Weight >= princessCakeSettings.MinWeight, "WeightRequiredToMove should be greater than or equal to pricessCake.MinWeight. Or -1 to not be moveable");
    }

    public int Weight { get { return _settings.Weight; } }

    public bool IsMovable { get { return _settings.Weight != -1; } }

    public bool CanMoveBy(PrincessCakeModel princessCake) {
        return princessCake.CanMove(this);
    }

    public bool CanMoveBy(TerrainButtonModel groundButton) {
        return groundButton.CanBePressedBy(this);
    }
}