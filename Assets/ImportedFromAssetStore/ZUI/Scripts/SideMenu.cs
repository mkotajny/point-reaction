﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("UI/ZUI/Side-menu", 3)]
public class SideMenu : ZUIElementBase {

    [Tooltip("Click on \"Update Animated Elements\" to get all the animated elements under this object.")]
    public List<UIElement> AnimatedElements = new List<UIElement>();
    [Tooltip("Should this side-menu deactivate while its not visible on the screen.")]
    public bool DeactivateWhileInvisible = true;

    [Tooltip("When back function is called while this side-menu is visible, should back functionality be disabled and fire a custom event instead?")]
    public bool OverrideBack;
    public UnityEvent MyBack;

    private bool forceVisibilityCall;          //Used to indicate if there was a ChangeVisibility call before changing it by default at Start() so it doesn't do it.
    private float hidingTime;
    private float showingTime;
    private IEnumerator completeEventEnum;

    void Start()
    {
        if (!forceVisibilityCall)
            ChangeVisibilityImmediate(false, IgnoreEventsOnInitialization);
    }

    /// <summary>
    /// Change the visibilty of the menu by playing the desired animation.
    /// </summary>
    /// <param name="visible">Should this menu be visible or not?</param>
    /// <param name="trivial">If true, sounds won't play and events won't fire</param>
    public override void ChangeVisibility(bool visible, bool trivial = false)
    {
        if (!Initialized)
            InitializeElements();
        if (!gameObject.activeSelf)
        {
            ChangeVisibilityImmediate(Visible, true);
            gameObject.SetActive(true);
        }

        forceVisibilityCall = true;

        if (!UseSimpleActivation)
        {
            foreach (UIElement e in AnimatedElements)
            {
                if (e == null || !e.MenuDependent) continue;

                e.ChangeVisibility(visible, trivial);
            }
        }
        else
            gameObject.SetActive(visible);

        if (Visible != visible && !trivial)
        {
            if (SFXManager.Instance)
                SFXManager.Instance.PlayClip(visible ? ShowingClip : HidingClip);
            else if (ShowingClip || HidingClip)
                Debug.LogError("You're trying to play sounds with no SFXManager in the scene. Please add one via Tools>ZUI>Creation Window...>Setup", gameObject);
        }

        Visible = visible;

        if (!trivial)
        {
            if (completeEventEnum != null)
                StopCoroutine(completeEventEnum);

            if (visible)
            {
                if (OnShow != null)
                    OnShow.Invoke();
                if (OnShowComplete != null)
                    completeEventEnum = FireEventAfter(OnShowComplete, showingTime);
            }
            else if (!visible)
            {
                if (OnHide != null)
                    OnHide.Invoke();
                //Only start waiting to fire the hide complete event if the element will not deactivate (because if it's deactivated before firing then the event won't fire)..
                //...and fire it in DeactivateMe function instead
                if (OnHideComplete != null && !DeactivateWhileInvisible)
                {
                    completeEventEnum = FireEventAfter(OnHideComplete, hidingTime);
                }
            }

            //Since coroutines can only start on active objects, then don't try starting them if the object isn't active.
            if (gameObject.activeInHierarchy && completeEventEnum != null)
                StartCoroutine(completeEventEnum);
        }

        if (DeactivateWhileInvisible)
        {
            if (!visible)
                Invoke("DeactivateMe", hidingTime);
            else
                CancelInvoke("DeactivateMe");
        }
    }
    /// <summary>
    /// Change the visibilty of the menu instantly without playing animation.
    /// </summary>
    /// <param name="visible">Should this menu be visible or not?</param>
    /// <param name="trivial">If true, sounds won't play and events won't fire</param>
    public override void ChangeVisibilityImmediate(bool visible, bool trivial = false)
    {
        if (!Initialized)
            InitializeElements();
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        forceVisibilityCall = true;

        if (!UseSimpleActivation)
        {
            foreach (UIElement e in AnimatedElements)
            {
                if (e == null || !e.MenuDependent) continue;

                e.ChangeVisibilityImmediate(visible, trivial);
            }
        }
        else
            gameObject.SetActive(visible);

        if (Visible != visible && !trivial)
        {
            if (SFXManager.Instance)
                SFXManager.Instance.PlayClip(visible ? ShowingClip : HidingClip);
            else if (ShowingClip || HidingClip)
                Debug.LogError("You're trying to play sounds with no SFXManager in the scene. Please add one via Tools>ZUI>Creation Window...>Setup", gameObject);
        }

        Visible = visible;

        if (!trivial)
        {
            if (visible)
            {
                if (OnShow != null)
                    OnShow.Invoke();
                if (OnShowComplete != null)
                    OnShowComplete.Invoke();
            }
            else if (!visible)
            {
                if (OnHide != null)
                    OnHide.Invoke();
                if (OnHideComplete != null)
                    OnHideComplete.Invoke();
            }
        }

        if (DeactivateWhileInvisible && !visible)
            DeactivateMe(false);
    }

    /// <summary>
    /// The duration it takes to finish the hiding animation.
    /// </summary>
    /// <returns></returns>
    public float GetAllHidingTime()
    {
        if (hidingTime != 0)
            return hidingTime;

        for (int i = 0; i < AnimatedElements.Count; i++)
        {
            UIElement uiA = AnimatedElements[i];
            float uiAHidingTime = uiA.GetAllHidingTime();

            if (uiAHidingTime > hidingTime)
                hidingTime = uiAHidingTime;
        }
        return hidingTime;
    }
    /// <summary>
    /// The duration it takes to finish the showing animation.
    /// </summary>
    /// <returns></returns>
    public float GetAllShowingTime()
    {
        if (showingTime != 0)
            return showingTime;

        for (int i = 0; i < AnimatedElements.Count; i++)
        {
            UIElement uiA = AnimatedElements[i];
            float uiAShowingTime = uiA.GetAllShowingTime();

            if (uiAShowingTime > showingTime)
                showingTime = uiAShowingTime;
        }
        return showingTime;
    }

    /// <summary>
    /// Initialize UIElements, call this first if you plan to change visibility in the same frame this menu is activated at.
    /// </summary>
    public void InitializeElements()
    {
        if (Initialized) return;

        for (int i = 0; i < AnimatedElements.Count; i++)
        {
            if (AnimatedElements[i] != null)
                AnimatedElements[i].Initialize();
            else
            {
                AnimatedElements.RemoveAt(i);
                i--;
            }
        }
        hidingTime = GetAllHidingTime();
        showingTime = GetAllShowingTime();
        Initialized = true;
    }

    #region Events Control
    IEnumerator FireEventAfter(UnityEvent e, float s)
    {
        yield return new WaitForSeconds(s);

        e.Invoke();
    }
    #endregion
}