using UnityEngine;

public class Drop : MonoBehaviour
{
    [Header("Characteristiques")]
    [SerializeField] private string _targetTag = "Player";

    // Doit etre plus rapid que le joueur
    [SerializeField] private float _speed = 2.0f;
    //
    // Randomizer ou pas?
    [SerializeField] private float _speedRandomizer = 0.3f;

    private Player _target;

    [SerializeField] bool _startCollection = false;

    private void Start()
    {
	_target = FindAnyObjectByType<Player>();
	_speed = MyFunctions.GetRandomizedByPercentage(_speed, _speedRandomizer);
	_startCollection = IsInRange();
    }

    // Pas ideal que la detection de distance soit dans le fixed update mais c'est mieux que si ca serait dans update
    private void FixedUpdate() {
    	if (IsInRange())
	{
	    _startCollection = true;
	}
    }

    private void Update()
    {
	if (_startCollection)
	{
	    Move();
	}
    }
    
    private bool IsInRange()
    {
	return (Vector2.Distance(gameObject.transform.position, _target.transform.position) <= _target.CollectionDistance);
    }

    private void GetCollected()
    {
	Debug.Log("GET COLLECTED LOOSER");
	GameManager.Instance.IncreaseCurrency();
        Destroy(this.gameObject);
    }

    private void Move()
    {
	transform.position = Vector2.MoveTowards(gameObject.transform.position, _target.transform.position, _speed * Time.deltaTime);
	if (transform.position == _target.transform.position)
	{
	    GetCollected();
	}
    }
}
