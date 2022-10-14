using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OutbreakLibrary;
using Microsoft.Xna.Framework;

namespace TestApplication
{
	class Program
	{
		static void Main(string[] args)
		{
			OutbreakManager manager = new OutbreakManager();
			manager.Populate(7, 11, Vector3.Zero, 100*Vector3.One);

			Console.WriteLine(manager.GetAllHumans().Count());
		}
	}
}
