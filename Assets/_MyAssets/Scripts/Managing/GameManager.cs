using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private int _rateIncreasePerPoints = 100;
    [SerializeField] private float _spawnRate = 1f;
    [SerializeField] private float _spawnRateIncreaseRate = 0.1f;

    [SerializeField] private UIGame _uiGame;

    private int _score;
    private int _currency;

    public int Currency => _currency;
    public int Score => _score;   
    public float SpawnRate => _spawnRate;

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

    private void Start()
    {
	_uiGame = FindAnyObjectByType<UIGame>();
    }

    private void IncreaseRate()
    {
	_spawnRate += _spawnRateIncreaseRate;
    }

    public void SetHealth(int p_health)
    {
	_uiGame.SetHealth(p_health);
    }
	
    public void IncreaseCurrency()
    {
	_currency++;
    }

    public void IncreaseScore(int p_points)
    {
	_score += p_points;
	// AJOUTER QQCH QUI FAIT UPDATE DANS UIGAMES
	// Plus optimiser que la version du prof hehe
    	if (GameManager.Instance.Score % _rateIncreasePerPoints == 0)
	{
	    IncreaseRate();
	}
    }



    public void GameOver()
    {
	//
    }
}
