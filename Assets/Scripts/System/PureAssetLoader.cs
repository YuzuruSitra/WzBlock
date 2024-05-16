using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PureAssetLoader
{
    public T LoadScriptableObject<T>(string address) where T : ScriptableObject
    {
        // �񓯊�����̊J�n
        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);
        // ����̊����܂őҋ@�i�����I�Ƀu���b�N�j
        handle.WaitForCompletion();

        // ���삪�������������`�F�b�N
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            // ���������ꍇ�͌��ʂ�Ԃ�
            return handle.Result;
        }
        else
        {
            // ���s�����ꍇ�̓G���[���O���o�͂��Anull��Ԃ�
            Debug.LogError($"Failed to load {typeof(T).Name} from address {address}.");
            return null;
        }
    }
}
