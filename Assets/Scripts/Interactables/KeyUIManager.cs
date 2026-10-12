using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;                   // Remove this line if you use legacy Text

/// <summary>
/// Attach this to a Canvas GameObject (Screen Space - Overlay).
/// 
/// Canvas children needed:
///   - KeyWorldAnchorImage  (Image)  - a temporary copy that flies from world-to-screen
///   - KeySlotImage         (Image)  - the destination slot in the top-left corner
///   - KeyCountText         (TMP_Text / Text) - optional count label
/// 
/// Assign the key icon sprite in the Inspector.
/// </summary>
public class KeyUIManager : MonoBehaviour
{
    [Header("References")]
    public Canvas canvas;                   
    public RectTransform keySlot;           
    public Image flyingKeyImage;            
    public TMP_Text keyCountText;         

    [Header("Key Icon")]
    public Sprite keySprite;               
    [Header("Animation Settings")]
    public float flyDuration = 0.8f;       
    public AnimationCurve flyCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private int _keyCount = 0;
    private Camera _mainCam;

    void Start()
    {

        _mainCam = Camera.main;

        // Hide the flying key image by default
        if (flyingKeyImage != null)
            flyingKeyImage.gameObject.SetActive(false);

        if (keySlot != null)
        {
                keySlot.gameObject.SetActive(false);
        }
            

        UpdateCountText();
    }

    
    public void OnKeyPickedUp(string keyID, Vector3 worldPosition)
    {
        _keyCount++;
       //UpdateCountText();

        Vector2 screenStart = WorldToCanvasPosition(worldPosition);
        Debug.Log(screenStart+ "Vector ");
       
        StartCoroutine(FlyKeyToCorner(screenStart));
    }

    
    private IEnumerator FlyKeyToCorner(Vector2 startCanvasPos)
{
    flyingKeyImage.gameObject.SetActive(true);

    RectTransform flyRT = flyingKeyImage.rectTransform;

    flyRT.anchoredPosition = startCanvasPos;
    flyRT.localScale = Vector3.one * 1.2f ;

    Vector2 endPos = keySlot.anchoredPosition;

    float elapsed = 0f;

    yield return new WaitForSeconds(0.5f);
    while (elapsed < flyDuration)
    {
        elapsed += Time.deltaTime;

        float t = elapsed / flyDuration;

        flyRT.anchoredPosition =
            Vector2.Lerp(startCanvasPos, endPos, t);

        yield return null;
    }

    flyRT.anchoredPosition = endPos;

    flyingKeyImage.gameObject.SetActive(false);

    keySlot.gameObject.SetActive(true);
}
    
    private IEnumerator PunchScale(RectTransform target, float peakScale, float duration)
    {
        float half = duration * 0.5f;
        float elapsed = 0f;

        // Scale up
        while (elapsed < half)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / half;
            target.localScale = Vector3.Lerp(Vector3.one, Vector3.one * peakScale, t);
            yield return null;
        }

        elapsed = 0f;

        // Scale back down
        while (elapsed < half)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / half;
            target.localScale = Vector3.Lerp(Vector3.one * peakScale, Vector3.one, t);
            yield return null;
        }

        target.localScale = Vector3.one;
    }

    
private Vector2 WorldToCanvasPosition(Vector3 worldPos)
{
    Vector3 screenPoint = _mainCam.WorldToScreenPoint(worldPos);

    return new Vector2(
        screenPoint.x,
        screenPoint.y - Screen.height
    );
}

    private void UpdateCountText()
    {
        if (keyCountText != null)
            keyCountText.text = "x" + _keyCount;
    }
}
