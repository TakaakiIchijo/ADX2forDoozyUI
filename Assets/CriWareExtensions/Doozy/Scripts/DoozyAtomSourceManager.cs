using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Doozy.Engine.Utils;
using UnityEngine;

namespace Doozy.Engine.ADX2
{
    public class DoozyAtomSourceManager : MonoBehaviour
    {
        private static DoozyAtomSourceManager s_instance;
        private static bool s_initialized;
        
        private static List<CriAtomSource> criAtomSources = new List<CriAtomSource>();

        public static DoozyAtomSourceManager Instance
        {
            get
            {
                if (s_instance != null) return s_instance;
                s_instance = FindObjectOfType<DoozyAtomSourceManager>();

                if (s_instance == null)
                {
                    GameObject instance = new GameObject(CriAtomDoozyUtils.Manager_GameObject_Name);
                    instance.AddComponent<DoozyAtomSourceManager>();
                    s_instance = instance.GetComponent <DoozyAtomSourceManager>();
                }
                
                return s_instance;
            }
        }
        
        private void Awake() { s_initialized = true; }
        
        public static void MuteAll()
        {
            //SoundyController.MuteAll();
            //Doozy経由のものだけミュート//
        }
        
        public static void PauseAll()
        {
            //SoundyController.MuteAll();
            //Doozy経由のものだけミュート//
        }
        
        public CriAtomSource Play(string cueSheetName, string cueName, Vector3 position = new Vector3())
        {
            if (!s_initialized) s_instance = Instance;
            if (CriAtom.GetCueSheet(cueSheetName) == null)
            {
                Debug.LogError("cueSheet "  + cueSheetName + " is not loaded");
                return null;
            }
            
            if (string.IsNullOrEmpty(cueName)) return null;

            CriAtomSource criAtomSource = GetCreatedAtomSourceFromPool(cueSheetName);

            criAtomSource.cueName = cueName;
            
            if (position != Vector3.zero)
            {
                criAtomSource.gameObject.transform.position = position;  
            }
            
            criAtomSource.Play();
            return criAtomSource;
        }

        public void StopAllSounds()
        {
            criAtomSources.ForEach(c => c.Stop());
        }

        public void PauseAllSounds()
        {
            criAtomSources.ForEach(c => c.Pause(true));
        }

        public void UnpauseAllSounds()
        {
            criAtomSources.ForEach(c => c.Pause(false));
        }

        public void MuteAllSounds()
        {
            criAtomSources.ForEach(c => c.volume = 0f);
        }        

        public void UnmuteAllSounds()
        {
            criAtomSources.ForEach(c => c.volume = 1f); 
        }        
        
        //public static CriAtomDoozyAtomSourceManager AddToScene(bool selectGameObjectAfterCreation = false) { return DoozyUtils.AddToScene<CriAtomDoozyAtomSourceManager>(CriAtomDoozyUtils.Manager_GameObject_Name, true, selectGameObjectAfterCreation); }

        private CriAtomSource GetCreatedAtomSourceFromPool(string cueSheetName)
        {
            CriAtomSource atomSource = criAtomSources.FirstOrDefault(a => a.cueSheet == cueSheetName);

            if (atomSource != null)
            {
                return atomSource;
            }
            else
            {
               return CreateAtomSourceComponent(cueSheetName);
            }
        }
        
        private CriAtomSource CreateAtomSourceComponent(string cueSheetName)
        {
            GameObject obj = new GameObject("AtomSource_" + cueSheetName);
            obj.transform.parent = this.gameObject.transform;
            
            CriAtomSource criAtomSource = obj.AddComponent<CriAtomSource>();
            criAtomSource.cueSheet = cueSheetName;
            criAtomSources.Add(criAtomSource);
            return criAtomSource;
        }

        public void LoadCueSheet(string cueSheetName, string acbFilePath,  string awbFilePath, Action cueSheetLoadCompleteCallback)
        {
            StartCoroutine(LoadCueSheetCoroutine(cueSheetName, acbFilePath, awbFilePath, cueSheetLoadCompleteCallback));
        }
        
        public IEnumerator LoadCueSheetCoroutine(string cueSheetName, string acbFilePath,  string awbFilePath, Action cueSheetLoadCompleteCallback)
        {
            CriAtom.AddCueSheetAsync(cueSheetName, acbFilePath, awbFilePath, null);

            while (CriAtom.CueSheetsAreLoading == true)
            {
                yield return null;
            }

            cueSheetLoadCompleteCallback();
            //cueSheet = CriAtom.GetCueSheet(cueSheetName).acb;
        }
    }
}