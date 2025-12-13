using UnityEngine;
using System.Collections;

public class Launcher : MonoBehaviour
{
    [Header("Characteristiques")]
    [SerializeField] private GameObject _projectile = default(GameObject);
    // Si tout les transforms le font en meme temps
    [SerializeField] private bool _allAtOnce = false;
    // Ne pas oublier que si _allAtOnce, on multiplie par resolution, sinon
    [SerializeField] private float _fireRate = 1f;

    [SerializeField] private float _radius = 3f;
    [SerializeField] private int _resolution = 1;
    
    [SerializeField] public bool _isEquiped = true;

    private Vector2[] _positions;
    private float[] _angles;
    
    private void Awake()
    {
	_positions = new Vector2[_resolution];
	_angles = new float[_resolution];

	if (_allAtOnce)
	{
	    _fireRate *= _resolution;
	}
	
	// Cree une array de tout les transforms
	float AngleIncrements = (Mathf.PI * 2f) / (float)_resolution;
	float Angle = 0f;

	for (int i = 0; i < _resolution; i++)
	{
	    _positions[i] = MyFunctions.MakeVectorUsingAngle(_radius, Angle);
	    _angles[i] = (Angle * Mathf.Rad2Deg);
	    Angle += AngleIncrements;
	}

	StartCoroutine(LaunchCoroutine());
    }

    IEnumerator LaunchCoroutine()
    {
	GameObject Projectile;
	while (_isEquiped)
	{
	    for (int i = 0; i < _resolution; i++)
	    {
		Projectile = Instantiate(_projectile, _positions[i] + (Vector2)gameObject.transform.position, Quaternion.identity);
		Projectile.transform.Rotate(0f, 0f, _angles[i]);

		if (!_allAtOnce) { yield return new WaitForSeconds(_fireRate); }
	    }
	    if (_allAtOnce) { yield return new WaitForSeconds(_fireRate); }
	}
	Destroy(this.gameObject);
    }
}
