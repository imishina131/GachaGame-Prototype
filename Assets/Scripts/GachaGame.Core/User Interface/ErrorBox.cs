using System;
using System.Collections;
using MatrixUtils.Attributes;
using PlayFab;
using TMPro;
using UnityEngine;
[RequireComponent(typeof(CanvasGroup))]
public class ErrorBox : MonoBehaviour
{
    bool m_isDisplayed;
    [SerializeField, RequiredField] TMP_Text m_errorText; 
    CanvasGroup m_canvasGroup;
    void Awake()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();
        m_canvasGroup.alpha = 0;
        m_canvasGroup.interactable = false;
        m_canvasGroup.blocksRaycasts = false;
    }
    public void HandleError(string error)
    {
        StartCoroutine(DisplayError(error));
    }
    public void HandleError(PlayFabError error)
    {
        HandleError(error.GenerateErrorReport());
    }
    IEnumerator DisplayError(string error)
    {
        if (m_isDisplayed)
        {
            yield return m_canvasGroup.FadeToOpacity(0, 0.5f);
        }
        m_isDisplayed = true;
        m_errorText.text = error;
        m_canvasGroup.interactable = true;
        m_canvasGroup.blocksRaycasts = true;
        yield return m_canvasGroup.FadeToOpacity(1, 0.5f);
    }
}
