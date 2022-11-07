using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    SlimeManager m_SlimeManager;
    Dictionary<SlimeColor, TMP_Text> m_InventoryOrbLabels;
    Dictionary<SlimeColor, Button> m_InventoryOrbButtons;

    void Start()
    {
        m_SlimeManager = GameObject.FindGameObjectWithTag("Player").GetComponent<SlimeManager>();
        m_InventoryOrbButtons = new Dictionary<SlimeColor, Button>()
        {
            {SlimeColor.Green, transform.Find("Green Orb").gameObject.GetComponent<Button>()},
            {SlimeColor.Pink, transform.Find("Pink Orb").gameObject.GetComponent<Button>()},
            {SlimeColor.Yellow, transform.Find("Yellow Orb").gameObject.GetComponent<Button>()},
            {SlimeColor.Orange, transform.Find("Orange Orb").gameObject.GetComponent<Button>()},
            {SlimeColor.Blue, transform.Find("Blue Orb").gameObject.GetComponent<Button>()}
        };
        m_InventoryOrbLabels = new Dictionary<SlimeColor, TMP_Text>()
        {
            {SlimeColor.Green, transform.Find("Green Orb").gameObject.GetComponentInChildren<TMP_Text>()},
            {SlimeColor.Pink, transform.Find("Pink Orb").gameObject.GetComponentInChildren<TMP_Text>()},
            {SlimeColor.Yellow, transform.Find("Yellow Orb").gameObject.GetComponentInChildren<TMP_Text>()},
            {SlimeColor.Orange, transform.Find("Orange Orb").gameObject.GetComponentInChildren<TMP_Text>()},
            {SlimeColor.Blue, transform.Find("Blue Orb").gameObject.GetComponentInChildren<TMP_Text>()}
        };

        foreach (var (slimeColor, button) in m_InventoryOrbButtons)
        {
            button.onClick.AddListener(() =>
            {
                m_SlimeManager.ForceChangeColor(slimeColor);
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var (slimeColor, tmpText) in m_InventoryOrbLabels)
        {
            tmpText.text = m_SlimeManager.Orbs[slimeColor].Amount.ToString();
            m_InventoryOrbButtons[slimeColor].interactable = m_SlimeManager.Orbs[slimeColor].Amount > 0;
        }
        m_InventoryOrbButtons[m_SlimeManager.CurrentColor].interactable = false;
    }
}
