# Sistema Integral de Gestión de Biblioteca Universitaria

Este proyecto es una aplicación de consola desarrollada en C# orientada a la administración de una biblioteca universitaria. El sistema permite gestionar un inventario de libros, un registro de estudiantes (usuarios) y controlar los préstamos y devoluciones de manera eficiente, manteniendo la persistencia de los datos mediante archivos de texto.

## Integrantes del Equipo

* **Arianna Valeria García Mozo** - GM260245
* **Diego Antonio Escobar Lopez** - EL260135

## Características Principales

El sistema está dividido en tres módulos funcionales principales:

1. **Módulo de Gestión de Libros**: Registro de nuevos títulos, búsqueda por código (8 caracteres), listado de inventario y eliminación de libros (validando que no posean préstamos activos).
2. **Módulo de Gestión de Usuarios**: Registro de estudiantes con validación estricta de carné (8 dígitos numéricos) y correo electrónico, búsquedas dinámicas y listado general.
3. **Módulo de Gestión de Préstamos**: Control de préstamos y devoluciones, actualización de stock de ejemplares en tiempo real e historial por usuario.
4. **Reporte Estadístico**: Generación de un reporte basado en el uso de una matriz bidimensional (arreglos) que detalla los ejemplares en préstamo activo frente a los devueltos.
5. **Persistencia de Datos**: Almacenamiento local automatizado en formato `.txt` (Usuarios y Préstamos) y `.csv` (Libros).

## Estructura del Repositorio

El proyecto cuenta con dos versiones para contrastar el desarrollo algorítmico, cumpliendo la rúbrica de evaluación:

* **Versión 1 (Desarrollo Manual)**: Ubicada en la raíz del repositorio (`programa.cs`). Desarrollo estructurado manual paso a paso con manejo de `structs`, arreglos estáticos y validaciones rigurosas. Guarda los datos en la carpeta `Data/`.
* **Versión 2 (Generada por Inteligencia Artificial)**: Ubicada en la carpeta `version_AI/`. Contiene el prompt utilizado (`prompt_utilizado.txt`) y el código generado (`codigo_generado.cs`), el cual incorpora manejo de UI por consola. Guarda los datos de forma aislada en la carpeta `Data_AI/`.

## Instrucciones de Compilación y Ejecución

Asegúrate de tener instalado el SDK de **.NET (versión 6.0 o superior, probado en .NET 10.0)**.

### Para ejecutar la Versión 1 (Manual)
1. Abre tu terminal y ubícate en la raíz del repositorio.
2. Compila el proyecto ejecutando:
   ```bash
   dotnet build
   ```
3. Ejecuta la aplicación con:
   ```bash
   dotnet run
   ```

### Para ejecutar la Versión 2 (Inteligencia Artificial)
1. Desde la raíz del repositorio, navega a la carpeta correspondiente:
   ```bash
   cd version_AI
   ```
2. Compila el proyecto ejecutando:
   ```bash
   dotnet build
   ```
3. Ejecuta la aplicación con:
   ```bash
   dotnet run
   ```

*(Nota: La versión de la IA configura automáticamente tu terminal a UTF-8 para renderizar correctamente la interfaz en caso de caracteres especiales).*
