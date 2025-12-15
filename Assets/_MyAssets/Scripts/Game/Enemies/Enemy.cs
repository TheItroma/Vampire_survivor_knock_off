using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{

    [Header("Characteristiques")]
    [SerializeField] private string _targetTag = "Player";
    [SerializeField] private int _enemyLife = 50;

    [Header("Le loot possible et ces chances")]
    [SerializeField] private int _points = 30;
    [SerializeField] private int _dropAmount = 1;
    [SerializeField] private List<GameObject> _drops = new List<GameObject>();
    [SerializeField] private List<float> _percentages = new List<float>();
    
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
	     _target.GetComponent<Player>().Damage(4);
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
	    _anim.SetBool("IsDamaged", false);
	    _anim.SetBool("IsDead", true);
	}
    }
    
    private List<GameObject> GetDrops()
    {
	float RandVal = Random.value;

	List<GameObject> Possible = new List<GameObject>();
	for (int i = 0; i < _percentages.Count; i++)
	{
	    if (_percentages[i] >= RandVal)
	    {
		Possible.Add(_drops[i]);
	    }
	}

	List<GameObject> Returned = new List<GameObject>();
	for (int i = 0; i < _dropAmount; i++)
	{
	    Returned.Add(Possible[Random.Range(0, Possible.Count)]);
	}
	return Returned;
    }
    // Cette fonction est activer a la fin de l'animation de mort pour des raisons evidentes
    public void SpawnDrops()
    {
	if (_drops.Count != _percentages.Count)
	{
	    Debug.Log("Table de loot pas bonne");
	}

	foreach (GameObject Drop in GetDrops())
	{
	    Instantiate(Drop, gameObject.transform.position, Quaternion.identity);
	}

	GameManager.Instance.IncreaseScore(_points);
	Destroy(this.gameObject);
    }
}
