using System.Collections;
using UnityEngine;

public class Ranger : Agent {
    IRangerLevels rangerLevel;

    public enum Level {
        Easy,
        Medium,
        Hard
    }

    public Level level;

    [SerializeField] private float projectileSpeed = 25f;

    public float ProjectileSpeed => projectileSpeed;


    public Ranger(IRangerLevels rangerLevel) {
        this.rangerLevel = rangerLevel;
    }

    protected override void Start() {
        base.Start();
        SwitchDificulty();
    }

    void SwitchDificulty() {
        switch (level) {
            case Level.Easy:
                rangerLevel = new EasyRanger();
                break;
            case Level.Medium:
                rangerLevel = new MediumRanger();
                break;
            case Level.Hard:
                rangerLevel = new HardRanger();
                break;
            default:
                break;
        }
    }

    protected override void ExecuteBehaviour() {
        
        rangerLevel.Execute(this);
    }

    public void StayInPlace() {
        m_rb.linearVelocity = Vector3.zero;
        EventBus<RunEvent>.Raise(new RunEvent {
            Source = gameObject,
            isRunnig = false
        });
    }

    public void MoveAwayFromPlayer() {
        SetBehavior(new FleeBehaviour());
        m_currentBehaviour?.Execute(this);
        m_rb.linearVelocity = m_currentVel;
        EventBus<RunEvent>.Raise(new RunEvent {
            Source = gameObject,
            isRunnig = true
        });
    }

    public void PredictPlayerPos() {
        FacePlayer();
        SetBehavior(new FleeBehaviour());
        m_currentBehaviour?.Execute(this);
    }

    public void Attack() {
        FacePlayer();
        if (!OnAttackCoolDown()) {
            StartCoroutine(InvokeRange());
        }
    }

    IEnumerator InvokeRange() {
        PrepareOnAttack();
        yield return new WaitForSeconds(.65f);
        InvokeOnAttack();
    }
}
