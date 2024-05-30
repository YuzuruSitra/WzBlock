using InGame.InGameSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DebugSystem
{
    public class DebugMetaAI : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField _rallyTimeField;
        [SerializeField]
        private TMP_InputField _avoidField;
        [SerializeField]
        private TMP_InputField _receiveField;
        [SerializeField]
        private Text _text1;
        [SerializeField]
        private Text _text2;
        [SerializeField]
        private Text _text3;

        private BoredomMetaAI _boredomMetaAI;
        
        void Start()
        {
            _boredomMetaAI = BoredomMetaAI.Instance;
        }

        public void DoLearning()
        {
            _boredomMetaAI.Learning(float.Parse(_rallyTimeField.text), int.Parse(_avoidField.text), int.Parse(_receiveField.text));
            // _text1.text = "学習回数 : " + _boredomMetaAI.LearnCount;
            // _text2.text = "退屈度 : " + _boredomMetaAI.CurrentSelectAction;
            // _text3.text = "決行Q値 : " + _boredomMetaAI.CurrentQValue;
        }
        
    }
}
