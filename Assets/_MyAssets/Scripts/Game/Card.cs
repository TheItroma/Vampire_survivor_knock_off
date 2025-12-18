using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private GameObject _back = default(GameObject);

    private Animation _anim;

    private void Start()
    {
	_back = Instantiate(_back, transform.position, Quaternion.identity);
	Debug.Log("Instantiated");
	_anim = _back.GetComponent<Animation>();
    }
    
    public void Reveal()
    {
	_anim.Play();
    }
}
