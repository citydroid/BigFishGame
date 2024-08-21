using UnityEngine;

namespace YG
{
    public class ViewingAds : MonoBehaviour
    {
        public enum PauseType { AudioPause, TimeScalePause, All };
        [Tooltip("Данный скрипт будет ставить звук или верменную шкалу на паузу при открытии рекламы. После закрытия рекламы звук и временная шкала придут в изначальное значение до открытия рекламы")]
        public PauseType pauseType;

        static bool audioPauseOnAd;
        static float timeScaleOnAd;

        private void OnEnable()
        {
            YandexGame.OpenFullAdEvent += OpenFullAd;
            YandexGame.CloseFullAdEvent += CloseFullAd;
            YandexGame.OpenVideoEvent += OpenRewAd;
            YandexGame.CloseVideoEvent += CloseRewAd;
            YandexGame.CheaterVideoEvent += CloseRewAdError;
        }
        private void OnDisable()
        {
            YandexGame.OpenFullAdEvent -= OpenFullAd;
            YandexGame.CloseFullAdEvent -= CloseFullAd;
            YandexGame.OpenVideoEvent -= OpenRewAd;
            YandexGame.CloseVideoEvent -= CloseRewAd;
            YandexGame.CheaterVideoEvent -= CloseRewAdError;
        }

        void OpenFullAd()
        {
            Pause(true);
        }

        void CloseFullAd()
        {
            Pause(false);
        }

        void OpenRewAd(int ID)
        {
            Pause(true);
        }

        void CloseRewAd(int ID)
        {
            Pause(false);
        }

        void CloseRewAdError()
        {
            Pause(false);
        }

        void Pause(bool pause)
        {
            if (pauseType == PauseType.AudioPause)
            {
                AudioPause(pause);
            }
            else if (pauseType == PauseType.TimeScalePause)
            {
                TimeScalePause(pause);
            }
            else
            {
                AudioPause(pause);
                TimeScalePause(pause);
            }
        }

        void AudioPause(bool pause)
        {
            if (pause)
            {
                audioPauseOnAd = AudioListener.pause;
                AudioListener.pause = true;
            }
            else
                AudioListener.pause = audioPauseOnAd;
        }

        void TimeScalePause(bool pause)
        {
            if (pause)
            {
                timeScaleOnAd = Time.timeScale;
                Time.timeScale = 0;
            }
            else
                Time.timeScale = timeScaleOnAd;
        }
    }
}
