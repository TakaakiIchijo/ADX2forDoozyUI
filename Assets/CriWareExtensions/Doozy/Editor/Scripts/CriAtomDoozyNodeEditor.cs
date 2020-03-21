using DG.DemiEditor;
using Doozy.Editor.Internal;
using Doozy.Editor.Nody.Editors;
using Doozy.Engine.UI.Nodes;
using Doozy.Engine.Utils;
using UnityEditor;
using UnityEngine;

namespace Doozy.Editor.UI.Nodes
{
    [CustomEditor(typeof(CriAtomDoozyNode))]
    public class CriAtomDoozyNodeEditor : BaseNodeEditor
    {
        private const string NO_SOUND = "NoSound";
        private const string SOUND_ACTION = "Adx2Action";

        private CriAtomDoozyNode TargetNode { get { return (CriAtomDoozyNode) target; } }

        private SerializedProperty
            m_cueSheetName,
            m_cueName,
            m_soundAction,
            m_acbFilePath,
            m_awbFilePath;

        protected override void LoadSerializedProperty()
        {
            base.LoadSerializedProperty();

            m_cueSheetName = GetProperty("cueSheetName");
            m_cueName = GetProperty("cueName");
            m_soundAction = GetProperty("adx2Action");
            m_acbFilePath = GetProperty("acbFilePath");
            m_awbFilePath = GetProperty("awbFilePath");
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            AddInfoMessage(NO_SOUND, new InfoMessage(InfoMessage.MessageType.Warning, CriAtomDoozyUtils.NoCueName, TargetNode.adx2Action == CriAtomDoozyNode.Adx2Actions.Play && !TargetNode.HasSound, Repaint));
            AddInfoMessage(SOUND_ACTION, new InfoMessage(InfoMessage.MessageType.Info, UILabels.NoSound, TargetNode.adx2Action != CriAtomDoozyNode.Adx2Actions.Play, Repaint));
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            //AtomSourceManager.StopAllPlayers();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            
            GUISkin skin = AssetDatabase.LoadAssetAtPath<GUISkin>(CriAtomDoozyUtils.GUISTYLE_CRI_ATOM_DOOZY_PATH);
            DrawHeader(skin.GetStyle("ComponentHeaderSoundNode"), CriAtomDoozyUtils.URL_ADX2_MANUAL, CriAtomDoozyUtils.URL_ADX2_YOUTUBE_CHANNEL);
            
            DrawDebugMode(true);
            GUILayout.Space(DGUI.Properties.Space(2));
            DrawNodeName();
            GUILayout.Space(DGUI.Properties.Space());
            DrawRenameNodeButton();
            GUILayout.Space(DGUI.Properties.Space(8));
            DrawInputSockets(BaseNode);
            GUILayout.Space(DGUI.Properties.Space(8));
            DrawOutputSockets(BaseNode);
            GUILayout.Space(DGUI.Properties.Space(16));
            DrawOptions();
            GUILayout.Space(DGUI.Properties.Space(2));
            serializedObject.ApplyModifiedProperties();
            SendGraphEventNodeUpdated();
        }

