using UnityEngine;

namespace burglar
{
    [CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/New item")]
    public class Item : ScriptableObject
    {
        public string title;
        public string slug;
        public Sprite icon;
        [TextArea(15, 20)]
        public string description;
    }
}
