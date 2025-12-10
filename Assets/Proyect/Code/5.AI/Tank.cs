using UnityEngine;

public class Tank : Agent {
    ITankLevels tankLevel;

    public Tank(ITankLevels tankLevel) {
        this.tankLevel = tankLevel; 
    }

    protected override void Start() {
        base.Start();
        tankLevel = new HardTank();;
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
