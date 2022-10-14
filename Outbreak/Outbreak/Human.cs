using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace OutbreakManager
{
    public class Human : Moveable
    {
        public const float MAX_RUN_SPEED = 6.25856f;        // m/s
        public const float MAX_WALK_SPEED = 1.38889f;       // m/s
        public const float MAX_VIEW_ANGLE = 2*1.0472f;        // rad
        public const float MAX_VIEW_DISTANCE = 30f;         // m
        public const float MAX_HEIGHT = 1.9812f;            // m
        public const float MAX_WIDTH = 0.6096f;             // m
        public const float MAX_LENGTH = 0.3048f;            // m

        Dictionary<Guid, Vector3> spottedThreats;
        public Vector3 targetLocation;
        float survivability;
        public bool infected;
        static Random r = new Random();


        public Human(Vector3 location, Vector3 lookDirection)
            : base()
        {
            this.location = location;
            this.lookDirection = lookDirection;

            infected = false;
            targetLocation = this.location;
            spottedThreats = new Dictionary<Guid, Vector3>();
            boundBox = OBB.CreateFromAABB(new BoundingBox(Vector3.Zero, new Vector3(MAX_WIDTH, MAX_HEIGHT, MAX_LENGTH)));
            survivability = r.Next(20, 100) / 100f;
        }

        public new void Update(GameTime gameTime)
        {
            if (Vector3.Dot(targetLocation - location, lookDirection) <= 0)
                targetLocation = this.location;

            if (spottedThreats.Count != 0)
                Run(gameTime);
            else
                Wander(gameTime);

            base.Update(gameTime);
        }

        public void CheckSurroundings(List<Zombie> inProximity)
        {
            foreach (Zombie zombie in inProximity)
            {
                Vector3 spotVector = Vector3.Normalize(zombie.location - this.location);
                float dot = Vector3.Dot(spotVector, this.lookDirection);

                if (dot > 0)
                {
                    double spotAngle = Math.Acos(dot);

					if (spotAngle >= -MAX_VIEW_ANGLE && spotAngle <= MAX_VIEW_ANGLE)
                    {
                        if (spottedThreats.ContainsKey(zombie.MoveableID))
                            spottedThreats.Remove(zombie.MoveableID);

                        spottedThreats.Add(zombie.MoveableID, zombie.location);
                    }
                }
            }

            foreach (KeyValuePair<Guid, Vector3> threat in spottedThreats.ToArray())
                if ((threat.Value - location).Length() > 3 * MAX_VIEW_DISTANCE)
                    spottedThreats.Remove(threat.Key);

            for (int i = spottedThreats.Count; i > 20; i--)
                spottedThreats.Remove(spottedThreats.First().Key);
        }

        public void Wander(GameTime gameTime)
        {
            if (targetLocation == this.location)
                targetLocation = new Vector3(Level.X_BOUND * (float)r.NextDouble(), Level.Y_BOUND * (float)r.NextDouble(), 0);

            lookDirection = Vector3.Normalize(targetLocation - location);
            velocity = lookDirection * MAX_WALK_SPEED;
        }

        public void Run(GameTime gameTime)
        {
            targetLocation = this.location;
            Vector3 runDirection = Vector3.Zero;

            foreach (Vector3 dangerZone in spottedThreats.Values)
            {
                Vector3 dangerDirection = this.location - dangerZone;
                runDirection += Vector3.Normalize(dangerDirection) / dangerDirection.Length();
            }

            lookDirection = Vector3.Normalize(runDirection);
            velocity = lookDirection * MAX_RUN_SPEED * survivability;
        }
    }
}
