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
		private readonly float r;
		private readonly float g;
		private readonly float b;

		public ColorPoint3D (float x, float y, float z, float r, float g, float b)
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

		public float getR {
			get {
				return this.r;
			}
		}

		public float getG {
			get {
				return this.g;
			}
		}

		public float getB {
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

