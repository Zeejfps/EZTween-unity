using UnityEngine;
using ENVCode.EZTween;

[RequireComponent(typeof(RectTransform))]
public class Example : MonoBehaviour
{
    [SerializeField] float m_MoveDuration = 0.5f;
    [SerializeField] AnimationCurve m_Curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    private RectTransform _rectTransform;
    public RectTransform RectTransform {
        get {
            if (_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();
            return _rectTransform;
        }
    }

    // A reference to our Tween object so we can reuse it
    Tween m_OpenTween;

    void Awake()
    {
        // Save our start position
        Vector2 startPos = RectTransform.anchoredPosition;

        // Save our end position
        Vector2 endPos = new Vector2(startPos.x, startPos.y + 300f);

        /*
        // Create a QuadraticInOut Tween
        m_OpenTween = EZTween.QuadraticInOut(m_MoveDuration, t => {
            // On every update we lerp our position based on the t
            RectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
        }).OnComplete(() => {
            // When the Tween is done, we move back to the start position
            RectTransform.anchoredPosition = startPos;
        });
        //*/

        m_OpenTween = EZTween.CubicOut(m_MoveDuration, t =>
        {
            // On every update we lerp our position based on the t
            RectTransform.anchoredPosition = Vector2.LerpUnclamped(startPos, endPos, t);
        }).OnComplete(() => {
            // When the Tween is done, we move back to the start position
            RectTransform.anchoredPosition = startPos;
        });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            m_OpenTween.Restart();
    }
}
