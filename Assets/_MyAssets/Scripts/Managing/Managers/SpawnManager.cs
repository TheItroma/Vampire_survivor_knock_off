using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    [Header("Characteristiques")]
    [SerializeField] private List<GameObject> _spawns = new List<GameObject>();
    [SerializeField] private List<float> _percentages = new List<float>();


    // Le radius des locations de spawn
    [SerializeField] private float _radius;
    // Le multiple qui gouverne la densite simuler des ennemies hors vue selon la direction du joueur
    [SerializeField] private float _directionModifier;


    [Header("Debug")]
    [SerializeField] private bool _debug = false;
    [SerializeField] private int _maxAmount = 0;

    private Transform _spawnerTransform;

    private Vector2 _oldPosition;
    private Vector2 _positionDifference;
    private Vector2 _directionalSpawnBuffer;
    private bool _spawnActif = false;

    private Coroutine _spawnCoroutine;

    private void Start()
    {
        _spawnerTransform = GetComponent<Transform>();
	ToggleSpawn();
    }

    private void Spawn(Vector2 p_spawnPosition)
    {
	p_spawnPosition += (Vector2)gameObject.transform.position;

	if (_debug)
	{
	    Debug.DrawLine(_spawnerTransform.position, p_spawnPosition, Color.red, 1f, false);
	    _maxAmount--;
	    if (_maxAmount <= 0) { _spawnActif = false; }
        }

	List<GameObject> Spawn = new List<GameObject>(MyFunctions.GetRandomObject(_spawns, _percentages, 1));
	if (Spawn != null)
	{
	    Instantiate(Spawn[0], p_spawnPosition, Quaternion.identity);
	}
    }

    public void ToggleSpawn()
    {
	_spawnActif = !_spawnActif;

	if (_spawnActif && _spawnCoroutine == null)
	{
	    _spawnCoroutine = StartCoroutine(SpawnCoroutine());
	}
	else if (_spawnCoroutine != null)
	{
	    StopCoroutine(_spawnCoroutine);
	    _spawnCoroutine = null;
	}
    }

    private IEnumerator SpawnCoroutine()
    {
	while (_spawnActif)
	{
	    float angle = Random.Range(0f, 2*Mathf.PI);
	    Vector2 spawnPositionCircle = MyFunctions.MakeVectorUsingAngle(_radius, angle);

	    Spawn(spawnPositionCircle);
	    
	    yield return new WaitForSeconds(GameManager.Instance.SpawnRate);
	}
    }
}
