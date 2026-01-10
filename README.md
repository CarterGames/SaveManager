<img width="100" height="100" alt="Logo - Save Manager - Filled BG" src="https://github.com/user-attachments/assets/4415ae30-ff63-456e-9912-e2b22cb46616" /></img>
# Save Manager

A free, modular, scriptable object based local save system for Unity.

![GitHub release (latest by date)](https://img.shields.io/github/v/release/CarterGames/SaveManager?style=for-the-badge&color=bf6f31)
<br>
![GitHub License](https://img.shields.io/github/license/CarterGames/SaveManager?style=for-the-badge&color=1e77fa)
<br>
![GitHub all releases](https://img.shields.io/github/downloads/CarterGames/SaveManager/total?style=for-the-badge&color=8d6ca1)
<br>
![GitHub repo size](https://img.shields.io/github/repo-size/CarterGames/SaveManager?style=for-the-badge)
<br>
![Unity](https://img.shields.io/badge/Unity-2020.3.x_or_higher-critical?style=for-the-badge&color=757575)
<br>
<br>
## Key Features
✔️ Save editor window for editing the save data in edit mode<br>
✔️ Save captures to load save states in the editor<br>
✔️ Modular save setup<br>
✔️ Save Encryption available<br>
✔️ WebGL support!<br>
✔️ Easily extendable with custom save storage locations & encryption options<br>
✔️ Custom savable dictionary type included.<br>
<br><br>
## Unity Supported Versions
The asset is developed and maintained in 2020.3.x and make use of available .Net updates in the version. Older versions of Unity are not supported for this asset. The asset has been tested pre-release in its development version: 2020.3.0f1.
<br><br>

## How To Install

### Unity Package Manager (Git URL) [Recommended]
<b>Latest:</b>
<br>
<i>The most up-to-date version of the repo that is considered stable enough for public use.</i>

```
https://github.com/CarterGames/SaveManager.git
```
<br>
<b>Specific branch:</b>
<br>
<i>You can also pull any public branch with the following structure.</i>

```
https://github.com/CarterGames/SaveManager.git#[BRANCH NAME HERE]
```

<i>An example using the pre-release branch for 3.0.0 would be:</i>

```
https://github.com/CarterGames/SaveManager.git#prerelease/3.0.0
```
<br>
<b>Unity Package:</b>
<br>
<i>You can download the .unitypackage from each release of the repo to get a importable package of that version. You do sacrifice the ease of updating if you go this route. See the latest releases <a href="https://github.com/CarterGames/SaveManager/releases">here</a></i>
<br><br>

## Setup & Basic Usage
```For more detailed instructions and API reference, please refer to the documentation.```
<br><br>


<!-- Save Objects -->
### Making Save Objects
A save object is basically a scriptable object that can store save values on it. When defined the save values on each object can be access in the editor and at runtime with ease. To make a save object you just need to make a class that implements the SaveObject class, or SlotSaveObject class if you want the data to be specifically used in save slots.

You can do this manually by making a class that inherits from the SaveObject clases or you can use the built-in SaveObject maker GUI. This can be found under:
<br> ```Tools > Carter Games > Save Manager > Save Object Creator```<br><br>
The tool looks like this:
<br>
<img width="408" height="181" alt="image" src="https://github.com/user-attachments/assets/4782f078-e6ab-47e3-b9c3-16900b04891a"/>
<br><br>
The save object creator window has a really simple set-up. You first enter the name of the class you want to make into the Save Object Name field on the GUI. Then if you have the Save Slots feature enabled you’ll be able to select between a global or slot save object. If not then it’ll be global by default behind the scenes and the option will be hidden from you.

Then all you need to do is press the Create Save Object button and choose where in the project’s assets folder the class should go. Once you confirm the location for the class it will be generated automatically for you.
<br><br>

<!-- Save Values -->
### Making Save Values
A save value defines an entry in the game save. You define save values by using the generic save value class SaveValue<T> as a field on in a SaveObject class. A valid save value MUST:
- Be placed inside of a class that inherits from SaveObject/SlotSaveObject, if not it will not function correctly. 
- Be of a serializable type.
- Have a uniquely defined save key.

You have the option to also define a default value for the save value if you wish, but this is totally optional.

An example of a defined save value below:
<br>
```
[SerializeField] private SaveValue<int> playerHealth = new SaveValue<int>("playerHealth");
```
<br>

<!-- Save Editor -->
### Save Editor
The save editor is the intended way for you to edit the save of your game. You can open the save editor window from the navigation menu’s Save Editor option.
<br> ```Tools > Carter Games > Save Manager > Save Editor```
<br><br>
The save editor is split into 4 tabs:
<br>
| Tab | Description |
|---|---|
| Global Data | Displays all the global save objects and their save values. |
| Save Slots | Displays all the save slots currently defines in the save and their save objects / save values. |
| Save Captures | A tool to help you store particular save files as a backup you can reload into your current save at any time.  |
| Save Backups | Lets you view the current save backups and make save captures from any backup should you wish. |

<br>
The editor window looks like this:
<img width="1101" height="551" alt="image" src="https://github.com/user-attachments/assets/898bc418-b653-4a21-a923-3c2aaf73f1f0" />
<br><br>
See more details on how to use this window in the documentation. 
<br><br>

<!-- Save Manager Basic API -->
## Basic API
All classes for the asset at runtime are under the following:
<br><br><br>
### Assembly

```CarterGames.SaveManager.Runtime ```

```CarterGames.Shared.SaveManager ``` 
(Optional, may be needed for some API)

### Base Namespace

```CarterGames.Assets.SaveManager ```

Below is a short rundown of the most common API you'd be using.
<br><br>

### SaveManager.IsBusy [Property]
```public static bool IsBusy { get; } ```
<br>
Gets if the save manager is currently running a save or load operation.
<br><br>


### SaveManager.SaveGame() [Method]
```public static void SaveGame() ```
<br>
Saves the game in its current state when called.
<br><br>


### SaveManager.LoadGame() [Method]
```public static void LoadGame() ```
<br>
Loads the game from the stored save data when called.
<br><br>


### SaveManager.GetGlobalSaveObject() [Method]
``` public static T GetGlobalSaveObject<T>() ```
<br>
Gets the global save object of the defined type. For a safer call, please use the ```TryGetGlobalSaveObject()``` method instead.
<br><br>


### SaveManager.GetActiveSlotSaveObject() [Method]
```public static T GetActiveSlotSaveObject<T>() ```
<br>
Gets a save object from the currently active slot of the defined save object type. For a safer call, please use the ```TryGetActiveSlotSaveObject()``` method instead.
<br><br>


### SaveManager.GetSlotSaveObject() [Method]
```public static T GetSlotSaveObject<T>(int slotId) ```
<br>
Gets a save object from the defined slot id of the defined save object type. For a safer call, please use the ```TryGetSlotSaveObject()``` method instead.
<br><br>


### SaveManager.TryGetSaveValue() [Method]
```public static bool TryGetSaveValue<T>(string saveKey, out SaveValue<T> value, SaveCtx ctx = SaveCtx.Unassigned)```
<br>
Tries to get a save value from anywhere in the save. Use SaveCtx to define the placement of the value in the save data set-up for a slightly faster call. 
<br><br>


### SaveValue.Value() [Property]
```public T Value { get; set; }```
<br>
The value stored in the save value. Use to access or edit the actual value stored in the save.
<br><br>


### SaveSlotManager.ActiveSlot [Property]
```public static SaveSlot ActiveSlot { get; }```
<br>
Gets the active save slot for use.
<br><br>


### SaveSlotManager.TryCreateSlot() [Method]
```public static bool TryCreateSlot(out SaveSlot newSlot)```
<br>
Tries to create a new save slot.
<br><br>


### SaveSlotManager.LoadSlot() [Method]
```public static void LoadSlot(int slotId)```
<br>
Loads the slot of the entered id.
<br><br>


### SaveSlotManager.UnloadCurrentSlot() [Method]
```public static void UnloadCurrentSlot()```
<br>
Unloads the currently loaded slot when called.
<br><br>


### SaveSlotManager.DeleteSlot() [Method]
```public static void DeleteSlot(int slotId)```
<br>
Deletes the slot of the entered id from the save system.
<br><br>

## Documentation
You can access a online of the documentation here: <a href="https://carter.games/savemanager">Online Documentation</a>. An offline copy is provided with the package and asset if needed. 
<br><br>

## Authors
- <a href="https://github.com/JonathanMCarter">Jonathan Carter</a>
<br><br>
## Licence
GNU V3
