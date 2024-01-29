using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace WaveFunctionCollapseTest
{
    class Map
    {
        public const int width = 160;
        public const int height = 160;

        Tile[,] tiles = new Tile[width,height];
        bool[,] checks = new bool[width,height];

        List<Point> availableTiles = new List<Point>();


        const int chunkSize = 16;

        const int chunksWidth = width / chunkSize;
        const int chunksHeight = height / chunkSize;

        private int chunkX = 0;
        private int chunkY = 0;

        private int east = chunksWidth;
        private int south = chunksHeight - 1;
        private int west = chunksWidth - 1;
        private int north = chunksHeight - 2;

        private int countedTilesInChunk = 1;

        public Map()
        {

        }

        public bool IsGenerationFinished()
        {
            foreach(Tile t in tiles)
            {
                if(t == null)
                {
                    return false;
                }
            }

            Cleanup();

            /*
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    tiles[i, j] = null;
                    checks[i, j] = false;
                }
            }
            */

            
            return true;
        }

        private void Cleanup()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if(tiles[i,j] is CoastalWater)
                    {
                        if(!IsAdjacentTilesSame(tiles[i,j]))
                        {
                            tiles[i, j] = new DeepWater(tiles[i, j].position, tiles[i, j].height);
                        }
                    }
                }
            }
            /*
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (tiles[i, j] is DeepWater)
                    {
                        if (!IsAdjacentTilesSame(tiles[i, j]))
                        {
                            tiles[i, j] = new CoastalWater(tiles[i, j].position, tiles[i, j].height);
                        }
                    }
                }
            }
            */
        }

        private bool IsAdjacentTilesSame(Tile tile)
        {
            int x = tile.position.X;
            int y = tile.position.Y;

            if (x <= 0)
            {
                x++;
            }
            if (y <= 0)
            {
                y++;
            }
            if (x > width - 1)
            {
                x--;
            }
            if (y > height - 1)
            {
                y--;
            }
            for (int i = -1; i < 1; i++)
            {
                for (int j = -1; i < 1; i++)
                {
                    if (tiles[x + i, y + j].GetType() == tile.GetType())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void Clear()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    tiles[i, j] = null;
                    checks[i, j] = false;
                }
            }
            for (int i = 0; i < 1; i++)
            {
                int x = Randomizer.random.Next(0, width);
                int y = Randomizer.random.Next(0, height);
                //tiles[x, y] = new Grass(new Point(width - 1, height - 1), Randomizer.random.Next(48, 96));

            }
            chunkX = 0;
            chunkY = 0;
            countedTilesInChunk = 1;
            tiles[0, 0] = new Grass(new Point(width - 1, height - 1), Randomizer.random.Next(48, 96));
            SetAvailableTiles();
            //tiles[0, height - 1] = new Grass(new Point(width - 1, height - 1), Randomizer.random.Next(48, 96));
            //tiles[width - 1, 0] = new Grass(new Point(width - 1, height - 1), Randomizer.random.Next(48, 96));
            tiles[width - 1, height - 1] = new Grass(new Point(width - 1, height - 1), Randomizer.random.Next(48, 96));
            //tiles[width / 2, height /2] = new Grass(new Point(width - 1, height - 1), Randomizer.random.Next(48, 96));
        }


        private void SetAvailableTiles()
        {
            availableTiles.Clear();
            for(int i = chunkX * chunkSize; i < (chunkX + 1) * chunkSize; i++)
            {
                for (int j = chunkY * chunkSize; j < ((chunkY + 1) * chunkSize); j++)
                {
                    if(tiles[Math.Min(i, width - 1),j] == null)
                    {
                        if(IsGeneratable(new Point(i,j)))
                        {
                            availableTiles.Add(new Point(i, j));
                        }
                        
                    }
                }
            }
        }

        private bool IsGeneratable(Point tile)
        {
            int x = tile.X;
            int y = tile.Y;

            if(x <= 0)
            {
                x++;
            }
            if(y <= 0)
            {
                y++;
            }
            if(x > width - 1)
            {
                x--;
            }
            if(y > height - 1)
            {
                y--;
            }
            for(int i = -1; i < 1; i++)
            {
                for(int j = -1; i < 1; i++)
                {
                    if(tiles[x + i, y + j] != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void RandomGenerate()
        {
            SetAvailableTiles();
            {
                Point p = availableTiles[Randomizer.random.Next(0, availableTiles.Count)];
                if(IsGeneratable(p))
                {
                    if (Randomizer.random.Next(2) == 1)
                    {
                        SubGen(p.X, p.Y);
                        if (tiles[p.X, p.Y] == null)
                        {
                            SubGenDiag(p.X, p.Y);
                        }
                    }
                    else
                    {
                        SubGenDiag(p.X, p.Y);
                        if(tiles[p.X, p.Y] == null)
                        {
                            SubGen(p.X, p.Y);
                        }
                    }
                    countedTilesInChunk++;
                }        
            }


            // OLD RANDOM GEN
            {
                /*
                int x = Randomizer.random.Next(0, width);
                int y = Randomizer.random.Next(0, height);

                if((chunkX + 1) * chunkSize > width)
                {
                    x = Randomizer.random.Next(width - chunkSize, width);
                }
                else
                {
                    x = Randomizer.random.Next(chunkSize * chunkX, chunkSize * (chunkX + 1));
                }

                if ((chunkY + 1) * chunkSize > height)
                {
                    y = Randomizer.random.Next(height - chunkSize, height);
                }
                else
                {
                    y = Randomizer.random.Next(chunkSize * chunkY, chunkSize * (chunkY + 1));
                }


                if (checks[x,y])
                {

                }
                else
                {
                    if (tiles[x, y] == null)
                    {


                        if(tiles[x,y] != null)
                        {
                            countedTilesInChunk++;
                        }
                    }
                }
                */
            }


            // SPIRAL RANDOM CHUNK GEN OUTSIDE-IN
            {
                /*
                if (countedTilesInChunk >= chunkSize * chunkSize)
                {
                    if(east >= south)
                    {
                        chunkX++;
                    }
                    else if(south >= west)
                    {
                        chunkY++;
                    }
                    else if(west >= north)
                    {
                        chunkX--;
                    }
                    else if(north >= east)
                    {
                        chunkY--;
                    }

                    if (chunkX < chunksWidth - west)
                    {
                        chunkX = chunksWidth - west;
                        west--;
                        chunkY--;
                    }

                    if (chunkY < chunksHeight - north)
                    {
                        chunkY = chunksHeight - north;

                    }

                    if(chunkX > chunksWidth - Math.Abs(east - chunksWidth))
                    {
                        chunkX = chunksWidth - Math.Abs(east - chunksWidth);
                        east -= 2;
                        chunkY++;
                    }

                    if (chunkY > chunksHeight - Math.Abs(south - chunksHeight))
                    {
                        chunkY = chunksHeight - Math.Abs(south - chunksHeight);
                        south -= 2;
                        chunkX--;
                    }
                }
                */
            }

            //ZIGZAG LEFT-RIGHT-LEFT AD INFINITUM, TOP TO BOTTOM
            {
                if (countedTilesInChunk >= chunkSize * chunkSize)
                {
                    if (chunkY % 2 == 0)
                    {
                        chunkX++;
                        if (chunkX >= width / chunkSize)
                        {
                            chunkX--;
                            chunkY++;
                        }
                    }
                    else
                    {
                        chunkX--;
                    }

                    countedTilesInChunk = 0;
                }
                if (chunkX * chunkSize >= width)
                {

                    chunkY++;
                }
                if (chunkX < 0)
                {
                    chunkX = 0;
                    chunkY++;
                }
            }
            
            
        }

        public void NewGenerate()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if(tiles[i,j] == null)
                    {
                        if(Randomizer.random.Next(2) == 1)
                        {
                             SubGen(i, j);
                        }
                        else
                        {
                            SubGenDiag(i, j);
                        }

                        //SubGenCardinal(i, j);

                        if (Randomizer.random.Next(100000) == 1)
                        {
                            //tiles[i, j] = new Grass(new Point(i, j));
                        }

                    }
                }
            }
        }

        private void SubGen(int x, int y)
        {
            Tile upTile = null;
            Tile downTile = null;
            Tile leftTile = null;
            Tile rightTile = null;
            if (y > 0)
            {
                upTile = tiles[x, y - 1];
            }
            if (y < height - 1)
            {
                downTile = tiles[x, y + 1];
            }
            if (x > 0)
            {
                leftTile = tiles[x - 1, y];
            }
            if (x < width - 1)
            {
                rightTile = tiles[x + 1, y];
            }


            tiles[x, y] = CreateTile(upTile, downTile, leftTile, rightTile, x, y);
        }

        
        private void SubGenDiag(int x, int y)
        {
            Tile upLeftTile = null;
            Tile upRightTile = null;
            Tile downLeftTile = null;
            Tile downRightTile = null;
            if (y > 0 && x > 0)
            {
                upLeftTile = tiles[x - 1, y - 1];
            }
            if (y > 0 && x < width - 1)
            {
                upRightTile = tiles[x + 1, y - 1];
            }
            if (x > 0 && y < height - 1)
            {
                downLeftTile = tiles[x - 1, y + 1];
            }
            if (x < width - 1 && y < height - 1)
            {
                downRightTile = tiles[x + 1, y + 1];
            }


            tiles[x, y] = CreateTile(upLeftTile, upRightTile, downLeftTile, downRightTile, x, y);
        }

        private void SubGenCardinal(int x, int y)
        {
            Tile upLeftTile = null;
            Tile upRightTile = null;
            Tile downLeftTile = null;
            Tile downRightTile = null;
            if (y > 0 && x > 0)
            {
                upLeftTile = tiles[x - 1, y - 1];
            }
            if (y > 0 && x < width - 1)
            {
                upRightTile = tiles[x + 1, y - 1];
            }
            if (x > 0 && y < height - 1)
            {
                downLeftTile = tiles[x - 1, y + 1];
            }
            if (x < width - 1 && y < height - 1)
            {
                downRightTile = tiles[x + 1, y + 1];
            }
            Tile upTile = null;
            Tile downTile = null;
            Tile leftTile = null;
            Tile rightTile = null;
            if (y > 0)
            {
                upTile = tiles[x, y - 1];
            }
            if (y < height - 1)
            {
                downTile = tiles[x, y + 1];
            }
            if (x > 0)
            {
                leftTile = tiles[x - 1, y];
            }
            if (x < width - 1)
            {
                rightTile = tiles[x + 1, y];
            }


            tiles[x, y] = CreateTile(upTile, downTile, leftTile, rightTile, upLeftTile, upRightTile, downLeftTile, downRightTile, x, y);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(Tile t in tiles)
            {
                if(t != null)
                {
                    t.Draw(spriteBatch);
                }
            }
        }

        private Tile CreateTile(Tile tile1, Tile tile2, Tile tile3, Tile tile4, Tile tile5, Tile tile6, Tile tile7, Tile tile8, int x, int y)
        {
            if (tile1 == null && tile2 == null && tile3 == null && tile4 == null && tile5 == null && tile6 == null && tile7 == null && tile8 == null)
            {
                return null;
            }

            Weighting weight1 = new Weighting(1, 1, 1, 1, 1, 1);
            Weighting weight2 = new Weighting(1, 1, 1, 1, 1, 1);
            Weighting weight3 = new Weighting(1, 1, 1, 1, 1, 1);
            Weighting weight4 = new Weighting(1, 1, 1, 1, 1, 1);
            Weighting weight5 = new Weighting(1, 1, 1, 1, 1, 1);
            Weighting weight6 = new Weighting(1, 1, 1, 1, 1, 1);
            Weighting weight7 = new Weighting(1, 1, 1, 1, 1, 1);
            Weighting weight8 = new Weighting(1, 1, 1, 1, 1, 1);

            List<int> heights = new List<int>();

            if (tile1 != null)
            {
                heights.Add(tile1.height);
                weight1 = tile1.weight;
            }
            if (tile2 != null)
            {
                heights.Add(tile2.height);
                weight2 = tile2.weight;
            }
            if (tile3 != null)
            {
                heights.Add(tile3.height);
                weight3 = tile3.weight;
            }
            if (tile4 != null)
            {
                heights.Add(tile4.height);
                weight4 = tile4.weight;
            }
            if (tile5 != null)
            {
                heights.Add(tile5.height);
                weight5 = tile5.weight;
            }
            if (tile6 != null)
            {
                heights.Add(tile6.height);
                weight6 = tile6.weight;
            }
            if (tile7 != null)
            {
                heights.Add(tile7.height);
                weight7 = tile7.weight;
            }
            if (tile8 != null)
            {
                heights.Add(tile8.height);
                weight8 = tile8.weight;
            }

            int avgHeight = 0;
            if(heights.Count > 0)
            {
                foreach(int i in heights)
                {
                    avgHeight += i;
                }
            }

            avgHeight = (int)(avgHeight / heights.Count);
            avgHeight += Randomizer.random.Next(-1, 2);

            if(avgHeight < 0)
            {
                avgHeight = 0;
            }

            int[] results = new int[6];
            
            

            for (int i = 0; i < 6; i++)
            {
                int rnd = Randomizer.random.Next(0, 100);
                results[i] = rnd * weight1.weight[i] * weight2.weight[i] * weight3.weight[i] * weight4.weight[i] * weight5.weight[i] * weight6.weight[i] * weight7.weight[i] * weight8.weight[i];
            }

            if(avgHeight != 64)
            {
                results[4] *= avgHeight - 64;
                results[5] *= avgHeight - 64;
            }
            

            int bestTile = 0;
            int givenTile = 0;
            for (int i = 0; i < 6; i++)
            {
                if (results[i] > bestTile)
                {
                    bestTile = results[i];
                    givenTile = i;
                }
            }
            Point pos = new Point(x, y);
            return givenTile switch
            {
                0 => new Grass(pos, avgHeight),
                1 => new Forest(pos, avgHeight),
                2 => new Woods(pos, avgHeight),
                3 => new Sand(pos, avgHeight),
                4 => new CoastalWater(pos, avgHeight),
                5 => new DeepWater(pos, avgHeight),
                _ => null,
            };
        }

        private Tile CreateTile(Tile tile1, Tile tile2, Tile tile3, Tile tile4, int x, int y)
        {
            if(tile1 == null && tile2 == null && tile3 == null && tile4 == null)
            {
                return null;
            }

            Weighting weight1 = new Weighting(1, 1, 1, 1, 1, 1);
            Weighting weight2 = new Weighting(1, 1, 1, 1, 1, 1);
            Weighting weight3 = new Weighting(1, 1, 1, 1, 1, 1);
            Weighting weight4 = new Weighting(1, 1, 1, 1, 1, 1);

            List<int> heights = new List<int>();
            if (tile1 != null)
            {
                heights.Add(tile1.height);
                weight1 = tile1.weight;
            }
            if (tile2 != null)
            {
                heights.Add(tile2.height);
                weight2 = tile2.weight;
            }
            if (tile3 != null)
            {
                heights.Add(tile3.height);
                weight3 = tile3.weight;
            }
            if (tile4 != null)
            {
                heights.Add(tile4.height);
                weight4 = tile4.weight;
            }

            int avgHeight = 0;
            if (heights.Count > 0)
            {
                foreach (int i in heights)
                {
                    avgHeight += i;
                }
            }

            avgHeight = (int)(avgHeight / heights.Count);
            avgHeight += Randomizer.random.Next(-2, 3);

            int[] results = new int[6];

            for (int i = 0; i < 6; i++)
            {
                int rnd = Randomizer.random.Next(0, 100);
                results[i] = rnd * weight1.weight[i] * weight2.weight[i] * weight3.weight[i] * weight4.weight[i];
            }

            int bestTile = 0;
            int givenTile = 0;
            for (int i = 0; i < 6; i++)
            {
                if (results[i] > bestTile)
                {
                    bestTile = results[i];
                    givenTile = i;
                }
            }

            Point pos = new Point(x, y);
            return givenTile switch
            {
                0 => new Grass(pos, avgHeight),
                1 => new Forest(pos, avgHeight),
                2 => new Woods(pos, avgHeight),
                3 => new Sand(pos, avgHeight),
                4 => new CoastalWater(pos, avgHeight),
                5 => new DeepWater(pos, avgHeight),
                _ => null,
            };
        }

        private Tile CreateTile(Tile tile1, Tile tile2, int x, int y)
        {
            
            int[] results = new int[6];

            for(int i = 0; i < 6; i++)
            {
                int rnd = Randomizer.random.Next(0, 100);
                results[i] = rnd * tile1.weight.weight[i] * tile2.weight.weight[i];
            }

            int bestTile = 0;
            int givenTile = 0;
            for(int i = 0; i < 6; i++)
            {
                if(results[i] > bestTile)
                {
                    bestTile = results[i];
                    givenTile = i;
                }
            }
            Point pos = new Point(x, y);

            int avgHeight = 64;
            return givenTile switch
            {
                0 => new Grass(pos, avgHeight),
                1 => new Forest(pos, avgHeight),
                2 => new Woods(pos, avgHeight),
                3 => new Sand(pos, avgHeight),
                4 => new CoastalWater(pos, avgHeight),
                5 => new DeepWater(pos, avgHeight),
                _ => null,
            };
        }
    }
}
