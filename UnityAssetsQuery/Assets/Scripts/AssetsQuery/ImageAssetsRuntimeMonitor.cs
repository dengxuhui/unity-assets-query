#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AssetsQuery
{
    public class ImageAssetsRuntimeMonitor : MonoBehaviour
    {
        private Dictionary<int, bool> _runtimeRecords;
        private Dictionary<string, bool> _texturePathDic;

        private void Awake()
        {
            _runtimeRecords = new Dictionary<int, bool>();
            _texturePathDic = new Dictionary<string, bool>();
        }

        private void Start()
        {
            InvokeRepeating(nameof(CheckSceneTree), 1.0f, 1.0f);
        }

        private void CheckSceneTree()
        {
            var scene = SceneManager.GetActiveScene();
            var rootObjArray = scene.GetRootGameObjects();
            for (var i = 0; i < rootObjArray.Length; i++)
            {
                var root = rootObjArray[i];
                var imgChildren = root.GetComponentsInChildren<Image>();
                for (var i1 = 0; i1 < imgChildren.Length; i1++)
                {
                    var img = imgChildren[i1];
                    if (_runtimeRecords.ContainsKey(img.GetInstanceID())) continue;
                    _runtimeRecords.Add(img.GetInstanceID(), true);
                    var sprite = img.sprite;
                    if (sprite == null) continue;
                    var path = AssetDatabase.GetAssetPath(sprite);
                    if (string.IsNullOrEmpty(path) || _texturePathDic.ContainsKey(path)) continue;
                    _texturePathDic.Add(path, true);
                }

                var rawChildren = root.GetComponentsInChildren<RawImage>();
                for (var i1 = 0; i1 < rawChildren.Length; i1++)
                {
                    var raw = rawChildren[i1];
                    if (_runtimeRecords.ContainsKey(raw.GetInstanceID())) continue;
                    _runtimeRecords.Add(raw.GetInstanceID(), true);
                    var texture = raw.texture;
                    if (texture == null) continue;
                    var path = AssetDatabase.GetAssetPath(texture);
                    if (string.IsNullOrEmpty(path) || _texturePathDic.ContainsKey(path)) continue;
                    _texturePathDic.Add(path, true);
                }
            }
        }

        private void OnApplicationQuit()
        {
        }
    }
}
#endif