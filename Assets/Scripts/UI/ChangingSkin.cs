using DungTran31.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace DungTran31.UI
{
    public class ChangingSkin : MonoBehaviour
    {
        [SerializeField] private Image squareHeadDisplay;
        [SerializeField] private Sprite sprite1;
        [SerializeField] private Sprite sprite2;
        [SerializeField] private Sprite sprite3;
        private int whatSkin = 1;

        private void Awake()
        {
            whatSkin = PlayerPrefs.GetInt("SelectedSkin", 1);
        }

        public void ChangeSkin(int skinNumber)
        {
            whatSkin = skinNumber;
            UpdateSkin();
            PlayerPrefs.SetInt("SelectedSkin", whatSkin);
        }

        private void UpdateSkin()
        {
            squareHeadDisplay.sprite = whatSkin switch
            {
                1 => sprite1,
                2 => sprite2,
                3 => sprite3,
                _ => sprite1, // Fallback to default skin
            };
        }

        public int GetCurrentSkin()
        {
            return whatSkin;
        }
    }
}