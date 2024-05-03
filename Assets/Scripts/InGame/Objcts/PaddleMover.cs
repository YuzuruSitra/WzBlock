using UnityEngine;

public class PaddleMover : MonoBehaviour
{
    [Range(0, 100)]
    [SerializeField]
    private float Speed = 1f;
    private Vector3 _launchPos;
    private GameStateHandler _gameStateHandler;
    [SerializeField]
    private GameObject _leftObj;
    [SerializeField]
    private GameObject _rightObj;
    private float _leftMaxPos;
    private float _rightMaxPos;

    void Start()
    {
        CalcMoveRange();
        _launchPos = transform.position;
        _gameStateHandler = GameStateHandler.Instance;
        _gameStateHandler.ChangeGameState += ChangeStatePaddle;
    }

    void Update()
    {
        if (_gameStateHandler.CurrentState != GameStateHandler.GameState.InGame) return;
        var horizontal = Input.GetAxis("Horizontal");
        // ����̐���
        float posX = transform.position.x;
        if (posX <= _leftMaxPos && horizontal < 0) 
            horizontal = 0;
        if (posX >= _rightMaxPos && horizontal > 0) 
            horizontal = 0;
        transform.position += Vector3.right * horizontal * Speed * Time.deltaTime;
    }

    private void ChangeStatePaddle(GameStateHandler.GameState newState)
    {
        if (newState == GameStateHandler.GameState.Launch)
            transform.position = _launchPos;
    }

    private void CalcMoveRange()
    {
        // ���̃I�u�W�F�N�g�̕����l��
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        float width = meshRenderer.bounds.size.x / 2;

        // ���[���v�Z
        MeshRenderer meshRendererLeft = _leftObj.GetComponent<MeshRenderer>();
        float widthLeft =  meshRendererLeft.bounds.size.x;
        _leftMaxPos = _leftObj.transform.position.x + widthLeft / 2 + width;

        // �E�[���v�Z
        MeshRenderer meshRendererRight = _rightObj.GetComponent<MeshRenderer>();
        float widthRight =  meshRendererRight.bounds.size.x;
        _rightMaxPos = _rightObj.transform.position.x - widthRight / 2 - width;
    }

}
