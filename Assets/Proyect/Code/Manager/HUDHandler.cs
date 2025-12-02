using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDHandler : MonoBehaviour {
    [Header("Icons")]
    [SerializeField] Image weapongWheelIcon;
    [SerializeField] Image hpSkullIcon;
    [SerializeField] Image manaSkullIcon;
    [SerializeField] Image dashIcon;
    [SerializeField] Image quiverIcon;
    [SerializeField] Image swordIcon;
    [SerializeField] Image bowIcon;
    [SerializeField] Image tomeIcon;

    [Header("Items' texts")]
    [SerializeField] TMP_Text hpPotionsText;
    [SerializeField] TMP_Text manaPotionsText;
    [SerializeField] TMP_Text arrowsText;

    EventBinding<StatsEvent> statsEventBinding;
    EventBinding<DashEvent> dashEventBinding;
    EventBinding<WeaponWheelEvent> weaponWheelEventBinding;

    private void OnEnable() {
        statsEventBinding = new EventBinding<StatsEvent>(UpdateStats);
        EventBus<StatsEvent>.Register(statsEventBinding);

        dashEventBinding = new EventBinding<DashEvent>(UpdateDashIcon);
        EventBus<DashEvent>.Register(dashEventBinding);

        weaponWheelEventBinding = new EventBinding<WeaponWheelEvent>(RotateWeaponWheel);
        EventBus<WeaponWheelEvent>.Register(weaponWheelEventBinding);
        weaponWheelEventBinding = new EventBinding<WeaponWheelEvent>(UpdateActualWeaponUsed);
        EventBus<WeaponWheelEvent>.Register(weaponWheelEventBinding);
    }

    private void OnDisable() {
        EventBus<StatsEvent>.Deregister(statsEventBinding);

        EventBus<DashEvent>.Deregister(dashEventBinding);

        EventBus<WeaponWheelEvent>.Deregister(weaponWheelEventBinding);
    }

    void UpdateStats(StatsEvent statsEvent) {
        hpSkullIcon.fillAmount = statsEvent.health / 100;
        manaSkullIcon.fillAmount = statsEvent.mana / 50;
        hpPotionsText.text = statsEvent.hpPot.ToString();
        manaPotionsText.text = statsEvent.manaPot.ToString();
        arrowsText.text = statsEvent.ammo.ToString();
    }
    
    void UpdateDashIcon(DashEvent dashEvent) {
        dashIcon.fillAmount = dashEvent.OnDashCDChanged / 1.5f;
    }

    [Header("WeaponWheel")]
    public float rotationDuration = 0.3f;
    float targetAngle = 0f;
    float currentAngle = 0f;
    float rotationStartTime;
    bool isRotating = false;
    int lastWeaponIndex = 0;

    void RotateWeaponWheel(WeaponWheelEvent weaponWheelEvent) {

        int difference = weaponWheelEvent.weaponWheelIdx - lastWeaponIndex;

        if (difference > 1) difference = -1;
        if (difference < -1) difference = 1;

        targetAngle = currentAngle + (difference * 120f);

        rotationStartTime = Time.time;
        isRotating = true;
        lastWeaponIndex = weaponWheelEvent.weaponWheelIdx;
    }

    void Update() {
        if (isRotating) {
            float timeSinceStart = Time.time - rotationStartTime;
            float progress = Mathf.Clamp01(timeSinceStart / rotationDuration);

            float smoothedProgress = Mathf.SmoothStep(0f, 1f, progress);
            float newAngle = Mathf.Lerp(currentAngle, targetAngle, smoothedProgress);

            weapongWheelIcon.transform.localRotation = Quaternion.Euler(0f, 0f, newAngle);
            currentAngle = newAngle;

            if (progress >= 1f) {
                isRotating = false;
                currentAngle = targetAngle;
                weapongWheelIcon.transform.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
            }
        }
    }

    void UpdateActualWeaponUsed(WeaponWheelEvent weaponWheelEvent) {
        switch (weaponWheelEvent.weaponWheelIdx) {
            case 0:
                tomeIcon.gameObject.SetActive(true);
                bowIcon.gameObject.SetActive(false);
                swordIcon.gameObject.SetActive(false);
                quiverIcon.gameObject.SetActive(false);
                arrowsText.gameObject.SetActive(false);
                break;
            case 1:
                tomeIcon.gameObject.SetActive(false);
                bowIcon.gameObject.SetActive(true);
                swordIcon.gameObject.SetActive(false);
                quiverIcon.gameObject.SetActive(true);
                arrowsText.gameObject.SetActive(true);
                break;
            case 2:
                tomeIcon.gameObject.SetActive(false);
                bowIcon.gameObject.SetActive(false);
                swordIcon.gameObject.SetActive(true);
                quiverIcon.gameObject.SetActive(false);
                arrowsText.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }
}
