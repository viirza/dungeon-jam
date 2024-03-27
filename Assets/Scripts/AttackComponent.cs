using TMPro;
using UnityEngine;

public class AttackComponent : MonoBehaviour
{
    public float DMG;
    public string Target;

    public void Attack()
    {
        RaycastHit hit;


        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 1))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward), Color.red);

            if (hit.collider.gameObject.tag == Target)
            {
                HealthComponent healthComponent = hit.collider.GetComponent<HealthComponent>();
                healthComponent.UpdateHP(-DMG);
            }
        }
    }
}
