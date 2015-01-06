/*
Asteroids Design

Game start
	4 large asteroids
	Player in the middle of screen
	3 player lives
	Player score (player 1 is top left)
	high score (centered top)

Levels
	first level is 4 asteroids
	every level cleared adds one extra asteroid, up to a limit TBD.	

Asteroids
	Random movement in one direction
	wraps around screen edges

	3 sizes
		large (20 points)
		medium (50 points)
		small (100 points)			
	
	Asteroids, when hit, splits into two of the next smaller size.

UFO's
	two sizes
		large (200 points)
		small (1000 points)
	
	large appears first after some predetermined time
	after a set number of large, smalls start appearing
	
	have random movement, not in one direction.
	When on screen large fire randomly, smalls target the player
	
	if UFO shoots asteroid, no points awarded, asteroid destroyed
	
	travels the screen from one edge to the other, will wrap top to bottom.
	
	randomly appear on either the right or left side.

Player
	rotation on the left and right keys
	thrust on forward(up)
		momentum, decreases to nothing slowly after thrust is released
	fire on space
	
	hyperspace on left and right shift
		when pressed, player moves to random location.
		removes all momentum.
	
	wraps around screen edges
	
	when player bullet hits asteroid
		destroys asteroid, award points
	when player collides with asteroid
		destroys player and asteroid
		awards points from asteroid
		player loses 1 life
	
	bullets have a set life and speed
				
	On player death
		respawns in middle of screen.
		waits until radius is clear before spawning
		
	3 lives to start
	extra life awarded at 10,000	
*/