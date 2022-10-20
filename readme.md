# Simple Sekiro

This is a action game demo using unity for self practising, mostly for **programming**

![](https://raw.githubusercontent.com/JOHNYXUU/Simple-Sekiro/main/pictures/QQ%E6%88%AA%E5%9B%BE20221020110309.png)



## What have done ?

### Game Loop

in directory : [Game Loop](https://github.com/JOHNYXUU/Simple-Sekiro/tree/main/Assets/Scripts/Main)

**GameConfig.cs** : store data like: **numerical data**(such as speed) ; **path of resource**

**GameLoop.cs** : Control the whole game. Update every frame

### Simple character controller system based on state machine

in directory : [state machine](https://github.com/JOHNYXUU/Simple-Sekiro/tree/main/Assets/Scripts/StateMachine)

![](https://raw.githubusercontent.com/JOHNYXUU/Simple-Sekiro/main/pictures/QQ%E6%88%AA%E5%9B%BE20221020111157.png)

construct with **State Machine**

note : this part **only for** **logic computing**, contains **no** data for player. 

#### Enum

State enum for every state that my player would be in

#### **Base** 

**Player State** : base Player State class for **Animation** and **Logic**,each includes some useful functions

**Hierarchy State** : maintain some player states , only  **one** sub state works in one frame

**Parallel State**  : maintain some player states , **all** sub state work in one frame

#### Logic

**maintain player's logic state in every frame with four hierarchy state together**

**Arm** :  decide weapon pose 

**Attack** : decide attack mode now 

**Move** : decide player's movement

**Pose** : decide player's lower body

(note :  these four hierarchy state are controlled by **LogicStateMachine which is a parallel state actually** )

![](https://raw.githubusercontent.com/JOHNYXUU/Simple-Sekiro/main/pictures/Logic%20State%20Machine.png)

#### Anim

**maintain player's animation state, make sure that player's animator controller can switch from one state to another.**

every anim state below has a c# script to change the **parameters** in animator controller,so that player can switch between different animation clips. 

![](https://raw.githubusercontent.com/JOHNYXUU/Simple-Sekiro/main/pictures/Anim%20State%20Machine.png)

### Animator Controller

In Directory : [Animator Controller](https://github.com/JOHNYXUU/Simple-Sekiro/blob/main/Assets/Scripts/Controllers/AnimatorController.cs)

each c# script in animStateMachine actually **represent one animator state** in animator(in Assets/Resources/Animators/) below

![](https://raw.githubusercontent.com/JOHNYXUU/Simple-Sekiro/main/pictures/QQ%E6%88%AA%E5%9B%BE20221020154402.png)

#### script 

AnimatorController.cs can change the parameter in animator

#### animator

two types of movement are accomplished in this game

##### **1Dir move**

when player doesn't lock his view on an enemy, he would move in one direction only

![](https://raw.githubusercontent.com/JOHNYXUU/Simple-Sekiro/main/pictures/QQ%E6%88%AA%E5%9B%BE20221020170300.png)

##### **8Dir move** 

when player lock his view on an enemy,he would move in 8 directions, accomplished with **2d blend** Tree in unity

![](https://raw.githubusercontent.com/JOHNYXUU/Simple-Sekiro/main/pictures/QQ%E6%88%AA%E5%9B%BE20221020170611.png)

##### other state...

just some simple states, nothing special to talk about

### Camera Controller

In directory : [camera](https://github.com/JOHNYXUU/Simple-Sekiro/blob/main/Assets/Scripts/Controllers/CameraController.cs)

In this game ,  camera has two states : **free** and **locked**

In free state, Camera's position and rotation depends on how mouse move



![](https://raw.githubusercontent.com/JOHNYXUU/Simple-Sekiro/main/pictures/free202210201641252.gif)

In locked state, Camera will always look at the locked enemy and move with player instead of  depending on mouse

![](https://raw.githubusercontent.com/JOHNYXUU/Simple-Sekiro/main/pictures/lock202210201642183.gif)

### Simple GO manager system

in directory : [Go manager](https://github.com/JOHNYXUU/Simple-Sekiro/tree/main/Assets/Scripts/Manager)

manage game object in scene , mainly for how these game objects update in every frame

**Audio** : control audio to play,pause...

**Enemy** : update enemy's position 

**Input** : record player's input from keyboard and mouse only

**Particle** :  manage particle system base on object pool

![](https://raw.githubusercontent.com/JOHNYXUU/Simple-Sekiro/main/pictures/QQ%E5%BD%95%E5%B1%8F20221020160654202210201615561.gif)

**Player** : 3c system and Camera update here

**UI** ：ui update here

### Entity

in directory : [entity](https://github.com/JOHNYXUU/Simple-Sekiro/tree/main/Assets/Scripts/Entity)

data for player and enemy (for now) stores in entity

**PlayerEntity** :  player's data,like hp,speed,state......

**EnemyEntity** :  enemy's data,like hp,id......

## What to be done next ? 

Enemy AI,

other anim state like being attacked, special attack in sekiro,

stamina system in sekiro