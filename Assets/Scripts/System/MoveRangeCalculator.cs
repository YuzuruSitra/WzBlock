using UnityEngine;

public class MoveRangeCalculator
{
    private float _leftMaxPos;
    public float LeftMaxPos => _leftMaxPos;
    private float _rightMaxPos;
    public float RightMaxPos => _rightMaxPos;

    public MoveRangeCalculator(GameObject targetObj, GameObject leftObj, GameObject rightObj)
    {
        CalcMoveRange(targetObj, leftObj, rightObj);
    }

    private void CalcMoveRange(GameObject targetObj, GameObject leftObj, GameObject rightObj)
    {
        // ���̃I�u�W�F�N�g�̕����l��
        MeshRenderer meshRenderer = targetObj.GetComponent<MeshRenderer>();
        float width = meshRenderer.bounds.size.x / 2;

        // ���[���v�Z
        MeshRenderer meshRendererLeft = leftObj.GetComponent<MeshRenderer>();
        float widthLeft =  meshRendererLeft.bounds.size.x;
        _leftMaxPos = leftObj.transform.position.x + widthLeft / 2 + width;

        // �E�[���v�Z
        MeshRenderer meshRendererRight = rightObj.GetComponent<MeshRenderer>();
        float widthRight =  meshRendererRight.bounds.size.x;
        _rightMaxPos = rightObj.transform.position.x - widthRight / 2 - width;
    }
}
