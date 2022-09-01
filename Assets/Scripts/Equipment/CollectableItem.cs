using UnityEngine;

[CreateAssetMenu(fileName = "new CollectableItem", menuName = "ScriptableObjects/CollectableItem")]
public class CollectableItem : ScriptableObject
{
    public string itemName;
    public string target;
    public int charge;
}
