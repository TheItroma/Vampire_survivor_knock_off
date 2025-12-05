using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{

    [Header("Characteristiques")]
    [SerializeField] private string _targetTag = "Player";
    
    [Header("Mouvement")]
    [SerializeField] private float _speed = 0.4f;

    [Header("Par raport au joueur")]
    [SerializeField] private float _minDistance = 0.6f;
    [SerializeField] private float _minDistanceRandomizer = 0.2f;

    [SerializeField] private float _colliderRadius = 0.03f;

    [SerializeField] private bool _encercle = true;

    [Header("Debug")]
    [SerializeField] private bool _debug = false;

    private GameObject _target;
    private bool _isObstructed = false;
    private static RaycastHit2D[] _iDoNotNeedThis = new RaycastHit2D[1];
    private Vector2 _encerclementModifier = new Vector2(0f, 0f);

    void Start()
    {
	_target = GameObject.FindGameObjectsWithTag(_targetTag)[0];
	_minDistance = _minDistance + (Random.Range(0f, (_minDistance * _minDistanceRandomizer)) - (_minDistanceRandomizer / 2));
	if (_encercle) { _encerclementModifier = CalculateRandomEncerclementVector(_minDistance); }
    }

    void Update()
    {
        Move();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
	// when a collision occures, it uses the colliders raycast methode from the attached gameObject
	// (less convoluted, collider already has starting position and already ignores itself)
	if (_debug) { Debug.DrawLine(gameObject.transform.position, Vector2.MoveTowards(gameObject.transform.position, _target.transform.position, _colliderRadius), Color.white, 0.1f); }
	// 1f as a movetowards step cuz it doesn't matter
	if (gameObject.GetComponent<Collider2D>().Raycast(_target.transform.position, _iDoNotNeedThis, _colliderRadius*2) > 0)
        {
	    _isObstructed = true;
	}
	else
	{
	    _isObstructed = false;
	}
    }

    private void OnCollisionExit2D()
    {
	_isObstructed = false;
    }

    private void Move()
    {
	Vector2 targetPosition = _target.transform.position;
	// Je doit faire cette partie puisque _target.transform.position est un vecteur3
	targetPosition = targetPosition + _encerclementModifier;

	if (!_isObstructed)
	{
	    if (_debug) { Debug.DrawLine(gameObject.transform.position, targetPosition, Color.red, 0.1f); }
	    transform.position = Vector2.MoveTowards(gameObject.transform.position, targetPosition, _speed * Time.deltaTime);
	}
    }

    private Vector2 CalculateRandomEncerclementVector(float minDistance)
    {
	float angle = Random.Range(0f, 2 * Mathf.PI);
	float y = (Mathf.Sin(angle) * minDistance);
	float x = (Mathf.Cos(angle) * minDistance);
	return new Vector2(x, y);
    }
}
