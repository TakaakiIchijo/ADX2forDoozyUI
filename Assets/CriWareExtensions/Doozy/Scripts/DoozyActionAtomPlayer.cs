using System;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Doozy.Engine.ADX2
{
    [RequireComponent(typeof(UIView))]
    public class DoozyActionAtomPlayer : MonoBehaviour
    {
        public string cueSheetName;

        public void PlayCue(string cueName)
        {
            DoozyAtomSourceManager.Instance.Play(cueSheetName, cueName);
        }
    }
}