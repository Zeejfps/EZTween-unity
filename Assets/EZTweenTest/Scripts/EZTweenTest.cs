using UnityEngine;
using ENVCode;

public class EZTweenTest : MonoBehaviour
{
    Tween openTween = EZTween.CubicInOut(0.5f, t => {

    }).OnComplete(() => {
        //Debug.Log("Open Tween Completed");
    });


    private void Awake()
    {
        EZTween.Init();
    }

    // Start is called before the first frame update
    void Start()
    {
        Tween tween = EZTween.Linear(2f, t => {

        }).OnComplete(() => {
            //Debug.Log("Done!");
        });

        //tween.Play();
        //tween.Pause();
        //tween.Stop();
        //tween.Restart();

        //EZTween.QuadraticInOut(1f, t => {
            //Debug.Log(t);
        //}).Play();

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            //Debug.Log("Open Tween Playing");
            openTween.Play();

            EZTween.QuadraticInOut(10f, t => {
                
            }).Play("ID");
        }
    }
}