        private void DrawOptions()
        {
            DrawBigTitleWithBackground(Styles.GetStyle(Styles.StyleName.IconAction), UILabels.Actions, DGUI.Colors.ActionColorName, DGUI.Colors.ActionColorName);
            GUILayout.Space(DGUI.Properties.Space(2));

            EditorGUI.BeginChangeCheck();
            DGUI.Property.Draw(m_soundAction, CriAtomDoozyUtils.Adx2Action, DGUI.Colors.ActionColorName, DGUI.Colors.ActionColorName);
            if (EditorGUI.EndChangeCheck()) NodeUpdated = true;

            GUILayout.Space(DGUI.Properties.Space(2));
            
            GetInfoMessage(NO_SOUND).DrawMessageOnly(TargetNode.adx2Action == CriAtomDoozyNode.Adx2Actions.Play && !TargetNode.HasSound);
            
            if (TargetNode.adx2Action != CriAtomDoozyNode.Adx2Actions.Play　|| TargetNode.adx2Action != CriAtomDoozyNode.Adx2Actions.LoadCueSheet)
            {
                switch (TargetNode.adx2Action)
                {
                    case CriAtomDoozyNode.Adx2Actions.Stop:
                        GetInfoMessage(SOUND_ACTION).Message = UILabels.StopAllSounds;
                        break;
                    case CriAtomDoozyNode.Adx2Actions.Pause:
                        GetInfoMessage(SOUND_ACTION).Message = UILabels.PauseAllSounds;
                        break;
                    case CriAtomDoozyNode.Adx2Actions.Unpause:
                        GetInfoMessage(SOUND_ACTION).Message = UILabels.UnpauseAllSounds;
                        break;
                    case CriAtomDoozyNode.Adx2Actions.Mute:
                        GetInfoMessage(SOUND_ACTION).Message = UILabels.MuteAllSounds;
                        break;
                    case CriAtomDoozyNode.Adx2Actions.Unmute:
                        GetInfoMessage(SOUND_ACTION).Message = UILabels.UnmuteAllSounds;
                        break;
                    case CriAtomDoozyNode.Adx2Actions.LoadCueSheet:
                        GetInfoMessage(SOUND_ACTION).Message = "LoadCueSheet";
                        break;
                }
            }

            GetInfoMessage(SOUND_ACTION).DrawMessageOnly(TargetNode.adx2Action != CriAtomDoozyNode.Adx2Actions.Play);

            if (TargetNode.adx2Action == CriAtomDoozyNode.Adx2Actions.Play)
            {
                EditorGUI.BeginChangeCheck();
                
                GUILayout.BeginHorizontal();
                {
                    DGUI.Label.Draw("Cue Sheet Name", Size.S, TextAlign.Left, DGUI.Colors.DisabledTextColorName,
                        DGUI.Properties.SingleLineHeight);
                    EditorGUILayout.PropertyField(m_cueSheetName, GUIContent.none, true);
                }
                GUILayout.EndHorizontal();
                
                GUILayout.BeginHorizontal();
                {
                    DGUI.Label.Draw("Cue Name", Size.S, TextAlign.Left, DGUI.Colors.DisabledTextColorName,
                        DGUI.Properties.SingleLineHeight);
                    EditorGUILayout.PropertyField(m_cueName, GUIContent.none, true);
                }
                GUILayout.EndHorizontal();

                if (EditorGUI.EndChangeCheck()) NodeUpdated = true;
            }
            
            if (TargetNode.adx2Action == CriAtomDoozyNode.Adx2Actions.LoadCueSheet)
            {
                EditorGUI.BeginChangeCheck();
                
                GUILayout.BeginHorizontal();
                {
                    DGUI.Label.Draw("Cue Sheet Name", Size.S, TextAlign.Left, DGUI.Colors.DisabledTextColorName,
                        DGUI.Properties.SingleLineHeight);
                    EditorGUILayout.PropertyField(m_cueSheetName, GUIContent.none, true);
                }
                GUILayout.EndHorizontal();
                
                GUILayout.BeginHorizontal();
                {
                    DGUI.Label.Draw("ACB File Path", Size.S, TextAlign.Left, DGUI.Colors.DisabledTextColorName,
                        DGUI.Properties.SingleLineHeight);
                    EditorGUILayout.PropertyField(m_acbFilePath, GUIContent.none, true);
                }
                GUILayout.EndHorizontal();
            
                GUILayout.BeginHorizontal();
                {
                    DGUI.Label.Draw("AWB File Path", Size.S, TextAlign.Left, DGUI.Colors.DisabledTextColorName,
                        DGUI.Properties.SingleLineHeight);
                    EditorGUILayout.PropertyField(m_awbFilePath, GUIContent.none, true);
                }
                GUILayout.EndHorizontal();

                if (EditorGUI.EndChangeCheck()) NodeUpdated = true;
            }
        }

        private void DrawRenameNodeButton()
        {
            string renameTo = "---";
            switch (TargetNode.adx2Action)
            {
                case CriAtomDoozyNode.Adx2Actions.Play:
                    if (TargetNode.cueName != null) renameTo = TargetNode.cueName;
                    break;
                case CriAtomDoozyNode.Adx2Actions.Stop:
                    renameTo = UILabels.StopAllSounds;
                    break;
                case CriAtomDoozyNode.Adx2Actions.Pause:   
                    renameTo = UILabels.PauseAllSounds;
                    break;
                case CriAtomDoozyNode.Adx2Actions.Unpause:
                    renameTo = UILabels.UnpauseAllSounds;
                    break;
                case CriAtomDoozyNode.Adx2Actions.Mute:
                    renameTo = UILabels.MuteAllSounds;
                    break;
                case CriAtomDoozyNode.Adx2Actions.Unmute:
                    renameTo = UILabels.UnmuteAllSounds;
                    break;
                case CriAtomDoozyNode.Adx2Actions.LoadCueSheet:
                    if (TargetNode.cueSheetName != null)
                    {
                        renameTo = "LoadCueSheet";
                    }
                    break;
            }

            DrawRenameButton(renameTo);
        }
    }
}