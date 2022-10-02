using UnityEngine;

public class DissolveEffect : MonoBehaviour
{
    [SerializeField] private Material[] dissolve = null;
    [SerializeField] private float timeForEffect = 1;

    private FloatLerper disolveLerper = null;

    private void Start()
    {
        disolveLerper = new FloatLerper();
    }

    private void Update()
    {
        if (disolveLerper != null && disolveLerper.Active)
        { 
            disolveLerper.UpdateLerper();

            for (int i = 0; i < dissolve.Length; i++)
            {
                dissolve[i].SetFloat("_Cutoff", disolveLerper.GetValue());
            }
            
        }
    }

    public void SetDissolve(float amountEffect)
    {
        disolveLerper.SetLerperValues(dissolve[0].GetFloat("_Cutoff"), amountEffect, timeForEffect, Lerper<float>.LERPER_TYPE.STEP_SMOOTHER, true);
    }
}
