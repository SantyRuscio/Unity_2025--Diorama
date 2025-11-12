using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new CloudData", menuName = "Assets/CloudData")]
public class CloudData : ScriptableObject
{
    public float m_Density;
    public Color32 m_Color;
    public float m_Speed;
}
