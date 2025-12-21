using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Avoir une classe "entite" permet de ne pas avoir a Utiliser les types "Player" ou "Enemy" lors de projectile seek ou de weapon equip et plein d'autres choses
// Ca rend le code plus propre aussi

public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected int _health;
    [SerializeField] protected float _speed;
    [SerializeField] protected List<GameObject> _weaponPrefabs = new List<GameObject>();

    protected float _damageMultiplier;
    protected bool _isPlayer;
    public bool _canMove = true;
    protected Animator _anim;
    protected List<GameObject> _weapons = new List<GameObject>();
    protected Dictionary<string, List<float>> _effects = new Dictionary<string, List<float>>();

    protected float[] _angles;

    protected virtual void Start()
    {
	GameManager.Instance.AddEntity(gameObject);
	_anim = GetComponent<Animator>();

	if (_weaponPrefabs.Count > 0)
	{
	    for (int i = 0; i < _weaponPrefabs.Count; i++)
	    {
		AddWeapon(_weaponPrefabs[i]);
	    }
	}

	_effects.Add("Speed", new List<float> { 1f });
	_effects.Add("Damage", new List<float> { 1f });
    }

    public virtual void Damage(int p_damage)
    {
	 AudioSource.PlayClipAtPoint(GameManager.Instance._entityHurt, Camera.main.transform.position, 0.8f);
	_health -= Mathf.RoundToInt((float)p_damage * GetModifier("Damage"));
	_anim.SetTrigger("IsDamaged");
	if (_health < 1)
	{
	    Dies();
	}
    }
    
    public bool IsPlayer()
    {
	return _isPlayer;
    }

    // --------------------- Suivie d'arme -----------------
    public List<GameObject> GetWeapons()
    {
	return new List<GameObject>(_weapons);
    }

    public void AddWeapon(GameObject p_weapon)
    {
	GameObject Weapon = Instantiate(p_weapon, transform);
	_weapons.Add(Weapon);
	UpdateWeaponAngles();
    }
    
    private void UpdateWeaponAngles()
    {
	_angles = MyFunctions.GetRotations(_weapons.Count);
	for (int i = 0; i < _weapons.Count; i++)
	{
	    _weapons[i].transform.rotation = Quaternion.Euler(0f, 0f, _angles[i]);
	}
    }

    public void RemoveWeapon(GameObject p_weapon)
    {
	_weapons.Remove(p_weapon);
	Destroy(p_weapon);
    }

    public void EquipAll(bool p_isEquiped)
    {
	for (int i = 0; i < _weapons.Count; i++)
	{
	    _weapons[i].GetComponent<Launcher>().Equip(p_isEquiped);
	}
    }

    // ------------------------- Suivie d'effects ----------------
    public float GetModifier(string p_effect)
    {
	float TotalMultiplier = 1f;
	foreach (float Multiplier in _effects[p_effect])
	{
	    TotalMultiplier *= Multiplier;
	}
	return TotalMultiplier;
    }

    public void ApplyEffect(string p_effect, float p_multiplier, float p_duration)
    {
	StartCoroutine(EffectCoroutine(p_effect, p_multiplier, p_duration));
    }

    protected IEnumerator EffectCoroutine(string p_effect, float p_multiplier, float p_duration)
    {
	_effects[p_effect].Add(p_multiplier);
	yield return new WaitForSeconds(p_duration);
	_effects[p_effect].Remove(p_multiplier);
    }

    protected abstract void Move();
    protected abstract void Dies();
}
