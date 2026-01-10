![C#](https://img.shields.io/badge/C%23-239120?style=flat&logo=csharp&logoColor=white)
![.NET 8](https://img.shields.io/badge/.NET%208-512BD4?style=flat&logo=dotnet&logoColor=white)
![Avalonia](https://img.shields.io/badge/Avalonia-9B4F96?style=flat&logo=avalonia&logoColor=white)



# <img width="35" height="35" alt="icon" src="https://github.com/user-attachments/assets/8e06aaeb-4705-4a68-b6f2-69b869dc9493" />   KeyboardGestures  

#### A global keyboard command tool for Windows
It listens for key sequences and runs actions such as launching apps, opening URLs, taking screenshots and more.

## Usage
* Overlay:
    Press and hold <b><i>ActivationKey</i> + Space </b> to show the command overlay
* Command Sequences:
    When <b><i>ActivationKey</i></b> is held, the app listenes for key sequences. <br>
    The rest of the sequence is typed in order, then the command executes after releasing <b><i>ActivationKey</i></b>.
* Command definitions:
    You can edit current commands and add new ones by going <br>
    to the <b> Settings </b> screen <br>
    <sub>(look for the <b> KeyboardGestures </b> tray icon)</sub>
* Set the <b><i>Activation Key</i></b> to whatever in the settings

## Tech
* C#/.NET 8
* Avalonia UI
* Win32 APIs (global keyboard hook)

## Download

[ **Windows (x64)** Latest Release](https://github.com/artiomAdam/KeyboardGestures/releases/download/v.1.0.0.beta.2/KeyboardGestures-1.0.0-beta.2-win64.zip)


