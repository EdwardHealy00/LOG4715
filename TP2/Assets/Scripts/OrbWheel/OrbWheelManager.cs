using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class OrbWheelManager : MonoBehaviour
{
    public SlimeManager Slime;
    public float m_SlowMotionTimeScale = 0.1f;
    private float m_StartTimeScale;
    private float m_StartFixedDeltaTime;
    private GameObject m_BlurPanel;
    private GameObject m_OrbWheel;
    private Material m_BlurMaterial;
    private int m_BlurIntensityID;
    private bool k_IsWheelOpened = false;

    private Dictionary<SlimeColor, Animator> m_orbAnimators;
    // Start is called before the first frame update
    void Start()
    {
        m_StartTimeScale = Time.timeScale;
        m_StartFixedDeltaTime = Time.fixedDeltaTime;
        
        m_BlurPanel = transform.Find("BlurPanel").gameObject;
        m_BlurMaterial = m_BlurPanel.GetComponent<Image>().material;
        m_BlurIntensityID = Shader.PropertyToID("_Size");

        m_BlurPanel = transform.Find("BlurPanel").gameObject;
        m_OrbWheel = transform.Find("OrbWheel").gameObject;
        
        m_OrbWheel.SetActive(true);
        m_orbAnimators = new Dictionary<SlimeColor, Animator>()
        {
            {SlimeColor.Green, m_OrbWheel.transform.Find("TypicalGreenBtn").gameObject.GetComponent<Animator>()},
            {SlimeColor.Yellow, m_OrbWheel.transform.Find("StickyYellowBtn").gameObject.GetComponent<Animator>()},
            {SlimeColor.Orange, m_OrbWheel.transform.Find("PiercingOrangeBtn").gameObject.GetComponent<Animator>()},
            {SlimeColor.Blue, m_OrbWheel.transform.Find("PiercingBlueBtn").gameObject.GetComponent<Animator>()},
            {SlimeColor.Pink, m_OrbWheel.transform.Find("BouncyPinkBtn").gameObject.GetComponent<Animator>()}
        };
        m_OrbWheel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            k_IsWheelOpened = true;
            StartSlowMotion();
            m_OrbWheel.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            k_IsWheelOpened = false;
            StopSlowMotion();
            m_OrbWheel.SetActive(false);
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
    }

    private void UpdateSelectedOrb()
    {
        var stepLength = 72.0f;
        var stepOffset = 0.0f;
        var mouseAngle = ((Vector3.SignedAngle(Vector3.up, Input.mousePosition - transform.position, Vector3.forward) +
                           stepLength / 2f) - stepOffset + 360f) % 360f;
        var selectedOrb = (SlimeColor)(int)(mouseAngle / stepLength);
        
        if (Slime.CurrentColor != selectedOrb)
        {
            m_orbAnimators[Slime.CurrentColor].SetBool("Hovered", false);
            m_orbAnimators[selectedOrb].SetBool("Hovered", true);
            Slime.CurrentColor = selectedOrb;
        }
    }   

    private void StartSlowMotion()
    {
        m_BlurMaterial.SetFloat(m_BlurIntensityID, 0f);
        m_BlurPanel.SetActive(true);
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
        if (m_BlurMaterial.GetFloat(m_BlurIntensityID) < 2.9f)
        {
            m_BlurMaterial.SetFloat(m_BlurIntensityID, m_BlurMaterial.GetFloat(m_BlurIntensityID) + 0.1f);
        }
    }
    
    void ClearScreen()
    {
        if (m_BlurMaterial.GetFloat(m_BlurIntensityID) >= 0.1f)
        {
            m_BlurMaterial.SetFloat(m_BlurIntensityID, m_BlurMaterial.GetFloat(m_BlurIntensityID) - 0.1f);
            return;
        }
        m_BlurPanel.SetActive(false);
    }
}
