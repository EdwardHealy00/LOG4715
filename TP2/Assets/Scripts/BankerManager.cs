using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BankerManager : MonoBehaviour
{
    public SlimeColor TakeColor;
    public SlimeColor ReceiveColor;
    public uint TakeAmount;
    public uint ReceiveAmount;
    public uint TimesYouCanBuy = 1;

    private SlimeManager m_slime;
    private GameObject m_sign;
    private Image m_takeImage;
    private Image m_receiveImage;
    private TMP_Text m_takeText;
    private TMP_Text m_receiveText;
    private Animator m_pressSpaceIcon;

    private bool k_isBankOpen;
    // Start is called before the first frame update
    void Start()
    {
        m_slime = GameObject.FindGameObjectWithTag("Player").GetComponent<SlimeManager>();
        m_sign = transform.Find("Sign").gameObject;
        m_takeImage = transform.Find("Sign").Find("SignCanvas").Find("TakeImage").GetComponent<Image>();
        m_receiveImage = transform.Find("Sign").Find("SignCanvas").Find("ReceiveImage").GetComponent<Image>();
        m_takeText = transform.Find("Sign").Find("TextCanvas").Find("Take").GetComponent<TMP_Text>();
        m_receiveText = transform.Find("Sign").Find("TextCanvas").Find("Receive").GetComponent<TMP_Text>();
        m_pressSpaceIcon = transform.Find("PressSpaceIcon").GetComponent<Animator>();

        m_takeImage.color  = m_slime.Orbs[TakeColor].Color;
        m_receiveImage.color = m_slime.Orbs[ReceiveColor].Color;
        m_takeText.text = TakeAmount.ToString();
        m_receiveText.text = ReceiveAmount.ToString();
        k_isBankOpen = TimesYouCanBuy > 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (k_isBankOpen && Vector3.Distance(transform.position, m_slime.transform.position) < 2f && m_slime.Grounded)
        {
            m_pressSpaceIcon.SetBool("CanTrade", true);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Trade();
            }
        }
        else
        {
            m_pressSpaceIcon.SetBool("CanTrade", false);
        }
    }

    private void Trade()
    {
        if (m_slime.Orbs[TakeColor].Amount >= TakeAmount)
        {
            m_slime.Orbs[TakeColor].Amount -= TakeAmount;
            m_slime.Orbs[ReceiveColor].Amount += ReceiveAmount;
            m_slime.AutoSetNextColor();
            if (--TimesYouCanBuy == 0)
            {
                CloseBank();
            }
        }
        
    }

    private void CloseBank()
    {
        k_isBankOpen = false;
        StartCoroutine(TurnSignAround(m_sign.transform));
    }

    IEnumerator TurnSignAround(Transform sign)
    {
        var percentTurnedAround = 0f;
        while (percentTurnedAround < 1f)
        {
            percentTurnedAround += 0.025f;
            sign.eulerAngles = Vector3.Lerp(sign.eulerAngles, 180 * Vector3.up, percentTurnedAround);
            yield return new WaitForSeconds(.05f);
        }
    }
}
