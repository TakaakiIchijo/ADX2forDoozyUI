using System;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Doozy.Engine.ADX2
{
    [RequireComponent(typeof(UIView))]
    public class CriAtomOnButtonPlayer : MonoBehaviour
    {
        public string cueSheetName;

        public void UIViewPlayADX2(string cueName)
        {
            DoozyAtomSourceManager.Instance.Play(cueSheetName, cueName);
        }
    }
}