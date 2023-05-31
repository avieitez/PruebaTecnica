using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        int cantidadNumeros = 10000000;
        string archivoSalida = @"C:\Alberto\PruebasCandidatos\PruebasCandidatos\Fuentes\Random\Random\numeros.txt";
        Random random = new Random();
        HashSet<int> numerosDistintos = new HashSet<int>();

        // Generar números aleatorios distintos
        while (numerosDistintos.Count < cantidadNumeros)
        {
            int numero = random.Next();
            numerosDistintos.Add(numero);
        }

        // Guardar los números en el archivo de texto
        using (StreamWriter writer = new StreamWriter(archivoSalida))
        {
            foreach (int numero in numerosDistintos)
            {
                writer.WriteLine(numero);
            }
        }

        Console.WriteLine($"Archivo generado: {archivoSalida}");
        Console.ReadLine();
    }
}
