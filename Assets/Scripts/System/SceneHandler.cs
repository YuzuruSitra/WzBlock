using UnityEngine.SceneManagement;

public class SceneHandler
{
    // �V���O���g��
    private static SceneHandler instance;
    public static SceneHandler Instance => instance ?? (instance = new SceneHandler());

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
