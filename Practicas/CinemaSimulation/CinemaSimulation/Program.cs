using System;
using static System.Console; //Evitar escribir .Console para comandos
using System.Text;
using CinemaSimulation.Enums;
using CinemaSimulation.Structs; //ICONOS
using Serilog; //Logger

//ICONOS
OutputEncoding = Encoding.UTF8;

//Instancia del logger
Serilog.ILogger logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

//Variables globales
Random randon = new Random();

Main(args);

void Main(string[] args){
    logger.Information("INICIANDO PROGRAMA!");
    
    //Variables
    var precioEntrada = 6.50;
    var entradasVendidas = 0;
    var recaudacionTotal = (entradasVendidas * precioEntrada);
    var entradasDevueltas = 0;
    int asientosLibres = 0;
    int asientosOcupados = 0;
    int asientosFueraServicio = 0;
    double porcentaje = 0;
    
    
    //Llamada a las dimesiones 
    Configuracion configuracion = new Configuracion();

    configuracion = ProcesarArgumentos(args);
    
    int filas = configuracion.Filas;
    int columnas = configuracion.Columnas;
    
    //Creamos la matriz
    Matrix[,] matrix = new Matrix[filas, columnas];
    
    //Inicializamos el estado de la matriz antes de entrar al bucle para guardar las posiciones
    InitMatrix(matrix, filas, columnas);
    
    bool repetir = false;
    
    do {
        
        //Menu
        PrintMenu();
        WriteLine();
    
        //Lectura de la entrada de datos del usuario
        string input = ReadLine();
        int readInput = int.TryParse(input, out readInput) ? readInput : 0;
        WriteLine();

        //Manejamos los casos
        switch(readInput){
            //Ver sala
            case 1:
                PrintMatrix(matrix, filas, columnas);
                WriteLine("Pulsa cualquier tecla para volver");
                ReadKey();
                WriteLine();
                repetir = true;
                break;
            
            //Comprar entrada
            case 2: 
                ProcesarButaca(matrix, ref entradasVendidas, ref recaudacionTotal);
                WriteLine("Pulsa cualquier tecla para volver");
                ReadKey();
                WriteLine();
                repetir = true;
                break;
            
            //Vender entrada
            case 3:
                DevolverButaca(matrix, precioEntrada,  ref entradasVendidas, ref entradasDevueltas, ref recaudacionTotal);
                WriteLine("Pulsa cualquier tecla para volver");
                ReadKey();
                WriteLine();
                repetir = true;
                break;
            
            //Recaudacion
            case 4:
                WriteLine($"Total recaudado : {recaudacionTotal} ");
                //WriteLine($"Entradas vendidas : {entradasVendidas}");
                //WriteLine($"Entradas devueltas : {entradasDevueltas}");
                ReadKey();
                WriteLine();
                repetir = true;
                break;
            
            //Informe
            case 5:
                ConteoFinal(matrix, filas, columnas, ref asientosLibres, ref entradasVendidas, ref asientosFueraServicio, ref asientosOcupados, ref recaudacionTotal, ref porcentaje);
                GenerarInforme(entradasVendidas, asientosLibres, asientosFueraServicio, recaudacionTotal, porcentaje, asientosOcupados);
                ReadKey();
                WriteLine();
                repetir = true;
                break;
            
            //Salir de la aplicacion
            case 6:
                WriteLine("Hasta la proxima!");
                repetir = false;
                break;
        }
    } while(repetir);
}

void GenerarInforme(int entradasVendidas, int asientosLibres, int asientosFueraServicio, double recaudacionTotal, double porcentajeOcupacion, int asientosOcupados) {
    logger.Information("Entrando a GenerarInforme()");
    
    WriteLine($"Entradas vendidas : {entradasVendidas}");
    WriteLine($"Asientos libres : {asientosLibres}");
    WriteLine($"Asientos ocupados : {asientosOcupados}");
    WriteLine($"Asientos no disponibles : {asientosFueraServicio}");
    WriteLine($"Total recaudado : {recaudacionTotal} €");
    WriteLine($"Porcentaje de ocupación : {porcentajeOcupacion}%");
}


