using MatrixUtils.Attributes;
using MatrixUtils.DependencyInjection;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Handles resetting the user's password through a <see cref="TMP_InputField"/> by sending data to the <see cref="PlayFabClientAPI"/>
/// </summary>
public class ForgotPasswordScreenUI : MonoBehaviour
{
    [SerializeField, RequiredField] TMP_InputField m_email;
    [SerializeField] UnityEvent<PlayFabError> m_onSignInFailed = new();
    [Inject] ISceneTransitionManager m_sceneTransitionManager;
    /// <summary>
    /// Makes a call to the <see cref="PlayFabClientAPI"/> to <see cref="PlayFabClientAPI.SendAccountRecoveryEmail"/> using data from the connected <see cref="TMP_InputField"/>
    /// </summary>
    public void OnUserConfirmRecovery()
    {
        PlayFabClientAPI.SendAccountRecoveryEmail
        (
            new()
            {
                Email = m_email.text,
                TitleId = PlayFabSettings.TitleId
            },
            OnSendRecoverySuccessSuccess,
            m_onSignInFailed.Invoke
        );
    }
    void OnSendRecoverySuccessSuccess(SendAccountRecoveryEmailResult result)
    {
        m_sceneTransitionManager.TransitionToScene("Log In Screen");
    }
}
