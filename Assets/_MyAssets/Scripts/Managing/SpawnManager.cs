using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    [Header("Characteristiques")]
    [SerializeField] private GameObject _spawn = default(GameObject);
    [SerializeField] private float _rate;
    [SerializeField] private float _rateRandomizer;

    public bool _spawnActif = true;

    [Header("Debug")]
    [SerializeField] private bool _debug = false;
    [SerializeField] private int _maxAmount = 0;

    private Vector3 _spawnerPosition;


    void Start()
    {
        _spawnerPosition = GetComponent<Transform>().position;
	StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
	while (_spawnActif)
	{
	   Instantiate(_spawn, _spawnerPosition, Quaternion.identity);
	   float waitTime = _rate + (Random.Range(0f, (_rate * _rateRandomizer) - (_rateRandomizer / 2)));
	   
	   _maxAmount--;
	   if (_maxAmount <= 0 && _debug) { _spawnActif = false; }

	   yield return new WaitForSeconds(waitTime);
	}
    }
}
