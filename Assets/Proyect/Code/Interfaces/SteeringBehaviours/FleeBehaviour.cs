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
        Vector3 desiredVel = t_agent.GetCurrentPos() - t_agent.GetTargetPos();
        CalculateSteer(desiredVel, t_agent);
    }
}
