using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WaveFunctionCollapseTest
{
    class Civilization
    {
        List<Tile> ownedTerritory = new List<Tile>();
        Tile capital;
        string name;
        int age = 0;
        int militarist;
        int scientific;
        int traditionalist;
        int isolationist;
        int expansionist;
        int artistic;
        int individualist;
        int commercialist;
        List<Civilization> discoveredCivilizations = new List<Civilization>();
        List<int> opinionOfCivilizations = new List<int>();

        public Civilization(Tile tile)
        {
            capital = tile;
            ownedTerritory.Add(capital);
            age = 0;
            militarist = Randomizer.random.Next(-50, 50);
            scientific = Randomizer.random.Next(-50, 50);
            traditionalist = Randomizer.random.Next(-50, 50);
            isolationist = Randomizer.random.Next(-50, 50);
            expansionist = Randomizer.random.Next(-50, 50);
            artistic = Randomizer.random.Next(-50, 50);
            individualist = Randomizer.random.Next(-50, 50);
            commercialist = Randomizer.random.Next(-50, 50);
        }

        public void Update()
        {

        }

        public void FirstContact(Civilization civ)
        {
            discoveredCivilizations.Add(civ);
           
            int opinion = 0;
            // warmonger check
            if (civ.militarist > militarist - 10 && civ.militarist < militarist + 10)
            {
                opinion += Math.Abs((civ.militarist - militarist) - 10);

                //based on respect for sciences if roughly equal in militarism
                if(civ.scientific > scientific && scientific > 33)
                {
                    //respect for being scientific nations
                    opinion += 10;
                }
                else if(civ.scientific < scientific && civ.scientific > 33)
                {
                    //respect for being scientific but being behind
                    opinion += 5;
                }
                else if(civ.scientific < -10 && scientific > 20)
                {
                    //seen as inferior
                    opinion -= 15;
                }
                else if(civ.scientific > scientific - 10 && civ.scientific < scientific + 10)
                {
                    // if all else fails, equality in science brings people somewhat closer together
                    opinion += 3;
                }

                //if both countries have a tradition of their level of militarism, mutual respect
                if(civ.traditionalist > 25 && traditionalist > 25)
                {
                    opinion += 15;
                }

                if(civ.expansionist > expansionist + 10)
                {
                    //seen as expansionist
                    opinion -= 10;
                }
                 
                //this is a lot of work


                //if(civ.)
                
            } 
            else
            {
                opinion -= Math.Abs(civ.militarist - militarist);
            }


            opinionOfCivilizations.Add(opinion);
        }
        
    }
}
