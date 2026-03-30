using DungTran31.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace DungTran31.UI
{
    public class DissolveMainMenu : Singleton<DissolveMainMenu>
    {
        private Material _material;
        private float _fade = 1f;
        private static readonly int DissolveAmount = Shader.PropertyToID("_DissolveAmount");
        private bool _isAppearing; 
        private bool _isVanishing;

        private void Start()
        {
            _material = GetComponent<Image>().material;
        }

        public void SetAppear()
        {
            _isAppearing = true;
            _isVanishing = false;
        }
        
        public void SetVanish()
        {
            _isAppearing = false;
            _isVanishing = true;
        }

        private void Update()
        {
            if (_isAppearing)
            {
                _fade -= Time.deltaTime;

                if (_fade >= 1f)
                {
                    _fade = 1f;
                    _isAppearing = false;
                }

                _material.SetFloat(DissolveAmount, _fade);
            }

            if (_isVanishing)
            {
                _fade += Time.deltaTime;
                
                if (_fade <= 0f)
                {
                    _fade = 0f;
                    _isVanishing = false;
                }

                _material.SetFloat(DissolveAmount, _fade);                
            }

        }
    }
}
