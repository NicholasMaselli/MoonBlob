using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using TMPro;

public class BetterButton : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Interactive Variables")]
    public bool interactable = true;
    [HideInInspector] public bool active = false; // deprecated
    public bool toggle = false;
    public bool toggleActive = false;

    public bool resetAfterClick = false; 
    public bool keepActive = false; // deprecated
    public bool keepActiveUntilClick = false; // deprecated

    [Header("Change Variables")]
    public bool useImage = true;
    public bool useColor = false;
    public bool useTextColor = false;

    [Header("Button Content")]
    public Image targetImage;
    public TextMeshProUGUI targetText;

    [Header("Image Variables")]    
    public Sprite normalImage;
    public Sprite highlightedImage;
    public Sprite pressedImage;
    public Sprite disabledImage;

    [Header("Color Variables")]
    public Color normalColor = Color.white;
    public Color highlightedColor = Color.white;
    public Color pressedColor = Color.white;
    public Color disabledColor = Color.white;

    [Header("Text Color Variables")]
    public Color normalTextColor = Color.white;
    public Color highlightedTextColor = Color.white;
    public Color pressedTextColor = Color.white;
    public Color disabledTextColor = Color.white;
        
    [Header("Toggle Image Variables")]
    public Sprite toggledImage;
    public Sprite toggledHighlightedImage;
    public Sprite toggledPressedImage;
    public Sprite toggledDisabledImage;

    [Header("Toggle Color Variables")]
    public Color toggledNormalColor = Color.white;
    public Color toggledHighlightedColor = Color.white;
    public Color toggledPressedColor = Color.white;
    public Color toggledDisabledColor = Color.white;

    [Header("Toggle Text Color Variables")]
    public Color toggledNormalTextColor = Color.white;
    public Color toggledHighlightedTextColor = Color.white;
    public Color toggledPressedTextColor = Color.white;
    public Color toggledDisabledTextColor = Color.white;

    [Header("Audio Variables")]
    public AudioSource audioSource;
    public AudioClip clickAudio;
    public AudioClip hoverAudio;
    public float clickVolume = 1.5f;
    public float hoverVolume = 0.6f;

    public UnityEvent OnClick;
    public UnityEvent OnDown;
    public UnityEvent OnUp;
    public UnityEvent OnEnter;
    public UnityEvent OnExit;

    //---------------------------------------------------------------------------------------------------//
    //Interactive Functions                                                                                
    //---------------------------------------------------------------------------------------------------//
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (interactable)
        {

            audioSource.PlayOneShot(clickAudio, clickVolume);
            OnClick.Invoke();

            if (keepActiveUntilClick)
            {
                active = !active;
            }

            if (resetAfterClick && !active)
            {
                SetNormal();
            }            
        }        
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (interactable && (!active || (active && !keepActive)))
        {
            OnDown.Invoke();
            SetPressed();
        }
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (interactable && PointerInsideGameObject(eventData) && (!active || (active && !keepActive)))
        {
            OnUp.Invoke();
            SetHighlighted();
        }
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (interactable && (!active || (active && !keepActive)))
        {
            audioSource.PlayOneShot(hoverAudio, hoverVolume);
            OnEnter.Invoke();
            SetHighlighted();
        }        
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (interactable && (!active || (active && !keepActive)))
        {
            OnExit.Invoke();
            SetNormal();
        }        
    }
    //---------------------------------------------------------------------------------------------------//


    //---------------------------------------------------------------------------------------------------//
    //Button Functions                                                                                
    //---------------------------------------------------------------------------------------------------//
    public virtual void SetNormal()
    {
        interactable = true;
        if (useImage)
        {
            targetImage.sprite = normalImage;
            if (toggle && !toggleActive)
            {
                targetImage.sprite = normalImage;
            }
            else if (toggle && toggleActive)
            {
                targetImage.sprite = toggledImage;
            }
        }

        if (useColor)
        {
            targetImage.color = normalColor;
            if (toggle && !toggleActive)
            {
                targetImage.color = normalColor;
            }
            else if (toggle && toggleActive)
            {
                targetImage.color = toggledNormalColor;
            }
        }

        if (useTextColor)
        {
            targetText.color = normalTextColor;
            if (toggle && !toggleActive)
            {
                targetText.color = normalTextColor;
            }
            else if (toggle && toggleActive)
            {
                targetText.color = toggledNormalTextColor;
            }
        }
    }

    public virtual void SetHighlighted()
    {
        interactable = true;
        if (useImage)
        {
            targetImage.sprite = highlightedImage;
            if (toggle && toggleActive)
            {
                targetImage.sprite = toggledHighlightedImage;
            }
        }

        if (useColor)
        {
            targetImage.color = highlightedColor;
            if (toggle && toggleActive)
            {
                targetImage.color = toggledHighlightedColor;
            }
        }

        if (useTextColor)
        {
            targetText.color = highlightedTextColor;
            if (toggle && toggleActive)
            {
                targetText.color = toggledHighlightedTextColor;
            }
        }        
    }

    public virtual void SetPressed()
    {
        interactable = true;
        if (useImage)
        {
            targetImage.sprite = pressedImage;
            if (toggle && toggleActive)
            {
                targetImage.sprite = toggledPressedImage;
            }
        }

        if (useColor)
        {
            targetImage.color = pressedColor;
            if (toggle && toggleActive)
            {
                targetImage.color = toggledPressedColor;
            }
        }

        if (useTextColor)
        {
            targetText.color = pressedTextColor;
            if (toggle && toggleActive)
            {
                targetText.color = toggledPressedTextColor;
            }
        }
    }

    public virtual void SetDisabled()
    {
        interactable = false;
        if (useImage)
        {
            targetImage.sprite = disabledImage;
            if (toggle && toggleActive)
            {
                targetImage.sprite = toggledDisabledImage;
            }
        }

        if (useColor)
        {
            targetImage.color = disabledColor;
            if (toggle && toggleActive)
            {
                targetImage.color = toggledDisabledColor;
            }
        }

        if (useTextColor)
        {
            targetText.color = disabledTextColor;
            if (toggle && toggleActive)
            {
                targetImage.color = toggledDisabledTextColor;
            }
        }
    }
    //---------------------------------------------------------------------------------------------------//

    //---------------------------------------------------------------------------------------------------//
    //Confirmation Functions                                                                                
    //---------------------------------------------------------------------------------------------------//
    public bool PointerInsideGameObject(PointerEventData eventData)
    {
        foreach (GameObject eventObject in eventData.hovered)
        {
            if (eventObject == this.gameObject)
            {
                return true;
            }
        }
        return false;
    }
    //---------------------------------------------------------------------------------------------------//

    //---------------------------------------------------------------------------------------------------//
    //Active Functions                                                                                
    //---------------------------------------------------------------------------------------------------//
    public virtual void Activate()
    {

    }

    public virtual void Deactivate()
    {
        active = false;
        SetNormal();
    }
    //---------------------------------------------------------------------------------------------------//

    //---------------------------------------------------------------------------------------------------//
    //Toggle Functions                                                                                
    //---------------------------------------------------------------------------------------------------//

    // Toggle Currently used in friends panel
    public void ChangeToggle()
    {
        toggleActive = !toggleActive;
    }

    public void ChangeToggle(bool toggle)
    {
        toggleActive = toggle;
    }
    //---------------------------------------------------------------------------------------------------//

}
