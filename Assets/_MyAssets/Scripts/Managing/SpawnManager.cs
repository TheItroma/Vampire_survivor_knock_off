using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    [Header("Characteristiques")]
    [SerializeField] private GameObject _spawn = default(GameObject);
    [SerializeField] private float _rate;
    [SerializeField] private float _rateRandomizer;

    public bool _spawnActif = true;


    [SerializeField] private bool _debug = false;
    [SerializeField] private int _maxAmount = 0;

    private Transform _spawnerTransform;


    void Start()
    {
        _spawnerTransform = GetComponent<Transform>();
	StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
	while (_spawnActif)
	{
	   Instantiate(_spawn, _spawnerTransform.position, Quaternion.identity);
	   float waitTime = _rate + (Random.Range(0f, (_rate * _rateRandomizer) - (_rateRandomizer / 2)));
	   
	   _maxAmount--;
	   if (_maxAmount <= 0 && _debug) { _spawnActif = false; }

	   yield return new WaitForSeconds(waitTime);
	}
    }
}
