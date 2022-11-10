using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class OrbManager : MonoBehaviour
{
    [SerializeField] private SlimeColor m_Color;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<SlimeManager>().Orbs[m_Color].Amount++;

            Destroy(gameObject);
        }
    }
}
