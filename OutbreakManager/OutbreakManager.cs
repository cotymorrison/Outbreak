using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace OutbreakLibrary
{
	public class OutbreakManager
	{
		private Dictionary<Guid, Moveable> entityDictionary;
		private List<Human> allHumans;
		private List<Zombie> allZombies;
		private Random random;


		public OutbreakManager()
		{
			allHumans = new List<Human>();
			allZombies = new List<Zombie>();
			random = new Random();
		}


		public void Populate(int num_humans, int num_zombies, Vector3 min_location, Vector3 max_location)
		{
			Random r = new Random();

			for (int i=0; i<num_humans; i++)
				allHumans.Add(new Human(NextVector3(min_location, max_location), Vector3.Normalize(NextVector3(-Vector3.One, Vector3.One))));

			for (int i=0; i<num_zombies; i++)
				allZombies.Add(new Zombie(NextVector3(min_location, max_location), Vector3.Normalize(NextVector3(-Vector3.One, Vector3.One))));
		}


        public void Update(GameTime gameTime)
		{
			// Sort all entities in the X direction
			SortedList<float, Guid> sortedEntities = new SortedList<float,Guid>();
			foreach (Moveable entity in entityDictionary.Values)
				sortedEntities.Add(entity.location.X, entity.GUID);

			// Update all sorted entities
			for (int i=0; i<sortedEntities.Count; i++)
			{
				List<Moveable> proximity = new List<Moveable>();
				Moveable currentEntity = entityDictionary[sortedEntities.ElementAt(i).Value];

				// Transform any infected humans
				if (currentEntity.GetType() == typeof(Human) && ((Human)currentEntity).infected == true)
				{
					Zombie transformed = new Zombie(currentEntity.location, currentEntity.lookDirection);
					entityDictionary.Remove(currentEntity.GUID);
					entityDictionary.Add(transformed.GUID, transformed);
				}

				// Check backwards through the array
				for (int j = i; j > 0; j--)
				{
					Moveable targetEntity = entityDictionary[sortedEntities.ElementAt(j).Value];

					if (currentEntity.GetType() == typeof(Zombie) && currentEntity.location.X - targetEntity.location.X <= Zombie.MAX_VIEW_DISTANCE)
					{
						if ((currentEntity.location - targetEntity.location).Length() <= Zombie.MAX_VIEW_DISTANCE)
							proximity.Add(targetEntity);
					}
					else if (currentEntity.GetType() == typeof(Human) && currentEntity.location.X - targetEntity.location.X <= Human.MAX_VIEW_DISTANCE)
					{
						if ((currentEntity.location - targetEntity.location).Length() <= Human.MAX_VIEW_DISTANCE)
							proximity.Add(targetEntity);
					}
					else
						break;
				}

				// Check forwards through the array
				for (int j = i; j < sortedEntities.Count; j++)
				{
					Moveable targetEntity = entityDictionary[sortedEntities.ElementAt(j).Value];

					if (currentEntity.GetType() == typeof(Zombie) && currentEntity.location.X - targetEntity.location.X <= Zombie.MAX_VIEW_DISTANCE)
					{
						if ((currentEntity.location - targetEntity.location).Length() <= Zombie.MAX_VIEW_DISTANCE)
							proximity.Add(targetEntity);
					}
					else if (currentEntity.GetType() == typeof(Human) && currentEntity.location.X - targetEntity.location.X <= Human.MAX_VIEW_DISTANCE)
					{
						if ((currentEntity.location - targetEntity.location).Length() <= Human.MAX_VIEW_DISTANCE)
							proximity.Add(targetEntity);
					}
					else
						break;
				}

				// Update the current entity
				if (currentEntity.GetType() == typeof(Zombie))
				{
					((Zombie)currentEntity).CheckSurroundings(proximity);
					((Zombie)currentEntity).Update(gameTime);
				}
				else if (currentEntity.GetType() == typeof(Human))
				{
					((Human)currentEntity).CheckSurroundings(proximity);
					((Human)currentEntity).Update(gameTime);
				}
			}
        }

		/// <summary>
		/// Returns an IEnumerable of all humans
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Human> GetHumans()
		{
			return entityDictionary.Values.OfType<Human>();
		}

		/// <summary>
		/// Returns an IEnumerable of all zombies
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Zombie> GetZombies()
		{
			return entityDictionary.Values.OfType<Zombie>();
		}

		/// <summary>
		/// Returns a random Vector3 within the min/max range
		/// </summary>
		/// <param name="min">lowest value</param>
		/// <param name="max">highest value</param>
		/// <returns></returns>
		private Vector3 NextVector3(Vector3 min, Vector3 max)
		{
			return new Vector3(NextFloat(min.X, max.X), NextFloat(min.Y, max.Y), NextFloat(min.Z, max.Z));
		}

		/// <summary>
		/// Returns a random float within the min/max range
		/// </summary>
		/// <param name="min">lowest value</param>
		/// <param name="max">highest value</param>
		/// <returns></returns>
		private float NextFloat(float min, float max)
		{
			return (float)random.NextDouble()*(max - min) + min;
		}
	}
}
