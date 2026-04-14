using System.Collections;
using UnityEngine;

public class RollResultDisplay : MonoBehaviour
{
    [SerializeField] CanvasGroup m_rollResultVisuals;
    [SerializeField] float m_displayTime = 1f;
    readonly RoutineQueue m_rollQueue = new();
    GameObject m_rollResultDisplay;
    void Awake()
    {
        m_rollResultVisuals.alpha = 0;
        m_rollQueue.Initialize(this);
    }
    public void DisplayRollResult(CharacterData characterData)
    {
        m_rollQueue.QueueRoutine(DisplayResultAsync(characterData));
    }
    IEnumerator DisplayResultAsync(CharacterData characterData)
    {
        m_rollResultDisplay = Instantiate(characterData.Prefab, transform);
        yield return m_rollResultVisuals.FadeToOpacity(1, 0.5f);
        yield return new WaitForSeconds(m_displayTime);
        yield return m_rollResultVisuals.FadeToOpacity(0, 0.5f);
        Destroy(m_rollResultDisplay);
    }
}
