//BRIONES ALMAGUER CINTHYA CRISTINA
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
/*
X - Requerimiento 1: Construir un metodo para escribir en el archivo Lenguaje.cs identando el codigo
                 { -> Incrementa una tabulador } -> Decrementa un tabulador
X - Requerimiento 2: Declarar un atributo "primera produccion" de tipo string y actualizarlo con 
                 la primera produccion de la gramatica 
X - Requerimiento 3: La primera producion es publica y el resto privadas
X - Requerimiento 4: El consttuctor Lexico parametrizado debe validar que la extension del archivo
                 a compilar sea .gen y si no levantar una excepcion
X - Requerimiento 5: Resolver la amiguedad de ST y SNT
                 Recorrer linea por linea el archivo gram para extraer cada nombre de producción
                 LEER LA LINEA 1 (Read.Line) Y DEPOSITAR EN UN STRING Y GARDARLO EN LA LISTA DE SNT
                 EL TOKEN ES EL CONTENIDO Y LA CLASIFICACION.
X - Requerimiento 6: Agregar el PIzquierdo y PDerecho escapados en la matriz de transiciones.
                 0 -(\) > 5      --LEXICO
Requerimietno 7: Implementar el 
                    Variables -> if (Identificador)
                    Listaidentificadores -> (Caracter | Numero)
                 
*/

namespace Generador
{
    public class Lenguaje : Sintaxis, IDisposable
    {
        int tabulaciones = 0;
        int contador = 0;
        string primeraProduccion = "";
        List<string> listaSNT;  //LISTA DE SIMBOLOS NO TERMINALES 
        public Lenguaje(string nombre) : base(nombre)
        {
            listaSNT = new List<string>();
        }

        public Lenguaje()
        {
            listaSNT = new List<string>();
        }

        public void Dispose()
        {
            //Console.WriteLine("\nDestructor");
            cerrar();
            //GC.SuppressFinalize(this);
        }

        private bool esSNT(string contenido)
        {
            return listaSNT.Contains(contenido);
            //return true;
        }

        private void agregarSNT(string contenido)
        {
            //Requerimiento 5.
            //leerArchivo();
            //https://learn.microsoft.com/es-es/dotnet/csharp/programming-guide/file-system/how-to-read-a-text-file-one-line-at-a-time
            string[] readText = System.IO.File.ReadAllLines(@"C:\Users\Cinthya Almaguer\OneDrive\Documentos\Visual Studio 2022\Generador\c2.gram");
            foreach (string line in readText)
            {
                if (line.Contains("->"))
                {
//https://learn.microsoft.com/es-es/dotnet/standard/base-types/divide-up-strings#code-try-3
                    string[] nuevo = line.Split("->",StringSplitOptions.RemoveEmptyEntries); 
/*String.Split ayudarle a dividir una cadena en un grupo de subcadenas en 
función de uno o más caracteres delimitadores que especifique. 
Para omitir las subcadenas vacías de la matriz resultante, puede llamar a la sobrecarga 
Split(Char[], StringSplitOptions) y especificar StringSplitOptions.RemoveEmptyEntries 
para el parámetro options.*/
                    string agregar = nuevo[0];
                    agregar = agregar.Replace(" ", "");
                    //Console.WriteLine(agregar);
                    listaSNT.Add(agregar);
                }
            }
        }


        private void Programa(string primeraProduccion)
        {
            agregarSNT(getContenido());
            programa.WriteLine("using System;");
            programa.WriteLine("using System.IO;");
            programa.WriteLine("using System.Collections.Generic;");
            programa.WriteLine();

            programa.WriteLine("namespace Generico");
            programa.WriteLine("{");
            programa.WriteLine();
            programa.WriteLine("\tpublic class Program");
            programa.WriteLine("\t{");
            programa.WriteLine("\t\tstatic void Main(string[] args)");
            programa.WriteLine("\t\t{");
            programa.WriteLine("\t\t\ttry");
            programa.WriteLine("\t\t\t{");
            programa.WriteLine("\t\t\t\tusing (Lenguaje a = new Lenguaje())");
            programa.WriteLine("\t\t\t\t{");
            //REQUERIMIENTO 2
            programa.WriteLine("\t\t\t\t\ta." + primeraProduccion + "();");
            programa.WriteLine("\t\t\t\t}");
            programa.WriteLine("\t\t\t}");
            programa.WriteLine("\t\t\tcatch (Exception e)");
            programa.WriteLine("\t\t\t{");
            programa.WriteLine("\t\t\t\tConsole.WriteLine(e.Message);");
            programa.WriteLine("\t\t\t}");
            programa.WriteLine("\t\t\t}");
            programa.WriteLine("\t\t}");
            programa.WriteLine("\t}");
        }
        public void gramatica()
        {
            cabecera();
            //REQUERIMIENTO 2
            primeraProduccion = getContenido();
            Programa(primeraProduccion);
            //Console.WriteLine(primeraProduccion); 
            cabeceraLenguaje();
            listaProducciones();
            lenguaje.WriteLine(tabulacion("}"));
            lenguaje.WriteLine(tabulacion("}"));
        }

