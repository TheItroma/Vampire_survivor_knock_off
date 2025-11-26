using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Characteristiques")]
    [SerializeField] private Player _player;
    [SerializeField] private float _speed;

    private Vector2 _direction;

    void Start()
    {
	_player = FindAnyObjectByType<Player>();
    }

    void Update()
    {
        Move();
    }
    private void Move()
    {
	Vector3 targetPosition = _player.gameObject.transform.position;
	_direction = new Vector2(targetPosition.x, targetPosition.y);
	_direction.Normalize();

	transform.Translate(_direction * Time.deltaTime * _speed);
    }
}
