using System;

using UnityEngine;

public class PrincessCakeModel {

    [Serializable]
    public class Settings {
        public int MaxWeight = 10;
        public int MinWeight = 1;
        public int WeightToAddOnEatCake = 1;
        public int WeightToSetOnDrinkTea = 1;
    }

    private Settings _settings;

    public int CakesEaten { get; private set; }
    public int TeasDrunk { get; private set; }
    public int Weight { get; private set; }

    public PrincessCakeModel(Settings settings) {
        _settings = settings;

        Debug.Assert(_settings != null, "PrincessCakeModel Config should not be null");
        Debug.Assert(_settings.MinWeight > 0, "MinWeight should be greater than zero");
        Debug.Assert(_settings.MaxWeight > _settings.MinWeight, "MaxWeight should be greater than MinWeight");
        Debug.Assert(_settings.WeightToAddOnEatCake > 0, "WeightToAddOnEatCake should be greater than zero");
        Debug.Assert(_settings.WeightToAddOnEatCake < _settings.MaxWeight, "WeightToAddOnEatCake should be less than MaxWeight(" + _settings.MaxWeight + ")");
        Debug.Assert(_settings.WeightToSetOnDrinkTea < _settings.MaxWeight, "WeightToSetOnDrinkTea should be less than MaxWeight(" + _settings.MaxWeight + ")");
        Debug.Assert(_settings.WeightToSetOnDrinkTea >= _settings.MinWeight, "WeightToSetOnDrinkTea should be less than or equal to MinWeight(" + _settings.MinWeight + ")");
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
}
