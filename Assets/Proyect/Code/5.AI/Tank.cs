using UnityEngine;

public class Tank : Agent {
    ITankLevels tankLevel;

    public enum Level {
        Easy,
        Medium,
        Hard
    }

    public Level level;

    protected override void Start() {
        base.Start();
        SwitchDificulty();
    }

    void SwitchDificulty() {
        switch (level) {
            case Level.Easy:
                tankLevel = new EasyTank();
                break;
            case Level.Medium:
                tankLevel = new MediumTank();
                break;
            case Level.Hard:
                tankLevel = new HardTank();
                break;
            default:
                break;
        }
    }

    protected override void ExecuteBehaviour() {
        tankLevel?.Execute(this);
    }

    public void StayInPlace() {
        m_rb.linearVelocity = Vector3.zero;
        EventBus<RunEvent>.Raise(new RunEvent {
            Source = gameObject,
            isRunnig = false
        });
    }

    public void GoForThePlayerSeek() {
        SetBehavior(new SeekBehaviour());
        FacePlayer();
        m_currentBehaviour?.Execute(this);
        m_rb.linearVelocity = m_currentVel;
        EventBus<RunEvent>.Raise(new RunEvent{
            Source = gameObject,
            isRunnig = true
        });
    }

    public void PredictPlayer() {
        if (DistanceFromPlayer() > 5) {
            FacePlayer();
            SetBehavior(new PursuitBehaviour());
            m_currentBehaviour?.Execute(this);
            m_rb.linearVelocity = m_currentVel;
            EventBus<RunEvent>.Raise(new RunEvent {
                Source = gameObject,
                isRunnig = true
            });
        }
        else {
            GoForThePlayerSeek();
        }
    }

    public void ProtectAgent() {
        SetBehavior(new GuardianBehaviour());
        FacePlayer();
        m_currentBehaviour?.Execute(this);
        m_rb.linearVelocity = m_currentVel;
        if(m_aTarget.GetRigidbody().linearVelocity != Vector3.zero) {
            EventBus<RunEvent>.Raise(new RunEvent {
                Source = gameObject,
                isRunnig = true
            });
        }
        else {
            EventBus<RunEvent>.Raise(new RunEvent {
                Source = gameObject,
                isRunnig = false
            });
        }
    }

    public void Attack() {
        if (!OnAttackCoolDown()) {
            FacePlayer();
            InvokeOnAttack();
        }
    }
}
