#Backpack system for vrchat sdk3.0 worlds  

Make sure UdonSharp compiler is enabled for your project!  

#How-To-Use  
>example scene if needed

Add Layers to Project  
"BackPackLid" - go into the Backpack prefab to the Lid, change layer  
"BackPack" - go into the Backpack prefab, change layer (no children)  
"BackPackItem" - go into RandomObject prefabs, change layer (no children)  
"BackPackHolder" - go into BackpackHolder prefab, change layer  
Set the layers up as in this image  
![project settings](https://i.imgur.com/vDjGii1.png)

Drag "WorldManager" prefab into your world  
Check "IsSpeedAdjustedByWeight" if you want players to get slowed down by weight  

Drag in as many Backpack prefabs as you want in your world  

Give items you want to be able to interact with the backpack the "BackPackItem" Script  
Give item the "BackPackItem" layer

Change the weight/customName/Sprite to whatever you want  

