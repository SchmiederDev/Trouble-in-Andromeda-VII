using System.Collections;
using UnityEngine;
using TMPro;

public class FlashText : MonoBehaviour
{
    private TMP_Text flashTxtTMP;    

    [SerializeField]
    private float fadeInSpan = 0.025f;

    [SerializeField]
    private float shortFadeInRate = 0.05f;
    [SerializeField]
    private float longFadeInRate = 0.01f;

    [SerializeField]
    private float fadeOutSpan = 0.025f;

    [SerializeField]
    private float shortFadeOutRate = 0.1f;

    [SerializeField]
    private float longFadeOutRate = 0.05f;

    private float flashTextAlpha;

    public delegate void OnMessageChanged(string message);
    public OnMessageChanged MessageChanged;

    // Start is called before the first frame update
    void Start()
    {
        MessageChanged = Set_FlashText;
        flashTxtTMP = GetComponent<TMP_Text>();
        flashTextAlpha = flashTxtTMP.color.a;
    }

    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(fadeInSpan);

        if (flashTextAlpha <= 1f)
        {
            if (flashTxtTMP.text.Length < 10f)
                flashTextAlpha += shortFadeInRate;
            else
                flashTextAlpha += longFadeInRate;

            flashTxtTMP.color = new Color(flashTxtTMP.color.r, flashTxtTMP.color.g, flashTxtTMP.color.b, flashTextAlpha);
            StartCoroutine(FadeIn());
        }

        else
            StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(fadeOutSpan);

        if(flashTextAlpha > 0f)
        {
            if(flashTxtTMP.text.Length < 10f)
                flashTextAlpha -= shortFadeOutRate;
            else
                flashTextAlpha -= longFadeInRate;

            flashTxtTMP.color = new Color(flashTxtTMP.color.r, flashTxtTMP.color.g, flashTxtTMP.color.b, flashTextAlpha);
            StartCoroutine(FadeOut());
        }
    }

    private void Set_FlashText(string message)
    {
        flashTxtTMP.text = message;        
        StartCoroutine(FadeIn());
    }
}
