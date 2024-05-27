using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace System
{
    public class PureAssetLoader
    {
        public T LoadScriptableObject<T>(string address) where T : ScriptableObject
        {
            var handle = Addressables.LoadAssetAsync<T>(address);
            handle.WaitForCompletion();
            
            if (handle.Status == AsyncOperationStatus.Succeeded)
                return handle.Result;
            Debug.LogError($"Failed to load {typeof(T).Name} from address {address}.");
            return null;
        }
    }
}
