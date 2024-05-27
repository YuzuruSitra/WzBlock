using UnityEngine.SceneManagement;

namespace System
{
    public class SceneHandler
    {
        
        private static SceneHandler _instance;
        public static SceneHandler Instance => _instance ??= new SceneHandler();

        private SceneHandler(){}

        public void GoTitleScene()
        {
            SceneManager.LoadScene("Title");
        }

        public void GoMainGameScene()
        {
            SceneManager.LoadScene("MainGame");
        }
    }
}
