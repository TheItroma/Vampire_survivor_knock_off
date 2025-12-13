using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private int _rateIncreasePerPoints = 100;
    [SerializeField] private float _spawnRate = 1f;
    [SerializeField] private float _spawnRateIncreaseRate = 0.1f;

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

    private void IncreaseRate()
    {
	_spawnRate += _spawnRateIncreaseRate;
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

    // Perdu ben trop de temp a essayer de faire une algo pour ca alors que linq orderby exist
    public GameObject[] GetNearestEnemies(Vector2 p_position, int p_amount)
    {
        return GameObject.FindGameObjectsWithTag("Enemy")
	    .OrderBy(Enemy => ((Vector2)Enemy.transform.position - p_position).sqrMagnitude)
	    .Take(p_amount)
	    .ToArray();
    }

    public void GameOver()
    {
	//
    }
}
