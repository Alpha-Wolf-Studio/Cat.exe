using System;
using System.Collections;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public event Action OnEnterCheckPoint;
    public event Action<CheckPoint> OnSaveCheckPoint;
    public event Action OnExitCheckPoint;

    [SerializeField] private Collider colliderTrigger;
    [SerializeField] private Collider wall;
    [SerializeField] private Transform spawnPoint;
    [Header("Graphic")]
    [SerializeField] private MeshRenderer graphicRenderer;
    [SerializeField] private float glowSpeed = 1f;
    [SerializeField] private Color glowColor; 
    [SerializeField] private Color blackColor;
    
    public bool wasActivated;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    public int ID { get; set; } = 0;

    private void Start()
    {
        StartCoroutine(GlowIEnumerator());
    }

    private void OnTriggerEnter (Collider other)
    {
        if (wasActivated)
            return;

        if (Utils.CheckLayerInMask(GameplayManager.Get().layerPlayer, other.gameObject.layer))
        {
            wasActivated = true;
            wall.enabled = true;
            OnSaveCheckPoint?.Invoke(this);
            OnEnterCheckPoint?.Invoke();
        }
    }

    private void OnTriggerExit (Collider other)
    {
        if (Utils.CheckLayerInMask(GameplayManager.Get().layerPlayer, other.gameObject.layer))
        {
            colliderTrigger.enabled = false;
            OnExitCheckPoint?.Invoke();
        }
    }

    public void ResetCheckPoint ()
    {
        colliderTrigger.enabled = true;
    }

    public Vector3 GetPositionSpawn () => spawnPoint.position;
    public Vector3 GetRotationSpawn () => spawnPoint.rotation.eulerAngles;

    private IEnumerator GlowIEnumerator()
    {
        while (enabled)
        {
            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime * glowSpeed;
                Color color = Color.Lerp(blackColor, glowColor, t); 
                graphicRenderer.material.SetColor(EmissionColor, color);
                yield return null;
            }
            graphicRenderer.material.SetColor(EmissionColor, glowColor);
            t = 1;
            while (t > 0)
            {
                t -= Time.deltaTime * glowSpeed;
                Color color = Color.Lerp(blackColor, glowColor, t); 
                graphicRenderer.material.SetColor(EmissionColor, color);
                yield return null;
            }
            graphicRenderer.material.SetColor(EmissionColor, blackColor);
        }
    }
    
}