using UnityEngine;
using burglar.managers;

namespace burglar
{
    public class MainMenu : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Start()
        {
            // Hide HUD
            UIManager.Instance.HideHUD();
        }
    }
}
