using System;
using System.Collections.Generic;
using System.Text;

namespace PolyglotSidequest_MEF2D
{
    class Tools
    {
		private void obtenerDatos(istream file, int nlines, int n, int mode, item[] item_list)
		{
			string line;
			file >> line;
			if (nlines == DOUBLELINE)
			{
				file >> line;
			}

			for (int i = 0; i < n; i++)
			{
				switch (mode)
				{
					case INT_FLOAT:
						int e0;
						float r0;
						file >> e0 >> r0;
						item_list[i].setValues(NOTHING, NOTHING, NOTHING, e0, NOTHING, NOTHING, r0);
						break;
					case INT_FLOAT_FLOAT:
						int e;
						float r;
						float rr;
						file >> e >> r >> rr;
						item_list[i].setValues(e, r, rr, NOTHING, NOTHING, NOTHING, NOTHING);
						break;
					case INT_INT_INT_INT:
						int e1;
						int e2;
						int e3;
						int e4;
						file >> e1 >> e2 >> e3 >> e4;
						item_list[i].setValues(e1, NOTHING, NOTHING, e2, e3, e4, NOTHING);
						break;
				}
			}
		}

		private void correctConditions(int n, condition[] list, int[] indices)
		{
			for (int i = 0; i < n; i++)
			{
				indices[i] = list[i].getNode1();
			}

			for (int i = 0; i < n - 1; i++)
			{
				int pivot = list[i].getNode1();
				for (int j = i; j < n; j++)
				{
					//Si la condiciÃ³n actual corresponde a un nodo posterior al nodo eliminado por
					//aplicar la condiciÃ³n anterior, se debe actualizar su posiciÃ³n.
					if (list[j].getNode1() > pivot)
					{
						list[j].setNode1(list[j].getNode1() - 1);
					}
				}
			}
		}

		private void addExtension(ref string newfilename, ref string filename, ref string extension)
		{
			int ori_length = filename.Length;
			int ext_length = extension.Length;
			int i;
			for (i = 0; i < ori_length; i++)
			{
				newfilename[i] = filename[i];
			}
			for (i = 0; i < ext_length; i++)
			{
				newfilename[ori_length + i] = extension[i];
			}
			newfilename[ori_length + i] = '\0';
		}

		private void leerMallayCondiciones(mesh m, ref string filename)
		{
			string inputfilename = new string(new char[150]);
			ifstream file = new ifstream();
			float k;
			float Q;
			int nnodes;
			int neltos;
			int ndirich;
			int nneu;

			addExtension(ref inputfilename, ref filename, ".dat");
			file.open(inputfilename);

			file >> k >> Q;
			//cout << "k y Q: "<<k<<" y "<<Q<<"\n";
			file >> nnodes >> neltos >> ndirich >> nneu;
			//cout << "sizes: "<<nnodes<<" y "<<neltos<<" y "<<ndirich<<" y "<<nneu<<"\n";

			m.setParameters(k, Q);
			m.setSizes(nnodes, neltos, ndirich, nneu);
			m.createData();

			obtenerDatos(file, SINGLELINE, nnodes, INT_FLOAT_FLOAT, m.getNodes());
			obtenerDatos(file, DOUBLELINE, neltos, INT_INT_INT_INT, m.getElements());
			obtenerDatos(file, DOUBLELINE, ndirich, INT_FLOAT, m.getDirichlet());
			obtenerDatos(file, DOUBLELINE, nneu, INT_FLOAT, m.getNeumann());

			file.close();

			//Se corrigen los Ã­ndices en base a las filas que serÃ¡n eliminadas
			//luego de aplicar las condiciones de Dirichlet
			correctConditions(ndirich, m.getDirichlet(), m.getDirichletIndices());
		}

		private bool findIndex(int v, int s, int[] arr)
		{
			for (int i = 0; i < s; i++)
			{
				if (arr[i] == v)
				{
					return true;
				}
			}
			return false;
		}
		private void obtenerDatos(istream file, int nlines, int n, int mode, item[] item_list)
		{
			string line;
			file >> line;
			if (nlines == DOUBLELINE)
			{
				file >> line;
			}

			for (int i = 0; i < n; i++)
			{
				switch (mode)
				{
					case INT_FLOAT:
						int e0;
						float r0;
						file >> e0 >> r0;
						item_list[i].setValues(NOTHING, NOTHING, NOTHING, e0, NOTHING, NOTHING, r0);
						break;
					case INT_FLOAT_FLOAT:
						int e;
						float r;
						float rr;
						file >> e >> r >> rr;
						item_list[i].setValues(e, r, rr, NOTHING, NOTHING, NOTHING, NOTHING);
						break;
					case INT_INT_INT_INT:
						int e1;
						int e2;
						int e3;
						int e4;
						file >> e1 >> e2 >> e3 >> e4;
						item_list[i].setValues(e1, NOTHING, NOTHING, e2, e3, e4, NOTHING);
						break;
				}
			}
		}

