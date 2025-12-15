using UnityEngine;

public class Player : MonoBehaviour
{
    // Variable definition
    [Header("Characteristiques")]
    [SerializeField] private float _playerSpeed = 10f;
    [SerializeField] private int _playerLife = 100;

    [SerializeField] private float _collectionDistance = 2f;
    public float CollectionDistance => _collectionDistance;

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

    private void OnCollisionEnter2D(Collision2D other)
    {
	if (other.collider.tag == "Enemy") {
	    _anim.SetBool("IsDamaged", true);
	    other.collider.GetComponent<Enemy>().Damage(115);
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

    public void Damage(int p_degat)
    {
	_playerLife -= p_degat;
	GameManager.Instance.SetHealth(_playerLife);
	if (_playerLife < 1)
	{
	    Destroy(this.gameObject);
	}
    }
    
    public void IncreaseCollectionDistance(int p_amount)
    {
	_collectionDistance += p_amount;
    }
}
