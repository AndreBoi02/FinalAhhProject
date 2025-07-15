using UnityEngine;

public class FleeBehaviour : SteeringBehaviour {
    /// <summary>
    /// Generates evasion steering away from the target at maximum capability.
    /// </summary>
    /// <param name="t_agent">The fleeing agent. Requires:
    /// <para>- Non-zero maxForce</para>
    /// <para>- Valid target reference</para>
    /// </param>
    /// <remarks>
    /// Emergency escape behavior - ignores arrival logic.
    /// For coordinated evasion, combine with obstacle avoidance.
    /// </remarks>
    public override void Execute(Agent t_agent) {
        Vector3 desiredVel = t_agent.m_pos - t_agent.GetTargetAgent().m_pos;
        BaseBehaviour(desiredVel, t_agent);
    }
}
