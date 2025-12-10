using UnityEngine;

public static class PredictionHelper {
    // Método simple para calcular dirección de disparo con predicción
    public static Vector3 CalculateShootingDirection(
        Vector3 shooterPosition,
        Vector3 targetPosition,
        Vector3 targetVelocity,
        float projectileSpeed) {
        // Si el objetivo está quieto o muy lento, dispara directo
        if (targetVelocity.magnitude < 1f) {
            return (targetPosition - shooterPosition).normalized;
        }

        // Calcular tiempo estimado de vuelo
        float distance = Vector3.Distance(shooterPosition, targetPosition);
        float estimatedTime = distance / projectileSpeed;

        // Predecir posición futura
        Vector3 futurePosition = targetPosition + (targetVelocity * estimatedTime);

        // Recalcular con la nueva posición (1 iteración extra para mejor precisión)
        float newDistance = Vector3.Distance(shooterPosition, futurePosition);
        estimatedTime = newDistance / projectileSpeed;
        futurePosition = targetPosition + (targetVelocity * estimatedTime);

        // Retornar dirección hacia la posición predicha
        return (futurePosition - shooterPosition).normalized;
    }
}
