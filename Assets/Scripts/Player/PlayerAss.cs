using CodeKriebels.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAss : MonoBehaviour
{
    public Transform leftCheek;
    public Transform rightCheek;

    public float vibrationSpeed = 10f;

    public float minFartLength = 1f;
    public float maxFartLength = 2f;

    public float fart;
    public float fartSideScale = 1f;
    public float fartUpScale = 1f;
    public float fartMovement = 0.5f;
    public float maxFart = 1f;
    public float noFartTime = 1f;

    private Vector3 leftStartPos;
    private Vector3 rightStartPos;


    public void DoFart ()
    {
        fart += Random.Range(minFartLength, maxFartLength);
    }

    private void Awake()
    {
        leftStartPos = leftCheek.localPosition;
        rightStartPos = rightCheek.localPosition;
    }

    private void Update ()
    {
        var fartSin = (1f + Mathf.Sin(Time.timeSinceLevelLoad * vibrationSpeed)) / 2f * fart;
        var fartCos = (1f + Mathf.Cos(Time.timeSinceLevelLoad * vibrationSpeed)) / 2f * fart;

        leftCheek.localScale = new Vector3((1f + fartSin) * fartSideScale, (1f + fartCos) * fartUpScale, 1f);
        rightCheek.localScale = new Vector3((1f + fartSin) * fartSideScale, (1f + fartCos) * fartUpScale, 1f);

        leftCheek.localPosition = leftStartPos + Vector3.right * -fartSin * fartMovement;
        rightCheek.localPosition = rightStartPos + Vector3.right * fartSin * fartMovement;

        fart = Mathf.Clamp(fart - Time.deltaTime / noFartTime, 0, maxFart);
    }


    //private IEnumerator AnimateCheeks (FartHandler.FartSize size)
    //{
    //    var duration = Random.Range(minFartLength, maxFartLength);
    //    var t = 0f;

    //    while (t < duration)
    //    {
    //        yield return new WaitForEndOfFrame(); 

    //        t += Time.deltaTime / duration;
    //    }
    //} 
}
