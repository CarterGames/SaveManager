# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

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
