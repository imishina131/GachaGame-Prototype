using MatrixUtils.Attributes;
using MatrixUtils.DependencyInjection;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SignUpScreenUI : MonoBehaviour
{
    [SerializeField, RequiredField] TMP_InputField m_email;
    [SerializeField, RequiredField] TMP_InputField m_username;
    [SerializeField, RequiredField] TMP_InputField m_password;

    [SerializeField] UnityEvent<PlayFabError> m_onSignUpFailed = new();
    [Inject] ISceneTransitionManager m_sceneTransitionManager;
    
    public void OnUserConfirmSignUp()
    {
        PlayFabClientAPI.RegisterPlayFabUser(new()
        {
            Email = m_email.text,
            Username = m_username.text,
            Password = m_password.text
        }, OnSignUpSuccess, m_onSignUpFailed.Invoke);
    }
    void OnSignUpSuccess(RegisterPlayFabUserResult result)
    {
        
    }
}