<p align="center">
    <img src="./Assets/Design/Menus/MainMenu/Hero.png" alt="FrostFire" />
</p>
<p align="center">
    <font size="5"><strong>FrostFire</strong></font><br />
    <em>Created by: Paul Cavallo, June Ka, John Lim, Jake Pauls, and Kiwon Song</em>
</p>
<p align="center">
    <a href="https://unity.com/"><img src="https://img.shields.io/badge/Made%20with-Unity-57b9d3.svg?style=flat&logo=unity" /></a>
    <img src="https://img.shields.io/badge/-v2021.1.17-57b9d3?style=flat" />
</p>

### Overview 

FrostFire is a top-down multiplayer survival game that enforces cooperation in the context of horde-mode style gameplay.

The goal is to survive the onslaught of enraged snowmen while keeping your only source of heat, the campfire, alive. 
Alongside defending the fire from the snowmen, players must also collect logs around the map to replenish its constantly diminishing fuel. 
If the campfire's fuel runs out, the players lose. If the campfire's still alive and the players clear all the waves of snowmen, the players win.

### Instructions

- Grab the latest [release for your platform](https://github.com/paulc100/FrostFire/releases)
- Extract the build and double click `FrostFire.exe` to open the game
- Click `Play` to begin the game
- To join players into the game, press enter on the keyboard or press `Select` on the controller
### Controls

Mouse/Keyboard
- `WASD` for movement
- `Space` to jump
- `Left Click` to attack
- `F` to share warmth

Controller
- `Left Stick` for movement
- South Button (`A` or `X`) to jump
- East Button (`X` or `Square`) to attack
- North Button (`Y` or `Triangle`) to share warmth

### Project Information
- Clone the project and import into Unity
```
$ git clone git@github.com:paulc100/FrostFire.git
```

**Note:** This project uses **Unity 2021.1.17**

### Outstanding Bugs

- Logs spawning in the same place
- Falling out of the map (if players navigate outside the backyard)
- Not being able to pickup logs that spawn within trees
- Sometimes red particles would appear on a player who is healing and should have only green particles
