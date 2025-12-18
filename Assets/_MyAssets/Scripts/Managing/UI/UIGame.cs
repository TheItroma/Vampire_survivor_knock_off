using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIGame : MonoBehaviour
{
    public static UIGame Instance;

    [SerializeField] private TextMeshProUGUI _txtScore = default;
    [SerializeField] private Slider _healthBar;
    [SerializeField] private GameObject _gameOverPanel = default(GameObject);
    
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

    public void SetHealth(int p_health)
    {
	_healthBar.value = p_health;
    }

    public void UpdateScore()
    {
        _txtScore.text = "Pointage : " + GameManager.Instance.Score.ToString();
    }

    public void ActivateGameOverPanel()
    {
	//Time.timeScale = 0f;
	_gameOverPanel.SetActive(true);
	_gameOverPanel.GetComponent<Animator>().SetTrigger("Lightning");
    }
}
