using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDHandler : MonoBehaviour
{
    [SerializeField] StatHandler stathandler;
    [SerializeField] AttackSystem attackSystem;

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
    [SerializeField] TMP_Text hpPotions;
    [SerializeField] TMP_Text manaPotions;
    [SerializeField] TMP_Text arrows;

    private void Awake() {
        stathandler = GameObject.Find("Player").GetComponent<StatHandler>();
        attackSystem = GameObject.Find("Player").GetComponent<AttackSystem>();
        stathandler.OnHpChanged += UpdateHpIcon;
        stathandler.OnManaChanged += UpdateManaIcon;
        stathandler.OnAmmoChanged += UpdateAmmoText;
        attackSystem.weaponChanged += RotateWeaponWheel;
        attackSystem.weaponChanged += UpdateActualWeaponUsed;
    }

    void UpdateHpIcon(float val) {
        hpSkullIcon.fillAmount = val / 100;
    }

    void UpdateManaIcon(float val) {
        manaSkullIcon.fillAmount += val / 100;
    }

    void UpdateAmmoText(float val) {
        arrows.text = val.ToString();
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

            // Lerp suave
            float smoothedProgress = Mathf.SmoothStep(0f, 1f, progress);
            float newAngle = Mathf.Lerp(currentAngle, targetAngle, smoothedProgress);

            weapongWheelIcon.transform.localRotation = Quaternion.Euler(0f, 0f, newAngle);
            currentAngle = newAngle; // Actualizar el ángulo actual

            if (progress >= 1f) {
                isRotating = false;
                currentAngle = targetAngle; // Asegurar el valor exacto al final
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
                arrows.gameObject.SetActive(false);
                break;
            case 1:
                tomeIcon.gameObject.SetActive(false);
                bowIcon.gameObject.SetActive(true);
                swordIcon.gameObject.SetActive(false);
                quiverIcon.gameObject.SetActive(true);
                arrows.gameObject.SetActive(true);
                break;
            case 2:
                tomeIcon.gameObject.SetActive(false);
                bowIcon.gameObject.SetActive(false);
                swordIcon.gameObject.SetActive(true);
                quiverIcon.gameObject.SetActive(false);
                arrows.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }
}
