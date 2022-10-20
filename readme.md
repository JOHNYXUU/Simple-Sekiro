# Simple Sekiro

This is a action game demo using unity for self practising, mostly for **programming**

![](https://raw.githubusercontent.com/JOHNYXUU/Simple-Sekiro/main/pictures/QQ%E6%88%AA%E5%9B%BE20221020110309.png)



## What have done ?

### Game Loop

in directory : [Game Loop](https://github.com/JOHNYXUU/Simple-Sekiro/tree/main/Assets/Scripts/Main)

**GameConfig.cs** : storing data like: **numerical data**(such as speed) ; **path of resource**

**GameLoop.cs** : Control the whole game. Update every frame

### Simple 3c system based on state machine

in directory : [3c system](https://github.com/JOHNYXUU/Simple-Sekiro/tree/main/Assets/Scripts/StateMachine)

![](https://raw.githubusercontent.com/JOHNYXUU/Simple-Sekiro/main/pictures/QQ%E6%88%AA%E5%9B%BE20221020111157.png)

construct with **State Machine **

note : this part **only for** **logic computing**, contains **no** data for player. 

#### Enum

State enum for every state that my player would be in

#### **Base** 

**Player State** : base Player State class for **Animation** and **Logic**,each includes some useful functions

**Hierarchy State** : maintain some player states , only  **one** sub state works in one frame

**Parallel State**  : maintain some player states , **all** sub state work in one frame

#### Logic

maintain player's logic state in every frame

Arm : 

Attack : 

Move :

Pose :

1111
