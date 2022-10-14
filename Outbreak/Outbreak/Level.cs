using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OutbreakLibrary;

namespace Outbreak
{
    public class Level
    {
        public const float X_BOUND = 1920;
        public const float Y_BOUND = 1080;

        Logic logic;


        public Level(int num_zombies, int num_humans)
        {
            Random r = new Random();
            List<Moveable> list = new List<Moveable>();

            for (int i = 0; i < num_zombies; i++)
                list.Add(new Zombie(new Vector3((float)r.NextDouble() * X_BOUND, (float)r.NextDouble() * Y_BOUND, 0), Vector3.Normalize(new Vector3((float)r.NextDouble(), (float)r.NextDouble(), 0))));

            for (int i = 0; i < num_humans; i++)
                list.Add(new Human(new Vector3((float)r.NextDouble() * X_BOUND, (float)r.NextDouble() * Y_BOUND, 0), Vector3.Normalize(new Vector3((float)r.NextDouble(), (float)r.NextDouble(), 0))));

            logic = new Logic(list);
        }

        public void Update(GameTime gameTime)
        {
            logic.Update(gameTime);
        }

		public void Draw(SpriteBatch spriteBatch, Texture2D smiley, Texture2D zombie)
        {
			logic.Draw(spriteBatch, smiley, zombie);
        }

        public int GetNumZombies()
        {
            return logic.entities.Count(i => i.GetType() == typeof(Zombie));
        }

        public int GetNumHumans()
        {
            return logic.entities.Count(i => i.GetType() == typeof(Human));
        }
    }
}
