using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour
{
    [Header("Characteristiques")]
    [SerializeField] private GameObject _spawn = default(GameObject);

    [SerializeField] private float _spawnRateRandomizer;

    // Le radius des locations de spawn
    [SerializeField] private float _radius;
    // Le multiple qui gouverne la densite simuler des ennemies hors vue selon la direction du joueur
    [SerializeField] private float _directionModifier;

    public bool _spawnActif = true;

    [Header("Debug")]
    [SerializeField] private bool _debug = false;
    [SerializeField] private int _maxAmount = 0;

    private Transform _spawnerTransform;

    private Vector2 _oldPosition;
    private Vector2 _positionDifference;
    private Vector2 _directionalSpawnBuffer;


    private void Start()
    {
        _spawnerTransform = GetComponent<Transform>();
	StartCoroutine(SpawnCoroutine());
    }

    private void FixedUpdate()
    {
	// NOTE : considerant que la coroutine genere a un debit de (Pi * r^2) / rates
	
	// Cree un vecteur pour la difference de position 
	//  entre la fixed update precedante et celle ci
	_positionDifference = _spawnerTransform.position;
	_positionDifference -= _oldPosition;

	_directionalSpawnBuffer += _positionDifference;
	
	// if (_directionalSpawnBuffer )

	// Calcul de l'angle perpendiculaire
	float angle = (Mathf.Atan(_positionDifference.y / _positionDifference.x) + (Mathf.PI/2));

	// Additione le vecteur de difference de position pour atteindre le radius et ajoute le vecteur perpendiculaire ayant une grandeur random
	Vector2 spawnPosition = (_positionDifference.normalized * _radius) + MyFunctions.MakeVectorUsingAngle((Random.Range(-_radius/2, _radius/2)), angle);

	// Calcule du nombre de spawns dans l'interval

	_oldPosition = _spawnerTransform.position;
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

	Instantiate(_spawn, p_spawnPosition, Quaternion.identity);
    }

    IEnumerator SpawnCoroutine()
    {
	while (_spawnActif)
	{
	    float angle = Random.Range(0f, 2*Mathf.PI);
	    Vector2 spawnPositionCircle = MyFunctions.MakeVectorUsingAngle(_radius, angle);

	    Spawn(spawnPositionCircle);
	    
	    yield return new WaitForSeconds(MyFunctions.GetRandomizedByPercentage(GameManager.Instance.SpawnRate, _spawnRateRandomizer));
	}
    }
}
