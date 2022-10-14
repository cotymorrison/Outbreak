using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace OutbreakLibrary
{
    public class Zombie : Moveable
    {
        public const float MAX_RUN_SPEED = 3.12928f;        // m/s
        public const float MAX_WALK_SPEED = 0.694445f;      // m/s
		public const float MAX_VIEW_ANGLE = 2*1.0472f;      // rad
        public const float MAX_VIEW_DISTANCE = 30f;         // m
        public const float MAX_HEIGHT = 1.9812f;            // m
        public const float MAX_WIDTH = 0.6096f;             // m
        public const float MAX_LENGTH = 0.3048f;            // m

        Human targetVictim;
        public Vector3 targetLocation;

        public Zombie(Vector3 location, Vector3 lookDirection)
            : base()
        {
            this.location = location;
            this.lookDirection = lookDirection;

            targetVictim = null;
            targetLocation = this.location;
            //boundBox = OBB.CreateFromAABB(new BoundingBox(Vector3.Zero, new Vector3(MAX_WIDTH, MAX_HEIGHT, MAX_LENGTH)));
        }


        public new void Update(GameTime gameTime)
        {
            if (Vector3.Dot(targetLocation - location, lookDirection) <= 0)
            {
                targetVictim = null;
                targetLocation = this.location;
            }

            if (targetVictim != null)
                Chase(gameTime);
            else
                Wander(gameTime);

            base.Update(gameTime);


            if (targetVictim != null && (targetVictim.location - this.location).Length() <= 1)
            {
                targetVictim.infected = true;
                targetVictim = null;
                targetLocation = this.location;
            }
        }


        public void CheckSurroundings(List<Moveable> proximity)
        {
			// Look at nearby Moveables
            foreach (Moveable entity in proximity)
            {
				if (entity.GetType() == typeof(Human) && ((Human)entity).infected == false)
				{
					Vector3 spotVector = Vector3.Normalize(entity.location - this.location);
					double spotAngle = Math.Acos(Vector3.Dot(spotVector, this.lookDirection));

					if (spotAngle >= -MAX_VIEW_ANGLE && spotAngle <= MAX_VIEW_ANGLE)
					{
						if (targetVictim == null || (entity.location - this.location).Length() < (targetLocation - this.location).Length())
						{
							targetVictim = (Human)entity;
							targetLocation = entity.location;
						}
					}
				}
            }
        }


        private void Wander(GameTime gameTime)
        {
			//if (targetLocation == this.location)
			//    targetLocation = new Vector3(Level.X_BOUND * (float)r.NextDouble(), Level.Y_BOUND * (float)r.NextDouble(), 0);

            lookDirection = Vector3.Normalize(targetLocation - location);
            velocity = lookDirection * MAX_WALK_SPEED;
        }


        private void Chase(GameTime gameTime)
        {
            lookDirection = Vector3.Normalize(targetLocation - location);
            velocity = lookDirection * MAX_RUN_SPEED;
        }
    }
}
