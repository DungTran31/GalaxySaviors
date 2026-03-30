using UnityEngine;

namespace DungTran31.UI
{
    public class BulletTypeUI : MonoBehaviour
    {
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void UpdateBulletTypeUI(int index)
        {
            animator.SetInteger("BulletType", index);
        }
    }
}
