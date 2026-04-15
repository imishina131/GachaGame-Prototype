using MatrixUtils.Attributes;
using MatrixUtils.DependencyInjection;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Handles signing up for the service through <see cref="TMP_InputField"/> and sending data to the <see cref="PlayFabClientAPI"/>
/// </summary>
public class SignUpScreenUI : MonoBehaviour
{
    [SerializeField, RequiredField] TMP_InputField m_email;
    [SerializeField, RequiredField] TMP_InputField m_username;
    [SerializeField, RequiredField] TMP_InputField m_password;

    [SerializeField] UnityEvent<PlayFabError> m_onSignUpFailed = new();
    [Inject] ISceneTransitionManager m_sceneTransitionManager;
    /// <summary>
    /// Makes a call to the <see cref="PlayFabClientAPI"/> to <see cref="PlayFabClientAPI.RegisterPlayFabUser"/> using data from the connected <see cref="TMP_InputField"/>
    /// </summary>
    public void OnUserConfirmSignUp()
    {
        PlayFabClientAPI.RegisterPlayFabUser(new()
        {
            Email = m_email.text,
            Username = m_username.text,
            Password = m_password.text,
            
        }, OnSignUpSuccess, m_onSignUpFailed.Invoke);
    }
    void OnSignUpSuccess(RegisterPlayFabUserResult result)
    {
        PlayFabClientAPI.AddOrUpdateContactEmail
        (
            new()
            {
                EmailAddress = m_email.text
            },
            OnEmailUpdateSuccess,
            m_onSignUpFailed.Invoke
       );
    }
    void OnEmailUpdateSuccess(AddOrUpdateContactEmailResult result)
    {
        m_sceneTransitionManager.TransitionToScene("Gacha Scene");
    }
}