using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrbWheelManager : MonoBehaviour
{
    public float m_SlowMotionTimeScale = 0.1f;
    private float m_StartTimeScale;
    private float m_StartFixedDeltaTime;
    private GameObject m_BlurPanel;
    private GameObject m_OrbWheel;
    private TMP_Text m_SelectedOrbText;
    private Material m_BlurMaterial;
    private Animator m_Anim;
    private int m_BlurIntensityID;
    private SlimeColor m_selectedOrb = SlimeColor.None;
    private bool k_IsWheelOpened = false;

    private SlimeManager m_Slime;
    private Dictionary<SlimeColor, Animator> m_OrbAnimators;
    private Dictionary<SlimeColor, TMP_Text> m_OrbAmountLabels;
    private Dictionary<SlimeColor, Button> m_OrbButtons;
    // Start is called before the first frame update
    void Start()
    {
        m_Slime = GameObject.FindGameObjectWithTag("Player").GetComponent<SlimeManager>();
        m_StartTimeScale = Time.timeScale;
        m_StartFixedDeltaTime = Time.fixedDeltaTime;
        
        m_BlurPanel = transform.Find("BlurPanel").gameObject;
        m_BlurMaterial = m_BlurPanel.GetComponent<Image>().material;
        m_BlurIntensityID = Shader.PropertyToID("_Size");

        m_BlurPanel = transform.Find("BlurPanel").gameObject;
        m_OrbWheel = transform.Find("OrbWheel").gameObject;
        m_Anim = m_OrbWheel.GetComponent<Animator>();
        
        m_SelectedOrbText = m_OrbWheel.transform.Find("SelectedOrbText").GetComponent<TMP_Text>();
        m_OrbAnimators = new Dictionary<SlimeColor, Animator>()
        {
            {SlimeColor.Green, m_OrbWheel.transform.Find("TypicalGreenBtn").gameObject.GetComponent<Animator>()},
            {SlimeColor.Yellow, m_OrbWheel.transform.Find("StickyYellowBtn").gameObject.GetComponent<Animator>()},
            {SlimeColor.Orange, m_OrbWheel.transform.Find("PiercingOrangeBtn").gameObject.GetComponent<Animator>()},
            {SlimeColor.Blue, m_OrbWheel.transform.Find("PiercingBlueBtn").gameObject.GetComponent<Animator>()},
            {SlimeColor.Pink, m_OrbWheel.transform.Find("BouncyPinkBtn").gameObject.GetComponent<Animator>()}
        };
        m_OrbAmountLabels = new Dictionary<SlimeColor, TMP_Text>()
        {
            {SlimeColor.Green, m_OrbWheel.transform.Find("TypicalGreenBtn").gameObject.GetComponentInChildren<TMP_Text>()},
            {SlimeColor.Yellow, m_OrbWheel.transform.Find("StickyYellowBtn").gameObject.GetComponentInChildren<TMP_Text>()},
            {SlimeColor.Orange, m_OrbWheel.transform.Find("PiercingOrangeBtn").gameObject.GetComponentInChildren<TMP_Text>()},
            {SlimeColor.Blue, m_OrbWheel.transform.Find("PiercingBlueBtn").gameObject.GetComponentInChildren<TMP_Text>()},
            {SlimeColor.Pink, m_OrbWheel.transform.Find("BouncyPinkBtn").gameObject.GetComponentInChildren<TMP_Text>()}
        };
        m_OrbButtons = new Dictionary<SlimeColor, Button>()
        {
            {SlimeColor.Green, m_OrbWheel.transform.Find("TypicalGreenBtn").gameObject.GetComponent<Button>()},
            {SlimeColor.Yellow, m_OrbWheel.transform.Find("StickyYellowBtn").gameObject.GetComponent<Button>()},
            {SlimeColor.Orange, m_OrbWheel.transform.Find("PiercingOrangeBtn").gameObject.GetComponent<Button>()},
            {SlimeColor.Blue, m_OrbWheel.transform.Find("PiercingBlueBtn").gameObject.GetComponent<Button>()},
            {SlimeColor.Pink, m_OrbWheel.transform.Find("BouncyPinkBtn").gameObject.GetComponent<Button>()}
        };
        m_SelectedOrbText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            k_IsWheelOpened = true;
            UpdateOrbAmountLabels();
            StartSlowMotion();
            m_Anim.SetBool("ShowWheel", true);
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            k_IsWheelOpened = false;
            StopSlowMotion();
            m_Anim.SetBool("ShowWheel", false);
            m_SelectedOrbText.text = "";
            m_Slime.ChangeColor(m_selectedOrb, !m_Slime.Grounded);
        }

        if (k_IsWheelOpened)
        {
            BlurScreen();
            UpdateSelectedOrb();
        }
        else
        {
            ClearScreen();
        }

        foreach (var (slimeColor, button) in m_OrbButtons)
        {
            if (m_Slime.Orbs[slimeColor].Amount == 0)
            {
                button.interactable = false;
            }
            else
            {
                button.interactable = true;
            }
        }
        m_OrbButtons[m_Slime.NextColor].interactable = false;

    }

    private void UpdateOrbAmountLabels()
    {
        foreach (var (slimeColor, tmpText) in m_OrbAmountLabels)
        {
            tmpText.text = m_Slime.Orbs[slimeColor].Amount.ToString();
        }
    }

    private void UpdateSelectedOrb()
    {
        var stepLength = 72.0f;
        var mouseAngle = ((Vector3.SignedAngle(Vector3.up, Input.mousePosition - transform.position, Vector3.forward) +
                           stepLength / 2f) + 360f) % 360f;
        var selectedOrb = (SlimeColor)(int)(mouseAngle / stepLength);
        
        if (m_selectedOrb != selectedOrb)
        {
            if(m_selectedOrb != SlimeColor.None) m_OrbAnimators[m_selectedOrb].SetBool("Hovered", false);
            if (m_Slime.Orbs[selectedOrb].Amount > 0 && selectedOrb != m_Slime.NextColor)
            {
                m_OrbAnimators[selectedOrb].SetBool("Hovered", true);
                m_SelectedOrbText.text = m_Slime.Orbs[selectedOrb].Name;
                m_SelectedOrbText.color = m_Slime.Orbs[selectedOrb].Color;
                
            }
            m_selectedOrb = selectedOrb;

        }
    }   

    private void StartSlowMotion()
    {
        m_BlurMaterial.SetFloat(m_BlurIntensityID, 0f);
        m_BlurPanel.SetActive(true);
        m_OrbWheel.SetActive(true);
        Time.timeScale = m_SlowMotionTimeScale;
        Time.fixedDeltaTime = m_StartFixedDeltaTime * m_SlowMotionTimeScale;
    }
    
    private void StopSlowMotion()
    {
        Time.timeScale = m_StartTimeScale;
        Time.fixedDeltaTime = m_StartFixedDeltaTime;
    }
    
    void BlurScreen()
    {
        if (m_BlurMaterial.GetFloat(m_BlurIntensityID) < 3f)
        {
            m_BlurMaterial.SetFloat(m_BlurIntensityID, m_BlurMaterial.GetFloat(m_BlurIntensityID) + 0.1f);
        }
    }
    
    void ClearScreen()
    {
        if (m_BlurMaterial.GetFloat(m_BlurIntensityID) > 0f)
        {
            m_BlurMaterial.SetFloat(m_BlurIntensityID, m_BlurMaterial.GetFloat(m_BlurIntensityID) - 0.1f);
            return;
        }
        m_BlurPanel.SetActive(false);
        m_OrbWheel.SetActive(false);
    }
}
