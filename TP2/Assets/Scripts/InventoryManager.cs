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
    GameObject m_Selection; 

    void Start()
    {
        m_SlimeManager = GameObject.FindGameObjectWithTag("Player").GetComponent<SlimeManager>();
        m_InventoryOrbButtons = new Dictionary<SlimeColor, Button>()
        {
            {SlimeColor.Green, transform.Find("Horizontal Layout Group").Find("Green Orb").gameObject.GetComponentInChildren<Button>()},
            {SlimeColor.Pink, transform.Find("Horizontal Layout Group").Find("Pink Orb").gameObject.GetComponentInChildren<Button>()},
            {SlimeColor.Yellow, transform.Find("Horizontal Layout Group").Find("Yellow Orb").gameObject.GetComponentInChildren<Button>()},
            {SlimeColor.Orange, transform.Find("Horizontal Layout Group").Find("Orange Orb").gameObject.GetComponentInChildren<Button>()},
            {SlimeColor.Blue, transform.Find("Horizontal Layout Group").Find("Blue Orb").gameObject.GetComponentInChildren<Button>()}
        };
        m_InventoryOrbLabels = new Dictionary<SlimeColor, TMP_Text>()
        {
            {SlimeColor.Green, transform.Find("Horizontal Layout Group").Find("Green Orb").gameObject.GetComponentInChildren<TMP_Text>()},
            {SlimeColor.Pink, transform.Find("Horizontal Layout Group").Find("Pink Orb").gameObject.GetComponentInChildren<TMP_Text>()},
            {SlimeColor.Yellow, transform.Find("Horizontal Layout Group").Find("Yellow Orb").gameObject.GetComponentInChildren<TMP_Text>()},
            {SlimeColor.Orange, transform.Find("Horizontal Layout Group").Find("Orange Orb").gameObject.GetComponentInChildren<TMP_Text>()},
            {SlimeColor.Blue, transform.Find("Horizontal Layout Group").Find("Blue Orb").gameObject.GetComponentInChildren<TMP_Text>()}
        };

        m_Selection = transform.Find("Selection").gameObject;

        foreach (var (slimeColor, button) in m_InventoryOrbButtons)
        {
            button.onClick.AddListener(() =>
            {
                m_SlimeManager.ChangeColor(slimeColor, true);
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
        
        m_Selection.transform.position = m_InventoryOrbButtons[m_SlimeManager.NextColor].transform.position;
        m_Selection.SetActive(m_SlimeManager.Orbs[m_SlimeManager.NextColor].Amount > 0);


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            m_SlimeManager.ChangeColor(SlimeColor.Green, true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) )
        {
            m_SlimeManager.ChangeColor(SlimeColor.Pink, true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            m_SlimeManager.ChangeColor(SlimeColor.Yellow, true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            m_SlimeManager.ChangeColor(SlimeColor.Blue, true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            m_SlimeManager.ChangeColor(SlimeColor.Orange, true);
        }
    }
}
