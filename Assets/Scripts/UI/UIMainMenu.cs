using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private UiButtonEffect btnBackground;
    [SerializeField] private UiButtonEffect btnPlay;
    [SerializeField] private UiButtonEffect btnSettings;
    [SerializeField] private UiButtonEffect btnCredits;
    [SerializeField] private UiButtonEffect btnBackOfCredits;
    [SerializeField] private UiButtonEffect btnLeadBoard;
    [SerializeField] private UiButtonEffect btnBackOfLeadBoard;
    [SerializeField] private UiButtonEffect btnAlphaWolf;

    [SerializeField] private float transitionTime;
    [SerializeField] private CanvasGroup[] menues;
    [SerializeField] private PlayableDirector playableDirector;
    private Menu menu = Menu.Main;

    private void Awake ()
    {
        foreach (CanvasGroup menu in menues)
        {
            menu.blocksRaycasts = false;
            menu.interactable = false;
            menu.alpha = 0;
        }

        menues[(int) Menu.Main].interactable = true;
        menues[(int) Menu.Main].blocksRaycasts = true;
        menues[(int) Menu.Main].alpha = 1;
    }

    private void Start()
    {
        Time.timeScale = 1;
        AddAllListeners();
    }

    private void OnDestroy()
    {
        RemoveAllListeners();
    }

    void AddAllListeners ()
    {
        btnBackground.AddBehaviours(OffSettings);

        btnPlay.AddBehaviours(ButtonPlay);
        btnSettings.AddBehaviours(ButtonSetting);
        btnCredits.AddBehaviours(ButtonCredits);
        btnLeadBoard.AddBehaviours(ButtonLeadBoard);
        btnAlphaWolf.AddBehaviours(ButtonAlphaWolf);

        btnBackOfCredits.AddBehaviours(ButtonBackCredits);
        btnBackOfLeadBoard.AddBehaviours(ButtonBackLeadBoard);
    }

    void RemoveAllListeners ()
    {

    }

    public void ButtonPlay()
    {
        SceneManagerSingleton.Get().LoadScene(SceneManagerSingleton.SceneIndex.GAMEPLAY, true);
        playableDirector.Play();
    }
    public void ButtonSetting () => StartCoroutine(SwitchPanel(transitionTime, (int) Menu.Settings, (int) Menu.Main));
    public void ButtonCredits () => StartCoroutine(SwitchPanel(transitionTime, (int) Menu.Credits, (int) Menu.Main));
    public void ButtonLeadBoard () => StartCoroutine(SwitchPanel(transitionTime, (int) Menu.LeadBoard, (int) Menu.Main));
    public void ButtonAlphaWolf() => Application.OpenURL("https://alphawolfstudiogam.wixsite.com/home");

    public void ButtonBackSettings () => StartCoroutine(SwitchPanel(transitionTime, (int) Menu.Main, (int) Menu.Settings));
    public void ButtonBackCredits () => StartCoroutine(SwitchPanel(transitionTime, (int) Menu.Main, (int) Menu.Credits));
    public void ButtonBackLeadBoard () => StartCoroutine(SwitchPanel(transitionTime, (int) Menu.Main, (int) Menu.LeadBoard));
    public void OffSettings() => StartCoroutine(OffPanel(transitionTime, (int)Menu.Settings));

    IEnumerator OffPanel (float maxTime, int offMenu)
    {
        float onTime = 0;
        CanvasGroup off = menues[offMenu];

        off.blocksRaycasts = false;
        off.interactable = false;

        while (onTime < maxTime)
        {
            onTime += Time.deltaTime;
            float fade = onTime / maxTime;
            off.alpha = 1 - fade;
            yield return null;
        }

        off.gameObject.SetActive(false);
    }

    IEnumerator SwitchPanel (float maxTime, int onMenu, int offMenu)
    {
        float onTime = 0;
        CanvasGroup on = menues[onMenu];
        CanvasGroup off = menues[offMenu];

        off.blocksRaycasts = false;
        off.interactable = false;
        
        on.gameObject.SetActive(true);

        while (onTime < maxTime)
        {
            onTime += Time.deltaTime;
            float fade = onTime / maxTime;
            on.alpha = fade;
            off.alpha = 1 - fade;
            yield return null;
        }

        on.blocksRaycasts = true;
        on.interactable = true;
        onTime = 0;

        menu = (Menu) onMenu;
        
        off.gameObject.SetActive(false);
    }
}

enum Menu
{
    Main,
    Settings,
    Credits,
    LeadBoard
}