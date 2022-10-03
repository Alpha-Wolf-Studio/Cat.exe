using UnityEngine;

public class DissolveEffect : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer[] dissolveMeshRenderers = null;
    [SerializeField] private float timeForEffect = 1;

    private FloatLerper disolveLerper = new FloatLerper();

    private void Update()
    {
        if (disolveLerper != null && disolveLerper.Active)
        { 
            disolveLerper.UpdateLerper();

            for (int i = 0; i < dissolveMeshRenderers.Length; i++)
            {
                dissolveMeshRenderers[i].material.SetFloat("_Cutoff", disolveLerper.GetValue());
            }
            
        }
    }

    public void SetDissolve(float amountEffect)
    {
        disolveLerper.SetLerperValues(dissolveMeshRenderers[0].material.GetFloat("_Cutoff"), amountEffect, timeForEffect, Lerper<float>.LERPER_TYPE.STEP_SMOOTHER, true);
    }
}
