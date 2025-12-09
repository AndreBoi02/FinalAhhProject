using UnityEngine;

public class Spawner : MonoBehaviour {
    public enum Difficulty {
        easy,
        medium,
        hard
    }

    [Header ("Current dificulty")]
    public Difficulty difficulty => Difficulty.easy;

    [Header("Player Reference")]
    [SerializeField] Agent player;

    [Header("Enemies Prefabs")]
    [SerializeField] GameObject[] m_gOMages;
    [SerializeField] GameObject[] m_gOMelees;
    [SerializeField] GameObject[] m_gORangers;

    [Header("Enemies stats")]
    [SerializeField] SO_EnemyVariables[] m_sOEnemyVariables;

    int randomIdx;

    private void Start() {

    }

    void RandomNumber() {
        randomIdx = Random.Range(0, 2);
    }

    void ChooseEnemy() {
        //Instantiate(m_gOEnemiesPrefab[randomIdx], m_spawnPoints[randomIdx].transform.position, Quaternion.identity);
        //m_gOEnemiesPrefab[randomIdx].GetComponent<Agent>().SetTarget(player);
        //m_gOEnemiesPrefab[randomIdx].GetComponent<Agent>().SetSteeringVars(m_sOEnemyVariables[randomIdx]);
    }

    void SpawnEnemies() {

    }
}
