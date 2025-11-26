using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    [Header("Characteristiques")]
    [SerializeField] private GameObject _spawn = default(GameObject);
    [SerializeField] private float _rate;
    [SerializeField] private float _rateRandomizer;

    private Vector3 _spawnerPosition;

    public bool _spawnActif = true;

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
	    yield return new WaitForSeconds(waitTime);
	}
    }
}
