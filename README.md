# AI_Game
Tristan and Joshua :D




gamemanager
	-calls level gen
	-places character in rooms

weapon gen:
-genWeaponStats(lvl, weaponType)

level gen: -rectangle shape
	-heuristics system
		-enum roomtype
		-allocatePoints()
			-structures (room itself, aka traps)
			-enemies
			
	-genRoom(int lvl)
	-genCreatures(int lvl, roomType(enum) roomType)
		-num of unique creatures
		-creature gen
		

creature gen:
-genCreatureStats(int lvl, bool isBoss false)
	-creatures have weapons, call weapongen
	-boss enemies drop special weapons
-spawnCreature(creatureStats)


room handler/gamemanger:
-scriptable object holding list of all enemies in the current room
	-base enemy has a ref to the scriptable object
		-on awake, add themself to the list
		-ondestroy, removes themself from the list

movement ai:
-not astar, do boids/flee stuff


thoughts:
-how will the combat system + my enemies interact
	-by unity tag
	-GetComponent<EntityBase>()
-traps enemy ai avoiding
