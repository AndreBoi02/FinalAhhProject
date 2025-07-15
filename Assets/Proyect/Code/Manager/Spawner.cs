using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] SO_EnemyVariables[] m_sOEnemyVariables;
    [SerializeField] GameObject[] m_gOEnemiesPrefab;
    [SerializeField] GameObject[] m_spawnPoints;
    [SerializeField] Agent player;
    int randomIdx;
    private void Start() {
        RandomNumber();
        ChooseEnemy();
    }

    void RandomNumber() {
        randomIdx = Random.Range(0, 2);
    }

    void ChooseEnemy() {
        Instantiate(m_gOEnemiesPrefab[randomIdx], m_spawnPoints[randomIdx].transform.position, Quaternion.identity);
        m_gOEnemiesPrefab[randomIdx].GetComponent<Agent>().SetTarget(player);
        m_gOEnemiesPrefab[randomIdx].GetComponent<Agent>().SetSteeringVars(m_sOEnemyVariables[randomIdx]);
    }
}