        private void cabecera()
        {
            match("Gramatica");
            match(":");
            match(Tipos.ST);
            match(Tipos.FinProduccion);
        }

        private void cabeceraLenguaje()
        {
            lenguaje.WriteLine(tabulacion("using System;"));
            lenguaje.WriteLine(tabulacion("using System.Collections.Generic;"));
            lenguaje.WriteLine(tabulacion("namespace Generico"));
            lenguaje.WriteLine(tabulacion("{"));
            lenguaje.WriteLine(tabulacion("public class Lenguaje : Sintaxis, IDisposable"));
            lenguaje.WriteLine(tabulacion("{"));
            lenguaje.WriteLine(tabulacion("string nombreProyecto;"));
            lenguaje.WriteLine(tabulacion("public Lenguaje(string nombre) : base(nombre)"));
            lenguaje.WriteLine(tabulacion("{"));
            lenguaje.WriteLine(tabulacion("}"));
            lenguaje.WriteLine(tabulacion("public Lenguaje()"));
            lenguaje.WriteLine(tabulacion("{"));
            lenguaje.WriteLine(tabulacion("}"));
            lenguaje.WriteLine(tabulacion("public void Dispose()"));
            lenguaje.WriteLine(tabulacion("{"));
            lenguaje.WriteLine(tabulacion("cerrar();"));
            lenguaje.WriteLine(tabulacion("}"));

        }

        private void listaProducciones()
        {
            if (contador == 0)
            {
                //primeraProduccion = getContenido();
                //Console.WriteLine(primeraProduccion); 
                lenguaje.WriteLine(tabulacion("public void " + getContenido() + "()"));
                contador++;
            }
            else
            {
                lenguaje.WriteLine(tabulacion("private void " + getContenido() + "()"));
            }
            lenguaje.WriteLine(tabulacion("{"));
            match(Tipos.ST);
            match(Tipos.Produce);
            simbolos();
            match(Tipos.FinProduccion);
            lenguaje.WriteLine(tabulacion("}"));
            if (!FinArchivo())
            {
                listaProducciones();
            }
        }
        private void simbolos()
        {
            if (getContenido() == "(")
            {
                match("(");
                //QUE ENTRA AQUI EN EL IF SI EL TOKEN ES UN GETTIPO CHECAR EN CLASIFICACION
                //Y CON GETCONTENIDO
                lenguaje.WriteLine(tabulacion("if ()"));
                lenguaje.WriteLine(tabulacion("{"));
                simbolos();
                match(")");
                lenguaje.WriteLine(tabulacion("}"));
            }
            else if (esTipo(getContenido()))
            {
                lenguaje.WriteLine(tabulacion("match(Tipos." + getContenido() + ");"));
                match(Tipos.ST);
            }
            else if (esSNT(getContenido()))
            {
                lenguaje.WriteLine(tabulacion("" + getContenido() + "();"));
                match(Tipos.ST);
            }
            else if (getClasificacion() == Tipos.ST)
            {
                lenguaje.WriteLine(tabulacion("match(\"" + getContenido() + "\");"));
                match(Tipos.ST);
            }

            if (getClasificacion() != Tipos.FinProduccion && getContenido() != ")")
            {
                simbolos();
            }

        }

        private bool esTipo(string clasificacion)
        {
            switch (clasificacion)
            {
                case "Identificador":
                case "Numero":
                case "Caracter":
                case "Asignacion":
                case "Inicializacion":
                case "OperadorLogico":
                case "OperadorRelacional":
                case "OperadorTernario":
                case "OperadorTermino":
                case "OperadorFactor":
                case "IncrementoTermino":
                case "IncrementoFactor":
                case "FinSentencia":
                case "Cadena":
                case "TipoDato":
                case "Zona":
                case "Condicion":
                case "Ciclo":
                    return true;
            }
            return false;
        }

        private string tabulacion(string contenido)
        {
            string cadena = "";
            if (contenido == "}")
            {
                tabulaciones--;
            }
            for (int i = 0; i < tabulaciones; i++)
            {
                cadena = ("\t") + cadena;
            }
            if (contenido == "{")
            {
                tabulaciones++;
            }
            return cadena + contenido;

        }
    }
}