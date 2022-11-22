//BRIONES ALMAGUER CINTHYA CRISTINA
using System;
using System.Collections.Generic;
/*
Requerimiento 1: Construir un metodo para escribir en el archivo Lenguaje.cs identando el codigo
                 { -> Incrementa una tabulador } -> Decrementa un tabulador
Requerimiento 2: Declarar un atributo "primera produccion" de tipo string y actualizarlo con 
                 la primera produccion de la gramatica 
Requerimiento 3: La primera producion es publica y el resto privadas
Requerimiento 4: El consttuctor Lexico parametrizado debe validar que la extension del archivo
                 a compilar sea .gen y si no levantar una excepcion
Requerimiento 5: Resolver la amiguedad de ST y SNT
*/

namespace Generador
{
    public class Lenguaje : Sintaxis, IDisposable
    {
        public Lenguaje (string nombre) : base(nombre)
        {
            //Console.WriteLine("Constructor");
        }

        public Lenguaje ()
        {
            //Console.WriteLine("Constructor");
        }

        public void Dispose()
        {
            Console.WriteLine("\nDestructor");
            cerrar();
            GC.SuppressFinalize(this);
        }

        private void Programa(string produccionPrincipal)
        {
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
            programa.WriteLine("\t\t\t\t\ta." + produccionPrincipal + "();");
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
            Programa("programa");
            cabeceraLenguaje();
            listaProducciones();
            lenguaje.WriteLine("\t}");
            lenguaje.WriteLine("}");
        }
        private void cabecera()
        {
            match("Gramatica");
            match(":");
            match(Tipos.SNT);
            match(Tipos.FinProduccion);
        }

        private void cabeceraLenguaje()
        {
            lenguaje.WriteLine("using System;");
            lenguaje.WriteLine("using System.Collections.Generic;");
            lenguaje.WriteLine("namespace Generico");
            lenguaje.WriteLine("{");
            lenguaje.WriteLine("\tpublic class Lenguaje : Sintaxis, IDisposable");
            lenguaje.WriteLine("\t{");
            lenguaje.WriteLine("\t\tstring nombreProyecto;");
            lenguaje.WriteLine("\t\tpublic Lenguaje(string nombre) : base(nombre)");
            lenguaje.WriteLine("\t\t{");
            lenguaje.WriteLine("\t\t}");
            lenguaje.WriteLine("\t\tpublic Lenguaje()");
            lenguaje.WriteLine("\t\t{");
            lenguaje.WriteLine("\t\t}");
            lenguaje.WriteLine("\t\tpublic void Dispose()");
            lenguaje.WriteLine("\t\t{");
            lenguaje.WriteLine("\t\t\tcerrar();");
            lenguaje.WriteLine("\t\t}");

        }
        
        private void listaProducciones()
        {
            lenguaje.WriteLine("\t\tprivate void "+getContenido()+"()");
            lenguaje.WriteLine("\t\t{");
            match(Tipos.SNT);
            match(Tipos.Produce);
            simbolos();
            match(Tipos.FinProduccion);
            lenguaje.WriteLine("\t\t}");
            if (!FinArchivo())
            {
                listaProducciones();
            }
        }
        private void simbolos()
        {
            if (esTipo(getContenido()))
            {
                lenguaje.WriteLine("\t\t\tmatch(Tipos." + getContenido() + ");");
                match(Tipos.SNT);
            }
            else if (getClasificacion() == Tipos.ST)
            {
                lenguaje.WriteLine("\t\t\tmatch(Tipos." + getContenido() + "();");
                match(Tipos.ST);
            }
            else if (getClasificacion() == Tipos.SNT)
            {
                lenguaje.WriteLine("\t\t\tmatch(Tipos." + getContenido() + "();");
                match(Tipos.SNT);
            }

            if(getClasificacion() != Tipos.FinProduccion)
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
    }
}