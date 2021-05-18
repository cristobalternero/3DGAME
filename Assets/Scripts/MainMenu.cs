using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Manager
{
    public class MainMenu : MonoBehaviour
    {
        Animator cameraObject;

        [Space]
        public bool isMobile = false;
        public string sceneName = ""; // Load Scene, the name of the scene in the build settings that will load

        [Header("Panels")]
        public GameObject[] menus;

        [Space]
        public GameObject mainCanvas; // The UI Panel who parenting all sub menus

        [Header("Setting Panels")] //Setting Panels
        public GameObject PanelControls;
        public GameObject PanelGame;

        [Header("SFX")]
        public AudioSource hoverSound;
        public AudioSource sliderSound;
        public AudioSource swooshSound;

        [Header("LOADING SCREEN")]
        public GameObject loadingMenu;
        public Slider loadBar;
        public TMP_Text finishedLoadingText;

        public static float volume = 0.3f;

        private void Start()
        {
            cameraObject = transform.GetComponent<Animator>();
            
            DisableMenus();
            
            if (isMobile)
            {
                menus[0].SetActive(true);
            }
        }

        public void OpenMenu(int menuIndex)
        {
            DisableMenus();
            menus[menuIndex].SetActive(true);
        }

        public void NewGame()
        {
            if(sceneName != "")
            {
                //SceneManager.LoadScene(sceneName);
                StartCoroutine(LoadAsynchronously(sceneName));
            }
        }

        public void Position2()
        {
            DisableMenus();
            cameraObject.SetFloat("Animate", 1);
        }

        public void Position1()
        {
            cameraObject.SetFloat("Animate", 0);
        }

        void DisablePanels()
        {
            PanelControls.SetActive(false);
            PanelGame.SetActive(false);
        }

        public void DisableMenus()
        {
            for (int i = 0; i < menus.Length; i++)
            {
                menus[i].SetActive(false);
            }
        }

        public void GamePanel()
        {
            DisablePanels();
            PanelGame.SetActive(true);
        }

        public void ControlsPanel()
        {
            DisablePanels();
            PanelControls.SetActive(true);
        }

        public void PlayHover()
        {
            hoverSound.Play();
        }

        public void PlaySFXHover()
        {
            sliderSound.Play();
        }

        public void PlaySwoosh()
        {
            swooshSound.Play();
        }

        public void QuitGame()
        {
            #if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
			#else
				Application.Quit();
			#endif
        }

        //Debug.Log("Quit!");
        //Application.Quit();


        IEnumerator LoadAsynchronously(string sceneName)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = false;
            mainCanvas.SetActive(false);
            loadingMenu.SetActive(true);

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / .9f);
                loadBar.value = progress;

                if (operation.progress >= 0.9f)
                {
                    finishedLoadingText.gameObject.SetActive(true);

                    if (Input.anyKeyDown)
                    {
                        operation.allowSceneActivation = true;
                    }
                }

                yield return null;
            }
        }

        public void ChangeLoudness(float value)
        {
            foreach (AudioSource audioSource in GameObject.FindObjectsOfType<AudioSource>())
            {
                audioSource.volume = value;
            }

        }

    }

}
