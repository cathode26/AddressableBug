using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.ResourceManagement.AsyncOperations;
using static UnityEngine.Localization.Tables.SharedTableData;

public class LocalizationManager : MonoBehaviour
{
    [SerializeField]
    private LocalizedAssetTable localizedAssetTable;
    AsyncOperationHandle<AssetTable> currentAssetTableOp;
    Dictionary<string, AsyncOperationHandle<Sprite>> spriteOpDict = new Dictionary<string, AsyncOperationHandle<Sprite>>();
    List<LocalizeSpriteEvent> localizedSpriteEvents = new List<LocalizeSpriteEvent>();
    private AssetTable currentAssetTable = null; 

    public IEnumerator Start()
    {
        // Wait for the localization system to initialize
        yield return LocalizationSettings.InitializationOperation;

        currentAssetTableOp = localizedAssetTable.GetTableAsync();
        yield return currentAssetTableOp;
        currentAssetTable = currentAssetTableOp.Result;

        foreach (SharedTableEntry sharedTableEntry in currentAssetTable.SharedData.Entries)
        {
            string id = sharedTableEntry.Key;
            AddSpriteAssetReference(currentAssetTable.TableCollectionName, id);
        }
    }
    public void AddSpriteAssetReference(string tableRef, string tableEntryRef)
    {
        LocalizeSpriteEvent localizeSpriteEvent = gameObject.AddComponent<LocalizeSpriteEvent>();
        LocalizedSprite localizedSprite = new LocalizedSprite
        {
            TableReference = tableRef,
            TableEntryReference = tableEntryRef,
        };

        localizedSprite.WaitForCompletion = false;
        localizeSpriteEvent.AssetReference = localizedSprite;
        localizedSpriteEvents.Add(localizeSpriteEvent);
    }
}

