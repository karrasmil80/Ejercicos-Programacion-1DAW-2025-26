using System;
using static System.Console; //Para evitar escribir console en los comandos de impresion etc...
using FlyGame_Matrix.Structs; //Importamos la structs
using System.Text; //Iconos

OutputEncoding = Encoding.UTF8;

//CONSTANTES GLOBALES
const int RowSize = 2;
const int ColSize = 2;
const int Flylifes = 3;
const int MaxPlayerAttempts = 5;

/*
 * Procedimiento que imprime el vector, la posicion de la mosca y la piedra
 */
void PrintVectorAndHit(FlyState position, HitInfo hitInfo){
    
    for(int i = 0; i < RowSize; i++){
        for(int j = 0; j < ColSize; j++){
            // Comprobamos si es la mosca
            if(i == position.PositionRow && j == position.PositionCol){
                Write("[🪰 ]"); 
                
            // Comprobamos si es el golpe
            } else if(i == hitInfo.HitRow && j == hitInfo.HitCol){
                Write("[🪨 ]");
            } else {
                Write($"[ x ]");
            }
            
        }
        WriteLine();
    }
}

/*
 * Genera la posicion de la mosca en una fila de la matriz
 */
int GenerateFlyPositionRow(){
    Random random = new Random();
    
    FlyState position;
    position.PositionRow = random.Next(0, RowSize);
    
    return position.PositionRow ;
}

/*
 * Genera la posicion de la mosca en una columna de la matriz
 */
int GenerateFlyPositionCol(){
    Random random = new Random();
    
    FlyState position;
    position.PositionCol = random.Next(0, ColSize); //Asig
    
    return position.PositionCol ;
}

/*
 * Tira la piedra a una columna concreta
 */
int ThrowRockCol() {
    
    //Utilizo él - 1 para que comience a pegar en un numero equivalente a la posicion de los indices
    int result = -1;
    
    //Variable bandera para decidir cuando se repite el bucle
    bool isOk = false;

    do {
        WriteLine($"[COLUMNA] Introduce el número de la casilla a la que lanzas la piedra (posición 1 a {ColSize})");
        string input = ReadLine();
        
        //Intenta realizar la conversion de string a entero
        if (int.TryParse(input, out result) && result >= 1 && result <= ColSize) {
            isOk = true; 
            
        } else {
            WriteLine($"Entrada no válida. Por favor, introduce un número entero entre 1 y {ColSize}");
            isOk = false;
        }
        
    } while (!isOk); //Si el parseo a entero falla el bucle se repite
    
    return result - 1; //Utilizo él - 1 para que comience a pegar en un numero equivalente a la posicion de los indices
}

/*
 * Tira la piedra a una fila en concreto
 */
int ThrowRockRow() {
    
    //Utilizo él - 1 para que comience a pegar en un número equivalente a la posicion de los índices
    int result = -1;
    
    //Variable bandera para decidir cuando se repite el bucle
    bool isOk = false;

    do {
        WriteLine($"[FILA] Introduce el número de la casilla a la que lanzas la piedra (posición 1 a {RowSize})");
        string input = ReadLine();
        
        //Intenta realizar la conversion de string a entero
        if (int.TryParse(input, out result) && result >= 1 && result <= RowSize) {
            isOk = true; 
            
        } else {
            WriteLine($"Entrada no válida. Por favor, introduce un número entero entre 1 y {RowSize}.");
            isOk = false;
        }
        
    } while (!isOk); //Si el parseo a entero falla el bucle se repite
    
    return result - 1; //Utilizo el - 1 para que comience a pegar en un numero equivalente a la posicion de los indices
}

/*
 * Esta funcion quita un intento al jugador
 */
int TakeOffPlayerLife(int intentos){
    return intentos - 1;
}

/*
 * Esta funcion quita una vida a la mosca
 */
int takeOffFlylife(ref int flyLifes){
    return flyLifes - 1; 
}

/*
 * Esta funcion analiza el golpe de la piedra y quita intentos al jugador o vidas a la mosca segun el resultado
 */
