
using System;
using Doozy.Engine.Nody.Attributes;
using Doozy.Engine.Nody.Connections;
using Doozy.Engine.Nody.Models;
using Doozy.Engine.Utils;
using Doozy.Engine.ADX2;
using UnityEngine;
using UnityEditor;

namespace Doozy.Engine.UI.Nodes
{
    [NodeMenu(CriAtomDoozyUtils.NODE_CONTEXT_MENU, 1)]
    public class CriAtomDoozyNode : Node
    {
#if UNITY_EDITOR
        public override bool HasErrors { get { return base.HasErrors || ErrorPlayActionHasNoSound; } }
        public bool ErrorPlayActionHasNoSound;
#endif
        
        [NonSerialized] private bool isCueSheetLoaded;

        public enum Adx2Actions
        {
            Play,
            Stop,
            Pause,
            Unpause,
            Mute,
            Unmute,
            LoadCueSheet,
        }

        public string acbFilePath;
        public string awbFilePath;
        public string cueSheetName;
        public string cueName;
        public Adx2Actions adx2Action = Adx2Actions.Play;

        public bool HasSound => !string.IsNullOrEmpty(cueName);

        public override void OnCreate()
        {
            base.OnCreate();
            CanBeDeleted = true;
            SetNodeType(NodeType.General);
            SetName(CriAtomDoozyUtils.NODENAME);
            SetAllowDuplicateNodeName(true);
        }
        
        public override void AddDefaultSockets()
        {
            base.AddDefaultSockets();
            AddInputSocket(ConnectionMode.Multiple, typeof(PassthroughConnection), false, false);
            AddOutputSocket(ConnectionMode.Override, typeof(PassthroughConnection), false, false);
        }
        
        public override void CopyNode(Node original)
        {
            base.CopyNode(original);
            var node = (CriAtomDoozyNode) original;
            
            adx2Action = node.adx2Action;
        }
        
        public override void OnEnter(Node previousActiveNode, Connection connection)
        {
            base.OnEnter(previousActiveNode, connection);
            if (ActiveGraph == null) return;

            switch (adx2Action)
            {
                case Adx2Actions.Play:
                    if (HasSound) CriAtomDoozyAtomSourceManager.Instance.Play(cueSheetName,cueName);
                    break;
                case Adx2Actions.Stop:
                    CriAtomDoozyAtomSourceManager.Instance.StopAllSounds();
                    break;
                case Adx2Actions.Pause:
                    CriAtomDoozyAtomSourceManager.Instance.PauseAllSounds();
                    break;
                case Adx2Actions.Unpause:
                    CriAtomDoozyAtomSourceManager.Instance.UnpauseAllSounds();
                    break;
                case Adx2Actions.Mute:
                    CriAtomDoozyAtomSourceManager.Instance.MuteAllSounds();
                    break;
                case Adx2Actions.Unmute:
                    CriAtomDoozyAtomSourceManager.Instance.UnmuteAllSounds();
                    break;
                case Adx2Actions.LoadCueSheet:
                    CriAtomDoozyAtomSourceManager.Instance.LoadCueSheet(cueSheetName, acbFilePath, awbFilePath, CueSheetLoaded);
                    break;
                default: throw new ArgumentOutOfRangeException();
            }

            if (adx2Action != Adx2Actions.LoadCueSheet)
            {
                ContinueToNextNode();
            }
        }
        
        private void CueSheetLoaded()
        {
            Debug.Log("CueSheet " +cueSheetName+ " is loaded");
            ContinueToNextNode();
        }

        private void ContinueToNextNode()
        {
            if (!FirstOutputSocket.IsConnected) return;
            ActiveGraph.SetActiveNodeByConnection(FirstOutputSocket.FirstConnection);
        }
        
        public override void CheckForErrors()
        {
            base.CheckForErrors();
#if UNITY_EDITOR
            ErrorPlayActionHasNoSound = adx2Action == Adx2Actions.Play && !HasSound;
#endif
        }
    }
}
