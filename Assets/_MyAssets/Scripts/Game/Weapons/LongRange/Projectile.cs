using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class Projectile : MonoBehaviour
{
    [Header("Characteristiques")]
    
    // Je suis pas trop sure si cette section est une bonne idee (public)
    [SerializeField] public int _damage;

    [Header("Solidite")]
    [SerializeField] private int _piercing = 1;
    [SerializeField] private bool _isIndestructible = false;
    [SerializeField] private float _maxDistance = 20f;
    // A vraiment besoin d'etre serialize?
    [SerializeField] private float _maxDuration = 20f;

    [Header("S'il chercher pour l'enemy")]
    [SerializeField] private bool _isSeeking = false;
    [SerializeField] private bool _seekingSelf = false;

    [Header("Vitesse")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _speedRandomizer = 0.1f;
    [SerializeField] private float _turnSpeed;
    [SerializeField] private float _turnSpeedRandomizer;   [SerializeField] private string _targetTag = "Enemy";
    [SerializeField] private bool _debug = false;

    private float _angle;
    private Vector2 _direction;

    private GameObject _target = default(GameObject);

    private void Start()
    {
	_speed = MyFunctions.GetRandomizedByPercentage(_speed, _speedRandomizer);
	_turnSpeed = MyFunctions.GetRandomizedByPercentage(_turnSpeed, _turnSpeedRandomizer);
	
	// Store la rotation z
	_angle = gameObject.transform.eulerAngles[0];
	_direction = MyFunctions.MakeVectorUsingAngle(1f, _angle * Mathf.Deg2Rad);

	if (_isSeeking) { SetTargetToNearest(); }
	if (!_isIndestructible) { StartCoroutine(DestroyAfter()); }
    }

    private void FixedUpdate()
    {
	float Distance = Vector2.Distance(gameObject.transform.position, FindAnyObjectByType<Player>().transform.position);
	if (Distance >= _maxDistance)
	{
	    Destroy(this.gameObject);
	}
    }

    private void Update()
    {
	if (_isSeeking)
	{
	    // S'ill exist, seek
	    if (_target) { Seek(); }
	    else { SetTargetToNearest(); }
	    transform.position = Vector2.MoveTowards(transform.position, _direction + (Vector2)transform.position, _speed * Time.deltaTime);
	}
	Move();
    }

    private void Move()
    {
	transform.Translate(_direction * _speed * Time.deltaTime);
    }

    private void Seek()
    {
	// Trouve l'angle a atteidre
	Vector2 TargetDirection = _target.transform.position - gameObject.transform.position;
	if (_debug) { Debug.DrawLine(transform.position, transform.position + (Vector3)TargetDirection, Color.green, 0.2f); }
	float TargetAngle = Mathf.Atan2(TargetDirection.y, TargetDirection.x) * Mathf.Rad2Deg;

	float Difference = Mathf.DeltaAngle(_angle, TargetAngle);
	// Si a passer un target et pas just changer de

	float StepAngle = Mathf.Clamp(Difference, -_turnSpeed * Time.deltaTime, _turnSpeed * Time.deltaTime);
        // tourne dans cette direction
	_angle += StepAngle;
	// Pour eviter que ca depass 0 ou 360
	_angle = ((_angle + 360) % 360);
	transform.rotation = Quaternion.Euler(0f, 0f, _angle);
	_direction = MyFunctions.MakeVectorUsingAngle(1f, _angle * Mathf.Deg2Rad);
	if (_debug) { Debug.DrawLine(transform.position, transform.position + (Vector3)_direction, Color.red, 0.2f); }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
    	if (other.tag == _targetTag)
	{
	    other.GetComponent<Enemy>().Damage(_damage);	
	    if (_piercing > 0)
	    {
		if (!_isIndestructible)
		{
		    _piercing--;
		}
	    }
	    else
	    {
		Destroy(this.gameObject);
	    }
	}
	
	if (_target && Vector2.Distance(other.transform.position,  _target.transform.position) <= 0.2f)
	{
	    SetTargetToNearest();
	}
    }

    private void SetTargetToNearest()
    {
	Vector3 SeekingFrom = Vector3.zero;
	if (_seekingSelf)
	{
	    SeekingFrom = transform.position;
	}
	else
	{
	    SeekingFrom = FindAnyObjectByType<Player>().transform.position;
	}
	GameObject[] Ennemies = GameObject.FindGameObjectsWithTag("Enemy");
	// INSERER CONDITION SI YA PAS D'ENNEMIES
	GameObject NearestEnnemy = Ennemies[0];
	float Distance = Vector3.Distance(SeekingFrom, Ennemies[0].transform.position);
	float Nearest = Distance;

	for (int i = 1; i < Ennemies.Length; i++)
	{
	    Distance = Vector3.Distance(SeekingFrom, Ennemies[i].transform.position);
	    if (Distance < Nearest && Ennemies[i] != _target)
	    {
		NearestEnnemy = Ennemies[i];
		Nearest = Distance;
	    }
	}
	_target = NearestEnnemy;
    }

    IEnumerator DestroyAfter()
    {
	yield return new WaitForSeconds(_maxDuration);
	Destroy(this.gameObject);
    }
}
