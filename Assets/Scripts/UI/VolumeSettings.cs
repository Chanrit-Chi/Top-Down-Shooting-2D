using UnityEngine;
using UnityEngine.UI;
using TopDown.Audio;

namespace TopDown.UI
{
    public class VolumeSettings : MonoBehaviour
    {
        [Header("UI Sliders")]
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;

        private void Start()
        {
            LoadVolumeSettings();
            
            // Add listeners to sliders
            if (musicSlider != null)
            {
                musicSlider.onValueChanged.AddListener(SetMusicVolume);
            }
            
            if (sfxSlider != null)
            {
                sfxSlider.onValueChanged.AddListener(SetSFXVolume);
            }
        }

        private void LoadVolumeSettings()
        {
            if (AudioManager.Instance != null)
            {
                if (musicSlider != null)
                {
                    musicSlider.value = AudioManager.Instance.GetMusicVolume();
                }
                
                if (sfxSlider != null)
                {
                    sfxSlider.value = AudioManager.Instance.GetSFXVolume();
                }
            }
        }

        public void SetMusicVolume(float volume)
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.SetMusicVolume(volume);
            }
        }

        public void SetSFXVolume(float volume)
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.SetSFXVolume(volume);
            }
        }

        private void OnDestroy()
        {
            // Remove listeners to avoid memory leaks
            if (musicSlider != null)
            {
                musicSlider.onValueChanged.RemoveListener(SetMusicVolume);
            }
            
            if (sfxSlider != null)
            {
                sfxSlider.onValueChanged.RemoveListener(SetSFXVolume);
            }
        }
    }
}
