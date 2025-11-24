using System;
using static System.Console;
using System.Text;
using FireModel.Structs;
using Serilog;

OutputEncoding = Encoding.UTF8;

Serilog.ILogger logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();

//Variables globales
Random random = new Random();

//Constantes globales
const int Filas = 20;
const int Columnas = 20;
const double PArder = 0.06;
const double PEspontanea = 0.05;
const double PCrecer = 0.01;
const int TiempoMaximo = 60;

logger.Information("Programa iniciado");

//Inicio del main
Main(args);
//Fin del main

void Main(string[] args){

    var celdasCrecidas = 0;
    var celdasQuemadas = 0;
    var celdasVacias = 0;
    var celdasArbolFinal = 0;
    var celdasVaciasFinal = 0;
    var tiempo = TiempoMaximo;
    
    logger.Information("Entrando en la funcion principal del programa Main()");
    Configuracion configuracion;

    configuracion.Filas = Filas;
    configuracion.Columnas = Columnas;
    
    WriteLine($"Configuración: FILAS: {configuracion.Filas} COLUMNAS: {configuracion.Columnas}");
    
    //Creamos la instancia del front buffer (Lectura)
    Matrix[,] frontBuffer = new Matrix[Filas, Columnas];
    
    //Clonamos la matriz para escribir en ella
    var backBuffer = CloneMatrix(frontBuffer, configuracion.Filas, configuracion.Columnas);
    
    InitForest(frontBuffer, configuracion.Filas, configuracion.Columnas);
    
    do{
        //Imprimimos la matriz
        PrintMatrix(frontBuffer, configuracion.Filas, configuracion.Columnas);
        
        GenerarFuego(frontBuffer, configuracion.Filas, configuracion.Columnas); //Probabilidad de generar un incendio
        GenerarArbol(frontBuffer, configuracion.Filas, configuracion.Columnas, ref celdasCrecidas); //Probabilidad de generar un arbol
        ArderEspontaneo(frontBuffer, configuracion.Filas, configuracion.Columnas); //Probabilidad de que un arbol arda de repente
        
        //Funcion que maneja la lectura y la escritura
        Step(frontBuffer, backBuffer, configuracion.Filas, configuracion.Columnas, ref celdasQuemadas);
        
        //Tupla para alternar el estado entre el front buffer y el back buffer
        (frontBuffer, backBuffer) = (backBuffer, frontBuffer);
        
        WriteLine();
        
        Thread.Sleep(100);
        tiempo--;
        
        WriteLine($"Tiempo restante : {tiempo}");
        
        WriteLine();
        
    } while(tiempo != 0);
    
    conteoFinal(frontBuffer, configuracion.Filas, configuracion.Columnas, ref celdasArbolFinal, ref celdasVaciasFinal);
    
    WriteLine("INFORME");
    WriteLine($"Celdas quemadas : {celdasQuemadas}");
    WriteLine($"Celdas crecidas {celdasCrecidas}");
    WriteLine($"Celdas con arboles : {celdasArbolFinal}");
    WriteLine($"Celdas vacias {celdasVaciasFinal}");
    
}

/*
 * Zona de funciones/procedimientos
*/

void PrintMatrix(Matrix[,] matrix, int filas, int columnas){
    //Imprime un icono segun el caso
    logger.Information("Entrando a el procedimiento PrintMatrix()");
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
    logger.Information("Entrando a la funcion CloneMatrix()");
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
    logger.Information("Entrando al procedimiento InitForest()");
    
    //Calculo de arboles a plantar
    var porcentaje = random.Next(30, 80);
    var totalCasillas = filas * columnas;
    var totalArbolesPlantar = totalCasillas * porcentaje / 100; //Numero de arboles a plantar

    logger.Information($"Plantando {totalArbolesPlantar} árboles ({porcentaje}% de ocupación inicial).");
    
    //Bucle que planta arboles en una fila y una columna aleatorias
    for(int i = 0; i < totalArbolesPlantar; i++){
        
        //logger.Information("Entrando al for [initForest]");
        //Colocamos aqui dentro los random para que se genere una nueva posicion por cada iteracion
        var colocarFila = random.Next(0, filas);
        var colocarColumna = random.Next(0, columnas);
        
        bool plant = false;
        
        while(!plant){
            //logger.information("Entrando al while [initForest]")
            if(matrix[colocarFila, colocarColumna] == Matrix.Vacio){
                matrix[colocarFila, colocarColumna] = Matrix.Arbol;
            }
            plant = true;
        }
        //logger.Information("saliendo del while [initForest]");
    }
    //logger.Information("Saliendo del for [initForest]");
}

