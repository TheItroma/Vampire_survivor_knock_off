using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Characteristiques")]
    [SerializeField] private string _targetTag = "Player";
    
    [Header("Mouvement")]
    [SerializeField] private float _speed = 0.4f;

    [Header("Par raport au joueur")]
    [SerializeField] private float _minDistance = 0.6f;
    [SerializeField] private float _minDistanceRandomizer = 0.2f;

    [SerializeField] private float _colliderRadius = 0.3f;

    private Vector2 _direction;
    private GameObject _target;
    private bool _isObstructed = false;

    void Start()
    {
	_target = GameObject.FindGameObjectsWithTag(_targetTag)[0];
	_minDistance = _minDistance + (Random.Range(0f, (_minDistance * _minDistanceRandomizer)) - (_minDistanceRandomizer / 2));
    }

    void Update()
    {
        Move();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
	// Seach about "collider2d" and use its raycast
	RaycastHit2D raycast = collision.Raycast(_target.transform.position, _colliderRadius*2);
	Debug.DrawLine(gameObject.transform.position, _target.transform.position, Color.white, _colliderRadius*2);

	if (raycast.collider)
        {
	    _isObstructed = true;
	}
	else
	{
	    _isObstructed = false;
	}
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
	_isObstructed = false;
    }
    private void Move()
    {
	Vector2 targetPosition = _target.transform.position;
	_direction = new Vector2(targetPosition.x - gameObject.transform.position.x, targetPosition.y - gameObject.transform.position.y);

	if (_direction.magnitude >= _minDistance && !_isObstructed)
	{
	    _direction.Normalize();
	    transform.Translate(_direction * Time.deltaTime * _speed);
	}
    }
}
