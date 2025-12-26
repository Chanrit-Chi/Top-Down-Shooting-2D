using UnityEngine;

namespace TopDown.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("Background Music")]
        [SerializeField] private AudioClip backgroundMusic;

        private const string MUSIC_VOLUME_KEY = "MusicVolume";
        private const string SFX_VOLUME_KEY = "SFXVolume";

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                SetupAudioSources();
                LoadVolumeSettings();
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        private void Start()
        {
            PlayBackgroundMusic();
        }

        private void SetupAudioSources()
        {
            // Create Music AudioSource if not assigned
            if (musicSource == null)
            {
                musicSource = gameObject.AddComponent<AudioSource>();
                musicSource.loop = true;
                musicSource.playOnAwake = false;
            }

            // Create SFX AudioSource if not assigned
            if (sfxSource == null)
            {
                sfxSource = gameObject.AddComponent<AudioSource>();
                sfxSource.playOnAwake = false;
            }
        }

        private void LoadVolumeSettings()
        {
            float musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 0.7f);
            float sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);

            SetMusicVolume(musicVolume);
            SetSFXVolume(sfxVolume);
        }

        public void PlayBackgroundMusic()
        {
            if (backgroundMusic != null && musicSource != null)
            {
                musicSource.clip = backgroundMusic;
                musicSource.Play();
            }
        }

        public void PlaySFX(AudioClip clip)
        {
            if (sfxSource != null && clip != null)
            {
                sfxSource.PlayOneShot(clip);
            }
        }

        public void SetMusicVolume(float volume)
        {
            volume = Mathf.Clamp01(volume);
            if (musicSource != null)
            {
                musicSource.volume = volume;
            }
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
            PlayerPrefs.Save();
        }

        public void SetSFXVolume(float volume)
        {
            volume = Mathf.Clamp01(volume);
            if (sfxSource != null)
            {
                sfxSource.volume = volume;
            }
            PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);
            PlayerPrefs.Save();
        }

        public float GetMusicVolume()
        {
            return musicSource != null ? musicSource.volume : PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 0.7f);
        }

        public float GetSFXVolume()
        {
            return sfxSource != null ? sfxSource.volume : PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);
        }

        public void StopMusic()
        {
            if (musicSource != null)
            {
                musicSource.Stop();
            }
        }

        public void PauseMusic()
        {
            if (musicSource != null)
            {
                musicSource.Pause();
            }
        }

        public void ResumeMusic()
        {
            if (musicSource != null)
            {
                musicSource.UnPause();
            }
        }
    }
}
