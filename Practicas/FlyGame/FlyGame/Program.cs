using static System.Console; //Utilizo esta herramienta para evitar poner Console en los comandos de impresion
using FlyGame.Structs; //Importamos las structs
using System.Text; //Para los iconos

OutputEncoding = Encoding.UTF8;

//CONSTANTES GLOBALES
const int MaxSize = 5;
const int GameAttempts = 5;

//---ZONA DE FUNCIONES---

/*
 * Esta funcion devuelve un número entre 0 y el maximo del tamaño del tablero
 */

int AssignFlyPosition() {
    
    //Creamos una nueva instancia de la clase Random
    Random random = new Random();
    
    //Devolvemos un número random entre 0 y el maximo de tamaño del vector
    return random.Next(0, MaxSize);
}

/*
 * Esta funcion lanza una piedra a la posicion del array con la mision de dar a la mosca
 */
int ThrowRock() {
    
    //Utilizo el - 1 para que comience a pegar en un numero equivalente a la posicion de los indices
    int result = -1;
    
    //Variable bandera para decidir cuando se repite el bucle
    bool isOk = false;

    do {
        WriteLine($"Introduce el número de la casilla a la que lanzas la piedra (posición 1 a {MaxSize}):");
        string input = ReadLine();
        
        //Intenta realizar la conversion de string a entero
        if (int.TryParse(input, out result) && result >= 1 && result <= MaxSize) {
            isOk = true; 
            
        } else {
            WriteLine($"Entrada no válida. Por favor, introduce un número entero entre 1 y {MaxSize}.");
            isOk = false;
        }
        
    } while (!isOk); //Si el parseo a entero falla el bucle se repite
    
    return result - 1; //Utilizo el - 1 para que comience a pegar en un numero equivalente a la posicion de los indices
}

void PlayFlyGame(){
    
    //Creo la variable [posicion] para asignarsela al parametro del struct [Fly]
    Fly flyPosition;
    flyPosition.Position = AssignFlyPosition();

    //Creo la variable [dead] para asignarsela al parametro del struct [Fly]
    Fly dead;
    dead.Dead = false;

    Hit throwRock = new Hit();

    //Creamos la variable [sizeMap] para asignarsela a el parametro Size dentro del struct
    Configuration sizeMap;
    sizeMap.Size = new int[MaxSize]; //Tamaño inicial del vector
    sizeMap.Size[flyPosition.Position] = 1; //Añadimos la posicion de la mosca al vector
    sizeMap.Size[throwRock.HitFly] = 2; //Añadimos el lanzamiento de la piedra al vector

    //Creamos la variable intentos para asignarsela a el parametro attempts dentro del struct
    Configuration attempts;
    attempts.Attempts = GameAttempts;

    Hit throwState;
    
    //Asigno los 3 estados que puede tener el lanzamiento de la piedra
    throwState.Goal = "🎯 Has dado a la mosca! Enhorabuena 🎯"; //La mosca morirá y se acabará el programa
    throwState.Almost = "☣️ Casi das a la mosca, cambiando de posicion... ☣️"; //La mosca cambiará de posición
    throwState.Miss = " ❌ Has fallado, sigue intentandolo ❌"; //La cambiará de posición
    
    //Impresion de la configuración
    WriteLine("La configuración con la que se ejecutara el juego de la mosca es:");
    WriteLine($"Tamaño del vector : {sizeMap.Size.Length}");
    WriteLine($"Intentos del jugador para acertar : {attempts.Attempts}");
    
    do{
    
        //Llamada a la funcion ThrowRock
        throwRock.HitFly =  ThrowRock();
    
        /*
         * Este bucle for imprime los iconos para una mayor visibilidad en el programa, en el juego real la mosca
         * permaneceria oculta hasta que le demos con la piedra o se acaben los intentos
         */
        for (int i = 0; i < MaxSize; i++) {
            if(i == flyPosition.Position){
                Write("[🪰]"); //Imprime la posicion de la mosca
            } else if(i == throwRock.HitFly){
                Write("[🪨]"); //Imprime la posicion donde has lanzado la piedra
            }
            else{
                Write("[-]"); //Resto del vector
            }
        }

        /*
         * Try - Catch para evitar que el programa lance un error al introducir mal el numero de lanzamiento de la piedra
         */
        try {
            
            //Si lanzas la piedra donde se posa la mosca, has ganado!
            if (throwRock.HitFly == flyPosition.Position) {
                WriteLine(throwState.Goal);
                WriteLine($"Te quedaba {attempts.Attempts} intento");
                dead.Dead =  true;
            }
    
            //Si te has quedado a una casilla de diferencia de darle a la mosca, la mosca cambia de posicion
            else if (flyPosition.Position == throwRock.HitFly - 1 || flyPosition.Position == throwRock.HitFly + 1) {
                WriteLine(throwState.Almost);
                flyPosition.Position =  AssignFlyPosition();
                attempts.Attempts--;
                WriteLine($"Intentos restantes {attempts.Attempts}");
                
            //Si ninguna de las demas condiciones se cumple, se ha fallado el tiro
            } else {
                WriteLine(throwState.Miss);
                flyPosition.Position =  AssignFlyPosition();
                attempts.Attempts--;
                WriteLine($"Intentos restantes {attempts.Attempts}");
            }

            //Si los intentos llegan a 0 acaba el programa
            if (attempts.Attempts == 0){
                WriteLine("Te has quedado sin intentos");
            }
        }
    
        //Captura la excepcion
        catch (IndexOutOfRangeException e) {
            WriteLine("Introduce un numero valido");
            throw;
        }
    
    } while (attempts.Attempts != 0 && dead.Dead != true); //El bucle se repetira mientras la mosca este viva y queden intentos
}

//--FIN DE ZONA DE FUNCIONES

//---MAIN START---
PlayFlyGame();
//---MAIN END---