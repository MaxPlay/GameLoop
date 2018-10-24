# GameLoop
My Master Thesis, a tool that can be used as a visual scripting system for card games in the Unity game engine.

## Getting Started
This tool is designed to be used in the Unity game engine. Check the prerequisites to see if you can use it!

### Prerequisites
This tool is designed to work with Unity 2018.2 and later, but may also work with prior versions. It needs at least .NET Scripting Backend support of 4.x.

[You can get Unity 2018.2 here.](https://unity3d.com/get-unity/download/archive)

### Installing
You can find a unitypackage in the download section. Simply download and import it into your project.

### Setup
To setup the tool correctly, do the following things:

1. (If Required) Set the scripting backend to 4.x in the player settings.
2. Create an instance of the following assets in your assets folder:
    1. ModuleSystem
    2. GameSetup
    3. InterfaceControl
3. Plug the GameSetup and the InterfaceControl into the Module System
4. Create as many players, card stacks and card packs as required for the game and add them into the game setup and the modulesystem.

You are now ready to go!

## Tested with
You may add new versions of Unity, after you tested it in them.

The current earliest version is *Unity 2018.2.0f2*.

## Authors
* **Maximilian Stürzl** - Author of the tool - [Website](https://www.maxstuerzl.com/)

## License
This work is licensed under GPL GNU Public License v3.0. See the [LICENSE](LICENSE) file for details.

## Acknowledgments
* [CHFHHD](https://github.com/chfhhd) - my examiner and mentor for the thesis
* [LotteMakesStuff](https://github.com/LotteMakesStuff) - because she is awesome and I learned a lot about editor scripting from her!