/*
 * Genera un fuego con una probabilidad del 0.006 en una casilla vacia
 */
void GenerarFuego(Matrix[,] frontBuffer, int filas, int columnas){
    logger.Information("Entrando a la funcion GenerarFuego()");
    var ponerFuego = random.Next(0, filas); //Elige un fila al azar
    var ponerFuego2 = random.Next(0, columnas); //Elige un columna al azar
    
    if(random.NextDouble() < PArder && frontBuffer[ponerFuego, ponerFuego] == Matrix.Vacio){
        //logger.Information("entrando al if [GenerarFuego]");
        frontBuffer[ponerFuego, ponerFuego2] = Matrix.Ardiendo;
        WriteLine("Fuego generado");
    }
}

/*
 * Genera un arbol con una probabilidad del 0.1 en una casilla vacia
 */
void GenerarArbol(Matrix[,] frontBuffer, int filas, int columnas ,ref int celdasCrecidas){
    logger.Information("Entrando a la funcion GenerarArbol()");
    var ponerArbol = random.Next(0, filas); //Elige un fila al azar
    var ponerArbol2 = random.Next(0, columnas); //Elige un columna al azar

    
    if(random.NextDouble() < PCrecer && frontBuffer[ponerArbol, ponerArbol2] == Matrix.Vacio){
        //logger.Information("entrando al if [GenerarArbol]");
        frontBuffer[ponerArbol, ponerArbol2] = Matrix.Arbol;
        celdasCrecidas++;
        WriteLine("Arbol generado");
    }
}

/*
 * Genera un incendio de manera espontanea en un arbol con una probabilidad del 0.05
 */
void ArderEspontaneo(Matrix[,] frontBuffer, int filas, int columnas){
    logger.Information("Entrando a la funcion ArderEspontaneo()");
    var ponerEspontaneo = random.Next(0, filas);
    var ponerEspontaneo2 = random.Next(0, columnas);

    if(random.NextDouble() < PEspontanea && frontBuffer[ponerEspontaneo, ponerEspontaneo2] == Matrix.Vacio){
        //logger.Information("entrando al if [ArderEspontaneo]");
        frontBuffer[ponerEspontaneo, ponerEspontaneo2] = Matrix.Arbol;
    }
}

/*
 * Devuelve verdadero o falso dependiendo de si un vecino es un arbol o esta ardiendo
 */
