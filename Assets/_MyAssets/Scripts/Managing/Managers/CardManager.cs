using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class CardManager : MonoBehaviour
{
    [Header("Characteristiques generales")]
    [SerializeField] private float _dramaticPause = 0.5f;

    [Header("Cartes")]
    [SerializeField] private List<GameObject> _cardFrontPrefabs = new List<GameObject>();
    [SerializeField] private List<float> _percentages = new List<float>();

    [SerializeField] private GameObject _cardBackPrefab = default(GameObject);

    private List<GameObject> _cards = new List<GameObject>();
    
    private bool _hasSelected = false;

    public static CardManager Instance;
    
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    private void ResetHand()
    {
	if (_cards.Count != 0)
	{
	    foreach (GameObject CardFront in _cards)
	    {
		Destroy(CardFront);
	    }
	    _cards.Clear();
	}
    }

    public void Selected()
    {
	_hasSelected = true;
    }

    // Bon, J'ai programmer un animation mais Je prefaire largement ca
    public IEnumerator GiveHand()
    {
	int CardNumber = _cardFrontPrefabs.Count;
	ResetHand();
	// Cree
	List<GameObject> DrawnCardFronts = MyFunctions.GetRandomObject(_cardFrontPrefabs, _percentages, CardNumber);
	for (int i = 0; i < CardNumber; i++)
	{
	    _cards.Add(Instantiate(DrawnCardFronts[i], transform));
	}

	for (int i = 0; i < CardNumber; i++)
	{
	    yield return new WaitForSeconds(_dramaticPause/2);
	    Instantiate(_cardBackPrefab, _cards[i].transform);
	}
	
	// L'animation "Spawn" du derrier utilise le rect transform
	yield return new WaitForSeconds(_dramaticPause);

	// Montre
	foreach (Transform CardContainer in transform)
	{
	    yield return new WaitForSeconds(_dramaticPause);
	    // De la maniere dont les card spawn, le front est index 1 et le back, 0
	    CardContainer.GetChild(1).gameObject.GetComponent<Animator>().SetTrigger("Reveal");
	    // Il y a une event dans l'animation qui vas reveler le front
	}
	EventSystem.current.SetSelectedGameObject(_cards[0]);

	while (!_hasSelected) { yield return null; }
	_hasSelected = false;
    }
}
