using UnityEngine;

namespace System
{
    public class MoveRangeCalculator
    {
        public float LeftMaxPos { get; private set; }

        public float RightMaxPos { get; private set; }

        private float _centerPosX;

        public MoveRangeCalculator(GameObject targetObj, GameObject leftObj, GameObject rightObj)
        {
            CalcMoveRange(targetObj, leftObj, rightObj);
        }

        private void CalcMoveRange(GameObject targetObj, GameObject leftObj, GameObject rightObj)
        {
            var meshRenderer = targetObj.GetComponent<MeshRenderer>();
            var width = meshRenderer.bounds.size.x / 2;
            
            _centerPosX = (leftObj.transform.position.x + rightObj.transform.position.x) / 2.0f;
            
            var meshRendererLeft = leftObj.GetComponent<MeshRenderer>();
            var widthLeft =  meshRendererLeft.bounds.size.x;
            LeftMaxPos = leftObj.transform.position.x + widthLeft / 2 + width;
            
            var meshRendererRight = rightObj.GetComponent<MeshRenderer>();
            var widthRight =  meshRendererRight.bounds.size.x;
            RightMaxPos = rightObj.transform.position.x - widthRight / 2 - width;
        }
    }
}
