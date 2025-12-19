using TMPro;
using UniRx;
using DG.Tweening;
using TopDown.Shooting;
using UnityEngine;


namespace TopDown.UI
{
    public class AmmoCounter : MonoBehaviour
    {
    
        [Header("References")]
        [SerializeField] private TextMeshProUGUI ammoText;
        [SerializeField] private GunController gunController;

        private int AmmoInClip;
        private int totalAmmo;
        private CompositeDisposable subscriptions = new CompositeDisposable();

        [Header("Popup Effect")]
        [SerializeField] private Vector2 popupIntensity;
        [SerializeField] private float popupDuration;


        private void OnEnable()
        {
            gunController.currentAmmoInClip.ObserveEveryValueChanged(Property => Property.Value).Subscribe(value =>
            {
                AmmoInClip = value;
                UpdateAmmoCounter(AmmoInClip, totalAmmo);
            }).AddTo(subscriptions);
            gunController.totalAmmo.ObserveEveryValueChanged(Property => Property.Value).Subscribe(value =>
            {
                totalAmmo = value;
                UpdateAmmoCounter(AmmoInClip, totalAmmo);
            }).AddTo(subscriptions);
        }

        private void OnDisable()
        {
            subscriptions.Clear();
        }
        private void UpdateAmmoCounter(int currentAmmo, int totalAmmo)
       {
           ammoText.text = $"{currentAmmo} / {totalAmmo}";
           transform.DOPunchScale(popupIntensity, popupDuration).OnComplete(() =>
           {
               transform.DORewind();
           });
       }
    
    }
}