void AnalizarGolpeo(FlyState position, HitInfo hitInfo, ref int attempts, ref int flyLifes){
    
    //Tipos de golpes predefinidos
    HitInfo hitType;
    hitType.Goal = "🎯 Has dado a la mosca! Enhorabuena 🎯";
    hitType.Almost = "☣️ Casi das a la mosca, cambiando de posicion... ☣️";
    hitType.Miss = " ❌ Has fallado, sigue intentandolo ❌";

    //Comprobamos las posiciones adyacentes en una casilla al lanzamiento
    if(position.PositionCol == hitInfo.HitCol && position.PositionRow == hitInfo.HitRow){
        WriteLine(hitType.Goal);
        flyLifes = takeOffFlylife(ref flyLifes);
        WriteLine($"A la mosca le quedan {flyLifes} vidas");


    } else if(position.PositionCol == hitInfo.HitCol && position.PositionRow == hitInfo.HitRow - 1){
        WriteLine(hitType.Almost);
        attempts = TakeOffPlayerLife(attempts);
        WriteLine($"Te quedan {attempts} intentos");
        
        
    } else if(position.PositionCol == hitInfo.HitCol + 1 && position.PositionRow == hitInfo.HitRow - 1){
        WriteLine(hitType.Almost);
        attempts = TakeOffPlayerLife(attempts);
        WriteLine($"Te quedan {attempts} intentos");
        
    } else if(position.PositionCol == hitInfo.HitCol + 1 && position.PositionRow == hitInfo.HitRow){
        WriteLine(hitType.Almost);
        attempts = TakeOffPlayerLife(attempts);
        WriteLine($"Te quedan {attempts} intentos");
        
    } else if(position.PositionCol == hitInfo.HitCol + 1 && position.PositionRow == hitInfo.HitRow + 1){
        WriteLine(hitType.Almost);
        attempts = TakeOffPlayerLife(attempts);
        WriteLine($"Te quedan {attempts} intentos");
        
    } else if(position.PositionCol == hitInfo.HitCol  && position.PositionRow == hitInfo.HitRow + 1 ){
        WriteLine(hitType.Almost);
        attempts = TakeOffPlayerLife(attempts);
        WriteLine($"Te quedan {attempts} intentos");
        
    } else if(position.PositionCol == hitInfo.HitCol - 1 && position.PositionRow == hitInfo.HitRow + 1){
        WriteLine(hitType.Almost);
        attempts = TakeOffPlayerLife(attempts);
        WriteLine($"Te quedan {attempts} intentos");
        
    } else if(position.PositionCol == hitInfo.HitCol - 1 && position.PositionRow == hitInfo.HitRow){
        WriteLine(hitType.Almost);
        attempts = TakeOffPlayerLife(attempts);
        WriteLine($"Te quedan {attempts} intentos");
        
    } else if(position.PositionCol == hitInfo.HitCol - 1 && position.PositionRow == hitInfo.HitRow - 1){
        WriteLine(hitType.Almost);
        attempts = TakeOffPlayerLife(attempts);
        WriteLine($"Te quedan {attempts} intentos");
        
    } else {
        WriteLine(hitType.Miss);
        attempts = TakeOffPlayerLife(attempts);
        WriteLine($"Te quedan {attempts} intentos");
    }
}

/*
 * Funcion principal que gestiona el flujo de procedimientos y funciones
 */
void PlayFlyGame(){

    //Vidas de el jugador
    int playerAttempts = MaxPlayerAttempts;
    
    //Vidas de la mosca
    int flylifes = Flylifes;
    
    //Bucle que acabara cuando los intentos del jugador o las vidas de la mosca lleguen a 0
    do {
        
        //Variables que llaman a las funciones para poner a la mosca en la matriz
        FlyState flyPosition = new FlyState();
        flyPosition.PositionCol = GenerateFlyPositionCol(); //La mosca se coloca en una columna
        flyPosition.PositionRow = GenerateFlyPositionRow(); //La mosca se coloca en una fila
    
        //Variables que llaman a lanzar roca
        HitInfo hitInfo = new HitInfo();
        hitInfo.HitRow = ThrowRockRow(); //Lanza la roca en el elemento fila
        hitInfo.HitCol = ThrowRockCol(); //Lanza la roca en el elemento columna

        //Imprime el vector con la posicion de la mosca y la piedra
        PrintVectorAndHit(flyPosition, hitInfo);
    
        //Analiza la situacion en el tablero con el lanzamiento de la piedra y la posicion de la mosca
        AnalizarGolpeo(flyPosition, hitInfo, ref playerAttempts, ref flylifes);

        //Si los intentos del jugador llegan a 0 se acaba el juego
        if(playerAttempts == 0){
            WriteLine("Te has quedado sin intentos, suerte la proxima vez");
        }

        //Si las vidas de la mosca llegan a 0 se acaba el juego
        if(flylifes == 0){
            WriteLine("La mosca se ha quedado sin vidas ¡Has ganado!");
        }
        
    } while(flylifes != 0 && playerAttempts != 0 );
}

//---------------
//INICIO DEL MAIN

//Llama a la funcion principal del juego
PlayFlyGame();

//FIN DEL MAIN
//---------------