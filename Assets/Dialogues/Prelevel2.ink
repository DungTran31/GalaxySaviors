INCLUDE globals.ink
{ player_nickname == "":
    ~ player_nickname = "The Savior"
}
->before_red_planet
=== before_red_planet ===
The stars blur past your ship as you make your way to the final destination – a planet shrouded in mystery and darkness, known only as **Eclipse**. The journey has been long, and the weight of your previous battles hangs heavy on your shoulders. But you know this is the endgame; all paths have led you here. #speaker:Narrator #layout:left #audio:1_3_raw

Your navigator's voice breaks the silence. "We're approaching Eclipse, <b><color=\#FFDF00>{player_nickname}</color></b>. The sensors are picking up strange energy signatures. This place… it’s unlike anything we've seen before." 
+ [Steady the course]
    "Stay the course," you reply, your voice firm. "We’ve come too far to turn back now." 
    -> approaching_eclipse
+ [Scan the planet]
    "Run a full scan," you command. "We need to know exactly what we’re dealing with."
    -> scan_eclipse
+ [Prepare the crew]
    "Get the crew ready," you order. "We can't afford any mistakes. This is our last shot."
    -> prepare_crew

=== approaching_eclipse ===
The ship glides silently through the void, drawing ever closer to the ominous planet. You can feel the tension in the air, the anticipation of the final battle.

-> enter_last_planet

=== scan_eclipse ===
The ship’s scanners hum as they analyze the planet. The results come in quickly, but they are confusing – the readings are erratic, almost as if the planet itself is alive, pulsing with an unknown force.

"We're flying blind," your navigator warns. "But whatever's down there, it's powerful."

-> enter_last_planet

=== prepare_crew ===
You walk through the ship, meeting the eyes of your crew. They’ve been with you through thick and thin, and now, they prepare for the ultimate confrontation. #speaker:Bro #portrait:ghost4 #layout:left #audio:low

"We’ll be ready, Captain," they assure you, their faces steeled with determination.

-> enter_last_planet

=== enter_last_planet ===
The planet looms large before you now, a swirling mass of shadow and light. As you prepare to land, you take a deep breath, steeling yourself for the challenges that await. #speaker:Narrator #layout:left #audio:1_3_raw

"Bring us down," you command. #speaker:Player #portrait:ghost #layout:right #audio:high

The ship descends, breaking through the thick atmosphere of Eclipse. The landscape below is a desolate wasteland, but at the center of it all, a massive fortress rises from the ground, glowing with an eerie light. This is where the final battle will take place. #speaker:Narrator #layout:left #audio:1_3_raw
-> END
