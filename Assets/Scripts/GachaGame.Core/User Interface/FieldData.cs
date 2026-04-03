using System;
using System.Collections.Generic;
using MatrixUtils.Attributes;
using TMPro;
using UnityEngine;
[Serializable]
public class FieldData
{
    [SerializeField, RequiredField] public TMP_InputField InputField;
    [SerializeField] public List<SerializableRegex> Regexes;
    public bool CheckFieldValidity()
    {
        foreach (SerializableRegex regex in Regexes)
        {
            if(!regex.Value.IsMatch(InputField.text))return false;
        }
        return true;
    }
}
