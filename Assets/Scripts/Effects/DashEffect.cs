using UnityEngine;

public class DashEffect : MonoBehaviour
{

    public void StartDashEffect() => gameObject.SetActive(true);
    public void StopDashEffect() => gameObject.SetActive(false);
    
}
