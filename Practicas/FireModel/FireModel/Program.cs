using System;
using static System.Console;
using System.Text;
using FireModel.Structs;
using Serilog;

OutputEncoding = Encoding.UTF8;

Serilog.ILogger logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

//Variables globales
Random random = new Random();

//Constantes globales
const int Filas = 20;
const int Columnas = 20;
const double PArder = 0.0006;
const double PCrecer = 0.01;
const int TiempoMaximo = 60;

logger.Information("Programa iniciado");

//Inicio del main
Main(args);
//Fin del main

void Main(string[] args){
    
    logger.Information("Entrando en la funcion principal del programa Main()");
    Configuracion configuracion = new Configuracion();

    configuracion.Filas = Filas;
    configuracion.Columnas = Columnas;
    
    WriteLine($"Configuración: FILAS: {configuracion.Filas} COLUMNAS: {configuracion.Columnas}");
    
    //Creamos la instancia del front buffer (Lectura)
    Matrix[,] frontBuffer = new Matrix[Filas, Columnas];
    
    //Creamos back buffer que sirve para leer y escribir los cambios en otra matriz aparte
    
    InitForest(frontBuffer, configuracion.Filas, configuracion.Columnas);
    
    //Imprimimos la matriz
    PrintMatrix(frontBuffer, configuracion.Filas, configuracion.Columnas);
    
}


/*
 * Zona de funciones/procedimientos
*/

void PrintMatrix(Matrix[,] matrix, int filas, int columnas){
    
    //Imprime un icono segun el caso
    for(int i = 0; i < filas; i++){
        for(int j = 0; j < columnas; j++){
            switch(matrix[i,j]){
                case Matrix.Arbol:
                    Write("[🌳]");
                    break;
                case Matrix.Ardiendo:
                    Write("[🔥]");
                    break;
                case Matrix.Vacio:
                    Write("[  ]");
                    break;
            }
        }
        WriteLine();
    }
}

Matrix[,] CloneMatrix(Matrix[,] matrix, int filas, int columnas){
    
    //Creamos una nueva matriz a partir de la anterior
    Matrix[,] newMatrix = new Matrix[filas, columnas];

    //Clonamos el contenido de la nueva (back) en la antigua (front)
    for(int i = 0; i < filas; i++){
        for(int j = 0; j < columnas; j++){
            newMatrix[i,j] = matrix[i,j];
        }
    }
    
    return newMatrix;
}

void InitForest(Matrix[,] matrix, int filas, int columnas){
    
    var porcentaje = random.Next(30, 80);
    var totalCasillas = filas * columnas;
    var totalArbolesPlantar = totalCasillas * porcentaje / 100; //Esto da el numero de arboles a plantar

    for(int i = 0; i < totalArbolesPlantar; i++){
        
        bool terminarPlantacion = false;
        
        var colocarFila = random.Next(0, filas);
        var colocarColumna = random.Next(0, columnas);

        while(!terminarPlantacion){
            if(matrix[colocarFila,colocarColumna] == Matrix.Vacio){
                matrix[colocarFila,colocarColumna] = Matrix.Arbol;
                terminarPlantacion = true;
            } 
        }
    }
}



