using UnityEngine;

// Avoir une classe "entite" permet de ne pas avoir a Utiliser les types "Player" ou "Enemy" lors de projectile seek ou de weapon equip et plein d'autres choses
// Ca rend le code plus propre aussi

public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected int _health;
    [SerializeField] protected float _speed;

    protected bool _isPlayer;
    protected bool _canMove = true;
    protected Animator _anim;

    protected virtual void Start()
    {
	GameManager.Instance.AddEntity(gameObject);
    }

    public virtual void Damage(int p_damage)
    {
	_health -= p_damage;
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

    protected abstract void Move();
    protected abstract void Dies();
}
