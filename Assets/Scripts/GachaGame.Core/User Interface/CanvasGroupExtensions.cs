using System.Collections;
using UnityEngine;
/// <summary>
/// Extension methods relating to <see cref="CanvasGroup"/>
/// </summary>
public static class CanvasGroupExtensions
{
    /// <summary>
    /// Fades a <see cref="CanvasGroup"/> to a particular opacity over a <see cref="duration"/>
    /// </summary>
    /// <param name="groupToFade">The <see cref="CanvasGroup"/> to execute the fade on</param>
    /// <param name="desiredAlpha">The alpha which this <see cref="CanvasGroup"/> should fade to</param>
    /// <param name="duration">The time over which the fade will occur</param>
    /// <returns>An <see cref="IEnumerator"/> to be executed as a <see cref="Coroutine"/></returns>
    public static IEnumerator FadeToOpacity(this CanvasGroup groupToFade, float desiredAlpha, float duration)
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
