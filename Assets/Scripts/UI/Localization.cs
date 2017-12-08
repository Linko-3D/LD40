using System;
using System.Collections.Generic;
using System.IO;

public class Localization {

    public enum LanguageId {
        English,
        French,
        Greek
    }

    [Serializable]
    public class Texts {
        public string StatWeight { get; set; }
        public string StatCakes { get; set; }
        public string StatTeas { get; set; }
        public string MenuStart { get; set; }
        public string MenuResume { get; set; }
        public string MenuControls { get; set; }
        public string MenuLanguage { get; set; }
        public string ControlsMovement { get; set; }
        public string ControlsJumping { get; set; }
        public string ControlsPickup { get; set; }
        public string ControlsResetLevel { get; set; }
        public string ControlsMenu { get; set; }
        public string TipWelcome { get; set; }
        public string TipUseCheckpointReset { get; set; }
        public string TipFirstCakeConsumedNomNom { get; set; }
        public string TipFirstCakeConsumedMaximizeWeight { get; set; }
        public string TipReachedMaxWeightPressButtons { get; set; }
        public string TipReachedMaxWeightPickupItem { get; set; }
        public string TipFirstTeaConsumedResetWeight { get; set; }
        public string TipFirstTeaConsumeFitCorridors { get; set; }
        public string TipFirstTeaConsumeJumpHigh { get; set; }
        public string TipHopedOnButtonNotEnoughWeight { get; set; }
        public string TipHopedOnButtonEnoughWeight { get; set; }
        public string TipHopedOnButtonEnoughWeightNoItem { get; set; }
    }

    public class Data {

        private const string LOCALIZATION_DIR = "Localization";

        public static Dictionary<LanguageId, Texts> LoadTexts() {
            Dictionary<LanguageId, Texts> data = new Dictionary<LanguageId, Texts>();
            
            string filePath;
            foreach (LanguageId lang in EnumExtensions.GetEnumValues<LanguageId>()) {
                filePath = Path.Combine(LOCALIZATION_DIR, lang.ToString());
                
                data[lang] = Game.Instance.Data.LoadReadonly<Texts>(filePath);
            }

            return data;
        }
    }

    public event Action OnLanguageUpdated;

    private static Localization _instance;

    private Logger _logger;

    private Dictionary<LanguageId, Texts> _allTexts;

    public LanguageId Language { get; private set; }
    public Texts Text { get; private set; }

    private Localization() { }

    public static Localization Instance {
        get {
            if (_instance != null) {
                return _instance;
            } else {
                _instance = new Localization();

                return _instance;
            }
        }
    }

    public void Initialize(LanguageId defaultLang) {
        Language = defaultLang;
        _allTexts = Data.LoadTexts();

        _logger = Game.Instance.LoggerFactory("Localization");
        _logger.Assert(_allTexts != null, "_allTexts is null, make sure to pass a valid argument");

        foreach (LanguageId l in EnumExtensions.GetEnumValues<LanguageId>()) {
            _logger.Assert(
                _allTexts.ContainsKey(l), "_allTexts does not contain texts for language: "
                + l + ". Make sure to load all the languages defined in the LanguageId enum type."
            );
        }

        SetLanguage(LanguageId.English);
    }

    public void SetLanguage(LanguageId lang) {
        Text = _allTexts[lang];

        OnLanguageUpdated();
    }

}

