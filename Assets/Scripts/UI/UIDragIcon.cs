using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class UIDragIcon : MonoBehaviour
{
    [SerializeField] private Image icon = null;
    [SerializeField] private TMP_Text tmpText = null;
    public RectTransform rectTransform = null;

    private GameObject buttonRef = null;

    public void Init(GameObject buttonRef, Sprite sprite, string text)
    {
        this.buttonRef = buttonRef;
        icon.sprite = sprite;
        tmpText.text = text;
    }

    public void DestroyButton()
    {
        Destroy(buttonRef);
    }
}