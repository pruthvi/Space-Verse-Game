using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Overlay out of range effect/Restart Effect
/// </summary>
public class OverlayDarkEffect : MonoBehaviour
{
    [HideInInspector] public bool canFadeOut = false;       //  Flag to enable Fade OUT Animation
    [HideInInspector] public bool canFadeIn = false;        //  Flag to enable Fade IN Animation
    [HideInInspector] public float speedMultiplier = 1;     //  Animation Speed Multiplier

    private Image _overlayImage;                            //  Overlay Image to Add FadeIN/OUT Effect
    private float _alphaColor = 0;                          //  Alpha Color of the Overlay Image
    private float _rColor = 0;              //  Red Color
    private float _gColor = 0;              //  Green Color
    private float _bColor = 0;              //  Blue Color
    
    /// <summary>
    /// Caching reference to the required Components
    /// </summary>
    private void Awake()
    {
        _overlayImage = GetComponent<Image>();

        var _color = _overlayImage.color;
        _rColor = _color.r;
        _gColor = _color.g;
        _bColor = _color.b;
    }

    /// <summary>
    /// Resetting Alpha Color to be transparent
    /// </summary>
    void Start()
    {
        _alphaColor = 0;
    }

    /// <summary>
    /// Applying the Effect
    /// </summary>
    void Update()
    {
        //  Fade In and make the scene go BLACK
        if (canFadeOut)
        {
            _alphaColor += 0.008f * speedMultiplier;
        }

        //  Fade Out and make the scene return back to REGULAR
        if (canFadeIn)
        {
            _alphaColor -= 0.009f * speedMultiplier;
        }

        _alphaColor = Mathf.Clamp(_alphaColor, 0, 1f);
        ModifyColor();
    }

    /// <summary>
    /// Modify the Image color by modifying alpha color
    /// </summary>
    private void ModifyColor()
    {
        //  Setting the new modified color
        _overlayImage.color = new Color(_rColor, _gColor, _bColor, _alphaColor);
        
        if (canFadeIn)
        {
            //  Disable Overlay Effect once FadeIn Effect is completed
            if (_alphaColor <= 0)
            {
                this.gameObject.SetActive(false);
                canFadeIn = false;
            }
        }
    }
}
