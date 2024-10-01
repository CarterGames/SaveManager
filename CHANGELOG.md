# 2.3.0
## Asset changes

- New editor only save so the build save is in the normal location.
- New save defaults setup that lets users assign defaults outside of constructors when creating save values.
- Save defaults now apply when making a build to all save objects so builds don't have persistent data from editor saves.
- New setting to see save defaults in the editor tab like save keys
- New editor GUI on save objects to see default values.
- Removed some older 2.0.x legacy issue fixers.
- Removed some now redundant API bits. 


# 2.2.0
## Asset changes

- Added support for git URL importing of the asset over .unityPackage. This is considered the recommended install method going forward for ease of updates etc.
- Added porting tool to transfer user asset settings to the new location required for the git URL update.
- Updated scriptable asset flow with the latest from the cart library which is more modular and easy to use on the backend.
- Some additional helper classes for more performant editor operations from within the asset.