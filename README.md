# Интерактивный картографический каталог
#### Требуется разработать каталог указателей с информацией. Указатели должны быстро загружаться при перезапуске ПО. Приложение должно корректно выполнять свои функции.

## - Работа приложения



https://github.com/user-attachments/assets/3238357b-ac6a-4fca-82ca-8f819873d564



## - Статистика

### - Методы ускорения
#### Кеширование данных

<img width="703" height="344" alt="{E3A830B1-F31B-4350-98BA-AC6656CA60AF}" src="https://github.com/user-attachments/assets/2fd316f1-0603-4e6c-957e-79a9a091704d" />
<br>

#

#### Многопоточность позволила параллельно загружать множество изображений

```cs
await cacheFiles.Select(async c =>
{
    var fileName = ExtractFileNameFromUrl(c);
    var sprite = await LoadSpriteByPath(c);
    _spriteCache.Add(fileName, sprite);
}).AsParallel();

// OLD
// foreach (var filePath in cacheFiles)
// {
//     var sprite = await LoadSpriteByPath(filePath);
//     _spriteCache.Add(filePath,sprite);
// }
```
<br>

#

#### URL валидация позволяет отбрасывать некорректные строки, тем самым не обращаться на такие адреса, а подставлять заглушку вместо изображения <br>
```cs
public static bool IsValidUrl(string url)
{
    if (string.IsNullOrWhiteSpace(url))
        return false;

    url = url.Trim();

    if (!Uri.TryCreate(url, UriKind.Absolute, out var uriResult))
        return false;

    return uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps;
}
```
<br>

#### Сохранение данных через библиотеку [MessagePack](https://github.com/MessagePack-CSharp/MessagePack-CSharp?tab=readme-ov-file#unity-support)
> Чрезвычайно быстрый сериализатор для C# с поддержкой сериализации unity-типов

```cs
[MessagePackObject]
public class PinData
{
    [Key(0)] public int ID { get; set; }
    [Key(1)] public string Name { get; set; }
    [Key(2)] public string ImageURL { get; set; }
    [Key(3)] public string Description { get; set; }
    [Key(4)] public Vector3 Position { get; set; }
}
```

Сохранение
```cs
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
```

Загрузка
```cs
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
        return pinModels;
    }
    catch (Exception e)
    {
        Debug.LogError($"Failed to load pins: {e.Message}");
    }
    return null;
}
```

### - Результаты

#### До 

<img width="485" height="153" alt="{1935D09A-0C18-4373-ADEF-676FC5BBA3C7}" src="https://github.com/user-attachments/assets/b4d1452c-cde6-4613-b294-8f4feb204834" />

#### После

> Без кэша
<img width="378" height="121" alt="{4ABE0D42-BDDF-49FA-8F19-FC5C687F7725}" src="https://github.com/user-attachments/assets/9b853973-1354-44b7-8b70-2431385020e0" />

> С кэшом
<img width="370" height="124" alt="{2BC83D4E-2E4B-44BB-AA00-D54FAC47FB1E}" src="https://github.com/user-attachments/assets/50bc0460-688b-4025-b173-f0ba976bc3c8" />



## - Добавление/Изменение/Удаление 

Добавление


https://github.com/user-attachments/assets/21ce6fc9-478f-4dc5-90eb-7f68296bd08b


Изменение


https://github.com/user-attachments/assets/f7fe4855-7f97-47e0-832d-2878f5d678bd


Удаление
![DeletePin](https://github.com/user-attachments/assets/e9f84a7b-f208-46e1-bdfd-d3e495773a88)


## - Ссылки:
[Скачать проект](https://github.com/Nuk1o/Cartographic-catalog/releases/tag/release)
