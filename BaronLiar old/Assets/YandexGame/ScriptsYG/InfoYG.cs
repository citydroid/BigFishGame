using UnityEngine;
using UnityToolbag;

namespace YG
{
    [CreateAssetMenu(fileName = "YandexGameData", menuName = "YG Settings")]
    public class InfoYG : ScriptableObject
    {
        [Tooltip("При инициализации объекта Player авторизованному игроку будет показано диалоговое окно с запросом на предоставление доступа к персональным данным. Запрашивается доступ только к аватару и имени, идентификатор пользователя всегда передается автоматически. Примерное содержание: Игра запрашивает доступ к вашему аватару и имени пользователя на сервисах Яндекса.\nЕсли вам достаточно знать идентификатор, а имя и аватар пользователя не нужны, используйте опциональный параметр scopes: false. В этом случае диалоговое окно не будет показано.")]
        public bool scopes = true;

        public enum PlayerPhotoSize { small, medium, large };
        [ConditionallyVisible(nameof(scopes))]
        [Tooltip("Размер подкачанного изображения пользователя")]
        public PlayerPhotoSize playerPhotoSize;

        [Space(10)]
        [Tooltip("Вкл/Выкл лидерборды")]
        public bool leaderboardEnable;

        [Header("Ad")]
        [Space(10)]

        [Tooltip("Защита от накруток вознаграждения при использовании рекламы за вознаграждение. Не даёт награду пользователям с AdBlock и другими аналогичными расширениями браузера. Пользователям, которые закрывают рекламу раньше времени. Предотвращает открытие нескольких рекламных блоков и соответственно получения чрезмерной награды")]
        public bool checkAdblock = true;

        public enum FullscreenAdChallenge { atStartupEndSwitchScene, onlyAtStartup };
        [Tooltip("Выберите atStartupEndSwitchScene если хотите, чтобы полноэкранная реклама вызывалась при запуске игры и при переключении сцены. Выберите onlyAtStartup если хотите, чтобы реклама вызывалась только при запуске игры.")]
        public FullscreenAdChallenge fullscreenAdChallenge;

        [Header("Language Translation")]
        [Space(10)]

        public bool LocalizationEnable;

        [Tooltip("Отображать параметры автоматической авторизации в инспекторе компонента LanguageYG")]
        [ConditionallyVisible(nameof(LocalizationEnable))]
        public bool autolocationInspector = true;

        public enum CallingLanguageCheck { FirstLaunchOnly, EveryGameLaunch, DoNotChangeLanguageStartup };
        [Tooltip("Менять язык игры в соответствии с языком браузера:\nFirstLaunchOnly - Только при первом запуске игры\nEveryGameLaunch - Каждый раз при запуске игры\nDoNotChangeLanguageStartup - Не менять язык при запуске игры")]
        [ConditionallyVisible(nameof(LocalizationEnable))]
        public CallingLanguageCheck callingLanguageCheck;

        [System.Serializable]
        public class Languages { public bool ru, en, tr, az, be, he, hy, ka, et, fr, kk, ky, lt, lv, ro, tg, tk, uk, uz; }
        [ConditionallyVisible(nameof(LocalizationEnable))]
        public Languages languages;

        [System.Serializable]
        public class Fonts { public Font defaultFont, ru, en, tr, az, be, he, hy, ka, et, fr, kk, ky, lt, lv, ro, tg, tk, uk, uz; }
        [ConditionallyVisible(nameof(LocalizationEnable))]
        public Fonts fonts;

        [Header("Other")]
        [Space(10)]

        public bool debug = true;
    }
}
