using System;
using UnityEngine;

namespace ENVCode
{
    public partial class EZTween
    {
        private const float k_HalfPI = Mathf.PI * 0.5f;

        public static Tween Linear(float duration, Action<float> onUpdate)
        {
            return new Tween((t) => t, duration, onUpdate);
        }

        public static Tween QuadraticIn(float duration, Action<float> onUpdate)
        {
            return new Tween((t) => EaseIn(t, 2), duration, onUpdate);
        }

        public static Tween QuadraticOut(float duration, Action<float> onUpdate)
        {
            return new Tween((t) => EaseOut(t, 2), duration, onUpdate);
        }

        public static Tween QuadraticInOut(float duration, Action<float> onUpdate)
        {
            return new Tween((t) => EaseInOut(t, 2), duration, onUpdate);
        }

        public static Tween CubicIn(float duration, Action<float> onUpdate)
        {
            return new Tween((t) => EaseIn(t, 3), duration, onUpdate);
        }

        public static Tween CubicOut(float duration, Action<float> onUpdate)
        {
            return new Tween((t) => EaseOut(t, 3), duration, onUpdate);
        }

        public static Tween CubicInOut(float duration, Action<float> onUpdate)
        {
            return new Tween((t) => EaseInOut(t, 3), duration, onUpdate);
        }

        public static Tween SineIn(float duration, Action<float> onUpdate)
        {
            return new Tween((t) => {
                return Mathf.Sin(t * k_HalfPI - k_HalfPI) + 1;
            }, duration, onUpdate);
        }

        public static Tween SineOut(float duration, Action<float> onUpdate)
        {
            return new Tween((t) => Mathf.Sin(t * k_HalfPI), duration, onUpdate);
        }

        public static Tween SineInOut(float duration, Action<float> onUpdate)
        {
            return new Tween((t) => {
                return (Mathf.Sin(t * Mathf.PI - k_HalfPI) + 1f) / 2f;
            }, duration, onUpdate);
        }

        private static float EaseIn(float t, int power)
        {
            return Mathf.Pow(t, power);
        }

        private static float EaseOut(float t, int power)
        {
            int sign = power % 2 == 0 ? -1 : 1;
            return (sign * (Mathf.Pow(t - 1, power) + sign));
        }

        private static float EaseInOut(float t, int power)
        {
            t *= 2.0f;
            if (t < 1) {
                return Mathf.Pow(t, power) / 2.0f;
            }
            else {
                int sign = power % 2 == 0 ? -1 : 1;
                return (sign / 2.0f * (Mathf.Pow(t - 2, power) + sign * 2));
            }
        }
    } 
}