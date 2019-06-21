using UnityEngine;
using ENVCode;

[RequireComponent(typeof(RectTransform))]
public class Example : MonoBehaviour
{
    [SerializeField] float m_MoveDuration = 0.5f;

    private RectTransform _rectTransform;
    public RectTransform RectTransform {
        get {
            if (_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();
            return _rectTransform;
        }
    }

    private Tween m_OpenTween;

    private void Awake()
    {
        Vector2 startPos = RectTransform.anchoredPosition;
        Vector2 endPos = new Vector2(startPos.x, startPos.y + 300f);
        m_OpenTween = EZTween.QuadraticInOut(m_MoveDuration, t => {
            RectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
        }).OnComplete(() => {
            RectTransform.anchoredPosition = startPos;
        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            m_OpenTween.Restart();
    }
}
