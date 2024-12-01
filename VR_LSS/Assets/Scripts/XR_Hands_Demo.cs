using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class XR_Hands_Demo : MonoBehaviour
{

    private enum ControllerSide
    {
        Left_Controler,
        Right_Controler,
    }
    [SerializeField]
    private ControllerSide m_controller;

    private InputDeviceCharacteristics m_characteristics;

    [SerializeField]
    private bool m_debugMode = true;

    private void Awake() {}
    private void Start()   
    {
        if (DebugLogger.current == null) m_debugMode=false;
        if (m_controller == ControllerSide.Left_Controler)
        {
            m_characteristics = InputDeviceCharacteristics.Left;
        }
        else
        {
            m_characteristics = InputDeviceCharacteristics.Right;
        }
    }

    private void Update()
    {
        List <InputDevice> m_device = new List<InputDevice> ();
        InputDevices.GetDevicesWithCharacteristics(m_characteristics,m_device);
        if (m_device.Count == 1)
        {
            CheckController(m_device[0]);
        }
        else
        {
            if (m_debugMode) DebugLogger.current.AddLine("Controller not found: ");
        }
    }

    private void CheckController(InputDevice d)
    {
        bool primaryButtonDown = false;
        d.TryGetFeatureValue(CommonUsages.primaryButton, out primaryButtonDown);
        if (primaryButtonDown)
        {
            if (m_debugMode) DebugLogger.current.AddLine("Button down");
        }
        else
        {
            if (m_debugMode) DebugLogger.current.AddLine("Button up");
        }
    }
}
