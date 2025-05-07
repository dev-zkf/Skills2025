using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TimeScaleDebugger : MonoBehaviour
{
   [Range(0.01f, 1f)]
   public float timeScale = 1f;

   private float previousScale;

   void Update()
   {
      if (!Application.isPlaying) return;

      if (!Mathf.Approximately(Time.timeScale, timeScale))
      {
         Time.timeScale = timeScale;
         Time.fixedDeltaTime = 0.02f * timeScale; // Sync physics
      }
   }

   void OnDisable()
   {
      Time.timeScale = 1f;
      Time.fixedDeltaTime = 0.02f;
   }
}