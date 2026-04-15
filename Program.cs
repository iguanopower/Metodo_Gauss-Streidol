using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Método_de_Gauss_Seidel
{
    class GaussSeidol
    {
        public float[] array;
        public float constante;
        public int numoperacion;

        public GaussSeidol() {}

        public void Registro(int num, int var)
        {
            array = new float[var];

            for (int i = 0; i < var; i++)
            {
                Console.Write("Valor de la variable {0}: ", i + 1);
                array[i] = float.Parse(Console.ReadLine());
            }

            Console.Write("\nCuál es el valor de la constante? ");
            constante = float.Parse(Console.ReadLine());
            Console.WriteLine();

            numoperacion = num;
        }

        public void Overwrite(float[] arreglo, float cons, int num)
        {
            array = arreglo;
            constante = cons;
            numoperacion = num;
        }

        public float[] Regreso()
        {
            return array;
        }

        public float Regresocons()
        {
            return constante;
        }

        public int Regresonum()
        {
            return numoperacion;
        }

        public int Check()
        {
            bool var = true;
            int error = 0;
            int pos = 0;

            foreach (float num in array)
            {
                if (num <= array[numoperacion])
                {
                    var = var && true;

                    pos++;
                }
                else
                {
                    var = false;

                    error = pos;
                }
                
            }
            if (var)
            {
                return 0;
            }
            else
            {
                return error;
            }
        }

        public void Mostrar()
        {
            Console.Write("#{0}: ", numoperacion+1);
            foreach(float num in array)
            {
                int j = 1;
                Console.Write("{0} x{1} + ", num, j);
            }
            Console.WriteLine("\b\b= {0}", constante);
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            int operaciones, operación = 0;

            Console.Write("Cuál es el número de operaciones a guardar? ");
            operaciones = int.Parse(Console.ReadLine());

            float[] arreglo = new float[operaciones];

            GaussSeidol[] ecuaciones = new GaussSeidol[operaciones];

            for (int i = 0; i < operaciones; i++)
            {
                Console.WriteLine("Operación Número {0}", operación + 1);

                ecuaciones[operación] = new GaussSeidol();

                ecuaciones[operación].Registro(operación, operaciones);

                operación++;
            }

            operación = 0;

            foreach(GaussSeidol ecuacion in ecuaciones)
            {
                ecuaciones[operación].Mostrar();
                operación++;
            }


            float[] temparray1 = new float[operaciones];
            float tempconst1 = 0;
            int tempnum1 = 0;

            bool used = false;

            float[] temparray2 = new float[operaciones];
            float tempconst2 = 0;
            int tempnum2 = 0;

            bool incorrect = false;
            int pos, poserror1 = 0, poserror2 = 0;

            do
            {
                foreach (GaussSeidol ecuacion in ecuaciones)
                {
                    pos = ecuacion.Check();

                    if (pos != 0)
                    {
                        incorrect = true;

                        if (used)
                        {
                            temparray1 = ecuacion.Regreso();
                            tempconst1 = ecuacion.Regresocons();
                            tempnum1 = ecuacion.Regresonum();

                            poserror2 = pos;
                        }
                        else
                        {
                            temparray2 = ecuacion.Regreso();
                            tempconst2 = ecuacion.Regresocons();
                            tempnum2 = ecuacion.Regresonum();

                            poserror1 = pos;

                            used = true;
                        }
                    }
                }
                if (incorrect)
                {
                    ecuaciones[poserror1].Overwrite(temparray2, tempconst2, tempnum2);
                    ecuaciones[poserror2].Overwrite(temparray1, tempconst1, tempnum1);
                }

            } while (incorrect);

            if (used)
            {
                operación = 0;

                foreach (GaussSeidol ecuacion in ecuaciones)
                {
                    ecuaciones[operación].Mostrar();
                    operación++;
                }
            }
        }
    }
}