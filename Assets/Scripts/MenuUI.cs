using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public InputField input;

    private void Start()
    {

    }
    public void SetPlayerName()
    {
        PlayerManager.Instance.playerName = input.textComponent.text;
    }

    public void LoadMainScene()
    {
        SetPlayerName();
        SceneManager.LoadScene(1);
    }
}