		private void correctConditions(int n, condition[] list, int[] indices)
		{
			for (int i = 0; i < n; i++)
			{
				indices[i] = list[i].getNode1();
			}

			for (int i = 0; i < n - 1; i++)
			{
				int pivot = list[i].getNode1();
				for (int j = i; j < n; j++)
				{
					//Si la condiciÃ³n actual corresponde a un nodo posterior al nodo eliminado por
					//aplicar la condiciÃ³n anterior, se debe actualizar su posiciÃ³n.
					if (list[j].getNode1() > pivot)
					{
						list[j].setNode1(list[j].getNode1() - 1);
					}
				}
			}
		}

		private void addExtension(ref string newfilename, ref string filename, ref string extension)
		{
			int ori_length = filename.Length;
			int ext_length = extension.Length;
			int i;
			for (i = 0; i < ori_length; i++)
			{
				newfilename[i] = filename[i];
			}
			for (i = 0; i < ext_length; i++)
			{
				newfilename[ori_length + i] = extension[i];
			}
			newfilename[ori_length + i] = '\0';
		}

		private void leerMallayCondiciones(mesh m, ref string filename)
		{
			string inputfilename = new string(new char[150]);
			ifstream file = new ifstream();
			float k;
			float Q;
			int nnodes;
			int neltos;
			int ndirich;
			int nneu;

			addExtension(ref inputfilename, ref filename, ".dat");
			file.open(inputfilename);

			file >> k >> Q;
			//cout << "k y Q: "<<k<<" y "<<Q<<"\n";
			file >> nnodes >> neltos >> ndirich >> nneu;
			//cout << "sizes: "<<nnodes<<" y "<<neltos<<" y "<<ndirich<<" y "<<nneu<<"\n";

			m.setParameters(k, Q);
			m.setSizes(nnodes, neltos, ndirich, nneu);
			m.createData();

			obtenerDatos(file, SINGLELINE, nnodes, INT_FLOAT_FLOAT, m.getNodes());
			obtenerDatos(file, DOUBLELINE, neltos, INT_INT_INT_INT, m.getElements());
			obtenerDatos(file, DOUBLELINE, ndirich, INT_FLOAT, m.getDirichlet());
			obtenerDatos(file, DOUBLELINE, nneu, INT_FLOAT, m.getNeumann());

			file.close();

			//Se corrigen los Ã­ndices en base a las filas que serÃ¡n eliminadas
			//luego de aplicar las condiciones de Dirichlet
			correctConditions(ndirich, m.getDirichlet(), m.getDirichletIndices());
		}

		private bool findIndex(int v, int s, int[] arr)
		{
			for (int i = 0; i < s; i++)
			{
				if (arr[i] == v)
				{
					return true;
				}
			}
			return false;
		}

		private void writeResults(mesh m, Vector T, ref string filename)
		{
			string outputfilename = new string(new char[150]);
			//--------------------------------------------------------------------------------------------------------------------
			//C++ TO C# CONVERTER TODO TASK: C# does not have an equivalent to pointers to value types:
			//ORIGINAL LINE: int *dirich_indices = m.getDirichletIndices();
			int dirich_indices = m.getDirichletIndices();
			condition[] dirich = m.getDirichlet();
			ofstream file = new ofstream();

			addExtension(ref outputfilename, ref filename, ".post.res");
			file.open(outputfilename);

			file << "GiD Post Results File 1.0\n";
			file << "Result \"Temperature\" \"Load Case 1\" 1 Scalar OnNodes\nComponentNames \"T\"\nValues\n";

			int Tpos = 0;
			int Dpos = 0;
			int n = m.getSize(NODES);
			int nd = m.getSize(DIRICHLET);
			for (int i = 0; i < n; i++)
			{
				if (findIndex(i + 1, nd, dirich_indices))
				{
					file << i + 1 << " " << dirich[Dpos].getValue() << "\n";
					Dpos++;
				}
				else
				{
					file << i + 1 << " " << T.at(Tpos) << "\n";
					Tpos++;
				}
			}

			file << "End values\n";

			file.close();
		}

	}
}
