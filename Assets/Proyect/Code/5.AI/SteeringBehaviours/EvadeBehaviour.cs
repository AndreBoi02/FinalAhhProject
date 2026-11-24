using UnityEngine;

public class EvadeBehaviour : SteeringBehaviour {
    protected ISteeringBehaviour m_currentBehaviour;

    /// <summary>
    /// Predicts the target's future position and applies flee behavior toward that point.
    /// </summary>
    /// <param name="t_agent">The evading agent. Requires:
    /// <para>- Valid target reference</para>
    /// <para>- Properly configured steering parameters</para>
    /// </param>
    public override void Execute(Agent t_agent) {
        m_currentBehaviour = new FleeBehaviour();
        float T = 3;
        Rigidbody targetRb = t_agent.GetTargetRigidbody();
        Vector3 targetVel = targetRb != null ? targetRb.linearVelocity : Vector3.zero;
        Vector3 futurePos = t_agent.GetTargetPos() + (targetVel * T);
        t_agent.SetTargetPos(futurePos);
        m_currentBehaviour.Execute(t_agent);
    }
}
