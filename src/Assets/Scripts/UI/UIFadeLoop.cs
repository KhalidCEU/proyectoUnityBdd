using UnityEngine;
using UnityEngine.UI;

public class UIFadeLoop : MonoBehaviour
{
    public Graphic uiElement; // puede ser Image, Text, etc.
    public float fadeDuration = 1f;
    public float waitTime = 0.05f;

    private void Start()
    {
        if (uiElement == null)
            uiElement = GetComponent<Graphic>();

        StartCoroutine(FadeLoop());
    }

    private System.Collections.IEnumerator FadeLoop()
    {
        while (true)
        {
            // Fade out
            yield return StartCoroutine(Fade(1f, 0f));

            yield return new WaitForSeconds(waitTime);

            // Fade in
            yield return StartCoroutine(Fade(0f, 1f));

            yield return new WaitForSeconds(waitTime);
        }
    }

    private System.Collections.IEnumerator Fade(float fromAlpha, float toAlpha)
    {
        float elapsed = 0f;
        Color color = uiElement.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(fromAlpha, toAlpha, elapsed / fadeDuration);
            uiElement.color = new Color(color.r, color.g, color.b, newAlpha);
            yield return null;
        }

        uiElement.color = new Color(color.r, color.g, color.b, toAlpha);
    }
}
