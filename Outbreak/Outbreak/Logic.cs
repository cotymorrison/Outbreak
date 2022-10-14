using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OutbreakManager
{
    public class Logic
    {
        public List<Moveable> entities;

        public Logic(List<Moveable> list)
        {
            entities = list;
        }

        public void Update(GameTime gameTime)
        {
            entities = entities.OrderBy(i => i.location.X).ToList();

            for (int i = 0; i < entities.Count; i++)
            {
                // Check for Humans close to the Zombie
                if (entities.ElementAt(i).GetType() == typeof(Zombie))
                {
                    List<Human> inProximity = new List<Human>();

                    // Check backwards through the array
                    for (int j = i; j > 0; j--)
                    {
                        if (entities.ElementAt(i).location.X - entities.ElementAt(j).location.X <= Zombie.MAX_VIEW_DISTANCE)
                        {
                            if (entities.ElementAt(j).GetType() == typeof(Human))
                            {
                                if (entities.ElementAt(i).location.Y - entities.ElementAt(j).location.Y <= Zombie.MAX_VIEW_DISTANCE)
                                    inProximity.Add((Human)entities.ElementAt(j));
                            }
                        }
                        else
                            break;
                    }

                    // Check forwards through the array
                    for (int j = i; j < entities.Count; j++)
                    {
                        if (entities.ElementAt(j).location.X - entities.ElementAt(i).location.X <= Zombie.MAX_VIEW_DISTANCE)
                        {
                            if (entities.ElementAt(j).GetType() == typeof(Human))
                            {
                                if (entities.ElementAt(i).location.Y - entities.ElementAt(j).location.Y <= Zombie.MAX_VIEW_DISTANCE)
                                    inProximity.Add((Human)entities.ElementAt(j));
                            }
                        }
                        else
                            break;
                    }

                    ((Zombie)entities.ElementAt(i)).CheckSurroundings(inProximity);
                    ((Zombie)entities.ElementAt(i)).Update(gameTime);
                }
                // Check for Zombies close to the Human
                else if (entities.ElementAt(i).GetType() == typeof(Human))
                {
                    // If the Human was infected, turn him into a Zombie
                    if (((Human)entities.ElementAt(i)).infected == true)
                    {
                        Vector3 tempLocation = entities.ElementAt(i).location;
                        Vector3 tempLookDirection = entities.ElementAt(i).lookDirection;
                        entities.RemoveAt(i);
                        entities.Add(new Zombie(tempLocation, tempLookDirection));
                    }
                    else
                    {
                        List<Zombie> inProximity = new List<Zombie>();

                        // Check backwards through the array
                        for (int j = i; j > 0; j--)
                        {
                            if (entities.ElementAt(i).location.X - entities.ElementAt(j).location.X <= Human.MAX_VIEW_DISTANCE)
                            {
                                if (entities.ElementAt(j).GetType() == typeof(Zombie))
                                {
                                    if (entities.ElementAt(i).location.Y - entities.ElementAt(j).location.Y <= Human.MAX_VIEW_DISTANCE)
                                        inProximity.Add((Zombie)entities.ElementAt(j));
                                }
                            }
                            else
                                break;
                        }

                        // Check forwards through the array
                        for (int j = i; j < entities.Count; j++)
                        {
                            if (entities.ElementAt(j).location.X - entities.ElementAt(i).location.X <= Human.MAX_VIEW_DISTANCE)
                            {
                                if (entities.ElementAt(j).GetType() == typeof(Zombie))
                                {
                                    if (entities.ElementAt(i).location.Y - entities.ElementAt(j).location.Y <= Human.MAX_VIEW_DISTANCE)
                                        inProximity.Add((Zombie)entities.ElementAt(j));
                                }
                            }
                            else
                                break;
                        }

                        ((Human)entities.ElementAt(i)).CheckSurroundings(inProximity);
                        ((Human)entities.ElementAt(i)).Update(gameTime);
                    }
                }
            }
        }

		public void Draw(SpriteBatch spriteBatch, Texture2D smiley, Texture2D zombie)
        {
			int magnifier = 30;

            foreach (Moveable entity in entities)
                if (entity.GetType() == typeof(Human))
					spriteBatch.Draw(smiley, new Rectangle((int)(entity.location.X - magnifier * Human.MAX_WIDTH / 2), (int)(entity.location.Y - magnifier * Human.MAX_LENGTH / 2), (int)(magnifier * Human.MAX_WIDTH), (int)(magnifier * Human.MAX_LENGTH)), Color.Yellow);
                else if (entity.GetType() == typeof(Zombie))
                {
					spriteBatch.Draw(zombie, new Rectangle((int)(entity.location.X - magnifier * Zombie.MAX_WIDTH / 2), (int)(entity.location.Y - magnifier * Zombie.MAX_LENGTH / 2), (int)(magnifier * Human.MAX_WIDTH), (int)(magnifier * Human.MAX_LENGTH)), Color.Green);
                    //spriteBatch.Draw(smiley, new Rectangle((int)(((Zombie)entity).targetLocation.X - 10 * Zombie.MAX_WIDTH / 2), (int)(((Zombie)entity).targetLocation.Y - 10 * Zombie.MAX_LENGTH / 2), (int)(10 * Human.MAX_WIDTH), (int)(10 * Human.MAX_LENGTH)), Color.Orange);
                }
        }
    }
}
