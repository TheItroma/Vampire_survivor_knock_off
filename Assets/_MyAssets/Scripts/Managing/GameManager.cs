using UnityEngine;
using System.Collections.Generic;
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

    private List<GameObject> _entities = new List<GameObject>();
    private int _nextRateIncrease;

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
	_nextRateIncrease = _rateIncreasePerPoints;
	UIGame.Instance.UpdateScore();
    }
    
    private void IncreaseRate()
    {
	_spawnRate += _spawnRateIncreaseRate;
    }

    public void SetHealth(int p_health)
    {
	UIGame.Instance.SetHealth(p_health);
    }
	
    public void IncreaseCurrency()
    {
	_currency++;
    }

    public void IncreaseScore(int p_points)
    {
	_score += p_points;
	UIGame.Instance.UpdateScore();

    	if (_score >= _nextRateIncrease)
	{
	    IncreaseRate();
	    _nextRateIncrease += _rateIncreasePerPoints;
	}
    }


    public void GameOver()
    {
	UIGame.Instance.ActivateGameOverPanel();
    }

    public void AddEntity(GameObject p_entity)
    {
	_entities.Add(p_entity);
    }

    public void RemoveEntity(GameObject p_entity)
    {
	_entities.Remove(p_entity);
    }

    public List<GameObject> GetEntities()
    {
	return _entities;
    }
}
