using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

public class Localization {

    public enum LanguageId {
        English,
        //French,
        Greek
    }

    [Serializable]
    public class Texts {
        public string StatWeight;
        public string StatCakes;
        public string StatTeas;
        public string MenuStart;
        public string MenuResume;
        public string MenuControls;
        public string MenuLanguage;
        public string MenuExit;
        public string ControlsMovement;
        public string ControlsJumping;
        public string ControlsPickup;
        public string ControlsResetLevel;
        public string ControlsMenu;
        public string TipWelcome;
        public string TipUseCheckpointReset;
        public string TipFirstCakeConsumedNomNom;
        public string TipFirstCakeConsumedMaximizeWeight;
        public string TipReachedMaxWeightPressButtons;
        public string TipReachedMaxWeightPickupItem;
        public string TipFirstTeaConsumedResetWeight;
        public string TipFirstTeaConsumeFitCorridors;
        public string TipFirstTeaConsumeJumpHigh;
        public string TipHopedOnButtonNotEnoughWeight;
        public string TipHopedOnButtonEnoughWeight;
        public string TipHopedOnButtonEnoughWeightNoItem;

        // Too bored to import JSON.NET or FullSerializer for dictionary support.
        // Let's dublicate memory >.< !!
        public Dictionary<string, string> ToDictionary() {
            return GetType()
                    .GetFields(BindingFlags.Instance | BindingFlags.Public)
                    .ToDictionary(field => field.Name, field => (string)field.GetValue(this));
        }
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
    private Dictionary<LanguageId, Dictionary<string, string>> _allTextsById;
    private Dictionary<string, string> _currTextsById;

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

    public void Initialize(LanguageId defaultLang = LanguageId.English) {
        Language = defaultLang;
        _allTexts = Data.LoadTexts();
        _allTextsById = new Dictionary<LanguageId, Dictionary<string, string>>();

        _logger = Game.Instance.LoggerFactory("Localization");
        _logger.Assert(_allTexts != null, "_allTexts is null, make sure to pass a valid argument");

        foreach (LanguageId l in EnumExtensions.GetEnumValues<LanguageId>()) {
            _logger.Assert(
                _allTexts.ContainsKey(l), "_allTexts does not contain texts for language: "
                + l + ". Make sure to load all the languages defined in the LanguageId enum type."
            );

            _allTextsById[l] = _allTexts[l].ToDictionary();
        }

        SetLanguage(LanguageId.English);
    }

    public string GetTextById(string textId) {
        string text = null;

        if (!_currTextsById.TryGetValue(textId, out text)) {
            _logger.Error("Localization textId(" + textId + ") not found, take a look at the files at the readonly Localization directory.");
        }

        return text;
    }

    public void SetLanguage(LanguageId lang) {
        Language = lang;

        Text = _allTexts[lang];
        _currTextsById = _allTextsById[lang];

        if (OnLanguageUpdated != null) {
            OnLanguageUpdated();
        }
    }

}

