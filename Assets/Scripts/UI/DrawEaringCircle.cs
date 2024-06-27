using UnityEngine;
using burglar.agent;

namespace burglar.UI
{
    public class DrawEaringCircle : MonoBehaviour
    {
        private Agent _agent;

        private SpriteRenderer _spriteRenderer;
        
        // Start is called before the first frame update
        private void Start()
        {
            _agent = GetComponentInParent<Agent>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            // _spriteRenderer.material.SetFloat("_Radius", _agent._earingDistance);
        }
    }
}
