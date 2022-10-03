using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerSingleton : MonoBehaviourSingleton<SceneManagerSingleton>
{
    [SerializeField] private Animator animator;
    [SerializeField] private float fadeAnimationSpeed = 1;
    [SerializeField] private float overlayAnimationSpeed = 1;
    [SerializeField] private float minLoadTime = 2f;

    private string FADE_ANIMATION_SPEED = "FadeSpeed";
    private string OVERLAY_ANIMATION_SPEED = "OverlaySpeed";
    private string FADE_OUT = "FadeOut";
    private string ZOOM_IN = "ZoomIn";
    
    public enum SceneIndex { MAIN_MENU, GAMEPLAY } 

    private SceneIndex sceneIndex;

    public void LoadScene(SceneIndex scene)
    {
        animator.SetFloat(FADE_ANIMATION_SPEED, fadeAnimationSpeed);
        animator.SetFloat(OVERLAY_ANIMATION_SPEED, overlayAnimationSpeed);
        animator.SetBool(FADE_OUT, true);
        
        sceneIndex = scene;
        FadeScreen();
    }

    public void ChangeZoomState(bool zoomIn) => animator.SetBool(ZOOM_IN, zoomIn);
    
    private void FadeScreen()
    {
        StartCoroutine(FadeInCoroutine());
    }

    private IEnumerator FadeInCoroutine()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync((int)sceneIndex);
        operation.allowSceneActivation = false;
        animator.SetBool("FadeOut", true);
        float currentLoadTime = minLoadTime;
        while (operation.progress < 0.9f || currentLoadTime > 0)
        {
            currentLoadTime -= Time.deltaTime;
            yield return null;
        }
        animator.SetBool("FadeOut", false);
        operation.allowSceneActivation = true;
    }
}
