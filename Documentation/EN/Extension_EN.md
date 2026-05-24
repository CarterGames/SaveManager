## Locale
- [EN](Extension_EN.md)

## Docs
- [Usage Documentation](Docs_EN.md)
- [Scripting API](Scripting_EN.md)
- [Listener API](Listeners_EN.md)
- [Extension API](Extension_EN.md)

<br/>

You can extend the save manager by making your own implementations of core features for the
asset to use. These can then be selected in the asset settings so the asset uses them over the default
provided options. It is recommended that you test any custom implementations before releasing
them for public use in-case of issues. When developing any custom locations or encryption set-ups
it is advised to save a backup of your game data to an alternative location, so you can restore it should
issues arise.

## Contents
- [IDataLocation](#idatalocation-)
  - Methods
    - [HasData](#hasdata-)
    - [SaveToLocation](#savetolocation-)
    - [LoadFromLocation](#loadfromlocation-)
- [ISaveDataLocation](#isavedatalocation-)
  - Properties
    - [DataLocation](#datalocation-)
    - [HasSaveData](#hassavedata-)
  - Methods
    - [SaveDataToLocation](#savedatatolocation-)
    - [LoadDataFromLocation](#loaddatafromlocation-)
- [ISaveBackupLocation](#isavebackuplocation-)
  - Properties
    - [Location](#location-)
  - Methods
    - [BackupData](#backupdata-)
    - [GetBackups](#getbackups-)
- [ISaveEncryptionHandler](#isaveencryptionhandler-)
  - Methods
    - [Encrypt](#encrypt-)
    - [Decrypt](#decrypt-)
- [SmJsonConverterBase](#smjsonconverterbase-)
  - Methods
    - [ReadFromJson](#readfromjson-)
    - [WriteToJson](#writetojson-)
- [ISaveMetaData](#isavemetadata-)
  - Properties
    - [Key](#key-)
    - [CanWriteMetaData](#canwritemetadata-)
  - Methods
    - [GetMetaData](#getmetadata-)
- [ILegacySaveHandler](#ilegacysavehandler-)
  - Methods
    - [ProcessLegacySaveData](#processlegacysavedata-)
    
<br/>

### `IDataLocation` [^](#contents)
Implement this interface to define a location at which any data can be stored at. Implement along-side
[`ISaveDataLocation`](#isavedatalocation-) to define a custom save data location.

### Methods

#### `HasData()` [^](#idatalocation-)
This method should return if the data location has any data currently stored or not.

```csharp
public bool HasData(string path);
```

```csharp
public sealed class DataLocationLocalFile : IDataLocation
{
    /// <summary>
    /// Gets if the location has data currently stored in it.
    /// </summary>
    /// <param name="path">The path to store the data at.</param>
    /// <returns>If there is data in the location.</returns>
    public bool HasData(string path)
    {
        return File.Exists(path);
    }
}
```

<br/>

#### `SaveToLocation()` [^](#idatalocation-)
This method should save the entered data to the location.

```csharp
public void SaveToLocation(string path, string data);
```

```csharp
public sealed class DataLocationLocalFile : IDataLocation
{
    /// <summary>
    /// Saves data to the location when called.
    /// </summary>
    /// <param name="path">The path to store the data at.</param>
    /// <param name="data">The data to store.</param>
    public void SaveToLocation(string path, string data)
    {
        if (!HasData(path))
        {
            CreateToDirectory(path);
            CreateSaveFile(path);
        }
                
        using var stream = new FileStream(path, FileMode.Open, FileAccess.Write, FileShare.Write);
                
        using (var writer = new StreamWriter(stream))
        {
            stream.SetLength(0);
            writer.Write(data);
            writer.Close();
        }
                    
        stream.Close();
    }
}
```

<br/>

#### `LoadFromLocation()` [^](#idatalocation-)
This method should load the data from the location.

```csharp
public void LoadFromLocation(string path);
```

```csharp
public sealed class DataLocationLocalFile : IDataLocation
{
    /// <summary>
    /// Loads the data from the location when called.
    /// </summary>
    /// <param name="path">The path to load the data at.</param>
    /// <returns>The data loaded from the location.</returns>
    public string LoadFromLocation(string path)
    {
        if (!HasData(path))
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            CreateWebSaveFile(path);                
#else
            CreateToDirectory(path);
            CreateSaveFile(path);
#endif
            
            return string.Empty;
        }

        string jsonData;

        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        
        using (var reader = new StreamReader(stream))
        {
            jsonData = reader.ReadToEnd();
            reader.Close();
        }

        stream.Close();

        return jsonData;
    }
}
```

<br/>

### `ISaveDataLocation` [^](#contents)
Implement this interface to define a custom game save data location to store your game save data at.
This requires a [`IDataLocation`](#idatalocation-) implementation to be set-up for the desired location as well.

### Properties

#### `DataLocation` [^](#isavedatalocation-)
Should get the [`IDataLocation`](#idatalocation-) implementation to use for this save method.

```csharp
public IDataLocation DataLocation { get; }
```

```csharp
public sealed class SaveLocationLocalFile : ISaveDataLocation
{
    /// <summary>
    /// The data location to use for the game save data.
    /// </summary>
    public IDataLocation DataLocation => new DataLocationLocalFile();
}
```

<br/>

#### `HasSaveData` [^](#isavedatalocation-)
Should get if there is save data stored in the location currently.

```csharp
public bool HasSaveData { get; }
```

```csharp
public sealed class SaveLocationLocalFile : ISaveDataLocation
{
    /// <summary>
    /// Gets if the location has a save data already.
    /// </summary>
    public bool HasSaveData => DataLocation.HasData(ActiveSavePath);
}
```

<br/>

### Methods

#### `SaveDataToLocation()` [^](#isavedatalocation-)
This method should save the entered json to the location when called.

```csharp
public void SaveDataToLocation(string json);
```

```csharp
public sealed class SaveLocationLocalFile : ISaveDataLocation
{
    /// <summary>
    /// Saves the data to the location defined in <see cref="DataLocation"/>
    /// </summary>
    /// <param name="json">The json to save to the location.</param>
    public void SaveDataToLocation(string json)
    {
        DataLocation.SaveToLocation(ActiveSavePath, json);
    }
}
```

<br/>

#### `LoadDataFromLocation()` [^](#isavedatalocation-)
This method should load the json from the location when called and return the received data.

```csharp
public string LoadDataFromLocation();
```

```csharp
public sealed class SaveLocationLocalFile : ISaveDataLocation
{
    /// <summary>
    /// Loads the data from the location defined in <see cref="DataLocation"/>
    /// </summary>
    /// <returns>The data loaded from the location.</returns>
    public string LoadDataFromLocation()
    {
        return DataLocation.LoadFromLocation(ActiveSavePath);
    }
}
```

<br/>

### `ISaveBackupLocation` [^](#contents)
Implement this interface to define a location at which any save backups may be stored. This
requires a [`IDataLocation`](#idatalocation-) implementation to be set-up for the desired location as well.

### Properties

#### `Location` [^](#isavebackuplocation-)
Should get the [`IDataLocation`](#idatalocation-) implementation to for the backups

```csharp
public IDataLocation Location { get; }
```

```csharp
public sealed class SaveBackupLocalFile : ISaveBackupLocation
{
    /// <summary>
    /// Gets the location implementation to use with the setup.
    /// </summary>
    public IDataLocation Location => new DataLocationLocalFile();
}
```

<br/>

### Methods

#### `BackupData()` [^](#isavebackuplocation-)
This method should save the entered data to the location.

```csharp
public void BackupData(JToken data);
```

```csharp
public sealed class SaveBackupLocalFile : ISaveBackupLocation
{
    /// <summary>
    /// Backs up the data when called.
    /// </summary>
    /// <param name="data">The data to backup.</param>
    public void BackupData(JToken data)
    {
        var currentBackups = GetBackups();
        
        if (currentBackups.Any())
        {
            foreach (var backup in currentBackups)
            {
                var newIteration = backup["iteration"].Value<int>() + 1;
        
                // Trim any extra backups off the list of saved ones.
                if (newIteration >= SmAssetAccessor.GetAsset<DataAssetSettings>().MaxBackups) continue;
                Location.SaveToLocation(string.Format(ParsedBackupsPath, newIteration), new JObject()
                {
                    ["iteration"] = newIteration,
                    ["json"] = backup["json"],
                }.ToString());
            }
        }

        Location.SaveToLocation(string.Format(ParsedBackupsPath, 0), new JObject()
        {
            ["iteration"] = 0,
            ["json"] = data,
        }.ToString());
    }
}
```

<br/>

#### `GetBackups()` [^](#isavebackuplocation-)
This method should load all the backups from the location for use.

```csharp
public IEnumerable<JObject> GetBackups();
```

```csharp
public sealed class SaveBackupLocalFile : ISaveBackupLocation
{
    /// <summary>
    /// Gets the backups stored for use.
    /// </summary>
    /// <returns>The backups found.</returns>
    public IEnumerable<JObject> GetBackups()
    {
        if (!Directory.Exists(ParsedBackupsLocation)) return Array.Empty<JObject>();

        var files = Directory.GetFiles(ParsedBackupsLocation);
        var loadedData = new JObject[files.Length];
        
        for (var i = 0; i < files.Length; i++)
        {
            var filePath = string.Format(ParsedBackupsPath, i);
            loadedData[i] = (JObject)JsonConvert.DeserializeObject(Location.LoadFromLocation(filePath), new JsonSerializerSettings()
            {
                DateParseHandling = DateParseHandling.None
            });
        }

        return loadedData;
    }
}
```

<br/>

### `ISaveEncryptionHandler` [^](#contents)
Implement this interface to make your own data encryption handler to use with the asset. It is best
to test you encryption set-up works before assigning it to the manager to avoid issues. I’d also
recommend making a backup of any save data beforehand just in-case.

### Methods

#### `Encrypt()` [^](#isaveencryptionhandler-)
This method should encrypt the data entered into it and return the result of the encryption back to
the system for use.

```csharp
public string Encrypt(string contentData);
```

```csharp
public sealed class SaveEncryptionBasicAes : ISaveEncryptionHandler
{
    /// <summary>
    /// Encrypts the save content when called.
    /// </summary>
    /// <param name="contentData">The content to encrypt.</param>
    /// <returns>The encrypted data</returns>
    public string Encrypt(string contentData)
    {
        if (string.IsNullOrEmpty(contentData))
        {
            return string.Empty;
        }

        SmDebugLogger.LogDev($"Data pre-encryption:\n{contentData}");
        
        using var aes = Aes.Create();
        aes.KeySize = 256;
        aes.Key = Key;
        aes.IV = Iv;

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();
        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var sw = new StreamWriter(cs))
        {
            sw.Write(contentData);
        }
        
        var encryptedData = Convert.ToBase64String(ms.ToArray());
        SmDebugLogger.LogDev($"Data post-encryption:\n{encryptedData}");
        return encryptedData;
    }
}
```

<br/>

#### `Decrypt()` [^](#isaveencryptionhandler-)
This method should decrypt the data entered into it and return the result of the decryption back to
the system for use.

```csharp
public string Decrypt(string encryptedData);
```

```csharp
public sealed class SaveEncryptionBasicAes : ISaveEncryptionHandler
{
    /// <summary>
    /// Decrypts the save content when called.
    /// </summary>
    /// <param name="encryptedData">The encrypted data to decrypt.</param>
    /// <returns>The decrypted data</returns>
    public string Decrypt(string encryptedData)
    {
        SmDebugLogger.LogDev($"Data pre-decryption:\n{encryptedData}");
        
        if (string.IsNullOrEmpty(encryptedData))
        {
            return string.Empty;
        }

        using var aes = Aes.Create();
        aes.KeySize = 256;
        aes.Key = Key;
        aes.IV = Iv;

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(Convert.FromBase64String(encryptedData));
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);
        
        var data = sr.ReadToEnd();
        SmDebugLogger.LogDev($"Data post-decryption:\n{data}");
        return data;
    }
}
```

<br/>

### `SmJsonConverterBase` [^](#contents)
Implement this abstract class to add your own json converters so the save manager can use them
when saving your data. By default, all public fields or private fields with the [SerializeField]
attribute will be saved if possible. You should implement a custom converter if a custom type is not
saving correctly, or if you need to save a Unity type that is not saving correctly in the current set-up.

### Methods

#### `ReadFromJson()` [^](#smjsonconverterbase-)
This method should read the json value for the required field if applicable. This should be done
through a switch statement or similar.

```csharp
protected abstract void ReadFromJson(ref T value, string name, JsonReader reader, JsonSerializer serializer);
```

```csharp
public sealed class JsonConverterVector2 : SmJsonConverterBase<Vector2>
{
    /// <summary>
    /// Reads from the json into the value.
    /// </summary>
    /// <param name="value">The value to read json for.</param>
    /// <param name="name">The name of the field to read.</param>
    /// <param name="reader">The reader</param>
    /// <param name="serializer">The serializer</param>
    protected override void ReadFromJson(ref Vector2 value, string name, JsonReader reader, JsonSerializer serializer)
    {
        switch (name)
        {
            case nameof(value.x):
                value.x = (float)reader.ReadAsDouble().GetValueOrDefault(0d);
                break;
            case nameof(value.y):
                value.y = (float)reader.ReadAsDouble().GetValueOrDefault(0d);
                break;
        }
    }
}
```

<br/>

#### `WriteToJson()` [^](#smjsonconverterbase-)
This method should define what is written into the json and assign a key/value pair for each value
that should be in the json.

```csharp
protected abstract IEnumerable<KeyValuePair<string, object>> WriteToJson(Vector2 value, JsonSerializer serializer);
```

```csharp
public sealed class JsonConverterVector2 : SmJsonConverterBase<Vector2>
{
    /// <summary>
    /// Gets the values to write into json from the value.
    /// </summary>
    /// <param name="value">The value to write from.</param>
    /// <param name="serializer">The serializer</param>
    /// <returns>A collection of values to store in JSON.</returns>
    protected override IEnumerable<KeyValuePair<string, object>> WriteToJson(Vector2 value, JsonSerializer serializer)
    {
        return new KeyValuePair<string, object>[]
        {
            new KeyValuePair<string, object>(nameof(value.x), value.x),
            new KeyValuePair<string, object>(nameof(value.y), value.y),
        };
    }
}
```

<br/>

### `ISaveMetaData` [^](#contents)
Implement to define extra data that is added to the `$metadata` tag in the save data structure. This is
read-only and is mainly intended to aid in context info for debugging etc.

### Properties

#### `Key` [^](#isavemetadata-)
Defines the string key the meta-data is saved under.

```csharp
public string Key { get; }
```

```csharp
public sealed class SaveMetaDataGameInfo : ISaveMetaData
{
    /// <summary>
    /// The key for the metadata to be stored under.
    /// </summary>
    public string Key => "$game_info";
}
```

<br/>

#### `CanWriteMetaData` [^](#isavemetadata-)
This property should define if the meta-data can be written to the save. Use to toggle the data off if
you don’t need it etc.

```csharp
public bool CanWriteMetaData { get; }
```

```csharp
public sealed class SaveMetaDataGameInfo : ISaveMetaData
{
    /// <summary>
    /// Gets if the metadata can be written to the save.
    /// </summary>
    public bool CanWriteMetaData => SmAssetAccessor.GetAsset<DataAssetSettings>().UseMetaDataGameInfo;
}
```

<br/>

### Methods

#### `GetMetaData()` [^](#isavemetadata-)
This should return all the values in a single object to write to the save when called.

```csharp
public JObject GetMetaData();
```

```csharp
public sealed class SaveMetaDataGameInfo : ISaveMetaData
{
    /// <summary>
    /// Gets the metadata to write to the save.
    /// </summary>
    /// <returns>A JObject with the data to write.</returns>
    public JObject GetMetaData()
    {
        return new JObject
        {
            ["$version"] = $"{Application.version}",
            ["$save_date"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss"),
        };
    }
}
```

<br/>

### `ILegacySaveHandler` [^](#contents)
Implement to define a custom solution to port older 2.x save manager saves to the 3.x set-up. By
default, the asset will try to port all 2.x save data to 3.x global data where the keys match. You can
implement this interface to change that to something custom should you wish.

### Methods

#### `ProcessLegacySaveData()` [^](#ilegacysavehandler-)
This method should return the processed data, adding the legacy data into the loaded json to
produce the converted result.

```csharp
public JToken ProcessLegacySaveData(JToken loadedJson, IReadOnlyDictionary<string, JArray> legacyData);
```

```csharp
public class LegacySaveHandlerGlobalOnly : ILegacySaveHandler
{
    /// <summary>
    /// Converts a legacy save to the new setup.
    /// </summary>
    /// <param name="loadedJson">The loaded JSON to work with (3.x).</param>
    /// <param name="legacyData">The legacy data to work with (2.x).</param>
    /// <returns>The updates json to continue loading from (3.x).</returns>
    public JToken ProcessLegacySaveData(JToken loadedJson, IReadOnlyDictionary<string, JArray> legacyData)
    {
        // Processes each save value based on if its key exists in the global save in 3.x
        // If its now slot data it'll not be transferred in this setup.
        // You'll have to your own handler if you want to load legacy to slots.
        JToken updated = loadedJson;

        var globalDataArray = loadedJson["$content"]["$global"].Value<JArray>();
        
        foreach (var entry in legacyData)
        {
            foreach (var legacySaveValue in entry.Value)
            {
                var legacySaveValueKey = legacySaveValue["$key"].Value<string>();

                for (var i = 0; i < globalDataArray.Count; i++)
                {
                    var currentSaveValue = globalDataArray[i];

                    if (currentSaveValue["$key"].Value<string>() != legacySaveValueKey) continue;

                    var adjusted = currentSaveValue;
                    adjusted["$value"] = legacySaveValue["$value"];

                    if (legacySaveValue["$default"] != null)
                    {
                        adjusted["$default"] = legacySaveValue["$default"];
                    }
                    
                    updated["$content"]["$global"][i] = adjusted;

                    SmDebugLogger.LogDev($"Legacy data converted:\n{updated["$content"]["$global"][i]}");
                }
            }
        }

        return updated;
    }
}
```

<br/>