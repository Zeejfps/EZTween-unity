# EZTween

EZTween is a simple tweening plugin for Unity. I hope to continue expanding this library and add new features with time.

# Getting Started
EZTween is located inside a ENVCode namespace
```csharp
using ENVCode;
```

EZTween will initialize itself on first use, however, if you want to control when the initialization takes place use the Init method.
```csharp
private void Awake() {
    EZTween.Init();
}
```

EZTween provides several built-in interpolation functions that can be utilized.  


```csharp
// A Quadratic In Out Interpolation
EZTween.QuadraticInOut(1f, t => {
    pos = Vector3.Lerp(startPos, endPos, t);
}).Play();

// Or a simple Linear Interpolation
EZTween.Linear(1f, t => {
    pos = Vector3.Lerp(startPos, endPos, t);
}).Play();
```

A Tween can also be created and saved for later use and re-use.
```csharp
Tween open = EZTween.CubicInOut(0.5f, t => {
    // Do something here
});
```

Moreover, some callback functions can be added to a Tween.
```csharp
Tween tween = EZTween.Linear(2f, t => {
    // Tween code here
}).OnComplete(() => {
    // What to do when complete
});
```

Also, a Tween contains multiple useful functions.
```csharp
open.Play();
open.Restart();
open.Pause();
open.stop();
```

You can use a custom interpolation function by creating a Tween yourself, then telling EZTween to Play it.
```csharp
Tween tween = new Tween(myCustomFunc, 1f, (t) => {
    // Tween code
});

EZTween.Play(tween);
// OR
tween.Play();
```

# Simple Example

```csharp
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
```