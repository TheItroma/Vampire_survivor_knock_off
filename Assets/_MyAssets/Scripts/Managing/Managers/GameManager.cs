using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private AudioClip _entityHurt = default;
    [SerializeField] private AudioClip _collectSound = default;
    [Header("Spawn Rate")]
    [SerializeField] private int _rateIncreasePerPoints = 100;
    [SerializeField] private float _spawnRate = 1f;
    [SerializeField] private float _spawnRateIncreaseRate = 0.1f;
    
    [Header("Perks")]
    [SerializeField] private int _cost = 15;
    [SerializeField] private float _inflationRate = 1.4f;

    private int _score;
    private int _currency;

    public int Currency => _currency;
    public int Score => _score;   
    public float SpawnRate => _spawnRate;

    private List<GameObject> _entities = new List<GameObject>();
    private int _nextRateIncrease;

    private Player _player;

    private float _pauseTimeTracker;
    private float _pauseStamp;
    private bool _isPaused = false;

    private int _collectibleAmount = 0;
    private bool _collectAllCollectibles = false;

    private int _waveCount = 0;
    private bool _endWaveRunning = false;

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
	_pauseTimeTracker = Time.time;
	_player = FindAnyObjectByType<Player>();
	_nextRateIncrease = _rateIncreasePerPoints;
	UIGame.Instance.UpdateScore();
    }

    private void Update()
    {
	UpdateTime();
    }
    public AudioClip GetCollectSound()
    {
	return _entityHurt;
    }

    public AudioClip GetEntityHurt()
    {
	return _entityHurt;
    }

    // ------------------------------------ Suivie du temp ecouler ----------------------------------
    private void UpdateTime()
    {
	if (!_isPaused)
	{
	    UIGame.Instance.SetTime(Time.time - _pauseTimeTracker);
	}
    }

    private void TogglePause()
    {
	_isPaused = !_isPaused;
	if (_isPaused)
	{
	    _pauseStamp = Time.time;
	    GenocideEntities();
	}
	else
	{
	    _pauseTimeTracker += Time.time - _pauseStamp;
	}

	FindAnyObjectByType<SpawnManager>().ToggleSpawn();
	_player.EquipAll(!_isPaused);
	_player._canMove = !_isPaused;
    }

    // ------------------------ Truc d'interface -------------------------------

    public void IncreaseCurrency()
    {
	_currency++;
	UIGame.Instance.SetCurrency((100f * _currency) / _cost);
	if (_currency >= _cost && !_endWaveRunning) { StartCoroutine(StartEndWave()); }
    }

    private IEnumerator StartEndWave()
    {
	TogglePause();
	_endWaveRunning = true;
	_collectAllCollectibles = true;
	while (_collectibleAmount > 0) { yield return null; }
	_collectAllCollectibles = false;

	int Amount = 0;
	while (_currency >= _cost)
	{
	    _currency -= _cost;
	    _cost = Mathf.RoundToInt(_cost * _inflationRate);
	    Amount++;
	}
	yield return StartCoroutine(GiveHands(Amount));
	_waveCount++;
	_endWaveRunning = false;
    }

    private IEnumerator GiveHands(int p_amount)
    {
	UIGame.Instance.CardPanel(true);

	for (int i = 0; i < p_amount; i++)
	{
	    yield return StartCoroutine(CardManager.Instance.GiveHand());
	}

	UIGame.Instance.CardPanel(false);
	TogglePause();
    }


    public void IncreaseScore(int p_points)
    {
	_score += p_points;
	UIGame.Instance.UpdateScore();

    	if (_score >= _nextRateIncrease)
	{
	    _spawnRate -= _spawnRate * _spawnRateIncreaseRate;
	    _nextRateIncrease += _rateIncreasePerPoints;
	}
    }
    
    // ------------------------------ Suivie des collectibles
    public bool CollectAll()
    {
	return _collectAllCollectibles;
    }

    public void IncreaseCollectible()
    {
	_collectibleAmount++;
    }

    public void RemoveCollectible()
    {
	_collectibleAmount--;
    }

    // ------------------------------------ Suivie des entites --------------------------------------
    public List<GameObject> GetEntities()
    {
	return _entities;
    }

    public void AddEntity(GameObject p_entity)
    {
	_entities.Add(p_entity);
    }

    public void RemoveEntity(GameObject p_entity)
    {
	_entities.Remove(p_entity);
    }

    private void GenocideEntities()
    {
	List<GameObject> Entities = new List<GameObject>(_entities);
	foreach(GameObject Entity in Entities)
	{
	    if (Entity.GetComponent<Player>() == null)
	    {
		RemoveEntity(Entity);
		Destroy(Entity);
	    }
	}
    }
    // ------------------------------ Autres --------------------------------------

    public void GameOver()
    {
	SceneManager.LoadScene(2);
    }
}
