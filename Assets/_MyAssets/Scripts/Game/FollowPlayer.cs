using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField] private float _zOffset;
    private Transform _toFollow;

    private void Awake()
    {
	_toFollow = FindAnyObjectByType<Player>().transform;
    }
    
    private void Update()
    {
	if (_toFollow)
	{
	    transform.position = new Vector3(_toFollow.position.x, _toFollow.position.y, _zOffset);
	}
    }
}
