using System.Collections;
using JetBrains.Annotations;
using MatrixUtils.DependencyInjection;
using UnityEngine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(CanvasGroup))]
public class SceneTransitionManager : PersistentService<ISceneTransitionManager>, ISceneTransitionManager
{
    [Provide, UsedImplicitly] ISceneTransitionManager GetSceneTransitionManager() => this;
    public bool IsTransitioning { get; private set;}
    CanvasGroup m_canvasGroup;

    void Awake()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();
        m_canvasGroup.alpha = 0;
    }
    public void TransitionToScene(string sceneName)
    {
        if(IsTransitioning) return;
        StartCoroutine(TransitionToSceneAsync(sceneName));
    }
    IEnumerator TransitionToSceneAsync(string sceneName)
    {
        IsTransitioning = true;
        m_canvasGroup.blocksRaycasts = true;
        m_canvasGroup.interactable = true;
        yield return FadeCanvasGroupAsync(m_canvasGroup, 1, 0.5f);
        yield return SceneManager.LoadSceneAsync(sceneName);
        yield return FadeCanvasGroupAsync(m_canvasGroup, 0, 0.5f);
        m_canvasGroup.blocksRaycasts = false;
        m_canvasGroup.interactable = false;
        IsTransitioning = false;
    }
    static IEnumerator FadeCanvasGroupAsync(CanvasGroup groupToFade, float desiredAlpha, float duration)
    {
        float elapsed = 0;
        float startAlpha = groupToFade.alpha;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            groupToFade.alpha = Mathf.Lerp(startAlpha, desiredAlpha, elapsed / duration);
            yield return null;
        }
    }
}
