using UnityEngine;

public class Player : MonoBehaviour
{
    // Variable definition
    [Header("Propreties")]
    [SerializeField] private float _playerSpeed = 10f;
    [SerializeField] private int _playerLife = 100;

    private Vector2 _direction;
    private Animator _anim;

    private void Start()
    {
	_anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
	if (collision.collider.tag == "Enemy") {
	    _anim.SetBool("IsDamaged", true);
	}
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
	_anim.SetBool("IsDamaged", false);
    }


    // Methode prives

    private void Move()
    {
	float directionX = Input.GetAxisRaw("Horizontal");
	float directionY = Input.GetAxisRaw("Vertical");

	_anim.SetFloat("InputX", directionX);
	_anim.SetFloat("InputY", directionY);

	_direction = new Vector2(directionX, directionY);
	_direction.Normalize();

	transform.Translate(_direction * Time.deltaTime * _playerSpeed);
    }

    // Methode publiques

    public void Degats(int degat)
    {
	_playerLife -= degat;
    }
}
