INCLUDE globals.ink
{ player_nickname == "":
    ~ player_nickname = "The Savior"
}
->boss_defeat_scene2
=== boss_defeat_scene2 ===
The final blow lands, and the dark entity that has plagued the galaxy for so long collapses before you. The fortress trembles as the dark energy that once fueled it begins to dissipate. You stand tall, <b><color=\#FFDF00>{player_nickname}</color></b>, the weight of your victory settling in. #speaker:Narrator #layout:left #audio:1_3_raw
The ground beneath you begins to crumble as the fortress starts to collapse. You rush to your ship, the sounds of destruction echoing all around. #speaker:Narrator #layout:left #audio:1_3_raw
+ [Make a quick escape.]
    You run to the ship, your heart pounding as the structure around you starts to fall apart. You barely make it inside before the entire fortress collapses into a heap of rubble. #speaker:Narrator #layout:left #audio:1_3_raw
    -> escape_eclipse
+ [Take a moment to reflect.]
    You pause for a brief moment, taking in the sight of your victory. The galaxy is safe, and the shadow that threatened it is no more. But there's no time to linger – you make your way to the ship, ready to leave this dark place behind. #speaker:Narrator #layout:left #audio:1_3_raw
    -> escape_eclipse
+ [Search for any survivors.]
    Despite the urgency, you take a moment to search for any potential survivors. But all you find is the empty shell of a once-mighty fortress. With no time to lose, you rush back to the ship. #speaker:Narrator #layout:left #audio:1_3_raw
    -> escape_eclipse
=== escape_eclipse ===
You strap in as the ship lifts off, leaving the collapsing fortress behind. The planet itself seems to be unraveling, the dark energy that held it together now gone. #speaker:Narrator #layout:left #audio:1_3_raw
As you break through the atmosphere and into the safety of space, your crew cheers. The nightmare is finally over. You’ve done what many thought was impossible – you’ve brought peace to the galaxy. #speaker:Narrator #layout:left #audio:1_3_raw
But even as the celebrations begin, you can’t help but feel a twinge of unease. The galaxy is vast, and who knows what other threats lie hidden in its depths? #speaker:Narrator #layout:left #audio:1_3_raw
Still, for now, you allow yourself to relax, knowing that today, you are the hero the galaxy needed. #speaker:Narrator #layout:left #audio:1_3_raw
-> END
