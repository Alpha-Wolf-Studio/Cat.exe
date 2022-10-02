using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class UIMainMenu : MonoBehaviour
{

    [SerializeField] private UiButtonEffect btnPlay;
    [SerializeField] private UiButtonEffect btnSettings;
    [SerializeField] private UiButtonEffect btnCredits;
    [SerializeField] private UiButtonEffect btnBackOfCredits;
    [SerializeField] private UiButtonEffect btnLeadBoard;
    [SerializeField] private UiButtonEffect btnBackOfLeadBoard;

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
        btnPlay.AddBehaviours(ButtonPlay);
        btnSettings.AddBehaviours(ButtonSetting);
        btnCredits.AddBehaviours(ButtonCredits);
        btnLeadBoard.AddBehaviours(ButtonLeadBoard);

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

    public void ButtonBackSettings () => StartCoroutine(SwitchPanel(transitionTime, (int) Menu.Main, (int) Menu.Settings));
    public void ButtonBackCredits () => StartCoroutine(SwitchPanel(transitionTime, (int) Menu.Main, (int) Menu.Credits));
    public void ButtonBackLeadBoard () => StartCoroutine(SwitchPanel(transitionTime, (int) Menu.Main, (int) Menu.LeadBoard));


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