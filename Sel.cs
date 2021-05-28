using System;
using System.Collections.Generic;
using System.Text;

namespace PolyglotSidequest_MEF2D
{
    class Sel
    {
		private void showMatrix(Matrix K)
		{
			for (int i = 0; i < K.at(0).size(); i++)
			{
				Console.Write("[\t");
				for (int j = 0; j < K.size(); j++)
				{
					Console.Write(K.at(i).at(j));
					Console.Write("\t");
				}
				Console.Write("]\n");
			}
		}

		private void showKs(List<Matrix> Ks)
		{
			for (int i = 0; i < Ks.Count; i++)
			{
				Console.Write("K del elemento ");
				Console.Write(i + 1);
				Console.Write(":\n");
				showMatrix(new List<Matrix>(Ks[i]));
				Console.Write("*************************************\n");
			}
		}

		private void showVector(Vector b)
		{
			Console.Write("[\t");
			for (int i = 0; i < b.size(); i++)
			{
				Console.Write(b.at(i));
				Console.Write("\t");
			}
			Console.Write("]\n");
		}

		private void showbs(List<Vector> bs)
		{
			for (int i = 0; i < bs.Count; i++)
			{
				Console.Write("b del elemento ");
				Console.Write(i + 1);
				Console.Write(":\n");
				showVector(new List<Vector>(bs[i]));
				Console.Write("*************************************\n");
			}
		}

		private float calculateLocalD(int i, mesh m)
		{
			float D;
			float a;
			float b;
			float c;
			float d;

			element e = m.getElement(i);

			node n1 = m.getNode(e.getNode1() - 1);
			node n2 = m.getNode(e.getNode2() - 1);
			node n3 = m.getNode(e.getNode3() - 1);

			a = n2.getX() - n1.getX();
			b = n2.getY() - n1.getY();
			c = n3.getX() - n1.getX();
			d = n3.getY() - n1.getY();
			D = a * d - b * c;

			return D;
		}

		private float calculateMagnitude(float v1, float v2)
		{
			return Math.Sqrt(Math.Pow(v1, 2) + Math.Pow(v2, 2));
		}

		private float calculateLocalArea(int i, mesh m)
		{
			//Se utiliza la fÃ³rmula de HerÃ³n
			float A;
			float s;
			float a;
			float b;
			float c;
			element e = m.getElement(i);
			node n1 = m.getNode(e.getNode1() - 1);
			node n2 = m.getNode(e.getNode2() - 1);
			node n3 = m.getNode(e.getNode3() - 1);

			a = calculateMagnitude(n2.getX() - n1.getX(), n2.getY() - n1.getY());
			b = calculateMagnitude(n3.getX() - n2.getX(), n3.getY() - n2.getY());
			c = calculateMagnitude(n3.getX() - n1.getX(), n3.getY() - n1.getY());
			s = (a + b + c) / 2;

			A = Math.Sqrt(s * (s - a) * (s - b) * (s - c));
			return A;
		}

		private void calculateLocalA(int i, Matrix A, mesh m)
		{
			element e = m.getElement(i);
			node n1 = m.getNode(e.getNode1() - 1);
			node n2 = m.getNode(e.getNode2() - 1);
			node n3 = m.getNode(e.getNode3() - 1);
			A.at(0).at(0) = n3.getY() - n1.getY();
			A.at(0).at(1) = n1.getY() - n2.getY();
			A.at(1).at(0) = n1.getX() - n3.getX();
			A.at(1).at(1) = n2.getX() - n1.getX();
		}

		private void calculateB(Matrix B)
		{
			B.at(0).at(0) = -1;
			B.at(0).at(1) = 1;
			B.at(0).at(2) = 0;
			B.at(1).at(0) = -1;
			B.at(1).at(1) = 0;
			B.at(1).at(2) = 1;
		}

