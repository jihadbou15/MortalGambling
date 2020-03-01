using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapPhase : MonoBehaviour
{
    [SerializeField] private Image _attackImage = null;
    [SerializeField] private Image _defendImage = null;
    [SerializeField] private Transform _Selection = null;
    private float _lerpSpeed;
    private bool _isAttacking;

    // Update is called once per frame
    void Update()
    {
        if (_isAttacking) ShowAttackingImage();
        else ShowDefendImage();

    }

    private void ShowAttackingImage()
    {
        _Selection.localPosition = Vector3.Lerp(_Selection.localPosition, _attackImage.transform.localPosition, 0.2f);
    } 

    private void ShowDefendImage()
    {
        _Selection.localPosition = Vector3.Lerp(_Selection.localPosition, _defendImage.transform.localPosition, 0.2f);
    }

    public void SetIsAttacking(bool isAttacking)
    {
        _isAttacking = isAttacking;
    }
}
