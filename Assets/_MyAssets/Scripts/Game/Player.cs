using UnityEngine;

public class Player : Entity
{
    // Variable definition
    [Header("Characteristiques")]
    [SerializeField] private float _collectionDistance = 2f;
    public float CollectionDistance => _collectionDistance;

    [SerializeField] private GameObject _weapon = default(GameObject);

    protected Vector2 _direction;

    private void Awake()
    {
	_isPlayer = true;
    }

    protected override void Start()
    {
	base.Start();
	_weapon = Instantiate(_weapon, transform);
	_anim = GetComponent<Animator>();
    }

    private void Update()
    {
	if (_canMove) { Move(); }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
	// Je sais que ca ce repete mais dans le if, il est utiliser come operateur bool
	if (other.collider.GetComponent<Enemy>()) {
	    // Et dans l'autre, un component
	    other.collider.GetComponent<Enemy>().Damage(115);
	}
    }

    // Methode prives

    protected override void Move()
    {
	float DirectionX = Input.GetAxisRaw("Horizontal");
	float DirectionY = Input.GetAxisRaw("Vertical");

	// J'aime pas le system de states machine, ce n'est pas ideale pour les animation pixel art nessesitant tres peux de transitions.
	// Blend tree are usefull tho as it allows to modulate speed from floats.
	_anim.SetBool("IsMoving", !(DirectionX == 0 && DirectionY == 0));
	_anim.SetFloat("InputX", DirectionX);
	_anim.SetFloat("InputY", DirectionY);

	_direction = new Vector2(DirectionX, DirectionY);
	_direction.Normalize();

	transform.Translate(_direction * Time.deltaTime * _speed);
    }

    protected override void Dies()
    {
	GameManager.Instance.RemoveEntity(gameObject);
	_anim.SetTrigger("IsDead");
	_canMove = false;
	GetComponent<CircleCollider2D>().enabled = false;
	GameManager.Instance.GameOver();
    }

    // Methode publiques

    public override void Damage(int p_degat)
    {
	GameManager.Instance.SetHealth(_health);
	base.Damage(p_degat);
    }
    
    public void IncreaseCollectionDistance(int p_amount)
    {
	_collectionDistance += p_amount;
    }

}
