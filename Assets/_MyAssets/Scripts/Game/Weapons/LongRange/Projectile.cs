using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Characteristiques")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _speedRandomizer = 0.1f;
    [SerializeField] private string _targetTag = "Enemy";
    [SerializeField] private int _piercing = 1;

    [SerializeField] private bool _isIndestructible = false;
    [SerializeField] private float _maxDistance = 1f;

    // Je suis pas trop sure si cette section est une bonne idee
    [SerializeField] public int _damage;

    [Header("S'il chercher pour l'enemy")]
    [SerializeField] private bool _isSeeking = false;
    [SerializeField] private float _turnSpeed;
    [SerializeField] private float _turnSpeedRandomizer;



    private float _angle;
    private Vector2 _direction;

    private GameObject _target = default(GameObject);

    private void Start()
    {
	_speed = MyFunctions.GetRandomizedByPercentage(_speed, _speedRandomizer);
	_turnSpeed = MyFunctions.GetRandomizedByPercentage(_turnSpeed, _turnSpeedRandomizer);
	
	// Store la rotation z
	_angle = gameObject.transform.eulerAngles[0];

	_direction = MyFunctions.MakeVectorUsingAngle(1f, _angle);

	if (_isSeeking)
	{
	    // Make it so that it only calculates nearest enemies before instantiation in the gameManager
	}
    }

    private void Update()
    {
	if (_isSeeking)
	{

	}
	Move();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
    	if (other.tag == _targetTag)
	{
	    if (_piercing > 0)
	    {
		if (!_isIndestructible)
		{
		    _piercing--;
		}
		other.GetComponent<Enemy>().Damage(_damage);	
	    }
	    else
	    {
		Destroy(this.gameObject);
	    }
	}
    }
    private void Move()
    {
	transform.Translate(_direction * _speed * Time.deltaTime);
    }
}
