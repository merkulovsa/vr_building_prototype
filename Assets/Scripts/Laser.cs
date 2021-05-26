using UnityEngine;

public class Laser : MonoBehaviour
{
    public delegate void OnHitHandler(RaycastHit hit);

    public event OnHitHandler OnHit;

    RaycastHit hit;

    void Update()
    {
        var localScale = transform.localScale;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            localScale.z = hit.distance;
            OnHit?.Invoke(hit);
        }
        else
        {
            localScale.z = 0;
        }

        transform.localScale = localScale;
    }
}
