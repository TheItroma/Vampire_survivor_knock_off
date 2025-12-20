using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    
    [SerializeField] public bool _isEquiped = false;

    private Vector2[] _positions;
    private float[] _angles;

    private List<GameObject> _projectiles = new List<GameObject>();

    private Coroutine _equipCoroutine;

    private void Start()
    {
	_positions = new Vector2[_resolution];
	_angles = new float[_resolution];

	// Si tous a la fois, s'assurer que le firerate rest proportionel
	if (_allAtOnce)
	{
	    _fireRate *= _resolution;
	}
	
	// Cree des listes des angles et position
	//float AngleIncrements = (Mathf.PI * 2f) / (float)_resolution;
	//float Angle = 0f;

	//for (int i = 0; i < _resolution; i++)
	//{
	//    _positions[i] = MyFunctions.MakeVectorUsingAngle(_radius, Angle);
	//    _angles[i] = (Angle * Mathf.Rad2Deg);
	//    Angle += AngleIncrements;
	//}
	_angles = MyFunctions.GetRotations(_resolution);
	for (int i = 0; i < _resolution; i++)
	{
	    _positions[i] = MyFunctions.MakeVectorUsingAngle(_radius, _angles[i] * Mathf.Deg2Rad);
	}

	Equip(true);
    }

    public void Equip(bool p_isEquiped)
    {
	_isEquiped = p_isEquiped;

	if (_isEquiped && _equipCoroutine == null)
	{
	    _equipCoroutine = StartCoroutine(LaunchCoroutine());
	}
	else if (!_isEquiped && _equipCoroutine != null)
	{
	    StopCoroutine(_equipCoroutine);
	    _equipCoroutine = null;
	}
    }

    private IEnumerator LaunchCoroutine()
    {
	while(true)
	{
	    for (int i = 0; i < _resolution; i++)
	    {
		GameObject Projectile = Instantiate(_projectile, _positions[i] + (Vector2)gameObject.transform.position, Quaternion.Euler(0f, 0f, _angles[i]));
		Projectile.GetComponent<Projectile>().SetParent(transform.parent.gameObject);
		if (!_allAtOnce) { yield return new WaitForSeconds(_fireRate); }
	    }
	    if (_allAtOnce) { yield return new WaitForSeconds(_fireRate); }
	}
    }
}
