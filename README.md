# Maze

Maze is a minimalistic procedurally-generated maze traversal game prototype with multiple levels, made in Unity. 

The player is represented by a small green square. The goal is to get from the violet tile to the green one. After completing each level, the player gets transitioned deeper along the z-axis towards the next level. During that, the next level is being visibly generated using a Unity Coroutine. Each consecutive level has ~2.25 times larger area than the previous one, making the game harder and harder.

While moving between levels, the player has a few seconds to examine the next level. For most of that time, however, it is impossible to see the "goal tile", as it is obscured by the green tile from the previous level, which is also transitioned. This forces the player to mentally traverse the maze from only one point – the violet "start tile".

The UI was implemented using Unity's new GameObject-based UI system, introduced in Unity 4.6.

## Pre-built versions

[Windows build](https://github.com/Fazan64/Maze/raw/master/Builds/WinBuild.zip)
[macOS build](https://github.com/Fazan64/Maze/raw/master/Builds/MacBuild.zip)