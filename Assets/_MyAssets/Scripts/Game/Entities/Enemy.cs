using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Entity
{
    [Header("Le loot possible et ces chances")]
    [SerializeField] protected int _points = 30;
    [SerializeField] protected int _dropAmount = 1;
    [SerializeField] protected List<GameObject> _drops = new List<GameObject>();
    [SerializeField] protected List<float> _percentages = new List<float>();
    
    [Header("Mouvement")]
    [SerializeField] protected float _speedRandomizer = 0.3f;
    [SerializeField] protected float _maxDistanceToPlayer = 0f;
    
    // system qui pourrait permetre des ennemies qui ce retourne contre les siens
    protected bool _isMad = false;
    protected GameObject _target;
    protected Rigidbody2D _rb;

    private void Awake()
    {
	_isPlayer = false;
	_target = FindAnyObjectByType<Player>().gameObject;
	_rb = GetComponent<Rigidbody2D>();
	_speed = MyFunctions.GetRandomizedByPercentage(_speed, _speedRandomizer);
    }

    private void FixedUpdate()
    {
	if (Vector2.Distance(transform.position, FindAnyObjectByType<Player>().gameObject.transform.position) > _maxDistanceToPlayer)
	{
	    Move();
	}
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
	if (other.collider.GetComponent<Entity>().IsPlayer() || _isMad)
	{
	    // Voire comment ne pas avoir a entre le <Player>
	    // Fixed avec la class <Entity>
	    _target.GetComponent<Entity>().Damage(4);
	}
    }

    protected override void Move()
    {
	Vector2 MoveTowards = Vector2.MoveTowards((Vector2)transform.position, _target.transform.position, (_speed * GetModifier("Speed")) * Time.fixedDeltaTime);

	_rb.MovePosition(MoveTowards);

	MoveTowards.Normalize();
	// Je sais pas pourquoi j'ai besoin d'inverser mais bon
	// REVENIRE ICI
	_anim.SetFloat("InputX", MoveTowards.x);
	_anim.SetFloat("InputY", MoveTowards.y);
    }

    protected override void Dies()
    {
	GameManager.Instance.RemoveEntity(gameObject);
	_canMove = false;
	GetComponent<CircleCollider2D>().enabled = false;
	_anim.SetTrigger("IsDead");
    }
    

    // Cette fonction est activer a la fin de l'animation de mort pour des raisons evidentes
    public void SpawnDrops()
    {
	foreach (GameObject Drop in MyFunctions.GetRandomObject(_drops, _percentages, _dropAmount))
	{
	    Instantiate(Drop, transform.position, Quaternion.identity);
	    GameManager.Instance.IncreaseCollectible();
	}

	Destroy(this.gameObject);

	GameManager.Instance.IncreaseScore(_points);
    }
}
