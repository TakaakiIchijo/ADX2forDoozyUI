using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.Soundy;
using UnityEditor;
using UnityEngine;

/*
To add Disable Soundy Option

1.Modify SoundySettings.cs

add field
        public bool DisableSoundy = DISABLE_SOUNDY_DEFAULT_VALUE;//added by adx2forDoozy
        
add this line in Reset() function
                    DisableSoundy = DISABLE_SOUNDY_DEFAULT_VALUE;//added by adx2forDoozy

2.Modify DoozyWindowDrawViewSoundy.cs the DrawViewSoundySettings() function

                    DrawSoundyIdleCheckInterval(SoundySettings.Instance.AutoKillIdleControllers);
                    DrawDynamicViewVerticalSpace(0.5f); //Added by adx2 adx2fordoozy
                    DrawSoundyDisable();　//added by adx2fordoozy
                    
3.Modify UIComponentBase.cs in Awake() fuction change SoundyManager.Init() line to this

			if(SoundySettings.Instance.DisableSoundy == false) SoundyManager.Init(); //modified by adx2fordoozy

*/
namespace Doozy.Editor.Windows
{
    public partial class DoozyWindow
    {
        private void DrawSoundyDisable()
        {
            GUILayout.BeginHorizontal();
            {
                bool disable = SoundySettings.Instance.DisableSoundy;
                EditorGUI.BeginChangeCheck();
                disable = DGUI.Toggle.Switch.Draw(disable, "Disable Soundy", CurrentViewColorName, false, false, false, NormalRowHeight);
                if (EditorGUI.EndChangeCheck())
                {
                    SoundySettings.Instance.DisableSoundy = disable;
                    SoundySettings.Instance.SetDirty(false);
                }
            }
            
            GUILayout.EndHorizontal();
            GUILayout.Space(DGUI.Properties.Space());
        }
    }
}