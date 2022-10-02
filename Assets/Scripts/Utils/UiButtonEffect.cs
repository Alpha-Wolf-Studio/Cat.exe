﻿using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UiButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public event Action OnMouseEnter;
    public event Action OnMouseExit;
    public event Action OnMouseClick;

    [Header("RayCast Collision:")]
    [Tooltip("Chequea Alphas en el raycast. Modificar el Read/Write Enabled en la imagen si éste es true.")]
    [SerializeField] private bool modifyHitBox;
    [SerializeField] private float alphaRayCast = 0.1f;
    
    [Header("Effect Scale:")]
    [SerializeField] private float scaleSpeed= 3;
    [SerializeField] private float scaleLimit = 1.2f;
    private bool increment = false;
    private Vector3 initialScale;
    private Vector3 scale;

    [Header("Effect Image:")]
    [SerializeField] private bool modifyImage;
    [SerializeField] private Sprite imageDefault;
    [SerializeField] private Sprite imageHighlighted;
    private Image currentImage;

    [Header("Effect Color Text:")] 
    [SerializeField] private bool textHighlight;
    [SerializeField] private TextMeshProUGUI textToHighlight;
    [SerializeField] private Color colorHighlight;
    private Color colorNormal;

    [Header("Other:")]
    [SerializeField] private bool enableObject;
    [SerializeField] private GameObject objectToEnable;

    private void Awake()
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
    }
    private void OnEnable()
    {
        transform.localScale = initialScale;
        increment = false;
    }
    private void Update()
    {
        ChangeScale();
    }
    private void OnDestroy()
    {
        RemoveBehaviours();
    }
    public void AddBehaviours(Action onClick = null, Action onEnter = null, Action onExit = null)
    {
        if (onClick != null) 
            OnMouseClick += onClick;
        if (onEnter != null)
            OnMouseEnter += onEnter;
        if (onExit != null)
            OnMouseExit += onExit; 
    }

    private void RemoveBehaviours()
    {
        OnMouseClick = null;
        OnMouseEnter = null;
        OnMouseExit = null;
    }
    
    public void OnMouseEnterButton()
    {
        OnMouseEnter?.Invoke();
        increment = true;
        //AkSoundEngine.PostEvent(AK.EVENTS.UIBUTTONENTER, gameObject);

        if (modifyImage)
            currentImage.sprite = imageHighlighted;

        if (enableObject)
            objectToEnable.SetActive(true);

        if (textHighlight)
            textToHighlight.color = colorHighlight;
    }
    public void OnMouseExitButton()
    {
        OnMouseExit?.Invoke();
        increment = false;
        //AkSoundEngine.PostEvent(AK.EVENTS.UIBUTTONEXIT, gameObject);

        if (modifyImage)
            currentImage.sprite = imageDefault;

        if (enableObject)
            objectToEnable.SetActive(false);

        if (textHighlight)
            textToHighlight.color = colorNormal;
    }
    private void ChangeScale()
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
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnMouseEnterButton();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        OnMouseExitButton();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        OnMouseClick?.Invoke();
        //AkSoundEngine.PostEvent(AK.EVENTS.UICLICKBUTTON, gameObject);
    }
}