# Juego de la Mosca (Fly Game)

Este es un juego de consola simple desarrollado en C# donde el objetivo del jugador es "cazar" una mosca escondida en un tablero virtual de tamaño fijo, con un número limitado de intentos.

# Cómo Jugar

El programa inicializa un tablero (vector) del numero de casillas que quieras. (ej. 10)

La mosca (🪰) se esconde aleatoriamente en una de estas casillas.

El jugador tiene 5 intentos para adivinar la posición de la mosca.

En cada turno, el jugador introduce el número de la casilla a la que lanza la piedra.

# Respuestas del Juego

🎯 Acierto (Goal): Si la piedra cae exactamente en la posición de la mosca, el jugador gana.

☣️ Casi (Almost): Si la piedra cae en la casilla adyacente (izquierda o derecha) a la mosca, la mosca se mueve a una nueva posición aleatoria.

❌ Fallo (Miss): Si la piedra cae lejos, la mosca se mueve a una nueva posición aleatoria.
