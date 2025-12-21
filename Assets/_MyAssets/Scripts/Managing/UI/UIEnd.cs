using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIEnd : MonoBehaviour
{
    [SerializeField] private GameObject _startButton = default(GameObject);


    private void Start()
    {
	EventSystem.current.SetSelectedGameObject(_startButton);
    }

    public void Replay()
    {
	SceneManager.LoadScene(1);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(2);
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
