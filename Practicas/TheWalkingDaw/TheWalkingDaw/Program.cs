using System;
using static System.Console;
using System.Text; //Iconos
using Serilog;
using TheWalkingDaw.Enums;
using TheWalkingDaw.Structs; //Logger

//Iconos
OutputEncoding = Encoding.UTF8;

//Constantes globales
const int tiempo = 0;

//Instancia del logger
Serilog.ILogger logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

Main(args);


void Main(string[] args){
    
    //Mostrar la configuracion
    CheckConfiguration();

    Matrix[,] frontBuffer = new Matrix[]
    
    //Mostramos la matriz
    PrintMatrix(frontBuffer);
}

/*
 * Funcion que busca, lee, confirma y muestra la configuracion
 */
void CheckConfiguration(){
    logger.Information("Entrando en CheckConfiguration");
    
    //Permitimos que configuracion acceda a los argumentos
    var configuracion = ProcesarArgumentos(args);
    
    /*
     * [A] = Alumno
     * [Z] = Zombie
     */
    WriteLine($"Tiempo: {configuracion.Tiempo}");
    WriteLine($"Dimension: {configuracion.Dimension}");
    WriteLine($"Infectados: {configuracion.Infectados}");
    WriteLine($"Probabilidad de matar [A]: {configuracion.Matar} %");
    WriteLine($"Probabilidad de muerte [Z]: {configuracion.Muerte} %");
    WriteLine($"Gente sana: {configuracion.Sanos}");
   
}

/*
 * Funcion que busca un valor en los argumentos y lo devuelve
 * Si no lo encuentra devuelve nulo
 */
string? BuscarValorArg(string[] args, string argumentoBuscado){
    var argumentoSeparado = argumentoBuscado.ToLower().Trim();

    foreach (var arg in args){
        var argPart = arg.Split(":");
        if(argPart.Length == 2){
            var actualPart =  argPart[0].ToLower().Trim();
            if(actualPart == argumentoSeparado){
                return argPart[1].Trim();
            }
        }
    }
    return null;
}

/*
 * Funcion que busca los argumentos y los intenta parsear
 */
Configuration ProcesarArgumentos(string[] args){
    logger.Information("Entrando a la funcion ProcesarArgumentos()");
    
    //Creamos una instancia de Configuration
    Configuration config = new Configuration();
    
    var dimension = BuscarValorArg(args, "dimension");
    if(dimension != null){
        if(int.TryParse(dimension, out int dimensionOut) && dimensionOut > 0){
            //Si no es nulo y se consigue parsear, lo almacenamos en el parametro de la configuracion
            config.Dimension =  dimensionOut; 
        }
        else{
            throw new ArgumentException("No existe un valor correcto para [dimension]");
        }
    }
    
    var infectados = BuscarValorArg(args, "infectado");
    if(infectados != null){
        if(int.TryParse(infectados, out int infectadoOut)){
            //Si no es nulo y se consigue parsear, lo almacenamos en el parametro de la configuracion
            config.Infectados =  infectadoOut;
        }
        else{
            throw new ArgumentException("No existe un valor correcto para [infectado]");
        }
    }

    var sanos = BuscarValorArg(args, "sanos");
    if(sanos != null){
        if(int.TryParse( sanos, out int sanoOut)){
            //Si no es nulo y se consigue parsear, lo almacenamos en el parametro de la configuracion
            config.Sanos = sanoOut;
        }
        else{
            throw new ArgumentException("No existe un valor correcto para [sano]");
        }
    }

    var tiempo = BuscarValorArg(args, "Tiempo");
    if(tiempo != null){
        if(int.TryParse(tiempo, out int tiempoOut)){
            //Si no es nulo y se consigue parsear, lo almacenamos en el parametro de la configuracion
            config.Tiempo = tiempoOut;
        }
        else{
            throw new ArgumentException("No existe un valor correcto para [tiempo]");
        }
    }

    var muerte = BuscarValorArg(args, "muerte");
    if(muerte != null){
        if(int.TryParse(muerte, out int muerteOut)){
            //Si no es nulo y se consigue parsear, lo almacenamos en el parametro de la configuracion
            config.Muerte = muerteOut;
        }
        else{
           throw new ArgumentException("No existe un valor correcto para [muerte]");
        }
    }
    
    var matar = BuscarValorArg(args, "matar");
    if(matar != null){
        if(int.TryParse(matar, out int matarOut)){
            //Si no es nulo y se consigue parsear, lo almacenamos en el parametro de la configuracion
            config.Matar = matarOut;
        }
        else{
            throw new ArgumentException("No existe un valor correcto para [matar]");
        }
    }
    
    return config;
}

void PrintMatrix(Matrix[,] matrix){
    
    var configuracion = ProcesarArgumentos(args);
    
    var filas = configuracion.Dimension;
    var columnas = configuracion.Dimension;

    for(int i = 0; i < filas; i++){
        for(int j = 0; j < columnas; j++){
            WriteLine("[]");
        }
    }
}







