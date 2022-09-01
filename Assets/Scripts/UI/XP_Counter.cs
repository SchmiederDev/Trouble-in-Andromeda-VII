using UnityEngine;
using TMPro;

public class XP_Counter : MonoBehaviour
{
    TMP_Text XP_Text;
    // Start is called before the first frame update
    void Awake()
    {
        XP_Text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        TheGame.theGameInst.PlayerUnionFighter.XP_Changed += UpdateXPText;
    }

    private void UpdateXPText()
    {
        XP_Text.text = TheGame.theGameInst.PlayerUnionFighter.Get_XP().ToString();
    }
}
