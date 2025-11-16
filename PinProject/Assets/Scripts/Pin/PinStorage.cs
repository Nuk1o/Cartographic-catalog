using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Cysharp.Threading.Tasks;
using DefaultNamespace.Tools;
using MessagePack;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

namespace DefaultNamespace.Pin
{
    public class PinStorage
    {
        private const string SaveFileName = "pins.dat";
        private readonly Dictionary<string, Sprite> _spriteCache = new();
        private static Sprite _fallbackSprite;

        private static readonly Regex FileNameRegex = new Regex(
            @"[^/\\&\?]+\.\w{3,4}(?=([\?&].*$|$))",
            RegexOptions.Compiled
        );

        private static string ExtractFileNameFromUrl(string urlOrPath)
        {
            return FileNameRegex.Match(urlOrPath).Value;
        }

        public async UniTask LoadLocalImageCache()
        {
            var cacheDirectory = Application.persistentDataPath;
            var cacheFiles = GetImageFiles(cacheDirectory);

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            await cacheFiles.Select(async c =>
            {
                var fileName = ExtractFileNameFromUrl(c);
                var sprite = await LoadSpriteByPath(c);
                _spriteCache.Add(fileName, sprite);
            }).AsParallel();

            // foreach (var filePath in cacheFiles)
            // {
            //     var sprite = await LoadSpriteByPath(filePath);
            //     _spriteCache.Add(filePath,sprite);
            // }

            stopWatch.Stop();

            var ts = stopWatch.Elapsed;

            var elapsedTime = $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds / 10:00}";
            Debug.Log("LoadLocalImageCache RunTime " + elapsedTime);
        }

        private static List<string> GetImageFiles(string directoryPath)
        {
            var imageFiles = new List<string>();
            if (Directory.Exists(directoryPath))
            {
                var files = Directory.GetFiles(directoryPath);
                foreach (var file in files)
                {
                    var extension = Path.GetExtension(file).ToLower();
                    if (extension is ".png" or ".jpg" or ".jpeg")
                    {
                        imageFiles.Add(file);
                    }
                }
            }
            else
            {
                Debug.LogError($"Директория не найдена: {directoryPath}");
            }

            return imageFiles;
        }

        public async UniTask SavePins(List<PinModel> models)
        {
            var pinDataList = models.Select(pin => new PinData
            {
                ID = pin.ID,
                Name = pin.Name,
                ImageURL = pin.ImageURL,
                Description = pin.Description,
                Position = pin.Position
            }).ToList();

            var bytes = MessagePackSerializer.Serialize(pinDataList);
            await File.WriteAllBytesAsync(GetSavePath(), bytes);
            Debug.Log($"Saved {models.Count} pins to {GetSavePath()}");
        }

        public async UniTask<List<PinModel>> LoadPins()
        {
            var path = GetSavePath();
            if (!File.Exists(path))
            {
                Debug.LogWarning("No save file found. Starting fresh.");
                return new List<PinModel>();
            }

            try
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();

                var bytes = await File.ReadAllBytesAsync(path);
                var pinDataList = MessagePackSerializer.Deserialize<List<PinData>>(bytes);

                var pinModels = pinDataList.Select(data => new PinModel(
                    data.ID,
                    data.Name,
                    data.ImageURL,
                    data.Description,
                    data.Position,
                    null
                )).ToList();

                Debug.Log($"Loaded {pinModels.Count} pins. Loading sprites asynchronously...");

                await LoadSpritesAsync(pinModels);

                stopWatch.Stop();

                var ts = stopWatch.Elapsed;

                var elapsedTime = $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{ts.Milliseconds / 10:00}";
                Debug.Log("LoadPins RunTime " + elapsedTime);

                return pinModels;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load pins: {e.Message}");
            }

            return null;
        }

        private static string GetSavePath() =>
            Path.Combine(Application.persistentDataPath, SaveFileName);

        private async UniTask LoadSpritesAsync(List<PinModel> models)
        {
            await models
                .Where(pin => !string.IsNullOrEmpty(pin.ImageURL))
                .Select(async m => m.Image = await LoadSpriteAsync(m.ImageURL))
                .AsParallel();

            Debug.Log("All sprites loaded successfully!");
        }

        private static async UniTask<Sprite> LoadSpriteByPath(string filePath)
        {
            var fileData = await File.ReadAllBytesAsync(filePath);
            var loadedTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false, true);
            if (!loadedTexture.LoadImage(fileData, true))
            {
                Debug.LogError($"Failed to load image data from {filePath}");
                return null;
            }

            loadedTexture.filterMode = FilterMode.Bilinear;

            return Sprite.Create(
                loadedTexture,
                new Rect(0, 0, loadedTexture.width, loadedTexture.height),
                new Vector2(0.5f, 0.5f),
                100f
            );
        }

        private async UniTask SaveTextureToDeviceCache(Texture2D texture2D, string fileName)
        {
            var path = Path.Join(Application.persistentDataPath, fileName);
            var bytes = texture2D.EncodeToPNG();
            await File.WriteAllBytesAsync(path, bytes);
        }

        public async UniTask<Sprite> LoadSpriteAsync(string url)
        {
            if (!UrlValidator.IsValidUrl(url))
            {
                return GetMissingSprite();
            }

            var fileName = ExtractFileNameFromUrl(url);

            if (_spriteCache.TryGetValue(fileName, out var cacheSprite))
            {
                return cacheSprite;
            }

            using var www = UnityWebRequestTexture.GetTexture(url);
            try
            {
                await www.SendWebRequest().ToUniTask();

                if (www.result != UnityWebRequest.Result.Success)
                    throw new Exception($"Error loading {url}: {www.error}");

                var texture = DownloadHandlerTexture.GetContent(www);
                var sprite = Sprite.Create(texture,
                    new Rect(0, 0, texture.width, texture.height),
                    Vector2.zero);
                _spriteCache.Add(fileName, sprite);
                await SaveTextureToDeviceCache(texture, fileName);
                return sprite;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load sprite from {url}: {ex.Message}");
                return GetMissingSprite();
            }
        }

        private static Sprite GetMissingSprite()
        {
            if (_fallbackSprite != null)
                return _fallbackSprite;

            var texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, Color.clear);
            texture.Apply();

            _fallbackSprite = Sprite.Create(
                texture,
                new Rect(0, 0, 1, 1),
                Vector2.zero
            );

            return _fallbackSprite;
        }
    }
}