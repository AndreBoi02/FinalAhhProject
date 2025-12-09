using UnityEngine;

[CreateAssetMenu(fileName = "SO_EnemyVariables", menuName = "Scriptable Objects/SO_EnemyVariables")]
public class SO_EnemyVariables : ScriptableObject {
    [SerializeField] public SteeringVars SteeringVars;
}
