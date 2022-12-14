## Backpack system for vrchat sdk3.0 worlds  

this is a Backpack system for vrchat, synced for late joiners!  
players can Attach one backpack on their back and only the attached player can remove it  
players can attach a backpack to another player when not already having a backpack attached  
everyone can open the backpack and take out items that are stored  
everyone can pick up a backpack and move it around  

# Make sure UdonSharp compiler is enabled for your project!  

## How-To-Use  
>example scene with basic items and backpacks

To make it work we will need to add 2 layers, and change what other layers they will interact with  
### Add Layers to Project / change Project Settings for layers  
go to any GameObject in the top right then change layer then "Add Layers" and create "BackPack" and "BackPackHolder"  
1. "BackPack" - go into the "BackPack" prefab, change layer to "BackPack" (no to children), go to the Lid inside the Backpack prefab, change layer to "BackPack"  
2. "BackPackHolder" - go into BackpackHolder prefab, change layer  


>at the top go to "Edit" then go to Project Settings and then Physics  
Set the layers up as in this image, marked in red  
![project settings](https://i.imgur.com/i0r7H3G.png)

Drag "WorldManager" prefab into your world  
Check "IsSpeedAdjustedByWeight" if you want players to get slowed down by weight  

Drag in as many Backpack prefabs as you want in your world  

Give items you want to be able to interact with the backpack the "BackPackItem" Script  

Change the weight/customName/Sprite to whatever you want  

