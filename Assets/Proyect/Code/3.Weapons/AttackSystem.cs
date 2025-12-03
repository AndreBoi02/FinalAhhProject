using FinalProyect;
using System;
using System.Collections;
using UnityEngine;
using static CombatEvent;

public class AttackSystem : MonoBehaviour {
    Action prepareAttackFunction;
    Action executeAttackFunction;

    StatHandler statHandler;

    public enum AttackType {
        magic,
        range,
        melee
    }

    [Header ("CurrentWeapon")]
    public AttackType weapon;
    int weaponIdx = 0;

    [Header("WeaponsReferences")]
    [SerializeField] GameObject _sword;
    [SerializeField] GameObject _proyectile;
    [SerializeField] GameObject _fireBall;
    [SerializeField] GameObject _fireBallPV;

    Vector3 worldPos;
    GameObject tempObject;

    public AttackType CurrentWeapon => weapon;
    public int CurrentWeaponIndex => weaponIdx;
    private IAttackSystem attacker;
    public Vector3 WorldPosition => worldPos;
    public bool IsPlayer => TryGetComponent<PlayerController>(out _);

    EventBinding<OnNextWeapon> nextWeaponBinding;
    EventBinding<OnPrevWeapon> prevWeaponBinding;

    void Start() {
        attacker = GetComponent<IAttackSystem>();

        statHandler = GetComponent<StatHandler>();

        if (attacker != null) {
            attacker.OnPrepareAttack += StartAttack;
            attacker.OnAttack += ExecuteAttack;

            nextWeaponBinding = new EventBinding<OnNextWeapon>(NextWeapon);
            EventBus<OnNextWeapon>.Register(nextWeaponBinding);
            prevWeaponBinding = new EventBinding<OnPrevWeapon>(PrevWeapon);
            EventBus<OnPrevWeapon>.Register(prevWeaponBinding);
        }

        SwitchBetweenWeapons();
        if(_sword == null)
            return;
        _sword?.SetActive(false);
    }

    private void FixedUpdate() {
        if(tempObject != null)
            tempObject.transform.position = worldPos;
    }

    void OnDestroy() {
        if (attacker != null) {
            attacker.OnPrepareAttack -= StartAttack;
            attacker.OnAttack -= ExecuteAttack;
            EventBus<OnNextWeapon>.Deregister(nextWeaponBinding);
            EventBus<OnPrevWeapon>.Deregister(prevWeaponBinding);
        }
    }

    public void RaisePrepareEvent() {
        EventBus<AttackPrepareEvent>.Raise(new AttackPrepareEvent {
            attacker = gameObject,
            attackType = weapon,
            targetPosition = worldPos,
            isPlayer = IsPlayer
        });
    }

    public void RaiseExecuteEvent() {
        EventBus<AttackExecuteEvent>.Raise(new AttackExecuteEvent {
            attacker = gameObject,
            attackType = weapon,
            position = transform.position,
            targetPosition = worldPos,
            isPlayer = IsPlayer
        });
    }

    public void RaiseWeaponChangedEvent() {
        bool isPlayer = TryGetComponent<PlayerController>(out _);
        EventBus<WeaponChangedEvent>.Raise(new WeaponChangedEvent {
            entity = gameObject,
            weaponIndex = weaponIdx,
            isPlayer = isPlayer
        });
    }

    void SwitchBetweenWeapons() {
        switch (weapon) {
            case AttackType.melee:
                executeAttackFunction = SwordAttack;
                break;
            case AttackType.range:
                executeAttackFunction = BowAttack;
                break;
            case AttackType.magic:
                prepareAttackFunction = PrepareMagicAttack;
                executeAttackFunction = ExecuteMagicAttack;
                break;
            default:
                break;  
        }
    }

    void StartAttack() {
        if(weapon == AttackType.magic && !statHandler.ManaAvailable())
            return;
        prepareAttackFunction();
        RaisePrepareEvent();
    }

    void ExecuteAttack() {
        if ((weapon == AttackType.range && !statHandler.AmmoAvailable()) 
            || (weapon == AttackType.magic && !statHandler.ManaAvailable()))
            return;
        executeAttackFunction();
        RaiseExecuteEvent();
    }

    void NextWeapon() {
        weaponIdx = (weaponIdx + 1) % 3;
        weapon = (AttackType)weaponIdx;
        bool isPlayer = TryGetComponent<PlayerController>(out _);
        RaiseWeaponChangedEvent();
        SwitchBetweenWeapons();
    }

    void PrevWeapon() {
        weaponIdx = (weaponIdx - 1 + 3) % 3;
        weapon = (AttackType)weaponIdx;
        bool isPlayer = TryGetComponent<PlayerController>(out _);
        RaiseWeaponChangedEvent();
        SwitchBetweenWeapons();
    }

    #region Sword

    void SwordAttack() {
        StartCoroutine(TurnOnAndOffTheSword());
    }

    IEnumerator TurnOnAndOffTheSword() {
        _sword?.SetActive(true);
        yield return new WaitForSeconds(.5f);
        _sword?.SetActive(false);
    }

    #endregion

    #region Bow

    void BowAttack() {
        if (_proyectile == null)
            return;

        GameObject tempProyectile;
        tempProyectile = Instantiate(_proyectile, transform.position, transform.localRotation);
        statHandler.Ammo -= 1;
    }

    #endregion

    #region Magic

    void PrepareMagicAttack() {
        tempObject = Instantiate(_fireBallPV, worldPos, Quaternion.identity);
    }

    void ExecuteMagicAttack() {
        if (tempObject != null) {
            Destroy(tempObject);
        }
        CreateFireBall();
        statHandler.Mana -= 10;
    }

    void CreateFireBall() {
        GameObject _tempfb = Instantiate(_fireBall, transform.position, Quaternion.identity);
        Fireball fireball = _tempfb.GetComponent<Fireball>();

        if (fireball != null) {
            fireball.LaunchTowards(worldPos, 3, -15f);
        }
    }

    #endregion

    public void SetWorldPosVector(Vector3 val) {
        worldPos = val;
    }
}
