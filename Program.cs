using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Método_de_Gauss_Seidel
{
    //Clase que representa una ecuacion del sistema
    class GaussSeidol
    {
        public float[] array;    //Array de coeficientes
        public float constante;    //Término independiente
        public int numoperacion;    //Índice de ecuación

        public GaussSeidol() {}

        //Método para registrar datos desde consola
        public void Registro(int num, int var)
        {
            array = new float[var]; // Número de variables

            // Se leen los coeficientes de cada variable
            for (int i = 0; i < var; i++)
            {
                Console.Write($"Valor de x{i + 1}: ");
                array[i] = float.Parse(Console.ReadLine());
            }

            // Se lee el término independiente
            Console.Write("Constante: ");
            constante = float.Parse(Console.ReadLine());

            // Se guarda el índice de la ecuación
            numoperacion = num;
        }

        //Sobreescribe una ecuación (útil pasra intercambios)
        public void Overwrite(float[] arreglo, float cons, int num)
        {
            array = arreglo;
            constante = cons;
            numoperacion = num;
        }

        // Métodos tipo "get" para acceder a los datos
        public float [] Regreso() => array;
        public float Regresocons() => constante;
        public int Regresonum() => numoperacion;

        //Verificar si la fila cumple diagonal dominante
        public bool EsDiagonalDominante(){
            float diagonal = Math.Abs(array[numoperacion]); // elemento diagonal
            float suma = 0;

            // Suma de valores absolutos excepto la diagonal
            for(int i = 0; i < array.Length; i++){
                if(i != numoperacion)
                    suma += Math.Abs(array[i]);
            }
            // Condición de diagonal dominante
            return diagonal >= suma;
        }

        //Muestra la ecuación en consola correctamente
        public void Mostrar()
        {
            Console.Write($"#{numoperacion + 1}: ");

            for (int j = 0; j < array.Length; j++)
            {
                Console.Write($"{array[j]}x{j + 1} + ");
            }

            // \b\b elimina el último "+ "
            Console.WriteLine($"\b\b= {constante}");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            int operaciones;

            // Número de ecuaciones (igual al número de incógnitas)
            Console.Write("Cuál es el número de operaciones a guardar? ");
            operaciones = int.Parse(Console.ReadLine());
        
            GaussSeidol[] ecuaciones = new GaussSeidol[operaciones];
        
            //Registro de ecuaciones
            for (int i = 0; i < operaciones; i++)
            {
                Console.WriteLine($"\nEcuación {i + 1}");
                ecuaciones[i] = new GaussSeidol();
                ecuaciones[i].Registro(i, operaciones);
            }
        
            //Mostrar sistema original
            Console.WriteLine("\nSistema ingresado:");
            foreach (var eq in ecuaciones)
                eq.Mostrar();
        
            //Intento de hacer diagonal dominante
            //Se intercambian filas si es necesario
            for (int i = 0; i < operaciones; i++)
            {
                if (!ecuaciones[i].EsDiagonalDominante())
                {
                    for (int j = i + 1; j < operaciones; j++)
                    {
                        // Se busca una fila con mejor coeficiente en la diagonal
                        if (Math.Abs(ecuaciones[j].array[i]) > Math.Abs(ecuaciones[i].array[i]))
                        {
                            // Intercambio de ecuaciones
                            var tempArray = ecuaciones[i].Regreso();
                            var tempConst = ecuaciones[i].Regresocons();
                            var tempNum = ecuaciones[i].Regresonum();

                            // Intercambiaio de filas
                            ecuaciones[i].Overwrite(
                                ecuaciones[j].Regreso(),
                                ecuaciones[j].Regresocons(),
                                ecuaciones[j].Regresonum()
                            );
        
                            ecuaciones[j].Overwrite(tempArray, tempConst, tempNum);
                        }
                    }
                }
            }
        
            Console.WriteLine("\nSistema reordenado:");
            foreach (var eq in ecuaciones)
                eq.Mostrar();
        
            //Verificación  global de diagonal dominante
            bool diagonalDominante = true;
        
            for (int i = 0; i < operaciones; i++)
            {
                float diagonal = Math.Abs(ecuaciones[i].array[i]);
                float suma = 0;
        
                for (int j = 0; j < operaciones; j++)
                {
                    if (i != j)
                        suma += Math.Abs(ecuaciones[i].array[j]);
                }
        
                if (diagonal < suma)
                {
                    diagonalDominante = false;
                    break;
                }
            }
        
            //Mensaje al usuario
            if (!diagonalDominante)
            {
                Console.WriteLine("\n⚠ ADVERTENCIA:");
                Console.WriteLine("La matriz NO es diagonal dominante.");
                Console.WriteLine("El método de Gauss-Seidel puede NO converger.");
        
                Console.Write("\n¿Deseas continuar? (s/n): ");
                string opcion = Console.ReadLine().ToLower();
        
                if (opcion != "s")
                {
                    Console.WriteLine("Programa detenido.");
                    return;
                }
            }
            else
            {
                Console.WriteLine("\n✔ La matriz es diagonal dominante. Se garantiza convergencia.");
            }
        
            //Convertir a matriz
            float[,] A = new float[operaciones, operaciones];
            float[] b = new float[operaciones];
        
            for (int i = 0; i < operaciones; i++)
            {
                for (int j = 0; j < operaciones; j++)
                    A[i, j] = ecuaciones[i].array[j];
        
                b[i] = ecuaciones[i].constante;
            }
        
            //Parámetros
            Console.Write("\nTolerancia: ");
            float tol = float.Parse(Console.ReadLine());
        
            Console.Write("Máx iteraciones: ");
            int maxIter = int.Parse(Console.ReadLine());
        
            float[] x = new float[operaciones];      // Solución actual
            float[] x_old = new float[operaciones];  // Solución anterior
        
            // Inicializar en 0
            for (int i = 0; i < operaciones; i++)
                x[i] = 0;
        
            Console.WriteLine("\nIteraciones:\n");
        
            //Métdo de Gauss-Seidel
            for (int k = 0; k < maxIter; k++)
            {
                // Guardar valores anteriores
                for (int i = 0; i < operaciones; i++)
                    x_old[i] = x[i];
        
                // Iteración
                for (int i = 0; i < operaciones; i++)
                {
                    float suma = 0;
        
                    for (int j = 0; j < operaciones; j++)
                    {
                        if (j != i)
                            suma += A[i, j] * x[j];
                    }

                    // Fórmula principal
                    x[i] = (b[i] - suma) / A[i, i];
                }
        
                // Cálculo del error
                float error = 0;
                for (int i = 0; i < operaciones; i++)
                {
                    float e = Math.Abs((x[i] - x_old[i]) / (x[i] == 0 ? 1 : x[i]));
                    if (e > error)
                        error = e;
                }
        
                // Mostrar iteración
                Console.Write($"Iter {k + 1}: ");
                for (int i = 0; i < operaciones; i++)
                    Console.Write($"x{i + 1}={x[i]:F5} ");
        
                Console.WriteLine($" Error={error:F6}");
        
                // Criterio de paro
                if (error < tol)
                {
                    Console.WriteLine("\nConvergencia alcanzada.");
                    break;
                }
            }
        
            //Resultado Final
            Console.WriteLine("\nSolución final:");
            for (int i = 0; i < operaciones; i++)
                Console.WriteLine($"x{i + 1} = {x[i]:F6}");
        }
    }
}
