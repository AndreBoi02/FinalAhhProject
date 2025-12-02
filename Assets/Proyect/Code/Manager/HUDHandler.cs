using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static CombatEvent;

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
    EventBinding<WeaponChangedEvent> weaponChangedEventBinding;

    private void OnEnable() {
        statsEventBinding = new EventBinding<StatsEvent>(UpdateStats);
        EventBus<StatsEvent>.Register(statsEventBinding);

        dashEventBinding = new EventBinding<DashEvent>(UpdateDashIcon);
        EventBus<DashEvent>.Register(dashEventBinding);

        weaponChangedEventBinding = new EventBinding<WeaponChangedEvent>(OnWeaponChanged);
        EventBus<WeaponChangedEvent>.Register(weaponChangedEventBinding);
    }

    private void OnDisable() {
        EventBus<StatsEvent>.Deregister(statsEventBinding);

        EventBus<DashEvent>.Deregister(dashEventBinding);

        EventBus<WeaponChangedEvent>.Deregister(weaponChangedEventBinding);
    }

    void OnWeaponChanged(WeaponChangedEvent weaponEvent) {
        if (!weaponEvent.isPlayer) return;

        RotateWeaponWheel(weaponEvent.weaponIndex);
        UpdateActualWeaponUsed(weaponEvent.weaponIndex);
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
    private float _targetAngle = 0f;
    private float _currentAngle = 0f;
    float rotationStartTime;
    public bool isRotating = false;
    int lastWeaponIndex = 0;

    public float CurrentTargetAngle => _targetAngle;
    public float CurrentAngle => _currentAngle;


    void RotateWeaponWheel(int i) {

        int difference = i - lastWeaponIndex;

        if (difference > 1) difference = -1;
        if (difference < -1) difference = 1;

        _targetAngle = _currentAngle + (difference * 120f);

        rotationStartTime = Time.time;
        isRotating = true;
        lastWeaponIndex = i;
    }

    void Update() {
        if (isRotating) {
            
            float timeSinceStart = Time.time - rotationStartTime;
            float progress = Mathf.Clamp01(timeSinceStart / rotationDuration);

            float smoothedProgress = Mathf.SmoothStep(0f, 1f, progress);
            float newAngle = Mathf.Lerp(_currentAngle, _targetAngle, smoothedProgress);

            weapongWheelIcon.transform.localRotation = Quaternion.Euler(0f, 0f, newAngle);
            _currentAngle = newAngle;

            if (progress >= 1f) {
                isRotating = false;
                _currentAngle = _targetAngle;
                weapongWheelIcon.transform.localRotation = Quaternion.Euler(0f, 0f, _currentAngle);
            }
        }
    }

    void UpdateActualWeaponUsed(int weaponIdx) {
        switch (weaponIdx) {
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
