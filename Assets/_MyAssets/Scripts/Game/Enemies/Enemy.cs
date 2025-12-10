using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{

    [Header("Characteristiques")]
    [SerializeField] private string _targetTag = "Player";
    
    [Header("Mouvement")]
    [SerializeField] private float _speed = 3.0f;


    private GameObject _target;
    private Rigidbody2D _rb;
    private Animator _anim;

    void Awake()
    {
	_target = GameObject.FindGameObjectsWithTag(_targetTag)[0];
	_rb = gameObject.GetComponent<Rigidbody2D>();
	_anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        move();
    }

    private void move()
    {
	Vector2 targetPosition = _target.transform.position;
	Vector2 moveTowards = Vector2.MoveTowards(gameObject.transform.position, targetPosition, _speed * Time.fixedDeltaTime);
	
	_rb.MovePosition(moveTowards);

	moveTowards.Normalize();
	_anim.SetFloat("InputX", moveTowards.x);
	_anim.SetFloat("InputY", moveTowards.y);
    }
}
