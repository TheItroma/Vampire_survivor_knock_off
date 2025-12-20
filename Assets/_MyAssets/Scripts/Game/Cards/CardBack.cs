using UnityEngine;

public class CardBack : MonoBehaviour
{
    // Appeler par la fin de l'animation de flip
    private void ShowFront()
    {
	transform.parent.GetChild(0).gameObject.SetActive(true);
	Destroy(this.gameObject);
    }
}
