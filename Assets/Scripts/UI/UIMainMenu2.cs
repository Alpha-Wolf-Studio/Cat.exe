using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UiMainMenu : MonoBehaviour
{

    [SerializeField] private Button btnPlay;
    [SerializeField] private Button btnSettings;
    [SerializeField] private Button btnBackOfSettings;
    [SerializeField] private Button btnCredits;
    [SerializeField] private Button btnBackOfCredits;
    [SerializeField] private Button btnLeadBoard;
    [SerializeField] private Button btnBackOfLeadBoard;

    [SerializeField] private float transitionTime;
    [SerializeField] private CanvasGroup[] menues;
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

    private void Start ()
    {
        Time.timeScale = 1;
        AddAllListeners();
    }

    void AddAllListeners ()
    {
        btnPlay.onClick.AddListener(ButtonPlay);
        btnSettings.onClick.AddListener(ButtonSetting);
        btnCredits.onClick.AddListener(ButtonCredits);
        btnLeadBoard.onClick.AddListener(ButtonLeadBoard);

        btnBackOfSettings.onClick.AddListener(ButtonBackSettings);
        btnBackOfCredits.onClick.AddListener(ButtonBackCredits);
        btnBackOfLeadBoard.onClick.AddListener(ButtonBackLeadBoard);
    }

    void RemoveAllListeners ()
    {
        btnPlay.onClick.RemoveAllListeners();
    }

    public void ButtonPlay () => SceneManagerSingleton.Get().LoadScene(SceneManagerSingleton.SceneIndex.GAMEPLAY);
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
    }
}

enum Menu
{
    Main,
    Settings,
    Credits,
    LeadBoard
}