using UnityEngine;

public class PursuitBehaviour : SteeringBehaviour {
    protected ISteeringBehaviour m_currentBehaviour;

    /// <summary>
    /// Predicts the target's future position and applies seek behavior toward that point.
    /// </summary>
    /// <param name="t_agent">The pursuing agent. Requires:
    /// <para>- Valid target reference (GetTarget() != null)</para>
    /// <para>- Target with current velocity (GetCurrentVel())</para>
    /// <para>- Properly configured steering parameters</para>
    /// </param>
    /// <remarks>
    /// Implementation details:
    /// 1. Predicts target position in T seconds using
    /// 2. Sets this predicted position as the temporary target
    /// 3. Delegates to Seek behavior to approach the predicted position
    /// 
    /// Design considerations:
    /// - Prediction time (T) could be made configurable
    /// - For moving targets, consider dynamic T calculation based on distance/velocity
    /// - May need distance checks to prevent overshooting
    /// </remarks>
    public override void Execute(Agent t_agent) {
        m_currentBehaviour = new SeekBehaviour();
        Vector3 futurePos = t_agent.GetTargetPos() + (t_agent.GetTarget().GetRigidbody().linearVelocity * t_agent.GetSteeringVars().m_predictionTime);
        t_agent.SetTargetPos(futurePos);
        m_currentBehaviour.Execute(t_agent);
    }
}
