using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField] private GameObject _toFollow;
    [SerializeField] private float _zOffset;

    void Update()
    {
	transform.position = new Vector3(_toFollow.transform.position.x, _toFollow.transform.position.y, _zOffset);
    }
}
