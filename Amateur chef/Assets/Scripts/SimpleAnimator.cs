using System;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SimpleAnimator : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Foldout("Target")]
    public Transform defaultTarget;

    [Foldout("Target Overrides")]
    public Transform onClickTarget, onEnterTarget, onExitTarget, onEnableTarget, onDisableTarget;

    [Foldout("Preset Data (Optional)")]
    public AnimationPreset preset;

    [Foldout("Manual Config (Used if no preset)")]
    public SimpleAnimation onClick = new(), onEnter = new(), onExit = new(), onEnable = new(), onDisable = new();
    
    private Tweener moveTweener, scaleTweener, sizeTweener;

    private void Start()
    {
        if (preset != null)
        {
            onClick = preset.onClick;
            onEnter = preset.onEnter;
            onExit = preset.onExit;
            onEnable = preset.onEnable;
            onDisable = preset.onDisable;
        }
    }

    public void OnPointerClick(PointerEventData eventData) => PlayAnimation(onClick, onClickTarget);
    public void OnPointerEnter(PointerEventData eventData) => PlayAnimation(onEnter, onEnterTarget);
    public void OnPointerExit(PointerEventData eventData) => PlayAnimation(onExit, onExitTarget);
    public void OnEnable() => PlayAnimation(onEnable, onEnableTarget);
    public void OnDisable() => PlayAnimation(onDisable, onDisableTarget);

    private void PlayAnimation(SimpleAnimation anim, Transform overrideTarget)
    {
        Transform target = overrideTarget ? overrideTarget : defaultTarget;
        if (!target) return;

        int activeTweens = 0;
        int completedTweens = 0;

        TweenCallback onAnyTweenComplete = () =>
        {
            completedTweens++;
            if (completedTweens >= activeTweens)
                anim.onFinished?.Invoke();
        };

        // Move
        if (anim.enableMove)
        {
            activeTweens++;
            if (moveTweener != null && moveTweener.IsActive())
            {
                moveTweener.ChangeEndValue(anim.moveTo, anim.moveDuration, true)
                           .SetEase(anim.ease)
                           .OnComplete(onAnyTweenComplete)
                           .Restart();
            }
            else
            {
                moveTweener = anim.moveType switch
                {
                    SimpleAnimation.AnimationType.Local => target.DOLocalMove(anim.moveTo, anim.moveDuration),
                    SimpleAnimation.AnimationType.World => target.DOMove(anim.moveTo, anim.moveDuration),
                    SimpleAnimation.AnimationType.Anchor when target is RectTransform rt => rt.DOAnchorPos(anim.moveTo, anim.moveDuration),
                    _ => null
                };
                if (moveTweener != null)
                    moveTweener.SetEase(anim.ease).OnComplete(onAnyTweenComplete) ;
            }
        }
        // Scale
        if (anim.enableScale)
        {
            activeTweens++;
            if (scaleTweener != null && scaleTweener.IsActive())
            {
                scaleTweener.ChangeEndValue(anim.scaleTo, anim.scaleDuration, true)
                            .SetEase(anim.ease)
                            .OnComplete(onAnyTweenComplete)
                            .Restart();
            }
            else
            {
                scaleTweener = target.DOScale(anim.scaleTo, anim.scaleDuration)
                                     .SetEase(anim.ease)
                                     .OnComplete(onAnyTweenComplete);
            }
            
    }

    // Size Delta (UI)
    if (anim.enableSizeDelta && target is RectTransform rect)
    {
        activeTweens++;
        if (sizeTweener != null && sizeTweener.IsActive())
        {
            sizeTweener.ChangeEndValue(anim.sizeTo, anim.sizeDuration, true)
                       .SetEase(anim.ease)
                       .OnComplete(onAnyTweenComplete)
                       .Restart();
        }
        else
        {
            sizeTweener = rect.DOSizeDelta(anim.sizeTo, anim.sizeDuration)
                              .SetEase(anim.ease)
                              .OnComplete(onAnyTweenComplete);
        }
    }

    if (anim.eventTiming == SimpleAnimation.EventTiming.OnStart || activeTweens == 0)
    {
        anim.onFinished?.Invoke();
    }
}

}

[Serializable]
public class SimpleAnimation
{
    public enum AnimationType { World, Local, Anchor }
    public enum EventTiming { OnStart, OnComplete }

    [Header("Move")]
    public bool enableMove;
    public AnimationType moveType;
    public Vector3 moveTo;
    public float moveDuration = 0.25f;

    [Header("Scale")]
    public bool enableScale;
    public Vector3 scaleTo = Vector3.one;
    public float scaleDuration = 0.25f;

    [Header("SizeDelta (UI only)")]
    public bool enableSizeDelta;
    public Vector2 sizeTo = Vector2.zero;
    public float sizeDuration = 0.25f;

    [Header("General")] 
    public Ease ease = Ease.OutQuad;
    public EventTiming eventTiming = EventTiming.OnComplete;
    public UnityEvent onFinished;
}

[CreateAssetMenu(fileName = "AnimationPreset", menuName = "Custom/Animation Preset")]
public class AnimationPreset : ScriptableObject
{
    public SimpleAnimation onClick = new();
    public SimpleAnimation onEnter = new();
    public SimpleAnimation onExit = new();
    public SimpleAnimation onEnable = new();
    public SimpleAnimation onDisable = new();
}
