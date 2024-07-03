using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] Button _Level01;

    void Start()
    {
        _Level01.onClick.AddListener(EnterLevel01);
    }

    private void EnterLevel01()
    {
        ScenesManager.Instance.LoadNewGame();
        //ScenesManager.Instance.LoadScene(ScenesManager.Scene.Level01);
    }
}