void ConteoFinal(Matrix[,] matrix, int filas, int columnas, ref int asientosLibres,ref int entradasVendidas, ref int asientosFueraServicio, ref int asientosOcupados, ref double recaudacionTotal, ref double porcentaje) {
    logger.Information("Entrando a ConteoFinal()");
    
    //Reinicio para limpiar los datos (si no se duplican)
    asientosLibres = 0;
    asientosOcupados = 0;
    asientosFueraServicio = 0;
    
    for(int i = 0; i < filas; i++){
        for(int j = 0; j < columnas; j++){
            switch(matrix[i,j]){
                case Matrix.Libre:
                    asientosLibres++;
                    break;

                case Matrix.Ocupada:
                    asientosOcupados++;
                    break;

                case Matrix.FueraDeServicio:
                    asientosFueraServicio++;
                    break;
            }
        }
    }
    
    int totalAsientos = filas * columnas;
    int asientosDisponibles = totalAsientos - asientosFueraServicio;

    recaudacionTotal = entradasVendidas * 6.50;

    porcentaje = (double)entradasVendidas / asientosDisponibles * 100;
}

/*
 * Busca un valor en los argumentos y los separa en partes para poder procesarlo
 */
string? BuscarValor(string[] args, string encontrado){
    logger.Information("Entrando a buscar valor");
    var argProcesamiento = encontrado.ToLower().Trim();

    foreach (var arg in args){
        var argPart = arg.Split(":");
        if(argPart.Length == 2){
            var actualPart = argPart[0].ToLower().Trim();
            if(actualPart == argProcesamiento){
                return argPart[1].Trim();
            }
        }
    }
    return null;
}

/*
 * Procesa el valor de los argumentos
 */
Configuracion ProcesarArgumentos(string[] args){
    logger.Information("Entrando a ProcesarArgumentos()");

    Configuracion configuracion = new Configuracion();

    var filas = BuscarValor(args, "filas");

    if(filas != null){
        if(int.TryParse(filas, out int filasOut) && filasOut > 0){
            configuracion.Filas = filasOut;
        } else {
            WriteLine("Introduce argumentos validos para fila");
        }
    }

    var columnas = BuscarValor(args, "columnas");
    if(columnas != null){
        if(int.TryParse(columnas, out int columnasOut) && columnasOut > 0){
            configuracion.Columnas =  columnasOut;
        } else {
            WriteLine("Introduce argumentos validos para columnas");
        }
    }
    
    return configuracion;
}

/*
 * Imprimimos el estado de la matriz
 */
void PrintMatrix(Matrix[,] matrix, int filas, int columnas){
    logger.Information("Entrando a PrintMatrix()");
    for(int i = 0; i < filas; i++){
        for(int j = 0; j < columnas; j++){
            switch(matrix[i, j]){
                
                //Imprime este icono cuando la butaca esta libre
                case Matrix.Libre : 
                    Write("[🟢]");
                    break;
                
                //Imprime este icono cuando la butaca esta ocupada
                case Matrix.Ocupada :
                    Write("[🔴]");
                    break;
                
                //Imprime este icono cuando la butaca esta fuera de servicio 
                case Matrix.FueraDeServicio :
                    Write("[🚫]");
                    break;
            }
        }
        WriteLine();
    }
}

/*
 * Inicializamos la matriz con 1 a 3 butacas fuera de servicio por defecto
 */
void InitMatrix(Matrix[,] matrix, int filas, int columnas){
    logger.Information("Entrando a InitMatrix()");
    
    //Número de butacas a generar
    var numFueraServicio = randon.Next(1, 3);

    //Coloca las butacas fuera de servicio
    for(int i = 0; i < numFueraServicio; i++){
        var colocarFila = randon.Next(1, filas); //Posicion de la butaca [Fila]
        var colocarColumna = randon.Next(1, columnas); //Posicion de la butaca [Columna]
        
        matrix[colocarFila, colocarColumna] = Matrix.FueraDeServicio;
    }
    
}

/*
 * Imprime el menu
 */
void PrintMenu(){
    //Menu
    WriteLine("1. Ver sala");
    WriteLine("2. Comprar");
    WriteLine("3. Devolver");
    WriteLine("4. Recaudacion");
    WriteLine("5. Informe");
    WriteLine("6. Salir");
}

