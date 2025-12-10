using UnityEngine;

public class GuardianBehaviour : SteeringBehaviour {
    public override void Execute(Agent t_agent) {
        Vector3 targetPosition = CalculateGuardianPosition(t_agent);

        Vector3 desiredVelocity = targetPosition - t_agent.GetCurrentPos();
        float distance = desiredVelocity.magnitude;

        CalculateSteer(desiredVelocity, t_agent);
        Arrive(t_agent, distance);
    }

    private Vector3 CalculateGuardianPosition(Agent t_agent) {
        Vector3 directionToAttacker = (t_agent.GetSheltered().transform.position - t_agent.GetTargetPos()).normalized;

        Vector3 idealPosition = t_agent.GetSheltered().transform.position - (directionToAttacker * t_agent.GetSteeringVars().m_radius);

        idealPosition.y = t_agent.GetCurrentPos().y;

        return idealPosition;
    }
}
