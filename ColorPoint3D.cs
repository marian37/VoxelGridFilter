using System;

namespace VoxelGridFilter
{
	/**
	 * 
	 * Trieda reprezentujúca farebný bod - súradnice x, y, z a RGB formát farby
	 * 
	 **/
	public class ColorPoint3D
	{
		private readonly float x;
		private readonly float y;
		private readonly float z;
		private readonly int r;
		private readonly int g;
		private readonly int b;

		public ColorPoint3D (float x, float y, float z, int r, int g, int b)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.r = r;
			this.g = g;
			this.b = b;
		}

		public float getX {
			get {
				return this.x;
			}
		}

		public float getY {
			get {
				return this.y;
			}
		}

		public float getZ {
			get {
				return this.z;
			}
		}

		public int getR {
			get {
				return this.r;
			}
		}

		public int getG {
			get {
				return this.g;
			}
		}

		public int getB {
			get {
				return this.b;
			}
		}

		public override string ToString ()
		{
			return this.getX + " " + this.getY + " " + this.getZ + " " + this.getR + " " + this.getG + " " + this.getB; 
		}
	}
}

