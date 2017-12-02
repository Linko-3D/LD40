using System;

public class PrincessCakeModel : IWeightable {

    [Serializable]
    public class Settings {
        public int MaxWeight = 10;
        public int MinWeight = 1;
        public int WeightToAddOnEatCake = 1;
        public int WeightToSetOnDrinkTea = 1;
    }

    private Logger _logger;
    private Settings _settings;

    public int CakesEaten { get; private set; }
    public int TeasDrunk { get; private set; }
    public int Weight { get; private set; }

    public PrincessCakeModel(Settings settings) {
        _logger = Game.Instance.LoggerFactory("PrincessCake");
        _settings = settings;

        _logger.Assert(_settings != null, "PrincessCakeModel Config should not be null");
        _logger.Assert(_settings.MinWeight > 0, "MinWeight should be greater than zero");
        _logger.Assert(_settings.MaxWeight > _settings.MinWeight, "MaxWeight should be greater than MinWeight");
        _logger.Assert(_settings.WeightToAddOnEatCake > 0, "WeightToAddOnEatCake should be greater than zero");
        _logger.Assert(_settings.WeightToAddOnEatCake < _settings.MaxWeight, "WeightToAddOnEatCake should be less than MaxWeight(" + _settings.MaxWeight + ")");
        _logger.Assert(_settings.WeightToSetOnDrinkTea < _settings.MaxWeight, "WeightToSetOnDrinkTea should be less than MaxWeight(" + _settings.MaxWeight + ")");
        _logger.Assert(_settings.WeightToSetOnDrinkTea >= _settings.MinWeight, "WeightToSetOnDrinkTea should be less than or equal to MinWeight(" + _settings.MinWeight + ")");
    }

    public void EatCake() {
        ++CakesEaten;
        Weight += _settings.WeightToAddOnEatCake;
        if (Weight > _settings.MaxWeight) {
            Weight = _settings.MaxWeight;
        }
    }

    public void DrinkTea() {
        ++TeasDrunk;
        Weight = _settings.WeightToSetOnDrinkTea;
    }

    public bool CanMove(BoxModel box) {
        return Weight >= box.Weight;
    }

    public bool CanPress(GroundButtonModel groudButton) {
        return groudButton.CanBePressedBy(this);
    }
}
