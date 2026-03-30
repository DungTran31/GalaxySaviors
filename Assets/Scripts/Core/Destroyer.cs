using UnityEngine;

namespace DungTran31.Core
{
    public class Destroyer : MonoBehaviour
    {
        [SerializeField] private float destroyTime = 2f;
        private void Start()
        {
        	Destroy(gameObject, destroyTime);
        }
    }
}
