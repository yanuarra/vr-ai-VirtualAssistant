using Unity.Sentis;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[System.Serializable]
public class AssetReferenceAudioClip : AssetReferenceT<AudioClip>
{
    public AssetReferenceAudioClip(string guid) : base(guid) { }
}

public class AddressablesHandler : MonoBehaviour
{
    [SerializeField]
    AssetReferenceGameObject assetPrefab;
    AsyncOperation asyncOperation;
    GameObject spawnedObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Addressables.LoadAssetAsync<GameObject>(assetPrefab); 
            
        assetPrefab.InstantiateAsync().Completed += (asyncOperation) => spawnedObject = asyncOperation.Result;
    }
}