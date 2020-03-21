using System;
using Doozy.Editor.Nody.NodeGUI;
using Doozy.Engine.Extensions;
using Doozy.Engine.Soundy;
using Doozy.Engine.UI.Nodes;
using Doozy.Engine.Utils;
using UnityEditor;
using UnityEngine;

namespace Doozy.Editor.UI.Nodes
{
    // ReSharper disable once UnusedMember.Global
    [CustomNodeGUI(typeof(CriAtomDoozyNode))]
    public class CriAtomDoozyNodeGUI : BaseNodeGUI
    {
        private CriAtomDoozyNode TargetNode { get { return (CriAtomDoozyNode) Node; } }

        protected override GUIStyle GetIconStyle()
        {
            switch (TargetNode.adx2Action)
            {
                case CriAtomDoozyNode.Adx2Actions.Play: return Styles.GetStyle(Styles.StyleName.NodeIconSoundNode);
                case CriAtomDoozyNode.Adx2Actions.Stop: return Styles.GetStyle(Styles.StyleName.IconFaStop);
                case CriAtomDoozyNode.Adx2Actions.Pause: return Styles.GetStyle(Styles.StyleName.IconFaPause);
                case CriAtomDoozyNode.Adx2Actions.Unpause: return Styles.GetStyle(Styles.StyleName.IconFaPlay);
                case CriAtomDoozyNode.Adx2Actions.Mute: return Styles.GetStyle(Styles.StyleName.IconFaVolumeMute);
                case CriAtomDoozyNode.Adx2Actions.Unmute: return Styles.GetStyle(Styles.StyleName.IconFaVolume);
                case CriAtomDoozyNode.Adx2Actions.LoadCueSheet: return Styles.GetStyle(Styles.StyleName.IconFaVolume);
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private GUIStyle m_icon;
        private string m_cueSheetName;
        private string m_cueName;
        
        protected override void OnNodeGUI()
        {
            DrawNodeBody();
            DrawSocketsList(Node.InputSockets);
            DrawSocketsList(Node.OutputSockets);
            DrawActionDescription();
        }
 
        private void DrawActionDescription()
        {
            DynamicHeight += DGUI.Properties.Space(4);
            float x = DrawRect.x + 16;
            float lineHeight = DGUI.Properties.SingleLineHeight;

            var soundActionRect = new Rect(x, DynamicHeight, DrawRect.width - 32, lineHeight);
            DynamicHeight += soundActionRect.height;
            DynamicHeight += DGUI.Properties.Space(2);

            float iconLineHeight = lineHeight * 2;
            float iconSize = iconLineHeight * 0.6f;
            var iconRect = new Rect(x, DynamicHeight + (iconLineHeight - iconSize) / 2, iconSize, iconSize);
            float textX = iconRect.xMax + DGUI.Properties.Space(4);
            float textWidth = DrawRect.width - iconSize - DGUI.Properties.Space(4) - 32;
            var titleRect = new Rect(textX, DynamicHeight, textWidth, lineHeight);

            if (TargetNode.adx2Action == CriAtomDoozyNode.Adx2Actions.Play|| TargetNode.adx2Action == CriAtomDoozyNode.Adx2Actions.LoadCueSheet) DynamicHeight += titleRect.height;
            var descriptionRect = new Rect(textX, DynamicHeight, textWidth, lineHeight);

            if (TargetNode.adx2Action == CriAtomDoozyNode.Adx2Actions.Play || TargetNode.adx2Action == CriAtomDoozyNode.Adx2Actions.LoadCueSheet)
            {
                DynamicHeight += descriptionRect.height;
                DynamicHeight += DGUI.Properties.Space(4);
            }

            if (ZoomedBeyondSocketDrawThreshold) return;

            string soundAction;
            switch (TargetNode.adx2Action)
            {
                case CriAtomDoozyNode.Adx2Actions.Play:
                    soundAction = UILabels.PlaySound;
                    break;
                
                case CriAtomDoozyNode.Adx2Actions.Stop:
                    soundAction = UILabels.StopAllSounds;
                    break;
                
                case CriAtomDoozyNode.Adx2Actions.Pause:
                    soundAction = UILabels.PauseAllSounds;
                    break;
                
                case CriAtomDoozyNode.Adx2Actions.Unpause:
                    soundAction = UILabels.UnpauseAllSounds;
                    break;
                
                case CriAtomDoozyNode.Adx2Actions.Mute: 
                    soundAction = UILabels.MuteAllSounds;
                    break;
                
                case CriAtomDoozyNode.Adx2Actions.Unmute:
                    soundAction = UILabels.UnmuteAllSounds;
                    break;
                case CriAtomDoozyNode.Adx2Actions.LoadCueSheet:
                    soundAction = "Load CueSheet: "+ TargetNode.cueSheetName;
                    break;
                
                default: throw new ArgumentOutOfRangeException();
            }
            
            Color soundActionTextColor = (DGUI.Utility.IsProSkin ? Color.white.Darker() : Color.black.Lighter()).WithAlpha(0.6f);
            GUI.Label(soundActionRect, soundAction, DGUI.Colors.ColorTextOfGUIStyle(DGUI.Label.Style(Doozy.Editor.Size.S, TextAlign.Center), soundActionTextColor));

            if (TargetNode.adx2Action == CriAtomDoozyNode.Adx2Actions.Play)
            {
                GUISkin skin = AssetDatabase.LoadAssetAtPath<GUISkin>(CriAtomDoozyUtils.GUISTYLE_CRI_ATOM_DOOZY_PATH);

                m_icon = skin.GetStyle("IconCriAtom");
                m_cueSheetName = TargetNode.cueSheetName;
                m_cueName = TargetNode.cueName;

                Color iconAndTextColor = DGUI.Colors.TextColor(TargetNode.HasSound ? DGUI.Colors.GeneralColorName : ColorName.Red).WithAlpha(0.6f);
                DGUI.Icon.Draw(iconRect, m_icon, iconAndTextColor);
                GUI.Label(titleRect, m_cueSheetName, DGUI.Colors.ColorTextOfGUIStyle(DGUI.Label.Style(Doozy.Editor.Size.S, TextAlign.Left), iconAndTextColor));
                GUI.Label(descriptionRect, m_cueName, DGUI.Colors.ColorTextOfGUIStyle(DGUI.Label.Style(Doozy.Editor.Size.M, TextAlign.Left), iconAndTextColor));
            }else if (TargetNode.adx2Action == CriAtomDoozyNode.Adx2Actions.LoadCueSheet)
            {
                Color iconAndTextColor = DGUI.Colors.TextColor(TargetNode.HasSound ? DGUI.Colors.GeneralColorName : ColorName.White).WithAlpha(0.6f);
                
                GUI.Label(titleRect, TargetNode.acbFilePath, DGUI.Colors.ColorTextOfGUIStyle(DGUI.Label.Style(Doozy.Editor.Size.M, TextAlign.Left), iconAndTextColor));
                GUI.Label(descriptionRect, TargetNode.awbFilePath, DGUI.Colors.ColorTextOfGUIStyle(DGUI.Label.Style(Doozy.Editor.Size.M, TextAlign.Left), iconAndTextColor));
            }
        }
    }
}