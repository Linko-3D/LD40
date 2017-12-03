using System;

public class PrincessCakeModel : IWeightableModel {

    [Serializable]
    public class Settings {
        public int MaxWeight = 10;
        public int MinWeight = 1;
        public int WeightStartsAt = 1;
        public int WeightToAddOnEatCake = 1;
        public int WeightToSetOnDrinkTea = 1;
    }

    private Logger _logger;
    private Settings _settings;

    public string Name { get; private set; }
    public int Weight { get; private set; }
    public int CakesEaten { get; private set; }
    public int TeasDrunk { get; private set; }

    public PrincessCakeModel(string name, Settings settings) {
        Name = name;
        _settings = settings;

        _logger = Game.Instance.LoggerFactory(Name + "::PrincessCakeModel");

        _logger.Assert(_settings != null, "PrincessCakeModel Config should not be null");
        _logger.Assert(_settings.MinWeight > 0, "MinWeight should be greater than zero");
        _logger.Assert(_settings.MaxWeight > _settings.MinWeight, "MaxWeight should be greater than MinWeight");
        _logger.Assert(_settings.WeightToAddOnEatCake > 0, "WeightToAddOnEatCake should be greater than zero");
        _logger.Assert(_settings.WeightToAddOnEatCake < _settings.MaxWeight, "WeightToAddOnEatCake should be less than MaxWeight(" + _settings.MaxWeight + ")");
        _logger.Assert(_settings.WeightToSetOnDrinkTea < _settings.MaxWeight, "WeightToSetOnDrinkTea should be less than MaxWeight(" + _settings.MaxWeight + ")");
        _logger.Assert(_settings.WeightToSetOnDrinkTea >= _settings.MinWeight, "WeightToSetOnDrinkTea should be less than or equal to MinWeight(" + _settings.MinWeight + ")");
        _logger.Assert(_settings.WeightStartsAt <= _settings.MaxWeight, "WeightStartsAt should be less than or equal MaxWeight(" + _settings.MaxWeight + ")");
        _logger.Assert(_settings.WeightStartsAt >= _settings.MinWeight, "WeightStartsAt should be less than or equal to MinWeight(" + _settings.MinWeight + ")");

        Weight = _settings.WeightStartsAt;
    }

    public void EatCake() {
        ++CakesEaten;
        Weight += _settings.WeightToAddOnEatCake;
        if (Weight > _settings.MaxWeight) {
            Weight = _settings.MaxWeight;
        }

        _logger.Info("CakeEaten", "CakesEaten: " + CakesEaten + ", Weight: " + Weight);
    }

    public void DrinkTea() {
        ++TeasDrunk;
        Weight = _settings.WeightToSetOnDrinkTea;

        _logger.Info("TeaDrunk", "TeasDrunk: " + TeasDrunk + ", Weight: " + Weight);
    }

    public bool CanMove(BoxModel box) {
        return Weight >= box.Weight;
    }

    public bool CanPress(TerrainButtonModel groudButton) {
        return groudButton.CanBePressedBy(this);
    }
}
