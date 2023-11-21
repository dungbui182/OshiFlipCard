using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrandDreams.Core.Utilities
{
    public class BaseParticleManager : MonoBehaviour
    {

        #region Declare Variables

        private static BaseParticleManager instance = null;
        public static BaseParticleManager Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject go = new GameObject("BaseParticleManager");
                    instance = go.AddComponent<BaseParticleManager>();
                }
                else
                {
                    if (instance.gameObject.IsDestroyed())
                    {
                        instance = null;
                        return Instance;
                    }
                }

                return instance;
            }
        }

        [SerializeField] private Transform[] prefabEffects;
        [SerializeField] private string[] effectNames;

        private Dictionary<string, Transform> dictionaryParticles;

        #endregion Declare Variables

        private void Awake()
        {
            instance = this;

            if (prefabEffects.Length != effectNames.Length)
            {
                Debug.LogError("Effect quantity not matched.");
            }

            dictionaryParticles = new Dictionary<string, Transform>();
            for (int index = 0; index < effectNames.Length; index++)
            {
                dictionaryParticles.Add(effectNames[index], prefabEffects[index]);
            }
        }

        private void OnDestroy()
        {
            DOTween.Complete(gameObject);
        }

        #region Public Function

        public RectTransform Spawn(string effectName, Vector2 position, Vector3 scale, Transform transformParent = null, float timeDelayedToDespawn = 1)
        {
            if (!dictionaryParticles.ContainsKey(effectName))
            {
                return null;
            }

            var tfParent = transformParent ?? transform;

            RectTransform rt = Lean.Pool.LeanPool.Spawn(dictionaryParticles[effectName], tfParent) as RectTransform;

            rt.localScale = scale;
            rt.localEulerAngles = Vector3.zero;
            rt.anchoredPosition = position;

            DOVirtual.DelayedCall(timeDelayedToDespawn, () =>
            {
                rt.localScale = Vector3.zero;
                Lean.Pool.LeanPool.Despawn(rt);
            });

            return rt;
        }

        public ParticleSystem Spawn2(string effectName, Vector2 position, Vector3 scale, Transform transformParent = null, bool playOnAwake = true, bool autoDespawn = true)
        {
            if (!dictionaryParticles.ContainsKey(effectName))
            {
                return null;
            }

            var tfParent = transformParent ?? transform;

            RectTransform rt = Lean.Pool.LeanPool.Spawn(dictionaryParticles[effectName], tfParent) as RectTransform;

            rt.localScale = scale;
            rt.localEulerAngles = Vector3.zero;
            rt.anchoredPosition = position;

            ParticleSystem ps = rt.GetComponent<ParticleSystem>();
            ParticleSystem.MainModule main = ps.main;

            main.playOnAwake = false;

            if (playOnAwake)
            {
                ps.Play();
            }

            if(autoDespawn)
            {
                //if(ieAutoDespawn != null)
                //{
                //    StopCoroutine(ieAutoDespawn);
                //}

                ieAutoDespawn = AutoDespawnRoutine(rt, main);
                StartCoroutine(ieAutoDespawn);
            }

            return ps;
        }

        #endregion Public Function

        #region Private Function

        private IEnumerator ieAutoDespawn;
        private IEnumerator AutoDespawnRoutine(RectTransform rt, ParticleSystem.MainModule main)
        {
            yield return new WaitForSeconds(main.duration);

            Lean.Pool.LeanPool.Despawn(rt);
        }

        #endregion Private Function

        #region Event



        #endregion Event

        #region Editor



        #endregion Editor

    }
}