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

    void Awake()
    {
	_target = GameObject.FindGameObjectsWithTag(_targetTag)[0];
	_rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        move();
    }

    private void move()
    {
	Vector2 targetPosition = _target.transform.position;
	_rb.MovePosition(Vector2.MoveTowards(gameObject.transform.position, targetPosition, _speed * Time.fixedDeltaTime));
    }
}
