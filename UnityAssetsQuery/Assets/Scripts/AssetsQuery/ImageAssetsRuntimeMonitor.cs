#if UNITY_EDITOR
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AssetsQuery
{
    public class ImageAssetsRuntimeMonitor : MonoBehaviour
    {
        /// <summary>
        /// 存储key
        /// </summary>
        private static readonly string ImageUseMonitorSaveKey = "#key#asset_query_image_use_monitor";

        private Dictionary<string, bool> _runtimePathRecords;

        private void Awake()
        {
            _runtimePathRecords = new Dictionary<string, bool>();
            var strSave = EditorPrefs.GetString(ImageUseMonitorSaveKey);
            if (!string.IsNullOrEmpty(strSave))
            {
                var guidArray = strSave.Split(',');
                if (guidArray.Length > 0)
                {
                    for (var i = 0; i < guidArray.Length; i++)
                    {
                        if (string.IsNullOrEmpty(guidArray[i]))
                        {
                            continue;
                        }

                        var path = AssetDatabase.GUIDToAssetPath(guidArray[i]);
                        if (string.IsNullOrEmpty(path) || _runtimePathRecords.ContainsKey(path))
                        {
                            continue;
                        }

                        _runtimePathRecords.Add(path, true);
                    }
                }
            }

            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            InvokeRepeating(nameof(CheckSceneTree), 1.0f, 1.0f);
        }

        private void CheckGameObject(GameObject obj)
        {
            if (obj == null)
            {
                return;
            }

            var imgArray = obj.GetComponentsInChildren<Image>();
            for (var i2 = 0; i2 < imgArray.Length; i2++)
            {
                var img = imgArray[i2];
                if (img.sprite == null) continue;
                var path = AssetDatabase.GetAssetPath(img.sprite);
                if (string.IsNullOrEmpty(path) || _runtimePathRecords.ContainsKey(path)) continue;
                _runtimePathRecords.Add(path, true);
            }

            var rawArray = obj.GetComponentsInChildren<RawImage>();
            for (var i2 = 0; i2 < rawArray.Length; i2++)
            {
                var img = rawArray[i2];
                if (img.texture == null) continue;
                var path = AssetDatabase.GetAssetPath(img.texture);
                if (string.IsNullOrEmpty(path) || _runtimePathRecords.ContainsKey(path)) continue;
                _runtimePathRecords.Add(path, true);
            }
        }

        private void CheckSceneTree()
        {
            var activeScene = SceneManager.GetActiveScene();
            var activeObjArray = activeScene.GetRootGameObjects();
            for (var i = 0; i < activeObjArray.Length; i++)
            {
                CheckGameObject(activeObjArray[i]);
            }

            var dontObjArray = this.gameObject.scene.GetRootGameObjects();
            for (int i = 0; i < dontObjArray.Length; i++)
            {
                CheckGameObject(dontObjArray[i]);
            }
        }

        private void OnApplicationQuit()
        {
            if (_runtimePathRecords.Count <= 0)
            {
                return;
            }

            var sb = new StringBuilder();
            foreach (var kv in _runtimePathRecords)
            {
                var guid = AssetDatabase.AssetPathToGUID(kv.Key);
                if (!string.IsNullOrEmpty(guid))
                {
                    sb.Append(guid);
                    sb.Append(",");
                }
            }

            var saveStr = sb.ToString();
            EditorPrefs.SetString(ImageUseMonitorSaveKey, saveStr);
        }
    }
}
#endif