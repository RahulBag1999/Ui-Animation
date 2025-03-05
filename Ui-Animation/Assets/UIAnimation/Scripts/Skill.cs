using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] private UIHandler _uiHandler;
    [SerializeField] private RotateUIAround _rotateUiAround;
    [SerializeField] private string _skillName;

    private int _index = -1;

    public void SetData(SkillData skillData, bool isZoomIn, int index)
    {
        _rotateUiAround.SetData(skillData.distanceFromCeter, skillData.targetAngle, skillData.sizeFactor);
        _rotateUiAround.RotateToAngle(isZoomIn);

        _index = index;
    }

    public void OnClick()
    {
        if (!_uiHandler.IsSkillTreeOpened)
            return;
        UIHandler.OnClickSkillItem?.Invoke(_index, _skillName);
    }
}
