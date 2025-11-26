using UnityEngine;

public class Player : MonoBehaviour
{
    // Variable definition
    [Header("Propreties")]
    [SerializeField] private float _playerSpeed = 10f;

    private Vector2 _direction;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
	float directionX = Input.GetAxisRaw("Horizontal");
	float directionY = Input.GetAxisRaw("Vertical");
	Debug.Log(Input.GetAxisRaw("Vertical"));
	Debug.Log(Input.GetAxisRaw("Horizontal"));

	_direction = new Vector2(directionX, directionY);
	_direction.Normalize();

	transform.Translate(_direction * Time.deltaTime * _playerSpeed);
    }
}
