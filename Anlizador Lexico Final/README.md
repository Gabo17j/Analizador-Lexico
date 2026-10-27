# Analizador Léxico

## Estructura

| Carpeta / proyecto | Contenido |
|---|---|
| `Analizador Lexico/Core` | Motor léxico sin dependencias de interfaz: `AnalizadorLexico`, `Token`, `TiposToken`, `PalabrasReservadas`, `TablaSimbolos`, `ElementoSimbolo`, `ManejadorErrores`, `ErrorLexico`, `ResultadoAnalisis`, `Reporte`, `Exportador` |
| `Analizador Lexico/Views` | `FrmPrincipal` (diseño en `FrmPrincipal.Designer.cs`, lógica en `FrmPrincipal.cs`), `ResaltadorSintaxis`, `Nativos` |
| `Analizador Lexico.Pruebas` | Programa de consola con 31 casos de prueba y los archivos de prueba en `Archivos/` |

El formulario solo llama a `AnalizadorLexico` y muestra su resultado; toda la lógica de análisis está en `Core`.

## Compilar y ejecutar

1. Abrir `Analizador Lexico.sln`.
2. Proyecto de inicio **Analizador Lexico** y pulsar `F5`.
3. Pruebas: proyecto de inicio **Analizador Lexico.Pruebas** y `Ctrl+F5`. Los resultados quedan en `bin\Debug\resultados_pruebas.txt`.

## Uso

1. **Abrir archivo** o escribir/arrastrar un `.txt` al editor.
2. **Analizar** `F5`: se llenan las pestañas *Tokens*, *Errores*, *Tabla de símbolos* y *Estadísticas*.
3. Doble clic en una fila de tokens, errores o símbolos para ir a esa posición en el editor.
4. **Exportar** `Ctrl+E`: reporte `.txt`, `.csv` (tokens, errores o símbolos) o `.html`.

Atajos: 
`F5` analizar 
`Ctrl+O` abrir
`Ctrl+E` exportar.
