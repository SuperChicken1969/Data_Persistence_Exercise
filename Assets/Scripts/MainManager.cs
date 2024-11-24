using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    public Text highScoreText;
    
    private bool m_Started = false;
    private int m_Points;
    public int highScore;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        ScoreText.text = PlayerManager.Instance.playerName + " " + $"Score : {m_Points}";
        LoadPlayerData();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        highScoreText.text = "High : " + highScore.ToString();
    }

    void AddPoint(int point)
    {
        m_Points += point;
        if(m_Points > highScore)
        {
            highScore = m_Points;
        }
        ScoreText.text = PlayerManager.Instance.playerName + " " + $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        SavePlayerData();
    }

    [System.Serializable]
    class PlayerData
    {
        public string playerName;
        public int highScore;
    }
    void SavePlayerData()
    {
        PlayerData player = new PlayerData();
        player.playerName = PlayerManager.Instance.playerName;
        player.highScore = highScore;

        string json = JsonUtility.ToJson(player);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    void LoadPlayerData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            PlayerData player = JsonUtility.FromJson<PlayerData>(json);
            if(player.highScore > 0)
            {
                highScore = player.highScore;
            }
        }
    }
}
