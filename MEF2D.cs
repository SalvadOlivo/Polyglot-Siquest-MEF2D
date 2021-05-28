using System;
using System.Collections.Generic;
using System.Text;
using static System.Math;
using static PolyglotSidequest_MEF2D.Classes;
using static PolyglotSidequest_MEF2D.Math_tools;
using static PolyglotSidequest_MEF2D.Sel;
using static PolyglotSidequest_MEF2D.Tools;
using System.Numerics;

//deje los comentarios originales del codigo como referencia para no perdernos.
//este es la clase main 
//se nos dificulta para encontrar equivalentes a las librerias utilizadas, librerias como "Ifstream" 

namespace PolyglotSidequest_MEF2D
{
    class MEF2D
    {
		private static void Main(string[] args)
		{
			string filename = new string(new char[150]);
			filename = args[1];

			List<Matrix> localKs = new List<Matrix>();
			List<Vector> localbs = new List<Vector>();
			Matrix K = new Matrix();
			Vector b = new Vector();
			Vector T = new Vector();

			Console.Write("IMPLEMENTACION DEL METODO DE LOS ELEMENTOS FINITOS\n");
			Console.Write("\t- TRANSFERENCIA DE CALOR\n");
			Console.Write("\t- 2 DIMENSIONES\n");
			Console.Write("\t- FUNCIONES DE FORMA LINEALES\n");
			Console.Write("\t- PESOS DE GALERKIN\n");
			Console.Write("\t- MALLA TRIANGULAR IRREGULAR\n");
			Console.Write("*********************************************************************************\n\n");
			
			//FALTA-----------------------------------------------------------------
			mesh m = new mesh();
			leerMallayCondiciones(m, filename);
			Console.Write("Datos obtenidos correctamente\n********************\n");

			crearSistemasLocales(m, localKs, localbs);
			//showKs(localKs); showbs(localbs);
			Console.Write("******************************\n");

			zeroes(K, m.getSize(NODES));
			zeroes(b, m.getSize(NODES));
			ensamblaje(m, localKs, localbs, K, b);
			//showMatrix(K); showVector(b);
			Console.Write("******************************\n");
			//cout << K.size() << " - "<<K.at(0).size()<<"\n";
			//cout << b.size() <<"\n";

			applyNeumann(m, b);
			//showMatrix(K); showVector(b);
			Console.Write("******************************\n");
			//cout << K.size() << " - "<<K.at(0).size()<<"\n";
			//cout << b.size() <<"\n";

			applyDirichlet(m, K, b);
			//showMatrix(K); showVector(b);
			Console.Write("******************************\n");
			//cout << K.size() << " - "<<K.at(0).size()<<"\n";
			//cout << b.size() <<"\n";

			zeroes(T, b.size());
			calculate(K, b, T);

			//cout << "La respuesta es: \n";
			//showVector(T);

			writeResults(m, T, filename);

		}

	}
}