bool HasBurningNeighbour(Matrix[,] matrix, int i, int j, int filas, int columnas) {
    //Arriba izquierda
    if (i > 0 && j > 0) {
        //logger.Information("Entrando a if Arriba-izq [HasBurningNeighbour]");
        if (matrix[i - 1, j - 1] == Matrix.Ardiendo) {
            return true;
        }
    }

    //Arriba
    if (i > 0) {
        //logger.Information("Entrando a if Arriba [HasBurningNeighbour]");
        if (matrix[i - 1, j] == Matrix.Ardiendo) {
            return true;
        }
    }

    //Arriba derecha
    if (i > 0 && j < columnas - 1) {
        //logger.Information("Entrando a if Arriba-der [HasBurningNeighbour]");
        if (matrix[i - 1, j + 1] == Matrix.Ardiendo) {
            return true;
        }
    }

    //Izquierda
    if (j > 0) {
        //logger.Information("Entrando a if Izq [HasBurningNeighbour]");
        if (matrix[i, j - 1] == Matrix.Ardiendo) {
            return true;
        }
    }

    //Derecha
    if (j < columnas - 1) {
        //logger.Information("Entrando a if Der [HasBurningNeighbour]");
        if (matrix[i, j + 1] == Matrix.Ardiendo) {
            return true;
        }
    }

    //Abajo izquierda
    if (i < filas - 1 && j > 0) {
        //logger.Information("Entrando a if Abajo-izq [HasBurningNeighbour]");
        if (matrix[i + 1, j - 1] == Matrix.Ardiendo) {
            return true;
        }
    }

    //Abajo
    if (i < filas - 1) {
        //logger.Information("Entrando a if Abajo [HasBurningNeighbour]");
        if (matrix[i + 1, j] == Matrix.Ardiendo) {
            return true;
        }
    }

    //Abajo derecha
    if (i < filas - 1 && j < columnas - 1) {
        //logger.Information("Entrando a if Abajo-der [HasBurningNeighbour]");
        if (matrix[i + 1, j + 1] == Matrix.Ardiendo) {
            return true;
        }
    }

    //Si no se cumple ninguna devolvemos falso
    return false;
}
/*
 * Funcion principal que consiste en hacer que funcione el doble buffer traspasando datos del front al back
 * Maneja los casos con los arboles, fuego y celdas vacias
 */
void Step(Matrix[,] frontBuffer, Matrix[,] backBuffer, int filas, int columnas, ref int celdasQuemadas){
    logger.Information("Entrando a funcion Step()");
    for(int i = 0; i < filas; i++){
        for(int j = 0; j < columnas; j++){
            //logger.information("Entrando a for [Step]")
            //Maneja todos los casos posibles
            switch(frontBuffer[i, j]){
                //logger.information("Entrando a switch [Step]")
                //Si esta ardiendo en la siguiente escritura la elimina
                case Matrix.Ardiendo:
                    //logger.information("entrando a case Matrix-Ardiendo [Step]")
                    backBuffer[i, j] = Matrix.Vacio;
                    celdasQuemadas++;
                    break;
                
                //Si esta ardiendo se propaga (si hay casilla adyacente), si no, se queda el arbol
                case Matrix.Arbol:
                    //logger.information("entrando a case Matrix-Arbol [Step]")
                    if(HasBurningNeighbour(frontBuffer, i, j, filas, columnas)){
                        backBuffer[i, j] = Matrix.Ardiendo;
                    } else {
                        backBuffer[i, j] = Matrix.Arbol;
                    }
                    break;
                
                //Si la casilla esta vacia se escribe de nuevo una casilla vacia
                case Matrix.Vacio:
                    //logger.information("entrando a case Matrix-Vacio [Step]")
                    backBuffer[i, j] = Matrix.Vacio;
                    break;
            }
        }
    }
}

/*
 * Funcion que realiza un conteo de los datos al final de la simulacion
 */
void conteoFinal(Matrix[,] matrix, int filas, int columnas, ref int celdasArbolFinal, ref int celdasVaciasFinal){
    for(int i = 0; i < filas; i++){
        for(int j = 0; j < columnas; j++){
            switch(matrix[i, j]){
                case Matrix.Arbol :
                    celdasArbolFinal++;
                    break;
                case Matrix.Vacio :
                    celdasVaciasFinal++;
                    break;
            }
        }
    }
}

/*
 * Dejo comentados los loggers por que los he utilizado para tema de depuracion y algunos se han repetido significativamente a lo largo de la simulacion
 *
 * El swap es mas eficiente que la clonacion, ya que este en vez de crear un nuevo objeto tan solo referencia las posiciones de los datos en memoria
 * lo cual lo hace mas eficiente a la hora de calcular matrices de gran tamaño.
*/