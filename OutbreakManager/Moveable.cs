using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace OutbreakLibrary
{
    public class Moveable
    {
        static int SPEED_MULTIPLIER = 10;

		private Guid guid;
        public Vector3 location;
        public Vector3 velocity;
        public Vector3 lookDirection;
		//public OBB boundBox;
		protected static Random r;

		public Guid GUID { get { return guid; } }

        protected Moveable()
        {
			guid = Guid.NewGuid();
            location = new Vector3(float.NaN);
            velocity = Vector3.Zero;
            lookDirection = new Vector3(float.NaN);
            //boundBox = new OBB();
			r = new Random();
        }

        protected void Update(GameTime gameTime)
        {
            location += velocity * (float)gameTime.ElapsedGameTime.TotalSeconds * SPEED_MULTIPLIER;

			//if (location.X < 0)
			//    location.X = 0;
			//else if (location.X > Level.X_BOUND)
			//    location.X = Level.X_BOUND;

			//if (location.Y < 0)
			//    location.Y = 0;
			//else if (location.Y > Level.Y_BOUND)
			//    location.Y = Level.Y_BOUND;
        }
    }
}
