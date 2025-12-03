using UnityEngine;

public class EvadeBehaviour : SteeringBehaviour {
    protected ISteeringBehaviour m_currentBehaviour;

    /// <summary>
    /// Predicts the target's future position and applies flee behavior toward that point.
    /// </summary>
    /// <param name="t_agent">The evading agent. Requires:
    /// <para>- Valid target reference (GetTarget() != null)</para>
    /// <para>- Target with current velocity (GetCurrentVel())</para>
    /// <para>- Properly configured steering parameters</para>
    /// </param>
    /// <remarks>
    /// Implementation details:
    /// 1. Predicts target position in T seconds using
    /// 2. Sets this predicted position as the temporary target
    /// 3. Delegates to Flee behavior to evade the predicted position
    /// 
    /// Design considerations:
    /// - Prediction time (T) could be made configurable
    /// - For moving targets, consider dynamic T calculation based on distance/velocity
    /// - May need distance checks to prevent overshooting
    /// </remarks>
    public override void Execute(Agent t_agent) {
        m_currentBehaviour = new FleeBehaviour();
        float T = 3;
        Vector3 futurePos = t_agent.GetTargetPos() + (t_agent.GetTarget().GetRigidbody().linearVelocity * T);
        t_agent.SetTargetPos(futurePos);
        m_currentBehaviour.Execute(t_agent);
    }
}
