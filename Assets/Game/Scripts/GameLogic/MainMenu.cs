using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private Text _bestResultUIText;

    [Tooltip("Регион размещения UI")]
    public GameObject uiArea;

    [Tooltip("Префаб диалога \"Настройки\"")]
    public GameObject prefencesDialogPrefub;

    private void Start()
    {
        _bestResultUIText = uiArea.transform.Find("General/BestResult/BestResultText").GetComponent<Text>();
        _bestResultUIText.text = $"Best score: {PlayerPrefs.GetInt("BestScore")}";
    }

    public void LoadScene(string sceneName)
    {        
        SceneManager.LoadScene(sceneName);
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void OpenPrefences()
    {
        GameObject dialog = Instantiate(prefencesDialogPrefub, uiArea.transform);
        Button closeButton = GameObject.Find("ClosePrefencesBtn").GetComponent<Button>();

        closeButton.onClick.AddListener(() => {
            Destroy(dialog);
        });
    }
    
}
