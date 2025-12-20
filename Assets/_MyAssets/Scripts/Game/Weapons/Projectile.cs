using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Projectile : MonoBehaviour
{
    [Header("Characteristiques")]
    
    // Je suis pas trop sure si cette section est une bonne idee (public)
    [SerializeField] public int _damage;

    [Header("Effet")]
    [SerializeField] private bool _hasEffect;
    [SerializeField] private string _effect;
    [SerializeField] private float _effectMultiplier;
    [SerializeField] private float _effectDurration;

    [Header("Solidite")]
    [SerializeField] private bool _isIndestructible = false;
    [SerializeField] private int _piercing = 1;
    [SerializeField] private float _maxDistance = 20f;
    // A vraiment besoin d'etre serialize?

    [Header("S'il chercher pour l'enemy")]
    [SerializeField] private bool _isSeeking = false;

    [Header("Vitesse")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _speedRandomizer = 0.1f;
    [SerializeField] private float _turnSpeed;
    [SerializeField] private float _turnSpeedRandomizer;
    
    [Header("Debug")]
    [SerializeField] private bool _debug = false;

    private GameObject _target = default(GameObject);

    private float _angle;
    private Vector2 _direction;
    private float _distance;

    private bool _canMove = true;
    private GameObject _parent;

    private Animator _anim;

    private void Start()
    {
	_speed = MyFunctions.GetRandomizedByPercentage(_speed, _speedRandomizer);
	_turnSpeed = MyFunctions.GetRandomizedByPercentage(_turnSpeed, _turnSpeedRandomizer);
	
	// Store la rotation z
	_angle = transform.eulerAngles.z;
	_direction = MyFunctions.MakeVectorUsingAngle(1f, _angle * Mathf.Deg2Rad);

	//_anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
	if (_parent == null)
	{
	    Destroy(this.gameObject);
	    return;
	}

	UpdateDistance();
	// Meme s'il est indestructible pour les performance
	if (_distance >= _maxDistance)
	{
	    Destroy(this.gameObject);
	}

	if (_isSeeking)
	{
	    // S'ill exist, seek
	    if (_target != null)
	    {
		Seek();
		Vector2 MoveTowards = Vector2.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);
		transform.position = MoveTowards;
	    }
	    else
	    {
		SetTargetToNearest();
	    }
	}
	if (_canMove) { Move(); }
    }

    private void Move()
    {
	//Vector2 MoveTowards = Vector2.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);
	transform.Translate(_direction * _speed * Time.deltaTime, Space.World);
	//transform.position += (Vector3)(_direction * _speed * Time.deltaTime);
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
	// Si le component entity est attacher et est enemy de parent
	Entity OtherEntity = other.gameObject.GetComponent<Entity>();
    	if (OtherEntity != null && OtherEntity.IsPlayer() != _parent.GetComponent<Entity>().IsPlayer())
	{
	    if (_hasEffect)
	    {
		if (_effect == "Fire")
		{
		    OtherEntity.ApplyEffect("Damage", _effectMultiplier, _effectDurration);
		    OtherEntity.ApplyEffect("Speed", _effectMultiplier, _effectDurration);
		}
		else { ApplyEffect(OtherEntity); }
	    }

	    OtherEntity.Damage(_damage);
	    if (!_isIndestructible)
	    {
		_piercing--;
	    }
	    if (_piercing > 0)
	    {

		// Si a toucher le target, change de (question de pas trop tourner en rond)
		if (_isSeeking) { SetTargetToNearest(); }
	    }
	    else
	    {
		Destroy(this.gameObject);
	    }
	}
    }
    
    // Oups, block de code
    private void Seek()
    {
	// Trouve l'angle a atteidre
	Vector2 TargetDirection = _target.transform.position - transform.position;
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

    private void SetTargetToNearest()
    {
	Entity ParentEntity = _parent.GetComponent<Entity>();
	List<GameObject> Entities = new List<GameObject>(GameManager.Instance.GetEntities());

	// On s'assure que le target n'est pas des siens
	for (int i = Entities.Count - 1; i >= 0; i--)
	{
	    Entity Entity = Entities[i].GetComponent<Entity>();
	    if (Entity.IsPlayer() == ParentEntity.IsPlayer() || Entities[i] == _target)
	    {
		Entities.RemoveAt(i);
	    }
	}

	if (Entities.Count == 0)
	{
	    _canMove = false;
	    return;
	    //_anim.SetBool("IsIdle", true);
	}
	_canMove = true;

	GameObject NearestEnnemy = Entities[0];
	float DistanceToEnemy;
	float Nearest = _maxDistance;

	for (int i = 0; i < Entities.Count; i++)
	{
	    DistanceToEnemy = Vector3.Distance(_parent.transform.position, Entities[i].transform.position);
	    // Si Entities[i] est le plus proch mais pas l'ancient target
	    if (DistanceToEnemy < Nearest)
	    {
		NearestEnnemy = Entities[i];
		Nearest = DistanceToEnemy;
	    }
	}
	_target = NearestEnnemy;
    }

    
    public void SetParent(GameObject p_parent)
    {
	_parent = p_parent;
	
	if (_isSeeking)
	{
	    SetTargetToNearest();
	}
    }


    private void ApplyEffect(Entity p_target)
    {
	p_target.ApplyEffect(_effect, _effectMultiplier, _effectDurration);
    }
    private void UpdateDistance() { _distance = Vector2.Distance(_parent.transform.position, transform.position); }
}
