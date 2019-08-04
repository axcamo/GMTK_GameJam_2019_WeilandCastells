using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    [SerializeField] private float fadeSpeed = 1f;

    [SerializeField] private TextMeshPro welcomeText;
    [SerializeField] private TextMeshPro controlsText;
    [SerializeField] private TextMeshPro walkText;
    [SerializeField] private GameObject startColumn;

    private void Start()
    {
        StartCoroutine("Tutorial");
    }

    IEnumerator Tutorial()
    {
        StartCoroutine("FadeTextIn", welcomeText);
        yield return new WaitForSeconds(10);
        StartCoroutine("FadeTextOut", welcomeText);

        StartCoroutine("FadeTextIn", controlsText);
        yield return new WaitForSeconds(12);
        StartCoroutine("FadeTextOut", controlsText);

        startColumn.SetActive(true);

        StartCoroutine("FadeTextIn", walkText);
        yield return new WaitForSeconds(12);
        StartCoroutine("FadeTextOut", walkText);
    }

    IEnumerator FadeTextIn(TextMeshPro text)
    {
        float alpha = 0;
        while (text.color.a < 1f)
        {
            alpha += fadeSpeed * Time.deltaTime;
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            yield return null;
        }
    }

    IEnumerator FadeTextOut(TextMeshPro text)
    {
        float alpha = 1;
        while (text.color.a > 0f)
        {
            alpha -= fadeSpeed * Time.deltaTime;
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            yield return null;
        }
    }
}
