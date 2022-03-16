using Godot;
using Godot.Collections;

namespace buildLayer
{
    public class BuildLayerManager : TileMap
    {
        [Export]
        Vector2 MainBaseSpawnLocation = Vector2.Zero;

        public static Dictionary<Vector2, BaseBuildTile> PlacedTiles = new Dictionary<Vector2, BaseBuildTile>();
        public SceneNode SceneNode;
        public override void _Ready()
        {
            SceneNode = (SceneNode)GetNode("/root/SceneNode"); 
            SceneNode.Connect("LoadIsComplete", this, nameof(SpawnMainStructures));
        }

        //Handles Spawning 1x1 small tiles
        public void SpawnTile(Vector2 location, Rotations currentRotation, int tile, BaseBuildable itemSelected)
        {

            if (PlacedTiles.ContainsKey(location))
            {
                GD.Print("tile already exist in location");
                return;
            }
            switch (currentRotation)
            {
                case Rotations.Up:
                    SetCellv(location, tile);
                    SpawnTileNode(itemSelected, location, currentRotation);
                    break;
                case Rotations.Right:
                    SetCellv(location, tile, true, false, true);
                    SpawnTileNode(itemSelected, location, currentRotation);
                    break;
                case Rotations.Down:
                    SetCellv(location, tile, true, true);
                    SpawnTileNode(itemSelected, location, currentRotation);
                    break;
                case Rotations.Left:
                    SetCellv(location, tile, false, true, true);
                    SpawnTileNode(itemSelected, location, currentRotation);
                    break;
            }
        }

        //For handling multi tile objects
        public void SpawnTile(Vector2 location, Rotations currentRotation, int tile, BaseBuildable itemSelected, int width, int height)
        {
            if (PlacedTiles.ContainsKey(location))
            {
                GD.Print("tile already exist in location");
                return;
            }
            switch (currentRotation)
            {
                case Rotations.Up:
                    Vector2 locationOffset = new Vector2(location.x, location.y - height + 1);
                    SetCellv(locationOffset, tile);
                    SpawnTileNode(itemSelected, location, currentRotation, width, height, 1, 1);
                    break;
                case Rotations.Right:
                    SetCellv(location, tile, true, false, true);
                    SpawnTileNode(itemSelected, location, currentRotation, height, width, 1, 1);
                    break;
                case Rotations.Down:
                    SetCellv(location, tile, true, true);
                    SpawnTileNode(itemSelected, location, currentRotation, width, height, -1, 1);
                    break;
                case Rotations.Left:
                    SetCellv(new Vector2(location.x - width, location.y), tile, true, true, true);
                    SpawnTileNode(itemSelected, location, currentRotation, height, width, -1, -1);
                    break;
            }
        }

        //Buildable Tile Nodes are stored as packed scenes which are then instanced when a tile is placed
        public void SpawnTileNode(BaseBuildable itemSelected, Vector2 location, Rotations rotation)
        {
            BaseBuildTile spawnedTileNode = (BaseBuildTile)itemSelected.TileNode.Instance();
            spawnedTileNode.Transform = new Transform2D(0, MapToWorld(location));
            spawnedTileNode.TileName = itemSelected.TileName;
            spawnedTileNode.TileLocation = location;
            spawnedTileNode.TileRotation = rotation;
            spawnedTileNode.Tier = itemSelected.Tier;
            spawnedTileNode.AssociatedItem = itemSelected;
            PlacedTiles.Add(location, spawnedTileNode);
            AddChild(spawnedTileNode);
        }

        public void SpawnTileNode(BaseBuildable itemSelected, Vector2 location, Rotations rotation, int width, int height, int tileOffSetY, int tileOffSetX)
        {
            BaseMachineMultiTile spawnedTileNode = (BaseMachineMultiTile)itemSelected.TileNode.Instance();
            spawnedTileNode.Transform = new Transform2D(0, MapToWorld(location));
            spawnedTileNode.TileName = itemSelected.TileName;
            spawnedTileNode.TileRotation = rotation;
            spawnedTileNode.Tier = itemSelected.Tier;
            spawnedTileNode.AssociatedItem = itemSelected;
            for (int currentHeightRow = 0; currentHeightRow < height; currentHeightRow++)
            {
                for (int currentWidthRow = 0; currentWidthRow < width; currentWidthRow++)
                {
                    Vector2 subTileLocation = new Vector2(location.x + (currentWidthRow * tileOffSetX), location.y + (currentHeightRow * (-tileOffSetY)));
                    spawnedTileNode.SubTileLocations.Add(subTileLocation);

                    PlacedTiles.Add(subTileLocation, spawnedTileNode);
                }
            }
            AddChild(spawnedTileNode);
        }

        public void RemoveTile(Vector2 location)
        {
            if (!PlacedTiles.ContainsKey(location))
            {
                GD.Print("tile does not exist in location");
                return;
            }
            BaseBuildTile tileNode = PlacedTiles[location];
            if (!tileNode.AssociatedItem.IsDestroyable) return;
            if (tileNode.AssociatedItem.Width > 1 || tileNode.AssociatedItem.Height > 1)
            {
                BaseMachineMultiTile multiTileNode = (BaseMachineMultiTile)tileNode;
                GD.Print("Deleting Large tile");
                multiTileNode.SubTileLocations.ForEach(subTileLocation =>
                {
                    PlacedTiles.Remove(subTileLocation);
                    SetCellv(subTileLocation, -1);
                });
            }
            else
            {
                GD.Print("Deleting Small tile");
                PlacedTiles.Remove(location);
                SetCellv(location, -1);
            }

            ReturnPlayerResources(tileNode);
            tileNode.Destroy();
        }

        private void ReturnPlayerResources(BaseBuildTile TileNode)
        {
            Dictionary TileRecipes = SceneNode.ProcessingRecipes["Player"];
            Dictionary TileRecipe = TileRecipes[TileNode.TileName.ToString()] as Dictionary;
            Dictionary Recipe = TileRecipe[TileNode.Tier.ToString()] as Dictionary;
            SceneNode.MainBase.ReturnResourceFromPlayerDestroy(Recipe);
        }

        public void UpdateTile(Vector2 tileLocation, int tileTextureIndex, Rotations currentRotation)
        {
            switch (currentRotation)
            {
                case Rotations.Up:
                    SetCellv(tileLocation, tileTextureIndex);
                    break;
                case Rotations.Right:
                    SetCellv(tileLocation, tileTextureIndex, true, false, true);
                    break;
                case Rotations.Down:
                    SetCellv(tileLocation, tileTextureIndex, true, true);
                    break;
                case Rotations.Left:
                    SetCellv(tileLocation, tileTextureIndex, false, true, true);
                    break;
            }
        }

        private void SpawnMainStructures()
        {
            MainCore MainBaseItem = (MainCore)SceneNode.BuildTiles.Find(tile => tile.TileName == TileTypes.MainBase);
            SpawnTile(MainBaseSpawnLocation, Rotations.Up, MainBaseItem.ItemID, MainBaseItem, MainBaseItem.Width, MainBaseItem.Height);

        }

    }

    public enum Rotations
    {
        Up,
        Down,
        Left,
        Right,
    }
}

