using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WaveFunctionCollapseTest
{
    public class Weighting
    {
        //grass 0, forest 1, woods 2, sand 3, coastal 4, deep 5, hill 6, mountain 7, snow, 8

        public int[] weight;

        public Weighting(int g, int f, int w, int s, int c, int d)
        {
            weight = new int[6] { g, f, w, s, c, d } ;
        }
    }
    class Tile
    {
        public Point position;
        protected Texture2D texture = Game1.texture;
        protected Color color;
        public int height;
        public string name;
        public Weighting weight;
        public Tile(Point pos, int height)
        {
            this.height = height;
            texture = Game1.texture;
            position = pos;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Color drawColor = color;
            drawColor.A /= 2;
            //drawColor.A = (byte)(height * 8);
            Color bgColor = new Color(height * 2, height * 2, height * 2, 255);
            spriteBatch.Draw(texture, new Rectangle(position.X * 4, position.Y * 4, 4, 4), bgColor);
            spriteBatch.Draw(texture, new Rectangle(position.X * 4, position.Y * 4, 4, 4), drawColor);
        }

    }

    class Hill : Tile
    {
        public Hill(Point pos, int height) : base(pos, height)
        {
            texture = Game1.texture;
            color = Color.MintCream;
            name = "hill";
            weight = new Weighting(65, 25, 0, 26, 0, 0);
        }
    }

    class Mountain : Tile
    {
        public Mountain(Point pos, int height) : base(pos, height)
        {
            texture = Game1.texture;
            color = Color.SandyBrown;
            name = "mountain";
            weight = new Weighting(65, 25, 0, 26, 0, 0);
        }
    }

    class Snow : Tile
    {
        public Snow(Point pos, int height) : base(pos, height)
        {
            texture = Game1.texture;
            color = Color.LightGray;
            name = "snow";
            weight = new Weighting(65, 25, 0, 26, 0, 0);
        }
    }

    class Grass : Tile
    {
        public Grass(Point pos, int height) : base(pos, height)
        {
            texture = Game1.texture;
            color = Color.LightGreen;
            name = "grass";
            weight = new Weighting(65, 25, 0, 26, 0, 0);
        }
    }
    class Forest : Tile
    {
        public Forest(Point pos, int height) : base(pos, height)
        {
            texture = Game1.texture;
            color = Color.Green;
            name = "forest";
            weight = new Weighting(15, 40, 25, 5, 5, 0);
        }
    }

    class Woods : Tile
    {
        public Woods(Point pos, int height) : base(pos, height)
        {
            texture = Game1.texture;
            color = Color.DarkGreen;
            name = "woods";
            weight = new Weighting(10, 40, 60, 2, 2, 0);
        }
    }

    class Sand : Tile
    {
        public Sand(Point pos, int height) : base(pos, height)
        {
            texture = Game1.texture;
            color = Color.Yellow;
            name = "sand";
            weight = new Weighting(21, 3, 0, 25, 20, 0);
        }
    }

    class CoastalWater : Tile
    {
        public CoastalWater(Point pos, int height) : base(pos, height)
        {
            texture = Game1.texture;
            color = Color.CornflowerBlue;
            name = "coastal water";
            weight = new Weighting(2, 1, 0, 15, 15, 40);
            
        }
    }

    class DeepWater : Tile
    {
        public DeepWater(Point pos, int height) : base(pos, height)
        {
            texture = Game1.texture;
            color = Color.Blue;
            name = "deep water";
            weight = new Weighting(1, 1, 1, 6, 22, 62);
        }
    }
}
