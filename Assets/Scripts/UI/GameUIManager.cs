using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    #region Singleton

    private static GameUIManager _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public static GameUIManager Instance => _instance;

    #endregion

    [SerializeField] private LoadingScreen inGameLoadingScreen;
    [SerializeField] private LoadingScreen menuLoadingScreen;

    private void Start()
    {
        inGameLoadingScreen.Init();
        //menuLoadingScreen.Init();
    }

    public void ToggleInGameLoading(bool isLoading)
    {
        inGameLoadingScreen.gameObject.SetActive(isLoading);

        if (isLoading) inGameLoadingScreen.StartLoading();
        else inGameLoadingScreen.StopLoading();
    }

    public void ChangeLoadingStatus(float percent)
    {
        inGameLoadingScreen.ChangeLoadingStatus(percent * 100);
    }
}