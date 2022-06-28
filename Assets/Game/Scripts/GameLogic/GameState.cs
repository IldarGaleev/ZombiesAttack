using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameState: MonoBehaviour
{

    private Text _scoreUI;
    private AudioSource _backgroundMusic;

    #region UI Components
    [Header("UI Components")]    
    
    [Tooltip("������ ���������� UI")]
    public GameObject uiArea;

    [Tooltip("������ ������� \"���� ��������\"")]
    public GameObject gameOverDialogPrefub;

    [Tooltip("������ ������� \"���������\"")]
    public GameObject prefencesDialogPrefub;
    #endregion

    #region Game state
    [Header("Game state")]

    [SerializeField]
    [Tooltip("����� ������������ �����")]
    private int _score = 0;

    [SerializeField]
    [Tooltip("������� ������ �����")]
    private float _zombieSpawnInterval = 2.0f;
    #endregion

    #region Game tune
    [Header("Game tune")]

    [Tooltip("�������� ���������� ���������")]
    public float increasingComplexityInterval = 10.0f;

    [Tooltip("����������� ����� ������ �����")]
    public float minZombieSpawnInterval = 0.5f;

    [Tooltip("�������� �� ������� ����� ����������� ����� ������ �����")]
    public float zombieSpawnIntervalDecreaseValue = 0.1f;
    #endregion


    public int BestScore
    {
        get
        {
            return PlayerPrefs.GetInt(nameof(BestScore));
        }
    }

    public int Score {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
            if(_score > BestScore)
            {
                PlayerPrefs.SetInt(nameof(BestScore), _score);
            }
            PlayerPrefs.SetInt(nameof(Score), _score);
            PlayerPrefs.Save();
        }
    }

    
    public float ZombieSpawnInterval {
        get
        {
            return _zombieSpawnInterval;
        }
        set
        {
            _zombieSpawnInterval = value;
            PlayerPrefs.SetFloat(nameof(ZombieSpawnInterval), _zombieSpawnInterval);
            PlayerPrefs.Save();
        }
    }

    private void Start()
    {
        _scoreUI = uiArea.transform.Find("Score").GetComponent<Text>();
        _backgroundMusic = GetComponentInChildren<AudioSource>();       

        InvokeRepeating(nameof(IncreaseingComplexity),increasingComplexityInterval, increasingComplexityInterval);
    }

    private void Update()
    {
        _scoreUI.text = $"Score: {_score}";   
    }

    private void IncreaseingComplexity()
    {
        if (ZombieSpawnInterval > minZombieSpawnInterval)
        {
            float newSpawnInterval = ZombieSpawnInterval - zombieSpawnIntervalDecreaseValue;
            ZombieSpawnInterval = (newSpawnInterval > minZombieSpawnInterval) ? newSpawnInterval : minZombieSpawnInterval;
            
            Debug.Log($"New zombie spawn interval: {ZombieSpawnInterval}");
        }

        if(ZombieSpawnInterval == minZombieSpawnInterval)
        {
            CancelInvoke(nameof(IncreaseingComplexity));
            
            Debug.Log("Complexity increasing cancelled");
        }
    }


    public void GameExit()
    {
        Application.Quit();
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        _backgroundMusic.Stop();
        Instantiate(gameOverDialogPrefub, uiArea.transform);
        GameObject.Find("DialogScore").GetComponent<Text>().text = $"Score: {Score}";

        GameObject.Find("NewGameBtn").GetComponent<Button>().onClick.AddListener(() => { LoadScene("MainScene"); });
        GameObject.Find("MainMenuBtn").GetComponent<Button>().onClick.AddListener(() => { LoadScene("MainMenu"); });
    }

    public void OpenPrefences()
    {
        Time.timeScale = 0;
        GameObject dialog = Instantiate(prefencesDialogPrefub, uiArea.transform);

        Button closeButton = GameObject.Find("ClosePrefencesBtn").GetComponent<Button>();

        closeButton.onClick.AddListener(() => { 
            Destroy(dialog);
            Time.timeScale = 1;
        });
    }
}