/*
 * Gestiona la compra de entradas
 */
void ProcesarButaca(Matrix[,] matrix, ref int entradasVendidas, ref double recaudacionTotal){
    logger.Information("Entrando a ProcesarButaca()");
    
    WriteLine("Introduzca la butaca que quiere comprar [FORMATO LETRA:NUMERO]");
    
    string input = ReadLine();
    var argLeer = input.ToUpper().Trim().Split(':'); //Dividimos el argumento en dos con el split
    
    //Variable bandera
    bool repetir = false;

    double entradaCosto = 6.50;
    var entradaTotal = entradaCosto;

    int fila;
    int columna;

    //Bucle do while para repetir si el formato se introduce mal
    do{
        
        //Si el argumento se ha dividido, tiene dos partes
        if(argLeer.Length == 2){
            
            fila = argLeer[0].ToLower()[0] switch{
                'a' => 0,
                'b' => 1,
                'c' => 2,
                'd' => 3,
                _ => -1
            };

            //Si no se introduce una letra valida, se repite el bucle
            if(fila == -1){
                WriteLine("Introduzca un formato valido LETRA:NUMERO");
                repetir = true;
            }
        
            if (!int.TryParse(argLeer[1], out columna)) {
                WriteLine("Columna inválida.");
                repetir = true;
            }


            columna = columna - 1;  //Para el indice 0
            
            if(matrix[fila, columna] == Matrix.FueraDeServicio || matrix[fila, columna] == Matrix.Ocupada){
                WriteLine("La butaca que has elegido se encuentra ocupada o fuera de servicio, por favor escoja otra");
                repetir = true;
            } else{
                WriteLine($"Butaca comprada con exito en: [Fila: {argLeer[0]}, NUMERO: {columna}]");
                matrix[fila, columna] = Matrix.Ocupada ;
                entradasVendidas++;
                recaudacionTotal += entradaCosto;

            }
        }
    } while(repetir);
    
}

/*
 * Gestiona la devolucion de entradas
 */
void DevolverButaca(Matrix[,] matrix, double valorEntrada, ref int entradasVendidas, ref int entradasDevueltas, ref double recaudacionTotal) {
    logger.Information("Entrando a DevolverButaca()");
    
    //Variable bandera
    bool repetir;

    //Bucle do while para repetir si el formato se introduce mal
    do {
        
        repetir = false;

        WriteLine("Introduzca la butaca que quiere devolver [FORMATO LETRA:NUMERO]");
        string input = ReadLine();
        var argLeer = input.ToUpper().Trim().Split(':');

        //Si el argumento se ha dividido, tiene dos partes
        if (argLeer.Length != 2) {
            WriteLine("Formato incorrecto. Use LETRA:NUMERO");
            repetir = true;
        }

        //Convertimos la letra introducida en un numero para identificarlo con la fila
        int fila = argLeer[0].ToLower()[0] switch {
            'a' => 0,
            'b' => 1,
            'c' => 2,
            'd' => 3,
            _ => -1
        };

        //Si no se introduce una letra valida, se repite el bucle
        if (fila == -1) {
            WriteLine("Fila inválida.");
            repetir = true;
        }

        if (!int.TryParse(argLeer[1], out int columna)) {
            WriteLine("Columna inválida.");
            repetir = true;
        }

        columna--; //Para el indice 0

        // Validar límites
        if (fila < 0 || fila >= matrix.GetLength(0) || columna < 0 || columna >= matrix.GetLength(1)){
            WriteLine("La butaca indicada no existe.");
            repetir = true;
        }

        // Validar estado de la butaca
        if (matrix[fila, columna] == Matrix.Libre || matrix[fila, columna] == Matrix.FueraDeServicio) {
            WriteLine("La butaca está libre o fuera de servicio. No se puede devolver.");
            repetir = true;
        } else {
            WriteLine($"Entrada devuelta con éxito en [Fila: {argLeer[0]} | Número: {columna + 1}]");

            matrix[fila, columna] = Matrix.Libre;

            entradasVendidas--;
            entradasDevueltas++;
            recaudacionTotal -= valorEntrada;
        }

    } while (repetir);
}

