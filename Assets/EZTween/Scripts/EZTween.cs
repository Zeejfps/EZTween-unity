using System.Collections.Generic;
using UnityEngine;

namespace ENVCode.EZTween
{
    public partial class EZTween : MonoBehaviour
    {
        #region Static
        /// <summary>
        /// Singleton instance of this class.
        /// </summary>
        static EZTween s_Instance;
        public static EZTween Instance {
            get {
                if (s_Instance == null) {
                    Init();
                }
                return s_Instance;
            }
        }

        /// <summary>
        /// Initializes the EZTween GameObject.
        /// Creates a EZTween GameObject with hideFlags set to HideAndDontSave.
        /// </summary>
        public static void Init()
        {
            if (s_Instance != null)
                return;

            GameObject obj = new GameObject("EZTween");
            obj.hideFlags = HideFlags.HideAndDontSave;
            s_Instance = obj.AddComponent<EZTween>();
        }

        /// <summary>
        /// Plays a given tween.
        /// The tween is added to a HashSet and then updated every frame.
        /// </summary>
        /// <param name="tween"></param>
        public static void Play(Tween tween)
        {
            Instance.m_TweenSet.Add(tween);
        }

        /// <summary>
        /// Plays a tween that can be retrieved via the key object.
        /// If the key already exists that means a tween is already playing and will be stoped before new tween is played.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="tween"></param>
        public static void Play(object key, Tween tween)
        {
            if (Instance.m_TweenDict.ContainsKey(key)) {
                Tween prev = Instance.m_TweenDict[key];
                if (prev.Playing) prev.Stop();
                Instance.m_TweenDict[key] = tween;
            }
            else {
                Instance.m_TweenDict.Add(key, tween);
            }
            Instance.m_TweenSet.Add(tween);
        }
        #endregion

        #region Properties
        List<Tween> m_TweenList = new List<Tween>();
        HashSet<Tween> m_TweenSet = new HashSet<Tween>();
        Dictionary<object, Tween> m_TweenDict = new Dictionary<object, Tween>();
        #endregion

        #region Unity Callbacks
        void Update()
        {
            // Clear the tween list
            m_TweenList.Clear();
            // Add the set to the list so we can iterate over it
            m_TweenList.AddRange(m_TweenSet);

            // Loop over all the tweens in our set and update them
            foreach (Tween tween in m_TweenList) {
                tween.Tick(Time.deltaTime);
                if (tween.Stopped) {
                    m_TweenSet.Remove(tween);
                }
            }
        }
        #endregion
    } 
}