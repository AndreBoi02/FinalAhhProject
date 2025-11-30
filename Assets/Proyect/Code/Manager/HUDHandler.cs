using FinalProyect;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDHandler : MonoBehaviour {
    [Header("References")]
    [SerializeField] StatHandler stathandler;
    [SerializeField] AttackSystem attackSystem;
    [SerializeField] PlayerController playerController; 

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

    private void Awake() {
        stathandler = GameObject.Find("Player").GetComponent<StatHandler>();
        attackSystem = GameObject.Find("Player").GetComponent<AttackSystem>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        stathandler.OnHpChanged += UpdateHpIcon;
        stathandler.OnManaChanged += UpdateManaIcon;

        stathandler.OnHpPotChanged += UpdateHpPotText;
        stathandler.OnManaPotChanged += UpdateManaPotText;
        stathandler.OnAmmoChanged += UpdateAmmoText;

        attackSystem.weaponChanged += RotateWeaponWheel;
        attackSystem.weaponChanged += UpdateActualWeaponUsed;

        playerController.OnDashCDChanged += UpdateDashIcon;
    }

    void UpdateHpIcon(float val) {
        hpSkullIcon.fillAmount = val / 100;
    }

    void UpdateManaIcon(float val) {
        manaSkullIcon.fillAmount = val / 50;
    }
    
    void UpdateDashIcon(float val) {
        dashIcon.fillAmount = val / 1.5f;
    }

    void UpdateHpPotText(float val) {
        hpPotionsText.text = val.ToString();
    }
    
    void UpdateManaPotText(float val) {
        manaPotionsText.text = val.ToString();
    }

    void UpdateAmmoText(float val) {
        arrowsText.text = val.ToString();
    }

    [Header("WeaponWheel")]
    public float rotationDuration = 0.3f;
    float targetAngle = 0f;
    float currentAngle = 0f;
    float rotationStartTime;
    bool isRotating = false;
    int lastWeaponIndex = 0;

    void RotateWeaponWheel(int newWeaponIndex) {
        // Calcular la diferencia para determinar la dirección más corta
        int difference = newWeaponIndex - lastWeaponIndex;

        // Ajustar para el caso de dar la vuelta (de 2 a 0 o de 0 a 2)
        if (difference > 1) difference = -1; // De 2 a 0 es como -1 en sentido antihorario
        if (difference < -1) difference = 1;  // De 0 a 2 es como +1 en sentido horario

        // Rotar en la dirección correcta
        targetAngle = currentAngle + (difference * 120f);

        rotationStartTime = Time.time;
        isRotating = true;
        lastWeaponIndex = newWeaponIndex;
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

    void UpdateActualWeaponUsed(int currentWeaponIndex) {
        switch (currentWeaponIndex) {
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
