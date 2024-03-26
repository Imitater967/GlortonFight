using UnityEngine;

namespace Script.MVC
{
    public class MvcController : MonoBehaviour
    {
        public virtual void Hide()
        { 
            if(gameObject!=null)
                gameObject.SetActive(false);
        }
        
        public virtual void Show()
        {
            if(gameObject!=null)
                gameObject.SetActive(true);
        }
    }
}