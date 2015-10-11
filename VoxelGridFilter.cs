using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace VoxelGridFilter
{
	class MainClass
	{
		private static List<ColorPoint3D> vstup;
		private static int pocetBodovVstupu;
		private static List<ColorPoint3D> vystup;
		private static float maxX = float.MinValue;
		private static float minX = float.MaxValue;
		private static float maxY = float.MinValue;
		private static float minY = float.MaxValue;
		private static float maxZ = float.MinValue;
		private static float minZ = float.MaxValue;
		private static List<List<List<List<ColorPoint3D>>>> mriezka;

		/**
		 * 
		 * Načítanie vstupu zo súboru. Očakáva sa vstupný súbor typu COFF.
		 * 
		 **/
		private static void nacitajVstupZoSuboru (string nazovSuboru)
		{
			string[] riadky = System.IO.File.ReadAllLines (nazovSuboru);
			string riadok = riadky [1];
			string[] tokeny = riadok.Split (new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
			pocetBodovVstupu = Int32.Parse (tokeny [0]);
			vstup = new List<ColorPoint3D> (pocetBodovVstupu);

			CultureInfo ci = CultureInfo.InvariantCulture;

			for (int i = 0; i < pocetBodovVstupu; i++) {
				riadok = riadky [i + 2];
				tokeny = riadok.Split (new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
				float x = float.Parse (tokeny [0], ci);
				float y = float.Parse (tokeny [1], ci);
				float z = float.Parse (tokeny [2], ci);
				int r = (int)(float.Parse (tokeny [3], ci) * 255);
				int g = (int)(float.Parse (tokeny [4], ci) * 255);
				int b = (int)(float.Parse (tokeny [5], ci) * 255);
				ColorPoint3D bod = new ColorPoint3D (x, y, z, r, g, b);
				vstup.Add (bod);
			}
		}

		/**
		 * 
		 * Zápis výstupu do súboru. Typ výstupného súboru COFF.
		 * 
		 **/
		private static void zapisVystupDoSuboru (string nazovSuboru)
		{
			StreamWriter writer = new StreamWriter (nazovSuboru);
			writer.WriteLine ("COFF");
			writer.WriteLine (vystup.Count + " 0 0");

			//CultureInfo ci = CultureInfo.InvariantCulture;

			foreach (ColorPoint3D bod in vystup) {
				writer.WriteLine (bod.getX + " " + bod.getY + " " + bod.getZ + " " + bod.getR + " " + bod.getG + " " + bod.getB);
			}

			writer.Close ();

			Console.WriteLine ("Výstup zapísaný do súboru " + nazovSuboru + ".");
		}

		/**
		 * 
		 * Zistí rozsah súradníc vstupu, veľkosť priestoru.
		 * 
		 **/
		private static void zistiRozsah ()
		{
			foreach (ColorPoint3D bod in vstup) {
				if (bod.getX > maxX) {
					maxX = bod.getX;
				} else {
					if (bod.getX < minX) {
						minX = bod.getX;
					}
				}

				if (bod.getY > maxY) {
					maxY = bod.getY;
				} else {
					if (bod.getY < minY) {
						minY = bod.getY;
					}
				}

				if (bod.getZ > maxZ) {
					maxZ = bod.getZ;
				} else {
					if (bod.getZ < minZ) {
						minZ = bod.getZ;
					}
				}
			}
		}

		/**
		 * 
		 * Rozdelí body vstupu do voxelov.
		 * 
		 * velkostVoxelu - veľkosť hrany kocky/voxelu
		 *
		 **/
		private static void rozdelBody (float velkostVoxelu)
		{
			int pocetX = (int)Math.Ceiling ((maxX - minX) / velkostVoxelu);
			int pocetY = (int)Math.Ceiling ((maxY - minY) / velkostVoxelu);
			int pocetZ = (int)Math.Ceiling ((maxZ - minZ) / velkostVoxelu);
			mriezka = new List<List<List<List<ColorPoint3D>>>> (pocetX);

			// vytvorenie 4-rozmerného poľa
			for (int i = 0; i < pocetX; i ++) {
				List<List<List<ColorPoint3D>>> zoznam = new List<List<List<ColorPoint3D>>> (pocetY);
				mriezka.Add (zoznam);
				for (int j = 0; j < pocetY; j++) {
					List<List<ColorPoint3D>> zoznam2 = new List<List<ColorPoint3D>> (pocetZ);
					zoznam.Add (zoznam2);
					for (int k = 0; k < pocetZ; k++) {
						List<ColorPoint3D> zoznam3 = new List<ColorPoint3D> ();
						zoznam2.Add (zoznam3);
					}
				}
			}

			// samotné zadelenie bodov
			foreach (ColorPoint3D bod in vstup) {
				float x = bod.getX;
				float y = bod.getY;
				float z = bod.getZ;
				int xID = (int)Math.Floor ((x - minX) / velkostVoxelu);
				int yID = (int)Math.Floor ((y - minY) / velkostVoxelu);
				int zID = (int)Math.Floor ((z - minZ) / velkostVoxelu);
				mriezka [xID] [yID] [zID].Add (bod);
			}
		}

		/**
		 * 
		 * Zrátanie ťažiska - aritmetický priemer bodov voxelu
		 * 
		 **/
		private static ColorPoint3D zratajTazisko (List<ColorPoint3D> body)
		{
			float sumX = 0;
			float sumY = 0;
			float sumZ = 0;
			int sumR = 0;
			int sumG = 0;
			int sumB = 0;
			int pocet = body.Count;

			foreach (ColorPoint3D bod in body) {
				sumX += bod.getX;
				sumY += bod.getY;
				sumZ += bod.getZ;
				sumR += bod.getR;
				sumG += bod.getG;
				sumB += bod.getB;
			}

			return new ColorPoint3D (sumX / pocet, sumY / pocet, sumZ / pocet, sumR / pocet, sumG / pocet, sumB / pocet);
		}

		/**
		 * 
		 * Spustí algoritmus VoxelGridFilter
		 * 
		 **/
		private static void VoxelGridFilter (float velkostVoxelu)
		{
			vystup = new List<ColorPoint3D> (pocetBodovVstupu);
			zistiRozsah ();
			rozdelBody (velkostVoxelu);
			for (int i = 0; i < mriezka.Count; i++) {
				for (int j = 0; j < mriezka[i].Count; j++) {
					for (int k = 0; k < mriezka[i][j].Count; k++) {
						if (mriezka [i] [j] [k].Count > 0) {
							if (mriezka [i] [j] [k].Count == 1) {
								vystup.Add (mriezka [i] [j] [k] [0]);
							} else {
								ColorPoint3D tazisko = zratajTazisko (mriezka [i] [j] [k]);
								vystup.Add (tazisko);
							}
						}
					}
				}
			}		
		}

		public static void Main (string[] args)
		{
			nacitajVstupZoSuboru ("../../pc0.off");
			Console.WriteLine ("Vstup načítaný.");
			VoxelGridFilter (0.02f);
			Console.WriteLine ("Algoritmus VoxelGridFilter zbehol.");
			zapisVystupDoSuboru ("../../vystup0.off");
		}
	}
}
