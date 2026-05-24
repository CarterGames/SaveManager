## Locale
- [EN](Listeners_EN.md)

## Docs
- [Usage Documentation](Docs_EN.md)
- [Scripting API](Scripting_EN.md)
- [Listener API](Listeners_EN.md)
- [Extension API](Extension_EN.md)

<br/>

You can listen to some of the assets set-up by implementing specific interfaces without needing any
extra logic, these are:

## Contents
- [IGameSaveListener](#igamesavelistener-)
- [IGameLoadListener](#igameloadlistener-)

### `IGameSaveListener` [^](#contents)
Implement to list to when the game saving is called to start and completed.

```csharp
public class MyClass : IGameSaveListener
{
    /// <summary>
    /// Implement to run logic when the save is called to save at runtime.
    /// </summary>
    public void OnGameSaveCalled()
    {
        
    }


    /// <summary>
    /// Implement to run logic when the save has completed saving at runtime.
    /// </summary>
    public void OnGameSaveCompleted()
    {
        
    }
}
```

<br/>

### `IGameLoadListener` [^](#contents)
Implement to list to when the game loading is called to start and completed or fails.

```csharp
public class MyClass : IGameSaveListener
{
    /// <summary>
    /// Implement to run logic when the save is called to load at runtime.
    /// </summary>
    public void OnGameLoadCalled()
    {
        
    }

    /// <summary>
    /// Implement to run logic when the save has failed to load the last saved data.
    /// </summary>
    public void OnGameLoadFailed(LoadFailInfo loadFailInfo)
    {
        
    }
    
    /// <summary>
    /// Implement to run logic when the save has failed to load (includes trying backups)
    /// </summary>
    public void OnGameLoadFailedCompletely()
    {
        
    }

    /// <summary>
    /// Implement to run logic when the save has completed loading at runtime.
    /// </summary>
    public void OnGameLoadCompleted()
    {
        
    }
}
```