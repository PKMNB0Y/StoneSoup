using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class mll507_room : Room {

	public float emptySpaceChance = 0.2f;
	public float blockExitChance = 0.4f;
	public float borderWallProbability = 0.8f;
	public float restockChance = 0.3f;
	public float flipperChance = 0.1f;
	public float enemyChance = 0.25f;

	[HideInInspector] public List<mll507_sand> sands = new List<mll507_sand>();

	public override void fillRoom(LevelGenerator ourGenerator, params Dir[] requiredExits)
	{

		float comp = Random.value;
		if (comp <= restockChance)
		{
			generateRestock(ourGenerator, requiredExits);
		} 
		else
		{
			// for rooms that go straight across
			if (containsDir(requiredExits, Dir.Left) && containsDir(requiredExits, Dir.Right))
			{
				generateHorizRoom(ourGenerator, requiredExits);
			} else if (containsDir(requiredExits, Dir.Down) && containsDir(requiredExits, Dir.Up))
			{
				generateVertRoom(ourGenerator, requiredExits);
			}
			else
			{
				generateRandomRoom(ourGenerator, requiredExits);
			}

		}
				
			
	}

	
	// generate all of the walls. have to use this for each one
	protected void generateWalls(LevelGenerator ourGenerator, Dir[] requiredExits) {
		// Basically we go over the border and determining where to spawn walls.
		bool[,] wallMap = new bool[LevelGenerator.ROOM_WIDTH, LevelGenerator.ROOM_HEIGHT];
		for (int x = 0; x < LevelGenerator.ROOM_WIDTH; x++) {
			for (int y = 0; y < LevelGenerator.ROOM_HEIGHT; y++) {
				if (x == 0 || x == LevelGenerator.ROOM_WIDTH-1
				    || y == 0 || y == LevelGenerator.ROOM_HEIGHT-1) {
					
					if (x == LevelGenerator.ROOM_WIDTH/2 
					    && y == LevelGenerator.ROOM_HEIGHT-1
					    && containsDir(requiredExits, Dir.Up)) {
						wallMap[x, y] = false;
					}
					else if (x == LevelGenerator.ROOM_WIDTH-1
					         && y == LevelGenerator.ROOM_HEIGHT/2
					         && containsDir(requiredExits, Dir.Right)) {
						wallMap[x, y] = false;
					}
					else if (x == LevelGenerator.ROOM_WIDTH/2
					         && y == 0
					         && containsDir(requiredExits, Dir.Down)) {
						wallMap[x, y] = false;
					}
					else if (x == 0 
					         && y == LevelGenerator.ROOM_HEIGHT/2 
					         && containsDir(requiredExits, Dir.Left)) {
						wallMap[x, y] = false;
					}
					else {
						wallMap[x, y] = Random.value <= borderWallProbability;
					}
					continue;
				}
				wallMap[x, y] = false;
			}
		}

		// Now actually spawn all the walls.
		for (int x = 0; x < LevelGenerator.ROOM_WIDTH; x++) {
			for (int y = 0; y < LevelGenerator.ROOM_HEIGHT; y++) {
				if (wallMap[x, y]) {
					Tile.spawnTile(ourGenerator.normalWallPrefab, transform, x, y);
				}
			}
		}
	}

	protected void generateRestock(LevelGenerator ourGenerator, Dir[] requiredExits)
	{
		generateWalls(ourGenerator, requiredExits);

		bool shovel_spawned = false;
		bool one_gren_spawned = false;
		int max_gren = Random.Range(2, 4);
		int num_gren = 0;
		
		for (int x = 2; x < LevelGenerator.ROOM_WIDTH-2; x++)
		{
			for (int y = 2; y < LevelGenerator.ROOM_HEIGHT-2; y++)
			{
				if (!shovel_spawned && Random.value <= 0.4f)
				{
					shovel_spawned = true;
					Tile.spawnTile(localTilePrefabs[4], transform, x, y);
				} else if (num_gren < max_gren && Random.value <= 0.425f)
				{
					num_gren++;
					Tile.spawnTile(localTilePrefabs[2], transform, x, y);
					one_gren_spawned = true;
				}
			}
		}

		if (!shovel_spawned)
		{
			Tile.spawnTile(localTilePrefabs[4], transform, Random.Range(2, LevelGenerator.ROOM_WIDTH - 2),
				Random.Range(2, LevelGenerator.ROOM_HEIGHT - 2));
		}

		if (!one_gren_spawned)
		{
			Tile.spawnTile(localTilePrefabs[2], transform, Random.Range(2, LevelGenerator.ROOM_WIDTH - 2),
				Random.Range(2, LevelGenerator.ROOM_HEIGHT - 2));
		}
	}

	protected void generateFlipper(LevelGenerator ourGenerator, Dir[] requiredExits)
	{
		generateWalls(ourGenerator, requiredExits);
		Tile.spawnTile(localTilePrefabs[3], transform, Random.Range(2, LevelGenerator.ROOM_WIDTH - 2),
			Random.Range(2, LevelGenerator.ROOM_HEIGHT - 2));
	}

	protected void generateHorizRoom(LevelGenerator ourGenerator, Dir[] requiredExits)
	{
		generateWalls(ourGenerator, requiredExits);
		
		int first_col_idx = Random.Range(3, LevelGenerator.ROOM_WIDTH - 3);
		int second_col_idx = 0;
		bool two_cols = false;
		if (first_col_idx == 3)
		{
			two_cols = true;
			second_col_idx = LevelGenerator.ROOM_WIDTH - 3;
		} else if (first_col_idx == LevelGenerator.ROOM_WIDTH - 3)
		{
			two_cols = true;
			second_col_idx = 3;
		}

		for (int y = 1; y < LevelGenerator.ROOM_HEIGHT - 1; y++)
		{
			float compf = Random.value;
			if (compf > 0.066f && compf < 0.5f)
			{
				Tile.spawnTile(ourGenerator.normalWallPrefab, transform, first_col_idx, y);
			}
			else
			{
				Tile.spawnTile(localTilePrefabs[6], transform, first_col_idx, y);
			}
		}

		
		if (Random.value <= 0.3f)
		{
			int xidx = Random.Range(1, LevelGenerator.ROOM_WIDTH - 1);
			if (xidx == first_col_idx || xidx == second_col_idx) {
				xidx++;
			}
			
			Tile.spawnTile(localTilePrefabs[5], transform, xidx, Random.Range(1, LevelGenerator.ROOM_WIDTH - 1));
		}
		
		if (two_cols)
		{
			for (int y = 1; y < LevelGenerator.ROOM_HEIGHT - 1; y++)
			{
				float compf = Random.value;
				if (compf > 0.066f && compf < 0.5f)
				{
					Tile.spawnTile(ourGenerator.normalWallPrefab, transform, second_col_idx, y);
				}
				else
				{
					Tile.spawnTile(localTilePrefabs[6], transform, second_col_idx, y);
				}
			}
		}


		bool right_hole = false;
		if (!two_cols)
		{
			Tile.spawnTile(localTilePrefabs[0], transform, Random.Range(1, first_col_idx),
				Random.Range(1, LevelGenerator.ROOM_HEIGHT - 1));

			if (Random.value <= 0.4f)
			{
				Tile.spawnTile(localTilePrefabs[0], transform, Random.Range(first_col_idx + 1, LevelGenerator.ROOM_WIDTH - 1),
					Random.Range(1, LevelGenerator.ROOM_HEIGHT - 1));
				right_hole = true;
			}

			if (!right_hole)
			{
				Tile.spawnTile(localTilePrefabs[1], transform, Random.Range(first_col_idx + 1, LevelGenerator.ROOM_WIDTH - 1),
					Random.Range(1, LevelGenerator.ROOM_HEIGHT - 1));
			}

		}
		else
		{
			Tile.spawnTile(localTilePrefabs[0], transform, Random.Range(1, first_col_idx),
				Random.Range(1, LevelGenerator.ROOM_HEIGHT - 1));

			if (Random.value <= 0.4f)
			{
				Tile.spawnTile(localTilePrefabs[0], transform, Random.Range(second_col_idx + 1, LevelGenerator.ROOM_WIDTH - 1),
					Random.Range(1, LevelGenerator.ROOM_HEIGHT - 1));
				right_hole = true;
			}

			if (!right_hole)
			{
				Tile.spawnTile(localTilePrefabs[1], transform, Random.Range(second_col_idx + 1, LevelGenerator.ROOM_WIDTH - 1),
					Random.Range(1, LevelGenerator.ROOM_HEIGHT - 1));
			}
		}

		if (Random.value <= 0.7f)
		{
			Tile.spawnTile(localTilePrefabs[2], transform, Random.Range(1, first_col_idx),
				Random.Range(1, LevelGenerator.ROOM_HEIGHT - 1));
		}

		if (Random.value <= 0.7f)
		{
			Tile.spawnTile(localTilePrefabs[2], transform, Random.Range(first_col_idx+1, LevelGenerator.ROOM_WIDTH-1),
				Random.Range(1, LevelGenerator.ROOM_HEIGHT - 1));
		}

		if (Random.value <= enemyChance)
		{
			int enemy_x = Random.Range(1, LevelGenerator.ROOM_WIDTH - 1);
			if (enemy_x == first_col_idx || enemy_x == second_col_idx)
			{
				enemy_x++;
			}
			Tile.spawnTile(localTilePrefabs[5], transform, enemy_x, Random.Range(1, LevelGenerator.ROOM_HEIGHT - 1));
		}
		
		if (Random.value <= enemyChance)
		{
			int enemy_x = Random.Range(1, LevelGenerator.ROOM_WIDTH - 1);
			if (enemy_x == first_col_idx || enemy_x == second_col_idx)
			{
				enemy_x++;
			}
			Tile.spawnTile(localTilePrefabs[3], transform, enemy_x, Random.Range(1, LevelGenerator.ROOM_HEIGHT - 1));
		}
	}

	protected void generateVertRoom(LevelGenerator ourGenerator, Dir[] requiredExits)
	{
		generateWalls(ourGenerator, requiredExits);

		
		int first_row_idx = Random.Range(3, LevelGenerator.ROOM_WIDTH - 3);
		int second_row_idx = 0;
		bool two_rows = false;
		if (first_row_idx == 3)
		{
			two_rows = true;
			second_row_idx = LevelGenerator.ROOM_WIDTH - 3;
		} else if (first_row_idx == LevelGenerator.ROOM_WIDTH - 3)
		{
			two_rows = true;
			second_row_idx = 3;
		}

		for (int x = 1; x < LevelGenerator.ROOM_HEIGHT - 1; x++)
		{
			float compf = Random.value;
			if (compf > 0.066f && compf < 0.5f)
			{
				Tile.spawnTile(ourGenerator.normalWallPrefab, transform, x, first_row_idx);
			}
			else
			{
				Tile.spawnTile(localTilePrefabs[6], transform, x, first_row_idx);
			}
		}

		if (two_rows)
		{
			for (int x = 1; x < LevelGenerator.ROOM_HEIGHT - 1; x++)
			{
				float compf = Random.value;
				if (compf > 0.066f && compf < 0.5f)
				{
					Tile.spawnTile(ourGenerator.normalWallPrefab, transform, x, second_row_idx);
				}
				else
				{
					Tile.spawnTile(localTilePrefabs[6], transform, x, second_row_idx);
				}
			}
		}


		if (Random.value <= 0.3f)
		{
			int y_idx = Random.Range(1, LevelGenerator.ROOM_HEIGHT - 1);
			if (y_idx == first_row_idx || y_idx == second_row_idx)
			{
				y_idx++;
			}
			
			Tile.spawnTile(localTilePrefabs[5], transform, Random.Range(1, LevelGenerator.ROOM_WIDTH - 1),
				y_idx);
		}
		
		bool right_hole = false;
		if (!two_rows)
		{
			Tile.spawnTile(localTilePrefabs[0], transform,
				Random.Range(1, LevelGenerator.ROOM_WIDTH - 1), Random.Range(1, first_row_idx));

			if (Random.value <= 0.4f)
			{
				Tile.spawnTile(localTilePrefabs[0], transform,
					Random.Range(1, LevelGenerator.ROOM_WIDTH - 1), Random.Range(first_row_idx + 1, LevelGenerator.ROOM_HEIGHT - 1));
				right_hole = true;
			}

			if (!right_hole)
			{
				Tile.spawnTile(localTilePrefabs[1], transform, 
					Random.Range(1, LevelGenerator.ROOM_WIDTH - 1), Random.Range(first_row_idx + 1, LevelGenerator.ROOM_HEIGHT - 1));
			}

		}
		else
		{
			Tile.spawnTile(localTilePrefabs[0], transform, 
				Random.Range(1, LevelGenerator.ROOM_WIDTH - 1), Random.Range(1, first_row_idx));

			if (Random.value <= 0.4f)
			{
				Tile.spawnTile(localTilePrefabs[0], transform, 
					Random.Range(1, LevelGenerator.ROOM_WIDTH - 1), Random.Range(first_row_idx+1, LevelGenerator.ROOM_HEIGHT-1));
				right_hole = true;
			}

			if (!right_hole)
			{
				Tile.spawnTile(localTilePrefabs[1], transform, 
					Random.Range(1, LevelGenerator.ROOM_WIDTH - 1), Random.Range(first_row_idx+1, LevelGenerator.ROOM_HEIGHT-1));
			}
		}

		if (Random.value <= 0.7f)
		{
			Tile.spawnTile(localTilePrefabs[2], transform, 
				Random.Range(1, LevelGenerator.ROOM_WIDTH - 1), Random.Range(1, first_row_idx));
		}

		if (Random.value <= 0.7f)
		{
			Tile.spawnTile(localTilePrefabs[2], transform,
				Random.Range(1, LevelGenerator.ROOM_WIDTH - 1), Random.Range(first_row_idx+1, LevelGenerator.ROOM_HEIGHT-1));
		}

		if (Random.value <= enemyChance)
		{
			int enemy_y = Random.Range(1, LevelGenerator.ROOM_HEIGHT- 1);
			if (enemy_y == first_row_idx || enemy_y == second_row_idx)
			{
				enemy_y++;
			}
			Tile.spawnTile(localTilePrefabs[5], transform, Random.Range(1, LevelGenerator.ROOM_WIDTH - 1), enemy_y);
		}
		
		if (Random.value <= enemyChance)
		{
			int enemy_y = Random.Range(1, LevelGenerator.ROOM_HEIGHT - 1);
			if (enemy_y == first_row_idx || enemy_y == second_row_idx)
			{
				enemy_y++;
			}
			Tile.spawnTile(localTilePrefabs[3], transform, Random.Range(1, LevelGenerator.ROOM_WIDTH - 1), enemy_y);
		}
	}

	protected void generateRandomRoom(LevelGenerator ourGenerator, Dir[] requiredExits)
	{
		if (Random.value <= 0.2f)
		{
			generateFlipper(ourGenerator, requiredExits);
			return;
		}
		
		generateWalls(ourGenerator, requiredExits);

		if (Random.value <= 0.4f)
		{
			Tile.spawnTile(localTilePrefabs[5], transform, Random.Range(1, LevelGenerator.ROOM_WIDTH - 1),
				Random.Range(1, LevelGenerator.ROOM_HEIGHT - 1));
		}

		if (Random.value <= 0.5f)
		{
			Tile.spawnTile(localTilePrefabs[7], transform, Random.Range(1, LevelGenerator.ROOM_WIDTH - 1),
				Random.Range(1, LevelGenerator.ROOM_HEIGHT - 1));
		}
		
		if (Random.value <= 0.3f)
		{
			Tile.spawnTile(localTilePrefabs[9], transform, Random.Range(1, LevelGenerator.ROOM_WIDTH - 1),
				Random.Range(1, LevelGenerator.ROOM_HEIGHT - 1));
		}
		
		if (Random.value <= 0.3f)
		{
			Tile.spawnTile(localTilePrefabs[8], transform, Random.Range(1, LevelGenerator.ROOM_WIDTH - 1),
				Random.Range(1, LevelGenerator.ROOM_HEIGHT - 1));
		}
	}
	
	protected bool containsDir (Dir[] dirArray, Dir dirToCheck) {
		foreach (Dir dir in dirArray) {
			if (dirToCheck == dir) {
				return true;
			}
		}
		return false;
	}
}
