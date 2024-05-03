using UnityEngine;
using TMPro;

public class InfoUIChanger : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _scoreText;
    private ScoreHandler _scoreHandler;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score : 0";
        _scoreHandler = ScoreHandler.Instance;
        _scoreHandler.ChangeScore += ChangeScoreUI;
    }

    private void ChangeScoreUI(int newValue)
    {
        _scoreText.text = "Score : " + newValue;
    }
}
