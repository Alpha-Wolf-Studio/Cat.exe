using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UiButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler,
                                             IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public event Action OnMouseEnter;
    public event Action OnMouseExit;
    public event Action OnMouseClick;
    public event Action OnMouseDoubleClick;
    public event Action OnIconTrash;

    [Header("RayCast Collision:")] 
    [Tooltip("Chequea Alphas en el raycast. Modificar el Read/Write Enabled en la imagen si éste es true.")] 
    [SerializeField] private bool modifyHitBox;
    [SerializeField] private float alphaRayCast = 0.1f;

    [Header("Effect Scale:")] 
    [SerializeField] private float scaleSpeed = 3;
    [SerializeField] private float scaleLimit = 1.2f;
    private bool increment = false;
    private Vector3 initialScale;
    private Vector3 scale;

    [Header("Effect Image:")]
    [SerializeField] private bool modifyImage;
    [SerializeField] private Image currentImage;
    [SerializeField] private Sprite imageDefault;
    [SerializeField] private Sprite imageHighlighted;

    [Header("Effect Color Text:")] 
    [SerializeField] private bool textHighlight;
    [SerializeField] private TextMeshProUGUI textToHighlight;
    [SerializeField] private Color colorHighlight;
    private Color colorNormal;

    [Header("Other:")] 
    [SerializeField] private bool enableObject;
    [SerializeField] private GameObject objectToEnable;

    [Header("Effect Drag Icon:")] 
    [SerializeField] private bool isDraggeable;

    [SerializeField] private bool isTrash;
    [SerializeField] private bool isTrasheable = true;
    [SerializeField] private GameObject dragIconPrefab;
    [SerializeField] private GameObject emptyPrefab;

    private Canvas canvas;
    private RectTransform holder;
    private UIDragIcon iconDraggeable;

    /// Double click
    private float timeBetweenClick = 0.5f;
    private bool isTimeCheckAllowed = true;
    private float firstClickTime;
    private int totalClicks;

    public bool IsDraggeable => isDraggeable;

    private void Awake ()
    {
        increment = false;
        initialScale = transform.localScale;

        if (modifyHitBox)
            GetComponent<Image>().alphaHitTestMinimumThreshold = alphaRayCast;

        if (modifyImage)
            currentImage = GetComponent<Image>();

        if (enableObject)
        {
            if (!objectToEnable)
            {
                Debug.LogWarning("No tiene un objeto asignado.", gameObject);
                enableObject = false;
            }
        }

        if (textHighlight)
        {
            if (!textToHighlight)
            {
                textHighlight = false;
                Debug.Log("No tiene asignado un Text.", gameObject);
            }
            else
                colorNormal = textToHighlight.color;
        }

        canvas = FindObjectOfType<Canvas>();
        holder = GetComponent<RectTransform>();
    }

    private void OnEnable ()
    {
        transform.localScale = initialScale;
        increment = false;
    }

    private void Update ()
    {
        ChangeScale();
    }

    private void OnDestroy ()
    {
        RemoveBehaviours();
    }

    public void AddBehaviours (Action onClick = null, Action onEnter = null, Action onExit = null, Action onTrash = null)
    {
        if (onClick != null)
            OnMouseClick += onClick;
        if (onEnter != null)
            OnMouseEnter += onEnter;
        if (onExit != null)
            OnMouseExit += onExit;
        if (onTrash != null)
            OnIconTrash += onTrash;
    }

    public void AddDoubleClick (Action onDoubleClick)
    {
        OnMouseDoubleClick += onDoubleClick;
    }

    private void RemoveBehaviours ()
    {
        OnMouseClick = null;
        OnMouseEnter = null;
        OnMouseExit = null;
    }

    public void Trash ()
    {
        gameObject.SetActive(false);
        OnIconTrash?.Invoke();
    }

    private void ChangeScale ()
    {
        float timeStep = scaleSpeed * Time.unscaledDeltaTime;
        scale = transform.localScale;
        if (increment)
        {
            if (transform.localScale.x < scaleLimit)
            {
                scale = new Vector3(scale.x + timeStep, scale.y + timeStep, scale.z + timeStep);
                transform.localScale = scale;
            }
            else
            {
                transform.localScale = new Vector3(scaleLimit, scaleLimit, scaleLimit);
            }
        }
        else
        {
            if (transform.localScale.x > initialScale.x)
            {
                scale = new Vector3(scale.x - timeStep, scale.y - timeStep, scale.z - timeStep);
                transform.localScale = scale;
            }
            else
            {
                transform.localScale = new Vector3(initialScale.x, initialScale.y, initialScale.z);
            }
        }
    }

    public void OnMouseEnterButton ()
    {
        OnMouseEnter?.Invoke();
        increment = true;

        if (modifyImage)
            currentImage.sprite = imageHighlighted;

        if (enableObject)
            objectToEnable.SetActive(true);

        if (textHighlight)
            textToHighlight.color = colorHighlight;
    }

    public void OnMouseExitButton ()
    {
        OnMouseExit?.Invoke();
        increment = false;

        if (modifyImage)
            currentImage.sprite = imageDefault;

        if (enableObject)
            objectToEnable.SetActive(false);

        if (textHighlight)
            textToHighlight.color = colorNormal;
    }

    public void OnPointerEnter (PointerEventData eventData)
    {
        OnMouseEnterButton();
    }

    public void OnPointerExit (PointerEventData eventData)
    {
        OnMouseExitButton();
    }

    public void OnPointerClick (PointerEventData eventData)
    {
        if (OnMouseDoubleClick == null)
        {
            OnMouseClick?.Invoke();
            return;
        }

        if (totalClicks == 0)
        {
            totalClicks++;
            StartCoroutine(CheckDoubleClick());
        }
        else if (totalClicks >0)
        {
            totalClicks++;
            firstClickTime = Time.time;
        }
    }

    IEnumerator CheckDoubleClick ()
    {
        isTimeCheckAllowed = false;
        bool isDoubleClick = false;
        float timing = 0;

        while (timing < timeBetweenClick)
        {
            timing += Time.deltaTime;
            if (totalClicks == 2)
            {
                OnMouseDoubleClick?.Invoke();
                isDoubleClick = true;
                break;
            }

            yield return null;
        }

        if (isDoubleClick)
        {
            Debug.Log("DobleClick.");
            OnMouseDoubleClick?.Invoke();
        }
        else
        {
            Debug.Log("UnClick.");
            OnMouseClick?.Invoke();
        }

        totalClicks = 0;
        isTimeCheckAllowed = true;
    }

    public void OnBeginDrag (PointerEventData eventData)
    {
        if (dragIconPrefab != null && isDraggeable)
        {
            iconDraggeable = Instantiate(dragIconPrefab, holder).GetComponent<UIDragIcon>();
            iconDraggeable.rectTransform.anchoredPosition = eventData.delta;

            iconDraggeable.Init(gameObject, currentImage?.sprite, textToHighlight?.text);
        }
    }

    public void OnDrag (PointerEventData eventData)
    {
        if (iconDraggeable != null)
        {
            iconDraggeable.rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnEndDrag (PointerEventData eventData)
    {
        if (iconDraggeable != null)
        {
            Destroy(iconDraggeable.gameObject);
        }
    }

    public void OnDrop (PointerEventData eventData)
    {
        if (!isTrash) return;

        if (eventData.pointerDrag != null)
        {
            UiButtonEffect uiButtonDrag = eventData.pointerDrag.GetComponent<UiButtonEffect>();
            if (uiButtonDrag != null && uiButtonDrag.isTrasheable)
            {
                int index = uiButtonDrag.transform.GetSiblingIndex();
                uiButtonDrag.Trash();

                GameObject emptyGO = Instantiate(emptyPrefab, holder.parent);
                emptyGO.transform.SetSiblingIndex(index);
            }
        }
    }
}