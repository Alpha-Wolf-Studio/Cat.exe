using UnityEngine;
using UnityEngine.UI;

public class UIMainMenu : MonoBehaviour
{
    
    [SerializeField] private Button playButton = default;

    private void Start()
    {
        playButton.onClick.AddListener(()=>
        {
            SceneManagerSingleton.Get().LoadScene(SceneManagerSingleton.SceneIndex.GAMEPLAY);
        });
    }
}
