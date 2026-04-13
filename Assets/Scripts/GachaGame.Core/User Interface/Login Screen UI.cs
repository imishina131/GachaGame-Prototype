using MatrixUtils.Attributes;
using MatrixUtils.DependencyInjection;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Handles logging into the service through <see cref="TMP_InputField"/> and sending data to the <see cref="PlayFabClientAPI"/>
/// </summary>
public class LoginScreenUI : MonoBehaviour
{
    [SerializeField, RequiredField] TMP_InputField m_username;
    [SerializeField, RequiredField] TMP_InputField m_password;

    [SerializeField] UnityEvent<PlayFabError> m_onSignInFailed = new();
    [Inject] ISceneTransitionManager m_sceneTransitionManager;
    /// <summary>
    /// Makes a call to the <see cref="PlayFabClientAPI"/> to <see cref="PlayFabClientAPI.LoginWithPlayFab"/> using data from the connected <see cref="TMP_InputField"/>
    /// </summary>
    public void OnUserConfirmLogin()
    {
        PlayFabClientAPI.LoginWithPlayFab(new()
        {
            Username = m_username.text,
            Password = m_password.text,
        }
        , OnLogInSuccess, m_onSignInFailed.Invoke);
    }
    void OnLogInSuccess(LoginResult result)
    {
        m_sceneTransitionManager.TransitionToScene("Gacha Scene");
    }
}