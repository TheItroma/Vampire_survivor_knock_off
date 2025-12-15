using UnityEngine;
using UnityEngine.UI;

public class UIGame : MonoBehaviour
{
    [SerializeField] private Slider _healthBar;

    public void SetHealth(int p_health)
    {
	_healthBar.value = p_health;
    }
}
