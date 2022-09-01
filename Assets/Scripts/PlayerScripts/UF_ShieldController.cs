using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UF_ShieldController : MonoBehaviour
{
    [SerializeField]
    private UnionFighter MySpaceShip;

    [SerializeField]
    private SpriteRenderer ShieldSprite;

    private float ShieldSpriteAlpha = 0f;

    private const float AlphaMin = 0.4f;
    private const float AlphaMax = 1f;

    // Start is called before the first frame update
    void Start()
    {
        MySpaceShip = GetComponentInParent<UnionFighter>();
        ShieldSprite = GetComponent<SpriteRenderer>();
        ShieldSprite.color = new Color(ShieldSprite.color.r, ShieldSprite.color.g, ShieldSprite.color.b, ShieldSpriteAlpha);
        MySpaceShip.ShieldStateChanged += Check_ShieldState;
    }

    private void Check_ShieldState()
    {
        if(MySpaceShip.IsShieldActive)
        {
            float TempAlpha = MySpaceShip.ShieldPower/100f;

            if(TempAlpha >= AlphaMin)
                ShieldSpriteAlpha = TempAlpha;

            else
                ShieldSpriteAlpha = AlphaMin;
        }

        else
            ShieldSpriteAlpha = 0f;

        if(ShieldSpriteAlpha < AlphaMax)
            ShieldSprite.color = new Color(ShieldSprite.color.r, ShieldSprite.color.g, ShieldSprite.color.b, ShieldSpriteAlpha);
        else
            ShieldSprite.color = new Color(ShieldSprite.color.r, ShieldSprite.color.g, ShieldSprite.color.b, AlphaMax);

    }
}
