using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShaderController : MonoBehaviour
{
    [SerializeField, Range(-1, 1)] private float curveX;
    [SerializeField, Range(-1, 1)] private float curveY;
    [SerializeField] private Material[] materials;
    private GameManager gameManager;
    private float waitForSecods;
    private bool gameOn;
    private float curveXRandom;
    private float curveYRandom;
    private float interpolateTime = 2;


    // Update is called once per frame

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        waitForSecods = gameManager.WaitTime;
        curveX = 0;
        curveY = 0;
        gameOn = gameManager.GameOn;
        StartCoroutine(StartCurveWorld());

        foreach (var m in materials)
        {
            m.SetFloat(Shader.PropertyToID("_Curve_X"), curveX);
            m.SetFloat(Shader.PropertyToID("_Curve_Y"), curveY);
        }
    }

    private IEnumerator StartCurveWorld()
    {
        yield return new WaitForSeconds(waitForSecods + 0.1f);
        while (!gameOn)
        {
            if (Random.Range(0, 10) > 4)
            {
                curveXRandom = Random.Range(-1, 2);
                curveYRandom = Random.Range(-1, 2);

                while (curveXRandom == 0)
                {
                    curveXRandom = Random.Range(-1, 2);
                }
                while (curveYRandom == 0)
                {
                    curveYRandom = Random.Range(-1, 2);
                }
            }
            gameOn = gameManager.GameOn;
            StartCoroutine(Lerp(curveXRandom, curveYRandom));
            yield return new WaitForSeconds(5);
        }
    }
    private IEnumerator Lerp(float randomX, float randomY)
    {
        float timeElapsedX = 0;
        float timeElapsedY = 0;
        while (timeElapsedX < interpolateTime && timeElapsedY < interpolateTime)
        {
            foreach (var m in materials)
            {
                m.SetFloat(Shader.PropertyToID("_Curve_X"), Mathf.Lerp(curveX, randomX, timeElapsedX / interpolateTime));
                m.SetFloat(Shader.PropertyToID("_Curve_Y"), Mathf.Lerp(curveY, randomY, timeElapsedY / interpolateTime));
                timeElapsedX += Time.deltaTime;
                timeElapsedY += Time.deltaTime;
            }
            yield return null;
        }
        curveX = randomX;
        curveY = randomY;
    }

}
