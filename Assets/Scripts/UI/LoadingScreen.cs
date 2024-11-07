using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    [Space]
    [SerializeField] private Image loadingImage;
    [SerializeField] private TMP_Text loadingText;
    [SerializeField] private float imageRotationSpeedMultiplier = 5;

    public void Init()
    {
        StopLoading();
        gameObject.SetActive(false);
    }

    public void StartLoading()
    {
        canvasGroup.alpha = 1;
    }

    public void StopLoading()
    {
        canvasGroup.alpha = 0;
    }

    private void Update()
    {
        loadingImage.rectTransform.Rotate(0, 0, Time.deltaTime * imageRotationSpeedMultiplier);
    }

    public void ChangeLoadingStatus(float objPercent)
    {
        loadingText.text = objPercent.ToString("F1") + "%";
        
#if UNITY_EDITOR
        Debug.Log($"Loading {loadingText.text}");
#endif
    }
}
