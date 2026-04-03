using MatrixUtils.Attributes;
using MatrixUtils.DependencyInjection;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SignUpScreenUI : MonoBehaviour
{
    [SerializeField, RequiredField] FieldData m_email;
    [SerializeField, RequiredField] FieldData m_username;
    [SerializeField, RequiredField] FieldData m_password;

    [SerializeField] UnityEvent<PlayFabError> m_onSignUpFailed = new();
    [Inject] ISceneTransitionManager m_sceneTransitionManager;
    
    public void OnUserConfirmSignUp()
    {
        if (!m_email.CheckFieldValidity() || !m_username.CheckFieldValidity() || !m_password.CheckFieldValidity()) return;
        PlayFabClientAPI.RegisterPlayFabUser(new()
        {
            Email = m_email.InputField.text,
            Username = m_username.InputField.text,
            Password = m_password.InputField.text
        }, OnSignUpSuccess, m_onSignUpFailed.Invoke);
    }
    void OnSignUpSuccess(RegisterPlayFabUserResult result)
    {
        
    }
}