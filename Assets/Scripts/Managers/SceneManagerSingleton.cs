using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerSingleton : MonoBehaviourSingleton<SceneManagerSingleton>
{
    [SerializeField] private Animator animator;
    [SerializeField] private float fadeSpeed = 1;
    [SerializeField] private float minLoadTime = 2f;

    public enum SceneIndex { MAIN_MENU, GAMEPLAY } 

    private SceneIndex sceneIndex;

    public void LoadScene(SceneIndex scene)
    {
        animator.SetFloat("Speed", fadeSpeed);
        animator.SetBool("FadeOut", true);
        sceneIndex = scene;
        FadeScreen();
    }

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
