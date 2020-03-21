using System;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace DG.Tweening
{
    public static class CriAtomDotweenExtension
    {
        #region AtomCraft
        
        public static TweenerCore<float, float, FloatOptions> DOFade(this CriAtomSource target, float endValue, float duration)
        {
            if (endValue < 0) endValue = 0;
            else if (endValue > 1) endValue = 1;
            TweenerCore<float, float, FloatOptions> t = DOTween.To(() => target.volume, x => target.volume = x, endValue, duration);
            t.SetTarget(target);
            return t;
        }
        
        public static TweenerCore<float, float, FloatOptions> DOPitch(this CriAtomSource target, float endValue, float duration)
        {
            TweenerCore<float, float, FloatOptions> t = DOTween.To(() => target.pitch, x => target.pitch = x, endValue, duration);
            t.SetTarget(target);
            return t;
        }

        public static TweenerCore<float, float, FloatOptions>  DOSetAisacControl(this CriAtomSource target, CriAtomExPlayback playback, string aisacName, float startValue,float endValue, float duration)
        {
            return DOSetAisacControl(target.player, playback, aisacName, startValue, endValue, duration);
        }
        
        public static TweenerCore<float, float, FloatOptions>  DOSetBusSendLevel(this CriAtomSource target, string busName, float startLevel,float endLevel, float duration)
        {
            return DOSetBusSendLevel(target.player, busName, startLevel, endLevel, duration);
        }
        
        public static TweenerCore<float, float, FloatOptions>  DOSetBusSendLevelOffset(this CriAtomSource target, string busName, float startLevel,float endLevel, float duration)
        {
            return DOSetBusSendLevelOffset(target.player, busName, startLevel, endLevel, duration);
        }

        #endregion

        #region AtomExPlayer
        
        public static TweenerCore<float, float, FloatOptions> DOFade(this CriAtomExPlayer target, CriAtomExPlayback playback, float startValue, float endValue, float duration)
        {
            if (endValue < 0) endValue = 0;
            else if (endValue > 1) endValue = 1;
            TweenerCore<float, float, FloatOptions> t = DOTween.To(() => startValue, (x) =>
            {
                target.SetVolume(x);
                target.Update(playback);
            }, endValue, duration);
            t.SetTarget(target);
            
            return t;
        }
        
        public static TweenerCore<float, float, FloatOptions> DOPitch(this CriAtomExPlayer target, CriAtomExPlayback playback, float startValue, float endValue, float duration)
        {
            TweenerCore<float, float, FloatOptions> t = DOTween.To(() => startValue, (x)=>
            {
                target.SetPitch(x);
                target.Update(playback);
            }, endValue, duration);
            t.SetTarget(target);
            return t;
        }
        
        public static TweenerCore<float, float, FloatOptions>  DOSetAisacControl(this CriAtomExPlayer target, CriAtomExPlayback playback, string aisacName, float startValue,float endValue, float duration)
        {
            TweenerCore<float, float, FloatOptions> t = DOTween.To(()=> startValue, x=>
            {
                target.SetAisacControl(aisacName, x);
                target.Update(playback);
            }, endValue, duration);
            t.SetTarget(target);
            
            return t;
        }
        
        public static TweenerCore<float, float, FloatOptions>  DOSetAisacControl(this CriAtomExPlayer target, CriAtomExPlayback playback, uint controlId, float startValue,float endValue, float duration)
        {
            TweenerCore<float, float, FloatOptions> t = DOTween.To(()=> startValue, x=>
            {
                target.SetAisacControl(controlId, x);
                target.Update(playback);
            }, endValue, duration);
            t.SetTarget(target);
            
            return t;
        }
        
        
        public static TweenerCore<float, float, FloatOptions>  DOSetBusSendLevel(this CriAtomExPlayer target, string busName, float startLevel,float endLevel, float duration)
        {
            TweenerCore<float, float, FloatOptions> t = DOTween.To(()=> startLevel, x=>
            {
                target.SetBusSendLevel(busName, x);
            }, endLevel, duration);
            t.SetTarget(target);
            
            return t;
        }
        
        public static TweenerCore<float, float, FloatOptions>  DOSetBusSendLevelOffset(this CriAtomExPlayer target, string busName, float startLevel,float endLevel, float duration)
        {
            TweenerCore<float, float, FloatOptions> t = DOTween.To(()=> startLevel, x=>
            {
                target.SetBusSendLevelOffset(busName, x);
            }, endLevel, duration);
            t.SetTarget(target);
            
            return t;
        }
        
        public static TweenerCore<float, float, FloatOptions>  DOSetPan3dAngle(this CriAtomExPlayer target, CriAtomExPlayback playback, float startAngle,float endAngle, float duration)
        {
            TweenerCore<float, float, FloatOptions> t = DOTween.To(()=> startAngle, x=>
            {
                target.SetPan3dAngle(x);
                target.Update(playback);
            }, endAngle, duration);
            t.SetTarget(target);
            
            return t;
        }
        
        public static TweenerCore<float, float, FloatOptions>  DOSetPan3dInteriorDistance(this CriAtomExPlayer target, CriAtomExPlayback playback, float startDistance,float endDistance, float duration)
        {
            TweenerCore<float, float, FloatOptions> t = DOTween.To(()=> startDistance, x=>
            {
                target.SetPan3dInteriorDistance(x);
                target.Update(playback);
            }, endDistance, duration);
            t.SetTarget(target);
            
            return t;
        }
        
        public static TweenerCore<float, float, FloatOptions>  DOSetPan3dVolume(this CriAtomExPlayer target, CriAtomExPlayback playback, float startVolume,float endVolume, float duration)
        {
            TweenerCore<float, float, FloatOptions> t = DOTween.To(()=> startVolume, x=>
            {
                target.SetPan3dVolume(x);;
                target.Update(playback);
            }, endVolume, duration);
            t.SetTarget(target);
            
            return t;
        }
        
        #endregion
    }
}