Overview

This is a Unity-based match-3 puzzle game where players click on groups of matching gems to remove them from the board. The game features gravity-based falling mechanics, obstacle blocks (Box Obstacles), and auto-shuffling when no matches are available.

-Project Structure-

BoardManager.cs -> Manages the game board, gem spawning, updates, and shuffling.

CollapseManager.cs -> Handles player clicks, gem removal, and board updates.

MatchFinder.cs -> Detects gem matches and applies damage to Box Obstacles.

Gem.cs -> Defines gem properties and destruction animations.

BoxObstacle.cs -> Special block that does not fall and requires two adjacent pops to be destroyed.

GemPool.cs -> Manages object pooling to optimize gem instantiations.

GemColorManager.cs -> Stores different gem color data.

LevelData.cs -> Stores level settings like board size, gem colors, and probabilities.

Gameplay Mechanics

-Matching & Destroying Gems-

Clicking on a group of 2 or more connected gems of the same color removes them.

Destroyed gems make space for the ones above to fall.

-Box Obstacles-

Box Obstacles block falling gems and require two adjacent gem removals to be destroyed.

When an adjacent group is destroyed, the Box Obstacle takes 1 damage.

On first damage, a cracked sprite is revealed.

On second damage, the box is destroyed and removed from the board.

-Auto-Shuffling When No Matches Exist-

If no valid matches exist, the entire board is shuffled (except for Box Obstacles).

-Code Details-

-BoardManager.cs - Board Initialization & Updates-

Handles:

Initializing the board with gems and box obstacles.

Spawning gems dynamically when gaps form.

Falling mechanics where gems move down if there is space.

Shuffling gems if no moves are available.

Key Methods:

SetupBoard() -> Initializes the board with gems and obstacles.

UpdateBoard() -> Updates board state after gems are removed.

ShuffleBoard() -> Reshuffles gems when no moves exist.

ClearGemFromBoard(Gem gem) -> Properly removes a gem from the board.

-CollapseManager.cs - Handling Player Clicks-

Handles:

Detecting player clicks and triggering gem destruction.

Removing gems from the board properly.

Calling UpdateBoard() to update falling mechanics.

Resetting Box Obstacle damage flags at the end of a turn.

Key Methods:

HandleGemClick(Gem clickedGem) -> Destroys selected gems and updates the board.

RemoveGemFromBoard(Gem gem) -> Clears gems and obstacles properly.

-MatchFinder.cs - Finding Matches & Damaging Boxes-

Handles:

Detecting connected gems of the same color.

Applying damage to adjacent Box Obstacles.

Key Methods:

FindMatchesFromGem(Gem startGem) -> Finds connected matching gems.

DamageAdjacentBoxes(Vector2Int position) -> Damages adjacent Box Obstacles.

-BoxObstacle.cs - Handling Obstacle Behavior-

Handles:

Preventing gems from falling through it.

Taking damage when adjacent gems are removed.

Destroying itself when health reaches 0.

Key Methods:

TakeDamage() -> Applies 1 damage to the box.