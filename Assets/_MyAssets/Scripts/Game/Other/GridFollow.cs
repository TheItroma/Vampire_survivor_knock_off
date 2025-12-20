using UnityEngine;

public class GridFollow : MonoBehaviour
{
    [SerializeField] Transform _target;
    [SerializeField] float _snap;

    private void Update()
    {
	// https://www.youtube.com/watch?v=7nFrizkDnYs
	transform.position = new Vector2(Mathf.Round(_target.position.x / _snap) * _snap, Mathf.Round(_target.position.y / _snap) * _snap);
    }
}
