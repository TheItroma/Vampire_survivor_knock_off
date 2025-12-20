using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIStart : MonoBehaviour
{
    [SerializeField] private GameObject _instructionPanel = default(GameObject);
    [SerializeField] private GameObject _returnButton = default(GameObject);
    [SerializeField] private GameObject _startScreen = default(GameObject);
    [SerializeField] private GameObject _startButton = default(GameObject);

    private bool _defaultPanelOn = true;

    private void Start()
    {
	EventSystem.current.SetSelectedGameObject(_startButton);
    }

    public void ToggleInstructions()
    {
	_defaultPanelOn = !_defaultPanelOn;
	_startScreen.SetActive(_defaultPanelOn);
	_instructionPanel.SetActive(!_defaultPanelOn);
	if (_defaultPanelOn)
	{
	    EventSystem.current.SetSelectedGameObject(_startButton);
	}
	else
	{
	    EventSystem.current.SetSelectedGameObject(_returnButton);
	}
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(1);
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
