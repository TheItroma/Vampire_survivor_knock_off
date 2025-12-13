using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{

[Header("Characteristiques")]
    [SerializeField] private string _targetTag = "Player";
    [SerializeField] private int _enemyLife = 50;
    [SerializeField] private GameObject _drop = default(GameObject);
    
    [Header("Mouvement")]
    [SerializeField] private float _speed = 3.0f;
    [SerializeField] private float _speedRandomizer = 0.3f;


    private GameObject _target;
    private Rigidbody2D _rb;
    private Animator _anim;

    private void Awake()
    {
	_target = GameObject.FindGameObjectsWithTag(_targetTag)[0];
	_rb = gameObject.GetComponent<Rigidbody2D>();
	_anim = GetComponent<Animator>();
	_speed = MyFunctions.GetRandomizedByPercentage(_speed, _speedRandomizer);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnCollisionEnter2D(Collision2D other) {
	if (other.collider.tag == _targetTag)
	{
	    // Voire comment ne pas avoir a entre le <Player>
	    // _target.GetComponent<Player>().Damage(4);
	}
    }

    private void Move()
    {
	Vector2 moveTowards = Vector2.MoveTowards(gameObject.transform.position, _target.transform.position, _speed * Time.fixedDeltaTime);
	
	_rb.MovePosition(moveTowards);

	moveTowards.Normalize();
	_anim.SetFloat("InputX", moveTowards.x);
	_anim.SetFloat("InputY", moveTowards.y);
    }
    
    public void Damage(int p_amount)
    {
	_enemyLife -= p_amount;
	_anim.SetBool("IsDamaged", true);
	// SET LES ISDAMAGED TO FALSE A QQPART

	if (_enemyLife < 1)
	{
	    // DONNE LES POINTS
	    _anim.SetBool("IsDamaged", false);
	    _anim.SetBool("IsDead", true);
	}
    }
    
    // Cette fonction est activer a la fin de l'animation de mort pour des raisons evidentes
    public void SpawnDrop()
    {
	Instantiate(_drop, GetComponent<Transform>().position, Quaternion.identity);
	Destroy(this.gameObject);
    }
}
