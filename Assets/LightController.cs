using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class LightController : MonoBehaviour
{
    [SerializeField] private float xAngle1, xAngle2, yAngle1, yAngle2;
    private float x, y;

    private void Start()
    {
        Turn();
    }
    void Turn()
    {
        x = Random.Range(xAngle1, xAngle2);
        y = Random.Range(yAngle1, yAngle2);
        transform.DOLocalRotate(new Vector3(x, y, 0f), 3f).SetEase(Ease.Linear).OnComplete(() =>
        {
            Turn();
        });
    }

}
