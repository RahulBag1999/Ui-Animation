using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class SkillData
{
    public float targetAngle;
    public float sizeFactor;
    public float distanceFromCeter;
}
public class UIHandler : MonoBehaviour
{
    [SerializeField] private Button _skillButton;
    [SerializeField] private RectTransform _skillButtonRectTransform;
    [SerializeField] private float _zoomSpeed = 0.1f;
    [SerializeField] private float _zoomInMultiplier;
    [SerializeField] private float _zoomOutMultiplier;

    [SerializeField] private Vector2 _source;
    [SerializeField] private Vector2 _destination;
    [SerializeField] private float _moveDuration = 1.5f;

    [SerializeField] private CanvasGroup _overlay;
    [SerializeField] private CanvasGroup _bgCanvasGroup;
    [SerializeField] private float _fadeDuration = 0.1f;

    [SerializeField] private List<Skill> _skillList = new List<Skill>();
    [Header("Zoom in")]
    [SerializeField] private List<SkillData> _skillDataZoomIn = new List<SkillData>();
    [Header("Zoom out")]
    [SerializeField] private List<SkillData> _skillDataZoomOut = new List<SkillData>();

    [SerializeField] private TMP_Text _skillNameText;

    public static Action<int, string> OnClickSkillItem;

    private bool _isSkillTreeOpened = false;

    public bool IsSkillTreeOpened => _isSkillTreeOpened;

    private void OnEnable()
    {
        OnClickSkillItem += HandleSkillButtonClick;
    }

    private void Start()
    {
        _bgCanvasGroup.alpha = 0;
    }  

    private void HandleOverlay(bool isOpen)
    {
        if (isOpen)
            StartCoroutine(FadeObject(_overlay, 0, 0.5f, 0.15f));
        else
            StartCoroutine(FadeObject(_overlay, 0.5f, 0, 0.15f));
    }

    private void HandleSkillsButtonZoomIn(bool isZoomIn)
    {
        for(int i=0; i<_skillList.Count; i++)
        {
            _skillList[i].SetData(_skillDataZoomIn[i], isZoomIn, i);
        }
    }

    private void HandleSkillsButtonZoomOut(bool isZoomIn)
    {
        for (int i = 0; i < _skillList.Count; i++)
        {
            _skillList[i].SetData(_skillDataZoomOut[i], isZoomIn, i);
        }
    }

    private IEnumerator ZoomObject(RectTransform rectTransform, float targetScale)
    {
        float startScale = rectTransform.localScale.x;
        float elapsedTime = 0f;

        while(elapsedTime < _zoomSpeed)
        {
            float newScale = Mathf.Lerp(startScale, targetScale, (elapsedTime / _zoomSpeed));
            rectTransform.localScale = new Vector3(newScale, newScale, 1f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        rectTransform.localScale = new Vector3(targetScale, targetScale, 1f);
    }

    private IEnumerator MoveObject(RectTransform rectTransform, Vector2 source, Vector2 destination, float duration)
    {
        float elapsedTime = 0f;

        while(elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            rectTransform.anchoredPosition = Vector2.Lerp(source, destination, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        rectTransform.anchoredPosition = destination;
    }

    private IEnumerator FadeObject(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float fadeDuration)
    {
        float elapsedTime = 0f;
        canvasGroup.alpha = startAlpha;
        float currentAlpha = canvasGroup.alpha;

        while(elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, (elapsedTime / fadeDuration));
            canvasGroup.alpha = alpha;
            yield return null;
        }
        canvasGroup.alpha = endAlpha;
    }

    private void HandleSkillButtonClick(int index, string skillName)
    {
        switch (index) 
        {
            //case 0:
            //    _skillList[0].SetData(_skillDataZoomIn[2], false, 2);
            //    break;
            //case 1:
            //    _skillList[1].SetData(_skillDataZoomIn[2], false, 2);

            //    _skillList[0].SetData(_skillDataZoomIn[1], false, 1);
            //    _skillList[2].SetData(_skillDataZoomIn[3], false, 3);
            //    _skillList[3].SetData(_skillDataZoomIn[4], false, 4);
            //    _skillList[4].SetData(_skillDataZoomIn[0], false, 0);
            //    break;

            case 3:
                _skillList[3].SetData(_skillDataZoomIn[2], true, 2);

                _skillList[0].SetData(_skillDataZoomIn[4], true, 4);
                _skillList[1].SetData(_skillDataZoomIn[0], true, 0);
                _skillList[2].SetData(_skillDataZoomIn[1], true, 1);
                _skillList[4].SetData(_skillDataZoomIn[3], true, 3);

                _skillNameText.text = skillName;
                break;
        }       
    }

    private void Update()
    {
        if(!_isSkillTreeOpened)
        {
            if (Input.GetKeyUp(KeyCode.I))
            {
                StartCoroutine(ZoomObject(_skillButtonRectTransform, _zoomInMultiplier));
                StartCoroutine(MoveObject(_skillButtonRectTransform, _source, _destination, _moveDuration));
                StartCoroutine(FadeObject(_bgCanvasGroup, 0, 1, _fadeDuration));

                HandleSkillsButtonZoomIn(true);
                HandleOverlay(true);

                _isSkillTreeOpened = true;
            }
        }
        
        if(_isSkillTreeOpened)
        {
            if (Input.GetKeyUp(KeyCode.O))
            {
                StartCoroutine(ZoomObject(_skillButtonRectTransform, _zoomOutMultiplier));
                StartCoroutine(MoveObject(_skillButtonRectTransform, _destination, _source, _moveDuration));
                StartCoroutine(FadeObject(_bgCanvasGroup, 1, 0, _fadeDuration));

                HandleSkillsButtonZoomOut(false);
                HandleOverlay(false);

                _isSkillTreeOpened = false;
            }
        }       
    }
}
