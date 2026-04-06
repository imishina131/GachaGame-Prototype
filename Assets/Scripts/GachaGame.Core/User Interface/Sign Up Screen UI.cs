using MatrixUtils.Attributes;
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

    ISceneTransitionManager m_sceneTransitionManager;
    [SerializeField] UnityEvent<PlayFabError> m_onSignUpFailed = new();
    void Start()
    {
        ServiceLocator.Global.Get(out m_sceneTransitionManager);
    }
    
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
        if (ServiceLocator.Global.Get(out PlayerProfile profile))
        {
            profile.PlayFabId =  result.PlayFabId;
            profile.Username = result.Username;
            profile.SessionTicket =  result.SessionTicket;
            profile.EntityToken = result.EntityToken;
        }
        else
        {
            ServiceLocator.Global.Register(new PlayerProfile(result.PlayFabId, result.Username, result.SessionTicket, result.EntityToken));
        }
    }
}