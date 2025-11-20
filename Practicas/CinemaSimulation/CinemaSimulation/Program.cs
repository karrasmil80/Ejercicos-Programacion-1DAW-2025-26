//Librerias
using System.Text;
using static System.Console;
using Serilog; //Importamos la libreria del logger
using System.Text; //Importamos UTF8 para los iconos

//Instancia del LOGGER
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

//Configuración de la consola
Console.Title = "Cinema Simulation";
Console.OutputEncoding = Encoding.UTF8;
Console.Clear();

//Constantes globales

//Programa principal
Main(args);



//Funcionalidades

/*
 * En esta funcion pediremos al usuario el tamaño del mapa como argumentos principales del programa
 * FILA [args[0]]
 * COLUMNA[args[1]] */
void Main(string[] args) {
    WriteLine("//////////////////////////////////");
    WriteLine("       CINEMA SIMULATION");
    WriteLine("//////////////////////////////////");
    WriteLine();
    
    PrintArgs();
    
    
}

void PrintArgs() {
    
    string[] arg1 = args[0].Split(' ');
    
    
    try{
        if(args.Length != 2) {
            Log.Debug("Entrando al if que comprueba argumentos");
            Log.Warning("¡NO SE HAN INTRODUCIDO ARGUMENTOS DE ENTRADA!");
            
        }
    } catch(Exception e){
        WriteLine("Introduce los argumentos en formato tipo entero");

    }
    
    WriteLine("ARGUMENTOS PRINCIPALES DEL PROGRAMA");
    WriteLine($"FILAS : {args[0]}");
    WriteLine($"COLUMNAS : {args[1]}");
        
    
}

