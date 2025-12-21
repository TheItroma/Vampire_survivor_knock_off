using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UIGame : MonoBehaviour
{
    public static UIGame Instance;

    [SerializeField] private TMP_Text _txtScore = default(TMP_Text);
    [SerializeField] private TMP_Text _txtTime = default(TMP_Text);
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Slider _currencyBar;
    [SerializeField] private GameObject _cardPanel = default(GameObject);
    
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
	SetHealth(100);
	SetCurrency(0);
    }

    public void SetHealth(int p_health)
    {
	_healthBar.value = p_health;
    }

    public void SetCurrency(float p_currency)
    {
	_currencyBar.value = p_currency;
    }

    public void UpdateScore()
    {
        _txtScore.text = "Pointage : " + GameManager.Instance.Score.ToString();
    }

    public void SetTime(float p_time)
    {
        _txtTime.text = "Temps écoulé : " + p_time.ToString("f2") + "S";
    }

    public void CardPanel(bool p_isActive)
    {
	_cardPanel.SetActive(p_isActive);
    }

    public void Restart()
    {
	SceneManager.LoadScene(1);
    }
    
    public void Return()
    {
	SceneManager.LoadScene(0);
    }
    
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
