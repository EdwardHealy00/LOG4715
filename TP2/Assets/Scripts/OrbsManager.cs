using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrbsManager : MonoBehaviour
{
    public float m_SlowMotionTimeScale = 0.1f;

    private float m_StartTimeScale;
    private float m_StartFixedDeltaTime;

    private GameObject m_OrbPanel;
    private Material m_BlurMaterial;
    private int m_BlurIntensityID;
    private bool k_IsInSlowMo = false;
    // Start is called before the first frame update
    void Start()
    {
        m_StartTimeScale = Time.timeScale;
        m_StartFixedDeltaTime = Time.fixedDeltaTime;
        
        m_OrbPanel = GameObject.Find("Canvas").transform.Find("OrbPanel").gameObject;
        m_BlurMaterial = m_OrbPanel.GetComponent<Image>().material;
        m_BlurIntensityID = Shader.PropertyToID("_Size");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            StartSlowMotion();
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            StopSlowMotion();
        }

        if (k_IsInSlowMo)
        {
            BlurScreen();
        }
        else
        {
            ClearScreen();
        }
    }

    private void StartSlowMotion()
    {
        k_IsInSlowMo = true;
        m_BlurMaterial.SetFloat(m_BlurIntensityID, 0f);
        m_OrbPanel.SetActive(true);
        Time.timeScale = m_SlowMotionTimeScale;
        Time.fixedDeltaTime = m_StartFixedDeltaTime * m_SlowMotionTimeScale;
    }
    
    private void StopSlowMotion()
    {
        k_IsInSlowMo = false;
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
        m_OrbPanel.SetActive(false);
    }
}
