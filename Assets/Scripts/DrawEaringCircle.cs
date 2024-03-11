using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace burglar
{
    public class DrawEaringCircle : MonoBehaviour
    {
        private Agent _agent;

        private SpriteRenderer _spriteRenderer;
        
        // Start is called before the first frame update
        void Start()
        {
            _agent = GetComponentInParent<Agent>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            // _spriteRenderer.material.SetFloat("_Radius", _agent._earingDistance);
        }
    }
}
