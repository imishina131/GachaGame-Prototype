using System;
using System.Collections;
using MatrixUtils.Attributes;
using PlayFab;
using TMPro;
using UnityEngine;
/// <summary>
/// A box responsible for displaying a <see cref="PlayFabError"/> as plain text to the end user
/// </summary>
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

    void HandleError(string error)
    {
        StartCoroutine(DisplayError(error));
    }
    /// <summary>
    /// Displays the <see cref="PlayFabError"/> in plaintext to the user in this box
    /// </summary>
    /// <param name="error">The <see cref="PlayFabError"/> to display</param>
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
