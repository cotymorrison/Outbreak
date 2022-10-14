using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace OutbreakLibrary
{
	public class Human : Moveable
	{
		public const float MAX_RUN_SPEED = 6.25856f;        // m/s
		public const float MAX_WALK_SPEED = 1.38889f;       // m/s
		public const float MAX_VIEW_ANGLE = 2*1.0472f;      // rad
		public const float MAX_VIEW_DISTANCE = 30f;         // m
		public const float MAX_HEIGHT = 1.9812f;            // m
		public const float MAX_WIDTH = 0.6096f;             // m
		public const float MAX_LENGTH = 0.3048f;            // m

		Dictionary<Guid, Vector3> spottedThreats;
		public Vector3 targetDirection;
		float survivability;
		public bool infected;


		public Human(Vector3 location, Vector3 lookDirection)
			: base()
		{
			this.location = location;
			this.lookDirection = lookDirection;

			infected = false;
			targetDirection = Vector3.Zero;
			spottedThreats = new Dictionary<Guid, Vector3>();
			//boundBox = OBB.CreateFromAABB(new BoundingBox(Vector3.Zero, new Vector3(MAX_WIDTH, MAX_HEIGHT, MAX_LENGTH)));
			survivability = r.Next(20, 100) / 100f;
		}


		public new void Update(GameTime gameTime)
		{
			if (spottedThreats.Count != 0)
				Run(gameTime);
			else
				Wander(gameTime);

			base.Update(gameTime);
		}


		public void CheckSurroundings(List<Moveable> proximity)
		{
			// Look at nearby Moveables
			foreach (Moveable entity in proximity)
			{
				if (entity.GetType() == typeof(Zombie))
				{
					Vector3 spotVector = Vector3.Normalize(((Zombie)entity).location - this.location);
					float dot = Vector3.Dot(spotVector, this.lookDirection);

					if (dot > 0)
					{
						double spotAngle = Math.Acos(dot);

						if (spotAngle >= -MAX_VIEW_ANGLE && spotAngle <= MAX_VIEW_ANGLE)
						{
							if (spottedThreats.ContainsKey(entity.GUID))
								spottedThreats.Remove(entity.GUID);

							spottedThreats.Add(entity.GUID, entity.location);
						}
					}
				}
			}

			// Remove old threats
			for (int i = spottedThreats.Count; i > 20; i--)
				spottedThreats.Remove(spottedThreats.First().Key);
		}


		public void Wander(GameTime gameTime)
		{
			if (targetDirection == Vector3.Zero)
				targetDirection = Vector3.Normalize(new Vector3((float)r.NextDouble(), (float)r.NextDouble(), 0));

			lookDirection = targetDirection;
			velocity = lookDirection * MAX_WALK_SPEED;
		}


		public void Run(GameTime gameTime)
		{
			targetDirection = Vector3.Zero;

			foreach (Vector3 dangerZone in spottedThreats.Values)
			{
				Vector3 dangerDirection = this.location - dangerZone;
				targetDirection += Vector3.Normalize(dangerDirection) / dangerDirection.LengthSquared();
			}

			targetDirection.Normalize();
			lookDirection = targetDirection;
			velocity = lookDirection * MAX_RUN_SPEED * survivability;
		}
	}
}
