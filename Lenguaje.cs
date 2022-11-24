//BRIONES ALMAGUER CINTHYA CRISTINA
using System;
using System.Collections.Generic;
/*
X - Requerimiento 1: Construir un metodo para escribir en el archivo Lenguaje.cs identando el codigo
                 { -> Incrementa una tabulador } -> Decrementa un tabulador
Requerimiento 2: Declarar un atributo "primera produccion" de tipo string y actualizarlo con 
                 la primera produccion de la gramatica 
Requerimiento 3: La primera producion es publica y el resto privadas
Requerimiento 4: El consttuctor Lexico parametrizado debe validar que la extension del archivo
                 a compilar sea .gen y si no levantar una excepcion
Requerimiento 5: Resolver la amiguedad de ST y SNT
                 Recorrer linea por linea el archivo gram para extraer cada nombre de producción
                 LEER LA LINEA 1 (Read.Line) Y DEPOSITAR EN UN STRING Y GARDARLO EN LA LISTA DE SNT
                 EL TOKEN ES EL CONTENIDO Y LA CLASIFICACION.
Requerimiento 6: Agregar el PIzquierdo y PDerecho escapados en la matriz de transiciones.
                 0 -(\) > 5      
Requerimietno 7: Implementar el OR y la Cerradura epsilon.
                    Variables -> (Identificador)?
                    Listaidentificadores -> (Caracter | Numero)
                 
*/

namespace Generador
{
    public class Lenguaje : Sintaxis, IDisposable
    {
        int tabulaciones = 0;
        List<string> listaSNT;  //LISTA DE SIMBOLOS NO TERMINALES 
        public Lenguaje (string nombre) : base(nombre)
        {
            listaSNT = new List<string>();
        }

        public Lenguaje ()
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
            //Requerimiento 6.
            
            listaSNT.Add(contenido);
        }

        private void Programa(string produccionPrincipal)
        {
            agregarSNT("Programa");
            agregarSNT("Librerias");
            agregarSNT("Variables");
            agregarSNT("Listaidentificadores");
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
            lenguaje.WriteLine(tabulacion("private void "+getContenido()+"()"));
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
            if (esTipo(getContenido()))
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

        private string tabulacion(string contenido)
        {
            string cadena="";
            if (contenido == "}")
            {
                tabulaciones--;
            }
            for (int i = 0; i < tabulaciones; i++)
            {
                cadena=("\t")+cadena;
            }
            if (contenido == "{")
            {
                tabulaciones++;
            }
            return cadena+contenido;

        }
    }
}