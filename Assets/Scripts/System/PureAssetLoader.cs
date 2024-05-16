using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class PureAssetLoader
{
    public T LoadScriptableObject<T>(string address) where T : ScriptableObject
    {
        // 非同期操作の開始
        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(address);
        // 操作の完了まで待機（同期的にブロック）
        handle.WaitForCompletion();

        // 操作が成功したかをチェック
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            // 成功した場合は結果を返す
            return handle.Result;
        }
        else
        {
            // 失敗した場合はエラーログを出力し、nullを返す
            Debug.LogError($"Failed to load {typeof(T).Name} from address {address}.");
            return null;
        }
    }
}
