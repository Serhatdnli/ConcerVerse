using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class LightController : MonoBehaviour
{
    private float x, y;
    private void Start()
    {
        Turn();
    }
    void Turn()
    {
        x = Random.Range(-90f, -180f);
        y = Random.Range(90f, -90f);
        transform.DOLocalRotate(new Vector3(x, y, 0f), 3f).SetEase(Ease.Linear).OnComplete(() =>
        {
            Turn();
        });
    }

}
