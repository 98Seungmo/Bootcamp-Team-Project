using UnityEngine;

namespace HJ
{
    /// <summary>
    /// IInteractable 인터페이스를 테스트하기 위해 만든 클래스
    /// </summary>
    public class InteractableDummy : MonoBehaviour, IInteractable
    {
        [SerializeField] GameObject GreenLight;

        public void InteractableOn()
        {
            GreenLight.SetActive(true);
        }

        public void InteractableOff()
        {
            GreenLight.SetActive(false);
        }

        public void Interaction(GameObject interactor)
        {
            Debug.Log("꺼져");
        }
    }
}
