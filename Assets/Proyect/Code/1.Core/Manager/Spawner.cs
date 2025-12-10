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
    [SerializeField] Transform[] spawnPoints;

    [Header("Enemies Prefabs")]
    [SerializeField] GameObject m_gOMage;
    [SerializeField] GameObject m_gOMelee;
    [SerializeField] GameObject m_gORanger;

    [Header("Enemies stats")]
    [SerializeField] SO_EnemyVariables[] m_sOEnemyVariables;

    int randomIdx;

    private void Start() {
        GameObject tempEnemy = Instantiate(m_gOMage, spawnPoints[0]);
        new Mage(new EasyMage());
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
