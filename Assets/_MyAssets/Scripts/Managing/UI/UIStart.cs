using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIStart : MonoBehaviour
{
    [SerializeField] private GameObject _instructionPanel = default(GameObject);
    [SerializeField] private GameObject _instructionButton = default(GameObject);

    [SerializeField] private GameObject _startButton = default(GameObject);

    private bool _instructionPanelOn = false;

    private void Start()
    {
	EventSystem.current.SetSelectedGameObject(_startButton);
    }

    public void ToggleInstructions()
    {
	_instructionPanelOn = !_instructionPanelOn;
	_instructionPanel.SetActive(_instructionPanelOn);
	if (_instructionPanelOn)
	{
	    EventSystem.current.SetSelectedGameObject(_instructionButton);
	}
	else
	{
	    EventSystem.current.SetSelectedGameObject(_startButton);
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
