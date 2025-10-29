public class Mage : Agent {
    ISteeringBehaviour m_steeringBehaviour;

    protected override void Start() {
        SetBehavior(new WanderBehaviour());
    }

    protected override void Move() {
        m_steeringBehaviour?.Execute(this);
        m_rb.linearVelocity = m_currentVel;
    }
}
