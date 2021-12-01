using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SceneTransition
{ 
    public class SceneTransition : MonoBehaviour
    {
        private static SceneTransition _instance;
        private static bool _playCloseAnimation = false;
        private AsyncOperation _loadSceneAsync;
        private Animator _animator;
        public Slider loadSlider;
 
        public static void SwitchScene(string sceneName)
        {
            GetInstance()._animator.SetTrigger("SceneOpen");
            GetInstance()._loadSceneAsync = SceneManager.LoadSceneAsync(sceneName);
            GetInstance()._loadSceneAsync.allowSceneActivation = false;
        }

        private static SceneTransition GetInstance()
        {
            if (_instance == null)
            {
                throw new Exception("Loading screen is null");
            }

            return _instance;
        }

        public void OnAnimationOver()
        {
            _playCloseAnimation = true;
            GetInstance()._loadSceneAsync.allowSceneActivation = true;
        }

        private void Start()
        {
            _instance = this;
            _instance._animator = GetComponent<Animator>();
            if (_playCloseAnimation)
            {
                GetInstance()._animator.SetTrigger("TransitionClose");
            }
        }

        private void Update()
        {
            if (_loadSceneAsync != null)
            {
                loadSlider.value = _loadSceneAsync.progress;
            }
        }
    }
}