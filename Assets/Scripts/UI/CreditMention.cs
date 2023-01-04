using System.Collections;
using UnityEngine;
using TMPro;

public class CreditMention : MonoBehaviour
{
    [SerializeField]
    Transform CreditTransform;
    TMP_Text CreditTxt;

    [SerializeField]
    float movementSpeed = 2f;

    [SerializeField]
    float maxYPos = 10f;

    [SerializeField]
    float fadeOutRate = 2f;

    [SerializeField]
    float shrinkRate = 0.35f;

    [SerializeField]
    float timeStep = 0.01f;

    float alpha = 1f;
    float alphaMin = 0;

    bool shouldFade = false;

    // Start is called before the first frame update
    void Start()
    {
        CreditTransform = GetComponent<Transform>();
        CreditTxt = GetComponentInChildren<TMP_Text>();
        StartCoroutine(FadeOut());
    }

    // Update is called once per frame
    void Update()
    {
        CheckYPos();
    }

    private void FixedUpdate()
    {        
        CreditTransform.Translate(0, movementSpeed * Time.deltaTime, 0);        
    }

    private void CheckYPos()
    {
        float YPos = CreditTransform.position.y;

        if (YPos > 0)
            shouldFade = true;

        if (YPos >= maxYPos)
            Destroy(gameObject);
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(timeStep);

        CreditTxt.fontSize -= shrinkRate * Time.deltaTime;

        if (shouldFade)
        {
            alpha -= fadeOutRate * Time.deltaTime;

            if (alpha > alphaMin)
            {
                CreditTxt.color = new Color(CreditTxt.color.r, CreditTxt.color.g, CreditTxt.color.b, alpha);                
            }
        }

        StartCoroutine(FadeOut());
    }
}
