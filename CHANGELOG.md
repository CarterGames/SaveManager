# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [3.0.8] - 2026-06-21

### Fixed
- Fixed an issue where some settings in the asset do not save changes when they are edited by the user [[KI-26](https://cartergames.notion.site/Some-settings-do-not-save-in-the-settings-provider-374f72ed3eaf804ebf60e2745806d3b1)].
- Fixed an issue reported by a user where the asset would not compile in Unity 6.0 > 6.3 versions due to an API change in a contribution [[KI-27](https://cartergames.notion.site/Compile-issue-with-Unity-6-6-3-x-386f72ed3eaf803fa80bf8e0d0553769)] [[GIT-ISS#42](https://github.com/CarterGames/SaveManager/issues/42)].

## [3.0.7] - 2026-05-24

### Changed
- Documentation updated to a .md file structure over a .pdf file to allow for contribution and changes.
- Refactored AssemblyClassDef with latest Cart lib setup changes.
- (Contribution: NoixDeXydre) Improved assembly API performance [[GIT-PR#39](https://github.com/CarterGames/SaveManager/pull/39)].

### Fixed
- Package json samples fixed (again).
- Fixed Backup & legacy interface implementation not been picked up in some cases.
- (Contribution: NoixDeXydre) Fixed runtime initialize call point causing issues around first scene load [[GIT-PR#38](https://github.com/CarterGames/SaveManager/pull/38)].
- Fixed an issue where required asset was not generated at the expected time, causing an error in some cases when imported for the first time.

## [3.0.6] - 2026-05-16

### Added
- Added link.xml to preserve runtime code in the asset from stripping. Based on fix in [[GIT-PR#33](https://github.com/CarterGames/SaveManager/pull/33)].

### Fixed
- (Contribution: NoixDeXydre) Fixed possible null ref for backup handler if it ever happens [[GIT-PR#34](https://github.com/CarterGames/SaveManager/pull/34)].

## [3.0.5] - 2026-05-04

### Fixed
- Fixed an API bug where trying to get a save object for a particular slot would fail to get the information for the intended slot. [[KI-23](https://www.notion.so/cartergames/Save-slot-object-API-bug-356f72ed3eaf8090968ce05e64c00b05)]

## [3.0.4] - 2026-04-06

### Changed
- Save slots now auto unload the currently loaded slot when auto-saving the game.
- Save slots will re-load the last loaded slot when focus restored if applicable.

### Fixed
- Fixed an issue where the save editor would lose the save objects when exiting playmode sometimes. [[KI-19](https://www.notion.so/cartergames/Save-editor-fails-to-refresh-when-expected-33af72ed3eaf80be8b25c0c09690ff04)]
- Fixed an issue where save slots would not unload when exiting the game when auto save was on. [[KI-20](https://www.notion.so/cartergames/Save-slot-do-not-unload-on-auto-save-33af72ed3eaf8077bc97f8fab7f6047a)]
- Fixed an issue where only having one save category would mean the save objects under it would not render in the save editor GUI. [[KI-21](https://www.notion.so/cartergames/Save-editor-is-missing-entries-when-only-1-category-is-used-33af72ed3eaf80ecaaf0db27f8a97dfd)]
- Fixed save slots losing data when the save slot was made from the save editor. [[KI-18](https://www.notion.so/cartergames/Save-slots-lose-data-sometimes-33af72ed3eaf80b482b9c023c64f29dc)]

## [3.0.3] - 2026-03-19

### Changed
- Updated the art in the sample scenes to match the new asset art style.

### Removed
- Removed old art files from the asset.
- Removed custom script file icons from asset scripts.

### Fixed
- Fixed an error with the save editor where it would fail to populate after exiting playmode.
- Fixed an error with the save editor where save slot save data would error in the GUI when no save categories were in use in the setup.

## [3.0.2] - 2026-02-27

### Fixed
- Fixed an error when expanded the save data foldout on a save slot when no slot save objects in the project.
- Fixed an issue where the save editor would fail to initialize on initial importing of the asset.
- Fixed some settings being editable even when the system that toggles them was disabled.

## [3.0.1] - 2026-01-31

### Changed
- Improved the look of the slots sample scene.

### Fixed
- Fixed an error on import due to a missing .meta file.
- Fixed sample scenes not importing with the correct assets.
- Fixed a bug with the slots sample scene where an empty slots save would error on entering play mode.

## [3.0.0] - 2026-01-10

### Added
- Save slots now fully supported within the asset.
- Automatic save backups now supported by the asset.
- Modular save locations now supported.
- Modular save encryption options now supported.
- Porting feature for 2.x save data into the 3.x global data setup. 
- Dependency on newtonsoft json to function for better json support.
- Metadata section added to the game save to support read-only save information.

### Changed
- Save Objects updated to not need an instance made in the project to function.
- Improved the save editor GUI to be more optimized and performant.

### Fixed
- Fixed the save json structure so it is actually proper json.
