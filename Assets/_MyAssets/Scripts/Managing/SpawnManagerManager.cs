using UnityEngine;

public class SpawnManagerManager : MonoBehaviour
{
    // Make it so that the spawn rate increases according to player direction to give the illusion of a uniform distribution in enemies

    [Header("Debit global et characteristiques simples")]
    [SerializeField] private float _globalStartingRate;
    [SerializeField] private float _globalRateIncrease;
    [SerializeField] private GameObject _spawnManager = default(GameObject);

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
