using UnityEngine;

namespace InGame.InGameSystem
{
    public class TimeScaleHandler
    {
        private static TimeScaleHandler _instance;
        public static TimeScaleHandler Instance => _instance ??= new TimeScaleHandler();

        private TimeScaleHandler()
        {
            Application.targetFrameRate = 60;
        }
        
        public void ChangeTimeScale(float t)
        {
            Time.timeScale = t;
        }
    }
}
