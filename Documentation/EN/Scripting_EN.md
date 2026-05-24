## Locale
- [EN](Scripting_EN.md)

## Docs
- [Usage Documentation](Docs_EN.md)
- [Scripting API](Scripting_EN.md)
- [Listener API](Listeners_EN.md)
- [Extension API](Extension_EN.md)

<br/>

Below is a small summary of the main API you’ll be accessing from the asset. Note that
I’ve only included the intended API for you to use and not every public property and/or
method.

## Contents
- [Save Manager](#savemanagercs-)
  - Properties
    - [IsInitialized](#isinitialized-)
    - [IsSaving](#issaving-)
    - [IsLoading](#isloading-)
    - [IsBusy](#isbusy-)
  - Events
    - [InitializedEvt](#initializedevt-)
    - [GameSaveStartedEvt](#gamesavestartedevt-)
    - [GameSaveCompletedEvt](#gamesavecompleteddevt-)
    - [GameLoadStartedEvt](#gameloadstartedevt-)
    - [GameLoadedEvt](#gameloadedevt-)
    - [GameLoadFailedEvt](#gameloadfailedevt-)
    - [GameLoadFailedCompletelyEvt](#gameloadfailedcompletelyevt-)
  - Methods
    - [SaveGame](#savegame-)
    - [LoadGame](#loadgame-)
    - [GetGlobalSaveObject](#getglobalsaveobject-)
    - [TryGetGlobalSaveObject](#trygetglobalsaveobject-)
    - [GetActiveSlotSaveObject](#getactiveslotsaveobject-)
    - [TryGetActiveSlotSaveObject](#trygetactiveslotsaveobject-)
    - [GetSlotSaveObject](#getslotsaveobject-)
    - [TryGetSlotSaveObject](#trygetslotsaveobject-)
    - [TryGetSaveValue](#trygetsavevalue-)
    - [TryGetGlobalSaveValue](#trygetglobalsavevalue-)
    - [TryGetActiveSlotSaveValue](#trygetactiveslotsavevalue-)
- [Save Value](#savevaluecs-)
  - Properties
    - [Value](#value-)
    - [ValueType](#valuetype-)
    - [ValueObject](#valueobject-)
    - [HasDefaultValue](#hasdefaultvalue-)
    - [DefaultValue](#defaultvalue-)
    - [DefaultValueObject](#defaultvalueobject-)
  - Methods
    - [ResetValue](#resetvalue-)
- [Save Objects / Slot Save Objects](#saveobjectcsslotsaveobjectcs-)
  - Attributes
    - [SaveCategory](#savecategory-)
  - Properties
    - [Lookup](#lookup-)
    - [HasValue](#hasvalue-)
    - [GetValue](#getvalue-)
    - [SetValue](#setvalue-)
    - [ResetObjectSaveValues](#resetobjectsavevalues-)
  - Methods
    - [ResetValue](#resetvalue-)
- [Save Slot Manager](#saveslotmanager-)
  - Properties
    - [SlotsEnabled](#slotsenabled-)
    - [HasLoadedSlot](#hasloadedslot-)
    - [TotalSlotsInUse](#totalslotsinuse-)
    - [ActiveSlotId](#activeslotid-)
    - [HasAnySlots](#hasanyslots-)
    - [ActiveSlot](#activeslot-)
    - [TotalSlotsRestricted](#totalslotsrestricted-)
    - [RestrictedSlotsTotal](#restrictedslotstotal-)
  - Events
    - [SlotCreatedEvt](#slotcreatedevt-)
    - [SlotDeletedEvt](#slotdeletedevt-)
    - [SlotUnloadedEvt](#slotunloadedevt-)
    - [SlotLoadedEvt](#slotloadedevt-)
    - [SlotLoadFailedEvt](#slotloadfailedevt-)
  - Methods
    - [TryCreateSlotAtId](#trycreateslotatid-)
    - [TryCreateSlot](#trycreateslot-)
    - [LoadSlot](#loadslot-)
    - [UnloadCurrentSlot](#unloadcurrentslot-)
    - [DeleteSlot](#deleteslot-)
- [Save Slot](#saveslot-)
  - Properties
    - [SlotId](#slotid-)
    - [LastSaveDate](#lastsavedate-)
    - [Playtime](#playtime-)

<br/>

All classes for the asset at runtime are under the following:

### Assembly
- `CarterGames.SaveManager.Runtime`
- `CarterGames.Shared.SaveManager` *(Optional, may be needed for some API)*

### Namespace
- `CarterGames.Assets.SaveManager`

*The asset is setup in the mindset that you’ll be leaving the editor setup as is. Most users will not
ever need to mess with it.*

<br/>

## Classes

### `SaveManager.cs` [^](#contents)
The save manager class is the main API class which you’ll use. It is a partial class for readability so
the different sections of the class are in different files.

<br/>

### Properties

#### `IsInitialized` [^](#savemanagercs-)
Gets if the save manager class has fully initialized. You should check this before accessing the other
API as if the asset is not initialized you may get unexpected errors.

```csharp
public static bool IsInitialized { get; }
```

```csharp
private void OnEnable()
{
    if (SaveManager.IsInitialized)
    {
        // Your logic here.
    }
}
```

<br/>

#### `IsSaving` [^](#savemanagercs-)
Gets if the save manager is currently saving data.

```csharp
public static bool IsSaving { get; }
```

```csharp
private void OnEnable()
{
    if (SaveManager.IsSaving)
    {
        // Your logic here.
    }
}
```

<br/>

#### `IsLoading` [^](#savemanagercs-)
Gets if the save manager is currently loading data.

```csharp
public static bool IsLoading { get; }
```

```csharp
private void OnEnable()
{
    if (SaveManager.IsLoading)
    {
        // Your logic here.
    }
}
```

<br/>

#### `IsBusy` [^](#savemanagercs-)
Gets if the save manager is currently running a save or load operation.

```csharp
public static bool IsBusy { get; }
```

```csharp
private void OnEnable()
{
    if (SaveManager.IsBusy)
    {
        // Your logic here.
    }
}
```

<br/>

### Events

#### `InitializedEvt` [^](#savemanagercs-)
Is raised when the save manager has initialized.
Add a listener to receive the evt when it is raised

```csharp
public static readonly Evt InitializedEvt;
```

```csharp
private void OnEnable()
{
    SaveManager.InitializedEvt.Add(OnInitialized);
}

private void OnInitialized()
{
    // Put logic here to run after the save manager has initialized.
    // So the SaveManager.IsInitialized property would return true at this point.
}
```

<br/>

#### `GameSaveStartedEvt` [^](#savemanagercs-)
Is raised when the save manager has started to save data.
Add a listener to receive the evt when it is raised

```csharp
public static readonly Evt GameSaveStartedEvt;
```

```csharp
private void OnEnable()
{
    SaveManager.GameSaveStartedEvt.Add(OnSaveStarted);
}

private void OnSaveStarted()
{
    // Put logic here to run when the save manager has started saving.
    // Like showing a graphic on screen to indicate to the player the game is saving etc.
}
```

<br/>

#### `GameSaveCompletedEvt` [^](#savemanagercs-)
Is raised when the save manager has completed saving data.
Add a listener to receive the evt when it is raised

```csharp
public static readonly Evt GameSaveCompletedEvt;
```

```csharp
private void OnEnable()
{
    SaveManager.GameSaveCompletedEvt.Add(OnSaveCompleted);
}

private void OnSaveCompleted()
{
    // Put logic here to run after the save manager has completed saving.
    // Like hiding a graphic that was on screen to indicate to the...
    // ...player the game was saving etc.
}
```

<br/>

#### `GameLoadStartedEvt` [^](#savemanagercs-)
Is raised when the save manager has started to load data.
Add a listener to receive the evt when it is raised

```csharp
public static readonly Evt GameLoadStartedEvt;
```

```csharp
private void OnEnable()
{
    SaveManager.GameLoadStartedEvt.Add(OnLoadStarted);
}

private void OnLoadStarted()
{
    // Put logic here to run when the save manager has started loading.
}
```

<br/>

#### `GameLoadedEvt` [^](#savemanagercs-)
Is raised when the save manager has completed loading data successfully.
Add a listener to receive the evt when it is raised

```csharp
public static readonly Evt GameLoadedEvt;
```

```csharp
private void OnEnable()
{
    SaveManager.GameLoadedEvt.Add(OnGameLoaded);
}

private void OnGameLoaded()
{
    // Put logic here to run when the save manager has finished loading successfully.
    // Such as loading your game scene or refreshing your game with the loaded data.
}
```

<br/>

#### `GameLoadFailedEvt` [^](#savemanagercs-)
Is raised when the save manager has failed to load a data set for any reason.
Add a listener to receive the evt when it is raised

```csharp
public static readonly Evt<LoadFailInfo> GameLoadFailedEvt;
```

```csharp
private void OnEnable()
{
    SaveManager.GameLoadFailedEvt.Add(OnGameLoadFail);
}

private void OnGameLoadFail(LoadFailInfo loadFailReasonInfo)
{
    // Put logic here to run when the save manager has failed to load.
    // The setup will try to load a backup if there are any left to try before failing completly.
    // Use the LoadFailInfo class to provide info about the issue.
}
```

<br/>

#### `GameLoadFailedCompletelyEvt` [^](#savemanagercs-)
Is raised when the save manager has failed to load data from the current save or any existing
backups. Add a listener to receive the evt when it is raised

```csharp
public static readonly Evt GameLoadFailedCompletelyEvt;
```

```csharp
private void OnEnable()
{
    SaveManager.GameLoadFailedCompletelyEvt.Add(OnGameLoadCompleteFail);
}

private void OnGameLoadCompleteFail()
{
    // Put logic here to run when the save manager has failed to load.
    // Use to display an error to the user that something has gone wrong etc.
    // Use the LoadFailInfo class to provide info about the issue.
}
```

<br/>

### Methods

#### `SaveGame()` [^](#savemanagercs-)
Saves the game in its current state when called.

```csharp
public static void SaveGame();
```

```csharp
private void OnEnable()
{
    // Saves the game when called.
    // Use when you want to save the game manually at critical points.
    SaveManager.SaveGame();
}
```

<br/>

#### `LoadGame()` [^](#savemanagercs-)
Loads the game from the stored save data when called.

```csharp
public static void LoadGame();
```

```csharp
private void OnEnable()
{
    // Loads the game when called.
    // Use if you need to manually load the game.
    // Most users will not need to call this method.
    SaveManager.LoadGame();
}
```

<br/>

#### `GetGlobalSaveObject()` [^](#savemanagercs-)
Gets the global save object of the defined type. For a safer call, please use the
`TryGetGlobalSaveObject` method instead.

`T` = SaveObject

```csharp
public static T GetGlobalSaveObject<T>();
```

```csharp
public class MySaveObject : SaveObject { }

private void OnEnable()
{
    // Can be null through this API if not found.
    var saveObject = SaveManager.GetGlobalSaveObject<MySaveObject>();
}
```

<br/>

#### `TryGetGlobalSaveObject()` [^](#savemanagercs-)
Tries to get the global save object of the defined type. It returns the result of the call, so you can
catch any issues instead of dealing with null values.

`T` = SaveObject

```csharp
public static bool TryGetGlobalSaveObject<T>(out T saveObject);
```

```csharp
public class MySaveObject : SaveObject { }

private void OnEnable()
{
    if (SaveManager.TryGetGlobalSaveObject<MySaveObject>(out var mySaveObject))
    {
        // Safe to access and use at this point.
    }
}
```

<br/>

#### `GetActiveSlotSaveObject()` [^](#savemanagercs-)
Gets a save object from the currently active slot of the defined save object type. For a safer call,
please use the `TryGetActiveSlotSaveObject` method instead.

`T` = SlotSaveObject

```csharp
public static T GetActiveSlotSaveObject<T>();
```

```csharp
public class MySaveObject : SlotSaveObject { }

private void OnEnable()
{
    // Can be null through this API if not found.
    var saveObject = SaveManager.GetActiveSlotSaveObject<MySaveObject>();
}
```

<br/>

#### `TryGetActiveSlotSaveObject()` [^](#savemanagercs-)
Tries to get a save object from the currently active slot of the defined save object type. It returns the
result of the call, so you can catch any issues instead of dealing with null values.

`T` = SlotSaveObject

```csharp
public static bool TryGetActiveSlotSaveObject<T>(out T slotSaveObject);
```

```csharp
public class MySaveObject : SlotSaveObject { }

private void OnEnable()
{
    if (SaveManager.TryGetActiveSlotSaveObject<MySaveObject>(out var mySaveObject))
    {
        // Safe to access and use at this point.
    }
}
```

<br/>

#### `GetSlotSaveObject()` [^](#savemanagercs-)
Gets a save object from the currently active slot of the defined save object type. For a safer call,
please use the `TryGetSlotSaveObject` method instead.

`T` = SlotSaveObject

```csharp
public static T GetSlotSaveObject<T>(int slotId);
```

```csharp
public class MySaveObject : SlotSaveObject { }

private void OnEnable()
{
    // Selecting slot 3
    var slotId = 3;
    
    // Can be null through this API if not found.
    var saveObject = SaveManager.GetSlotSaveObject<MySaveObject>(slotId);
}
```

<br/>

#### `TryGetSlotSaveObject()` [^](#savemanagercs-)
Tries to get a save object from the currently active slot of the defined save object type. It returns the
result of the call, so you can catch any issues instead of dealing with null values.

`T` = SlotSaveObject

```csharp
public static bool TryGetSlotSaveObject<T>(int slotId, out T saveObject);
```

```csharp
public class MySaveObject : SlotSaveObject { }

private void OnEnable()
{
    // Selecting slot 3
    var slotId = 3;
    
    if (SaveManager.TryGetSlotSaveObject<MySaveObject>(slotId, out var mySaveObject))
    {
        // Safe to access and use at this point.
    }
}
```

<br/>

#### `TryGetSaveValue()` [^](#savemanagercs-)
Tries to get a save value from anywhere in the save. Use SaveCtx to define the placement of the
value in the save data set-up for a slightly faster call.

`T` = SaveValue

```csharp
public static bool TryGetSaveValue<T>(string saveKey, out SaveValue<T> value, SaveCtx ctx = SaveCtx.Unassigned);
```

```csharp
public class MySaveObject : SaveObject 
{
    public SaveValue<int> savePlayerHealth = new SaveValue<int>("playerHealth", 10);
}

private void OnEnable()
{
    // Gets the save value in any context. 
    if (SaveManager.TryGetSaveValue<int>("playerHealth", out var value, SaveCtx.All)
    {
        // Safe to access and use at this point.
    }
    
    // Gets the save value in the context of the global save only.
    if (SaveManager.TryGetSaveValue<int>("playerHealth", out value, SaveCtx.Global)
    {
        // Safe to access and use at this point.
    }
}
```

<br/>

#### `TryGetGlobalSaveValue()` [^](#savemanagercs-)
Tries to get a save value from just the global save set-up in the save.

`T` = SaveValue

```csharp
public static bool TryGetGlobalSaveValue<T>(string saveKey, out SaveValue<T> value);
```

```csharp
public class MySaveObject : SaveObject 
{
    public SaveValue<int> savePlayerHealth = new SaveValue<int>("playerHealth", 10);
}

private void OnEnable()
{
    // Gets the save value in the context of the global save only.
    if (SaveManager.TryGetGlobalSaveValue<int>("playerHealth", out value)
    {
        // Safe to access and use at this point.
    }
}
```

<br/>

#### `TryGetActiveSlotSaveValue()` [^](#savemanagercs-)
Tries to get a save value from just the active save slot in the save.

`T` = SaveValue

```csharp
public static bool TryGetActiveSlotSaveValue<T>(string saveKey, out SaveValue<T> value);
```

```csharp
public class MySaveObject : SlotSaveObject 
{
    public SaveValue<int> savePlayerHealth = new SaveValue<int>("playerHealth", 10);
}

private void OnEnable()
{
    // Gets the save value in the context of the active save slot save only.
    if (SaveManager.TryGetActiveSlotSaveValue<int>("playerHealth", out value)
    {
        // Safe to access and use at this point.
    }
}
```

<br/>
<br/>

### `SaveValue.cs` [^](#contents)
Defines a value that is stored in the game save. All save values **MUST** have a save key assigned to
them for the system to save them. Any without a key will be flagged in the editor and not be saved
to the save data until corrected.

```csharp
public SaveValue(string key);
public SaveValue(string key, T defaultValue);
```

<br/>

### Properties

#### `Value` [^](#savevaluecs-)
The value stored in the save value. Use to access or edit the actual value stored in the save.

```csharp
public T Value { get; set; }
```

```csharp
public class MySaveObject : SaveObject 
{
    public SaveValue<int> savePlayerHealth = new SaveValue<int>("playerHealth", 10);
    
    private void OnEnable()
    {
        // Changes the save value to 15.
        savePlayerHealth.Value = 15;
    }
}
```

<br/>

#### `ValueType` [^](#savevaluecs-)
The type the save value is, mainly used for the asset itself. But it is public if you wish to use it.

```csharp
public Type ValueType { get; }
```

```csharp
public class MySaveObject : SaveObject 
{
    public SaveValue<int> savePlayerHealth = new SaveValue<int>("playerHealth", 10);
    
    private void OnEnable()
    {
        // Gets the type stored in the save value.
        Debug.Log(savePlayerHealth.ValueType);
    }
}
```

<br/>

#### `ValueObject` [^](#savevaluecs-)
The object-typed value for the saved value. Please use T Value for accessing the save value instead
of this.

```csharp
public object ValueObject { get; set; }
```

```csharp
public class MySaveObject : SaveObject 
{
    public SaveValue<int> savePlayerHealth = new SaveValue<int>("playerHealth", 10);
    
    private void OnEnable()
    {
        // Gets the values stored as a generic object.
        Debug.Log(savePlayerHealth.ValueObject);
    }
}
```

<br/>

#### `HasDefaultValue` [^](#savevaluecs-)
Gets if a default value has been set to this save value. Will return false if the default value is the
type default.

```csharp
public bool HasDefaultValue { get; }
```

```csharp
public class MySaveObject : SaveObject 
{
    public SaveValue<int> savePlayerHealth = new SaveValue<int>("playerHealth", 10);
    
    private void OnEnable()
    {
        if (savePlayerHealth.HasDefaultValue)
        {
            // Save value has a default if it gets here.    
        }
    }
}
```

<br/>

#### `DefaultValue` [^](#savevaluecs-)
Defined the default value of the save value. You can set this post constructor if you wish to. Default
values are stored in the save along-side the current value.

```csharp
public T DefaultValue { get; set; }
```

```csharp
public class MySaveObject : SaveObject 
{
    public SaveValue<int> savePlayerHealth = new SaveValue<int>("playerHealth", 10);
    
    private void OnEnable()
    {
        if (savePlayerHealth.HasDefaultValue)
        {
            // Save value has a default if it gets here.
            Debug.Log(savePlayerHealth.DefaultValue);
        }
    }
}
```

<br/>

#### `DefaultValueObject` [^](#savevaluecs-)
Like the value object variant it is the object-typed value for the saved default value. Please use T
DefaultValue for accessing the save default value instead of this.

```csharp
public object DefaultValueObject { get; set; }
```

```csharp
public class MySaveObject : SaveObject 
{
    public SaveValue<int> savePlayerHealth = new SaveValue<int>("playerHealth", 10);
    
    private void OnEnable()
    {
        if (savePlayerHealth.HasDefaultValue)
        {
            // Save value has a default if it gets here.
            Debug.Log(savePlayerHealth.DefaultValueObject);
        }
    }
}
```

<br/>

### Methods

#### `ResetValue()` [^](#savevaluecs-)
Reset the value to its user defined default if there is one set, or to its type default value if there is no
defined default value.

```csharp
public void ResetValue(bool useDefault = true);
```

```csharp
public class MySaveObject : SaveObject 
{
    public SaveValue<int> savePlayerHealth = new SaveValue<int>("playerHealth", 10);
    
    private void OnEnable()
    {
        // Sets the value to 15.
        savePlayerHealth.Value = 15;
        
        // Reset the value back to 10 (as defined).
        savePlayerHealth.ResetValue();
    }
}
```

<br/>
<br/>

### `SaveObject.cs/SlotSaveObject.cs` [^](#contents)
Defines a scriptable object that can contain save values that the save manager will detect and
handle.

### Attributes

#### `[SaveCategory]` [^](#saveobjectcsslotsaveobjectcs-)
A save object specific attribute that allows you to define a save category for a save object to go
under in the save editor GUI.

```csharp
protected sealed class SaveCategoryAttribute : Attribute
```

```csharp
[SaveCategory("MyCategory")]
public class MySaveObject : SaveObject 
{
    public SaveValue<int> savePlayerHealth = new SaveValue<int>("playerHealth", 10);
}
```

<br/>

### Properties

#### `Lookup` [^](#saveobjectcsslotsaveobjectcs-)
A lookup that is generated when first accessed. It stores a key lookup of all the save values
contained on the save object for access. Mainly used by the assets own API, but you can also use it
if you wish.

```csharp
public Dictionary<string, SaveValueBase> Lookup { get; }
```

<br/>

### Methods

#### `HasValue()` [^](#saveobjectcsslotsaveobjectcs-)
Gets if the save object has a save value defined on it with the entered key.

```csharp
public bool HasValue(string key);
```

```csharp
public class MySaveObject : SaveObject 
{
    public SaveValue<int> savePlayerHealth = new SaveValue<int>("playerHealth", 10);
}

private void OnEnable()
{
    if (SaveManager.TryGetGlobalSaveObject<MySaveObject>(out var mySaveObject))
    {
        // Will return true in this instance.
        if (mySaveObject.HasValue("playerHealth"))
        {
            // Logic if the value key exists.   
        }
    }
}
```

<br/>

#### `GetValue()` [^](#saveobjectcsslotsaveobjectcs-)
Gets a save value on the save object that matches the entered key.

`T` = SaveValue

```csharp
public SaveValue<T> GetValue<T>(string key);
```

```csharp
public class MySaveObject : SaveObject 
{
    public SaveValue<int> savePlayerHealth = new SaveValue<int>("playerHealth", 10);
}

private void OnEnable()
{
    if (SaveManager.TryGetGlobalSaveObject<MySaveObject>(out var mySaveObject))
    {
        // Gets the health save value.
        // Can return null if not found.
        var saveValue = mySaveObject.GetValue("playerHealth");
    }
}
```

<br/>

#### `SetValue()` [^](#saveobjectcsslotsaveobjectcs-)
Sets a save value on the save object that matches the entered key to the entered value.

```csharp
public void SetValue(string key, object value);
```

```csharp
public class MySaveObject : SaveObject 
{
    public SaveValue<int> savePlayerHealth = new SaveValue<int>("playerHealth", 10);
}

private void OnEnable()
{
    if (SaveManager.TryGetGlobalSaveObject<MySaveObject>(out var mySaveObject))
    {
        // Sets the health save value.
        // Fails if the value is not found.
        var saveValue = mySaveObject.SetValue("playerHealth", 100);
    }
}
```

<br/>

#### `ResetObjectSaveValues()` [^](#saveobjectcsslotsaveobjectcs-)
Another API intended for the assets own use. You may use this method to reset all save values on
the save object if need be.

```csharp
public virtual void ResetObjectSaveValues();
```

```csharp
public class MySaveObject : SaveObject 
{
    public SaveValue<int> savePlayerHealth = new SaveValue<int>("playerHealth", 10);
}

private void OnEnable()
{
    if (SaveManager.TryGetGlobalSaveObject<MySaveObject>(out var mySaveObject))
    {
        // Reset all save values on the save object.
        mySaveObject.ResetObjectSaveValues();
    }
}
```

<br/>
<br/>

### `SaveSlotManager.cs` [^](#contents)
The main manager class for the save slots set-up. Use to manage the save slots for users.

### Properties

#### `SlotsEnabled` [^](#saveslotmanager-)
Gets if the save slots set-up is enabled or not.

```csharp
public static bool SlotsEnabled { get; }
```

```csharp
private void OnEnable()
{
    // Gets if slots are enabled.
    Debug.Log(SaveSlotManager.SlotsEnabled);
}
```

<br/>

#### `HasLoadedSlot` [^](#saveslotmanager-)
Gets if a save slot has been loaded into the set-up.

```csharp
public static bool HasLoadedSlot { get; }
```

```csharp
private void OnEnable()
{
    // Returns if a slot is loaded currently.
    Debug.Log(SaveSlotManager.HasLoadedSlot);
}
```

<br/>


#### `TotalSlotsInUse` [^](#saveslotmanager-)
Gets the total number of save slots the system has registered for use.

```csharp
public static int TotalSlotsInUse { get; }
```

```csharp
private void OnEnable()
{
    // Gets the total number of slots that have data.
    Debug.Log(SaveSlotManager.TotalSlotsInUse);
}
```

<br/>

#### `ActiveSlotId` [^](#saveslotmanager-)
Gets the current active slot id.

```csharp
public static int ActiveSlotId { get; }
```

```csharp
private void OnEnable()
{
    // Gets the slot id in use.
    Debug.Log(SaveSlotManager.ActiveSlotId);
}
```

<br/>

#### `HasAnySlots` [^](#saveslotmanager-)
Gets if any slots have been defined or not.

```csharp
public static bool HasAnySlots { get; }
```

```csharp
private void OnEnable()
{
    // Gets if any slots have data.
    Debug.Log(SaveSlotManager.HasAnySlots);
}
```

<br/>

#### `ActiveSlot` [^](#saveslotmanager-)
Gets the active save slot for use.

```csharp
public static SaveSlot ActiveSlot { get; }
```

```csharp
private void OnEnable()
{
    // Gets the slot for use.
    var mySaveSlot = SaveSlotManager.ActiveSlot;
}
```

<br/>

#### `AllSlots` [^](#saveslotmanager-)
Gets all the save slots in a lookup if needed.

```csharp
public static IReadOnlyDictionary<int, SaveSlot> AllSlots { get; }
```

```csharp
private void OnEnable()
{
    // Gets all the slots stored in the setup.
    var allSlots = SaveSlotManager.AllSlots;
}
```

<br/>

#### `TotalSlotsRestricted` [^](#saveslotmanager-)
Gets if the save slot total is limited by the asset settings.

```csharp
public static bool TotalSlotsRestricted { get; }
```

```csharp
private void OnEnable()
{
    // Gets if slots are restricted.
    Debug.Log(SaveSlotManager.TotalSlotsRestricted);
}
```

<br/>

#### `RestrictedSlotsTotal` [^](#saveslotmanager-)
Gets the number of save slots set-up is limited by in the asset settings.

```csharp
public static int RestrictedSlotsTotal { get; }
```

```csharp
private void OnEnable()
{
    // Returns the max amount of slots possible.
    Debug.Log(SaveSlotManager.RestrictedSlotsTotal);
}
```

<br/>

### Events

#### `SlotCreatedEvt` [^](#saveslotmanager-)
Is raised when the slot manager has created a new save slot. The slot is passed as a param.
Add a listener to receive the evt when it is raised.

```csharp
public static readonly Evt<SaveSlot> SlotCreatedEvt;
```

```csharp
private void OnEnable()
{
    SaveSlotManager.SlotCreatedEvt.Add(OnSlotCreated);
}

private void OnSlotCreated(SaveSlot createdSaveSlot)
{
    // Do stuff with the new slot.
    // Such as load the slot or modify it.
}
```

<br/>

#### `SlotDeletedEvt` [^](#saveslotmanager-)
Is raised when the slot manager is deleted. The id of that slot is passed as a param.
Add a listener to receive the evt when it is raised.

```csharp
public static readonly Evt<int> SlotDeletedEvt;
```

```csharp
private void OnEnable()
{
    SaveSlotManager.SlotDeletedEvt.Add(OnSlotDeleted);
}

private void OnSlotDeleted(int deletedSlotId)
{
    // Do any logic on slot deletion
    // Such as updating UI etc.
}
```

<br/>

#### `SlotUnloadedEvt` [^](#saveslotmanager-)
Is raised when the slot manager has unloaded a slot. The id of that slot is passed as a param.
Add a listener to receive the evt when it is raised.

```csharp
public static readonly Evt<int> SlotUnloadedEvt;
```

```csharp
private void OnEnable()
{
    SaveSlotManager.SlotUnloadedEvt.Add(OnSlotUnloaded);
}

private void OnSlotUnloaded(int unloadedSlotId)
{
    // Run logic on slot unload
    // Such as updating UI etc.
}
```

<br/>

#### `SlotLoadedEvt` [^](#saveslotmanager-)
Is raised when the slot manager has loaded a slot. The id of that slot is passed as a param.
Add a listener to receive the evt when it is raised.

```csharp
public static readonly Evt<int> SlotLoadedEvt;
```

```csharp
private void OnEnable()
{
    SaveSlotManager.SlotLoadedEvt.Add(OnSlotLoaded);
}

private void OnSlotLoaded(int unloadedSlotId)
{
    // Run logic on slot load
    // Such as updating UI
    // Or load a game scene etc.
}
```

<br/>

#### `SlotLoadFailedEvt` [^](#saveslotmanager-)
Is raised when the slot manager has failed to load a slot. The id of that slot is passed as a param.
Add a listener to receive the evt when it is raised.

```csharp
public static readonly Evt<int> SlotLoadFailedEvt;
```

```csharp
private void OnEnable()
{
    SaveSlotManager.SlotLoadFailedEvt.Add(OnSlotLoadFailed);
}

private void OnSlotLoadFailed(int unloadedSlotId)
{
    // Run logic on slot load failed
    // Such as showing an error message etc.
}
```

<br/>

### Methods

#### `TryCreateSlotAtId()` [^](#saveslotmanager-)
Tries to create a new save slot at the entered id.

```csharp
public static bool TryCreateSlotAtId(int slotId, out SaveSlot newSlot);
```

```csharp
private void OnEnable()
{
    // Tries to create a slot of the id - 1
    if (SaveSlotManager.TryCreateSlotAtId(1, out var slot))
    {
        // Safe to load the new slot or mess with its data here!
    }
}
```

<br/>

#### `TryCreateSlot()` [^](#saveslotmanager-)
Tries to create a new save slot.

```csharp
public static bool TryCreateSlot(out SaveSlot newSlot);
```

```csharp
private void OnEnable()
{
    // Tries to create a slot.
    if (SaveSlotManager.TryCreateSlot(out var slot))
    {
        // Safe to load the new slot or mess with its data here!
    }
}
```

<br/>

#### `LoadSlot()` [^](#saveslotmanager-)
Loads the slot of the entered id.

```csharp
public static void LoadSlot(int slotId);
```

```csharp
private void OnEnable()
{
    // Loads the slot of id - 1
    SaveSlotManager.LoadSlot(1);
}
```

<br/>

#### `UnloadCurrentSlot()` [^](#saveslotmanager-)
Unloads the currently loaded slot when called.

```csharp
public static void UnloadCurrentSlot();
```

```csharp
private void OnEnable()
{
    // Unloads the current slot, doesn't load another until you call LoadSlot().
    SaveSlotManager.UnloadCurrentSlot();
}
```

<br/>

#### `DeleteSlot()` [^](#saveslotmanager-)
Deletes the slot of the entered id from the save system.

```csharp
public static void DeleteSlot(int slotId);
```

```csharp
private void OnEnable()
{
    // Deletes the slot of id - 1
    SaveSlotManager.DeleteSlot(1);
}
```

<br/>
<br/>

### `SaveSlot.cs` [^](#contents)
The class that holds the data for a save slot in the slots save set-up.

### Properties

#### `SlotId` [^](#slotid-)
Gets the id assigned to this slot.

```csharp
public int SlotId { get; }
```

```csharp
private void OnEnable()
{
    var saveSlot = SaveSlotManager.ActiveSlot;
    
    // Gets the slot id of the active slot.
    Debug.Log(saveSlot.SlotId);
}
```

<br/>

#### `LastSaveDate` [^](#slotid-)
Gets the last time the slot was saved at.

```csharp
public DateTime LastSaveDate { get; }
```

```csharp
private void OnEnable()
{
    var saveSlot = SaveSlotManager.ActiveSlot;
    
    // Gets the last save date.
    Debug.Log(saveSlot.LastSaveDate);
}
```

<br/>

#### `Playtime` [^](#slotid-)
Gets the total playtime of the slot. Is calculated from load to save.

```csharp
public TimeSpan Playtime { get; }
```

```csharp
private void OnEnable()
{
    var saveSlot = SaveSlotManager.ActiveSlot;
    
    // Gets the total playtime of the save slot.
    Debug.Log(saveSlot.Playtime);
}
```

<br/>