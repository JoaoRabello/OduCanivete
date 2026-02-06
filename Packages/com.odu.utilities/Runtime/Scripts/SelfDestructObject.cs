using UnityEngine;
using System;
using System.Collections;

namespace Odu.Utilities.GameObjects
{
    public class SelfDestructObject : MonoBehaviour
    {
        public Action OnBeforeDestroy;

        /// <summary>
        /// Inicia o tempo de auto destruição do objeto
        /// </summary>
        /// <param name="timer">Tempo de espera antes de destruir o objeto</param>
        public void StartDestroyTimer(float timer)
        {
            StartCoroutine(Countdown(timer));
        }

        IEnumerator Countdown(float timer)
        {
            yield return new WaitForSeconds(timer);
            OnBeforeDestroy?.Invoke();
            Destroy(this.gameObject);
        }
    }
}
