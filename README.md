![2022 1 SM Banner 1200x630](https://user-images.githubusercontent.com/33253710/166158530-56169759-079e-403c-9053-78ed5e7d05da.jpg)

<b>Save Manager</b> is a <b>FREE</b> local save system for Unity games. 
> Version 1.1.0

## Key Features
- Custom editor window to assist in save class creation.
- Simple and easy to use.
- Save & Load Vectors, Colors and your own Custom Classes (with savable values).
- Ideal for beginner developers

## How To Install
Either download and import the package from the releases section or the <a href="https://assetstore.unity.com/packages/tools/utilities/save-manager-cg-176437">Unity Asset Store</a> and use the package manager. Alternatively, download this repo and copy all files into your project. 

## Setup & Basic Usage
The setup for the asset is very straight forward as there is a handy editor tool to do all the hard work for you. The tool assists in the creation of the SaveData.cs file which a blank version of is provided with the asset to avoid errors in the package. Now you can just write in the file yourself if you know what you are doing and put in your save types. 

![SM-Tools-Tab](https://user-images.githubusercontent.com/33253710/154558890-427c2505-f0e8-4c69-8404-807b293f4aa1.png)

To access the editor tool, all you have to do is navigate to the tools tab on the navigation bar at the top of the Unity window and enter the Save Manager | CG tab and press the Save Data Editor option. Once pressed an editor window will popup which can be moved, resized and docked as you would with any other tab like the inspector, hierarchy and game view tabs for instance.

![SM-EditTab](https://user-images.githubusercontent.com/33253710/154558933-96594740-35c7-485b-9177-806f66b1ac81.png)

The editor tool comprises of 2 main tabs, the first one allows you to create and edit the Save Data class with and the second display information about the asset as well as useful links relevant to the asset. When opening the tool, if you already have an existing Save Data setup which is valid, the editor will update to show the current values for you to adjust and edit. Note that if you edit the class outside of the tool and then open the editor tool, you may find some values will not read correctly. 

![SM-ReadyToGenerate](https://user-images.githubusercontent.com/33253710/154558962-1cc91867-d958-4c18-931a-af7247c03af9.png)

### Creating a variable

Once you have added a field, you be presented with a grouping of fields to edit for this variable like what you can see on the left here. There are 4 options that the field will show by default, but this varies on the setup. Each grouping will always have:

- Variable Name - Which is what the variable will be called.
- Data Type - Which is the data type the variable is defined as.
- Collection Type - Which defines whether this variable is a collection like a list, array, queue, stack or if it is a normal single variable.

**Other Options**

If the data type is a class value then an extra field will appear for the class name, which is where you'd but the name of the class you want the variable to be of. This field is CaSe SeNiStIvE! 

If you have the grouping with no collection type then a field will be available for a default value. This can be left blank or have a value that is valid for the type. The field will adjust based on the data type you have selected. 

> ⚠️ Note: Sprites & Classes can't have a default value.

You can keep adding more variables with the + Add Field button or the **green + button** next to each grouping and remove a grouping with the **red - button.** 

### Generating the class for use

Once you have filled in the variables you want to save, you can press the **Generate New SaveData Class button** to make the class with the inputs you gave. At this point the editor will refresh with the class will be placed in the following directory in your project, please delete the SaveData.cs class provided in asset package if you haven't already at this point as it is no longer needed:

> ℹ️ Assets/Scripts/Save/SaveData.cs

## Limitations
For technical reasons, the asset doesn't support WebGL game builds at present. This system is also local ONLY.

## Basic Scripting

### Loading Game with some values
> SaveData loadData = SaveManager.LoadGame(); 
>
> player.name = loadData.playerName;
> 
> player.health = loadData.playerHealth.ToString();
> 
> player.trandform.position = loadData.playerPosition.ToString();
> 
> player.sprite = loadData.playerSprite;

### Saving Game with some values

> SaveData saveData = new SaveData();
> 
> saveData.playerName = player.name;
> 
> saveData.playerHealth = player.health;
> 
> saveData.playerPosition = player.transform.position;
> 
> saveData.playerSprite = player.sprite;
> 
> SaveManager.SaveGame(saveData);

## Documentation
You can access a online of the documentation here: <a href="https://carter.games/savemanager">Online Documentation</a>. A offline copy if provided with the package and asset if needed. 

## Authors
- <a href="https://github.com/JonathanMCarter">Jonathan Carter</a>

## Licence
MIT Licence
