using UnityEngine;

public class CardFront : MonoBehaviour
{
    [SerializeField] GameObject _weapon = default(GameObject);

    public void WhenClicked()
    {
	FindAnyObjectByType<Player>().AddWeapon(_weapon);
	CardManager.Instance.Selected();
    }
}
