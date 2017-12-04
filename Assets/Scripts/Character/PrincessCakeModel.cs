using System;

[Serializable]
public class PrincessCakeModel : IWeightableModel {

    public event Action OnConsumeCake;
    public event Action OnConsumeTea;

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
    private string _name;
    private int _weight;
    private int _cakesEaten;
    private int _teasDrunk;

    public string Name { get { return _name; } }
    public int Weight { get { return _weight; } }
    public int CakesEaten { get { return _cakesEaten; } }
    public int TeasDrunk { get { return _teasDrunk; } }

    public PrincessCakeModel(string name, Settings settings) {
        _name = name;
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

        _weight = _settings.WeightStartsAt;
        _cakesEaten = 0;
        _teasDrunk = 0;
    }

    public PrincessCakeModel(PrincessCakeModel other)
        : this(other.Name, other._settings) {

        CopyStats(other);
    }

    public void CopyStats(PrincessCakeModel other) {
        _weight = other.Weight;
        _cakesEaten = other.CakesEaten;
        _teasDrunk = other.TeasDrunk;
    }

    public void EatCake() {
        ++_cakesEaten;
        _weight += _settings.WeightToAddOnEatCake;
        if (_weight > _settings.MaxWeight) {
            _weight = _settings.MaxWeight;
        }

        _logger.Info("CakeEaten", "CakesEaten: " + CakesEaten + ", Weight: " + _weight);

        if (OnConsumeCake != null) {
            OnConsumeCake();
        }
    }

    public void DrinkTea() {
        ++_teasDrunk;
        _weight = _settings.WeightToSetOnDrinkTea;

        _logger.Info("TeaDrunk", "TeasDrunk: " + TeasDrunk + ", Weight: " + Weight);

        if (OnConsumeTea != null) {
            OnConsumeTea();
        }
    }

    public bool CanMove(BoxModel box) {
        return box.IsMovable && Weight >= box.Weight;
    }

    public bool CanPress(TerrainButtonModel groudButton) {
        return groudButton.CanBePressedBy(this);
    }
}
