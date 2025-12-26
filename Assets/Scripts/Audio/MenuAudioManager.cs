using UnityEngine;

namespace TopDown.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class MenuAudioManager : MonoBehaviour
    {
        [Header("Menu Sound")]
        [SerializeField] private AudioClip introSound;
        
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        private void Start()
        {
            if (introSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(introSound);
            }
        }
    }
}