		private Matrix createLocalK(int element, mesh m)
		{
			// K = (k*Ae/D^2)Bt*At*A*B := K_3x3
			float D;
			float Ae;
			float k = m.getParameter(THERMAL_CONDUCTIVITY);
			Matrix K = new Matrix();
			Matrix A = new Matrix();
			Matrix B = new Matrix();
			Matrix Bt = new Matrix();
			Matrix At = new Matrix();

			D = calculateLocalD(element, new mesh(m));
			Ae = calculateLocalArea(element, new mesh(m));

			zeroes(A, 2);
			zeroes(B, 2, 3);
			calculateLocalA(element, A, new mesh(m));
			calculateB(B);
			transpose(A, At);
			transpose(B, Bt);

			productRealMatrix(k * Ae / (D * D), productMatrixMatrix(Bt, productMatrixMatrix(At, productMatrixMatrix(A, B, 2, 2, 3), 2, 2, 3), 3, 2, 3), K);

			return new Matrix(K);
		}

		

private float calculateLocalJ(int i, mesh m)
	{
		float J;
		float a;
		float b;
		float c;
		float d;
		element e = m.getElement(i);
		node n1 = m.getNode(e.getNode1() - 1);
		node n2 = m.getNode(e.getNode2() - 1);
		node n3 = m.getNode(e.getNode3() - 1);

		a = n2.getX() - n1.getX();
		b = n3.getX() - n1.getX();
		c = n2.getY() - n1.getY();
		d = n3.getY() - n1.getY();
		J = a * d - b * c;

		return J;
	}

	private Vector createLocalb(int element, mesh m)
	{
		Vector b = new Vector();

		float Q = m.getParameter(HEAT_SOURCE);
		float J;
		float b_i;
		J = calculateLocalJ(element, new mesh(m));

		b_i = Q * J / 6;
		b.push_back(b_i);
		b.push_back(b_i);
		b.push_back(b_i);

		return new Vector(b);
	}

	private void crearSistemasLocales(mesh m, List<Matrix> localKs, List<Vector> localbs)
	{
		for (int i = 0; i < m.getSize(ELEMENTS); i++)
		{
			localKs.Add(createLocalK(i, m));
			localbs.Add(createLocalb(i, m));
		}
	}

	private void assemblyK(element e, Matrix localK, Matrix K)
	{
		int index1 = e.getNode1() - 1;
		int index2 = e.getNode2() - 1;
		int index3 = e.getNode3() - 1;

		K.at(index1).at(index1) += localK.at(0).at(0);
		K.at(index1).at(index2) += localK.at(0).at(1);
		K.at(index1).at(index3) += localK.at(0).at(2);
		K.at(index2).at(index1) += localK.at(1).at(0);
		K.at(index2).at(index2) += localK.at(1).at(1);
		K.at(index2).at(index3) += localK.at(1).at(2);
		K.at(index3).at(index1) += localK.at(2).at(0);
		K.at(index3).at(index2) += localK.at(2).at(1);
		K.at(index3).at(index3) += localK.at(2).at(2);
	}

	private void assemblyb(element e, Vector localb, Vector b)
	{
		int index1 = e.getNode1() - 1;
		int index2 = e.getNode2() - 1;
		int index3 = e.getNode3() - 1;

		b.at(index1) += localb.at(0);
		b.at(index2) += localb.at(1);
		b.at(index3) += localb.at(2);
	}

	private void ensamblaje(mesh m, List<Matrix> localKs, List<Vector> localbs, Matrix K, Vector b)
	{
		for (int i = 0; i < m.getSize(ELEMENTS); i++)
		{
			element e = m.getElement(i);
			assemblyK(new element(e), new List<Matrix>(localKs[i]), K);
			assemblyb(new element(e), new List<Vector>(localbs[i]), b);
		}
	}

	private void applyNeumann(mesh m, Vector b)
	{
		for (int i = 0; i < m.getSize(NEUMANN); i++)
		{
			condition c = m.getCondition(i, NEUMANN);
			b.at(c.getNode1() - 1) += c.getValue();
		}
	}

	private void applyDirichlet(mesh m, Matrix K, Vector b)
	{
		for (int i = 0; i < m.getSize(DIRICHLET); i++)
		{
			condition c = m.getCondition(i, DIRICHLET);
			int index = c.getNode1() - 1;

			K.erase(K.begin() + index);
			b.erase(b.begin() + index);

			for (int row = 0; row < K.size(); row++)
			{
				float cell = K.at(row).at(index);
				K.at(row).erase(K.at(row).begin() + index);
				b.at(row) += -1 * c.getValue() * cell;
			}
		}
	}

	private void calculate(Matrix K, Vector b, Vector T)
	{
		Console.Write("Iniciando calculo de respuesta...\n");
		Matrix Kinv = new Matrix();
		Console.Write("Calculo de inversa...\n");
		inverseMatrix(K, Kinv);
		Console.Write("Calculo de respuesta...\n");
		productMatrixVector(Kinv, b, T);
	}


}
}


