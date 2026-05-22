using System;
using System.IO;

struct Libro
{
    public string Codigo;               // Formato: LIB00001 (8 caracteres)
    public string Titulo;
    public string Autor;
    public string Editorial;
    public int    AnioPublicacion;
    public string Categoria;
    public int    EjemplaresDisponibles;
}

struct Usuario
{
    public string Carne;                // 8 dígitos numéricos
    public string NombreCompleto;
    public string Carrera;
    public string CorreoElectronico;
    public string Telefono;
    public string Estado;               // "activo" o "inactivo"
}

struct Prestamo
{
    public int    Id;
    public string CarneUsuario;
    public string CodigoLibro;
    public string FechaPrestamo;        // Formato: dd/mm/yyyy
    public string FechaDevolucion;      // Formato: dd/mm/yyyy
    public string Estado;               // "activo" o "devuelto"
}

//Clase principal
class Program
{
    const int MAX_LIBROS    = 10;
    const int MAX_USUARIOS  = 5;
    const int MAX_PRESTAMOS = 10;

    static Libro[]    libros    = new Libro[MAX_LIBROS];
    static Usuario[]  usuarios  = new Usuario[MAX_USUARIOS];
    static Prestamo[] prestamos = new Prestamo[MAX_PRESTAMOS];

    static int totalLibros    = 0;
    static int totalUsuarios  = 0;
    static int totalPrestamos = 0;

    static string rutaData         = "Data/";
    static string archivoLibros    = "Data/libros.csv";
    static string archivoUsuarios  = "Data/usuarios.txt";
    static string archivoPrestamos = "Data/prestamos.txt";

    static void Main(string[] args)
    {
        if (!Directory.Exists(rutaData))
            Directory.CreateDirectory(rutaData);

        int opcion;
        do
        {
            MostrarMenuPrincipal();
            opcion = LeerEntero("Seleccione una opción: ");

            switch (opcion)
            {
                case 1:
                    MenuGestionLibros();
                    break;
                case 2:
                    Console.WriteLine("\n  [Módulo B - próximamente]\n");
                    Pausa();
                    break;
                case 3:
                    Console.WriteLine("\n  [Módulo C - próximamente]\n");
                    Pausa();
                    break;
                case 4:
                    Console.WriteLine("\n  ¡Hasta pronto!\n");
                    break;
                default:
                    Console.WriteLine("\n  [!] Opción inválida. Intente de nuevo.\n");
                    Pausa();
                    break;
            }

        } while (opcion != 4);
    }

    //menu principal
    static void MostrarMenuPrincipal()
    {
        Console.Clear();
        Console.WriteLine("╔══════════════════════════════════════════════╗");
        Console.WriteLine("║   SISTEMA DE GESTIÓN DE BIBLIOTECA UDB       ║");
        Console.WriteLine("╠══════════════════════════════════════════════╣");
        Console.WriteLine("║  1. Gestión de Libros                        ║");
        Console.WriteLine("║  2. Gestión de Usuarios                      ║");
        Console.WriteLine("║  3. Gestión de Préstamos                     ║");
        Console.WriteLine("║  4. Salir del Sistema                        ║");
        Console.WriteLine("╚══════════════════════════════════════════════╝");
    }

    //Modulo A-Gestión de libros
    static void MenuGestionLibros()
    {
        int opcion;
        do
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine("║         MÓDULO A – GESTIÓN DE LIBROS         ║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");
            Console.WriteLine("║  1. Registrar nuevo libro                    ║");
            Console.WriteLine("║  2. Buscar libro por código                  ║");
            Console.WriteLine("║  3. Listar todos los libros                  ║");
            Console.WriteLine("║  4. Eliminar un libro                        ║");
            Console.WriteLine("║  0. Volver al menú principal                 ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");

            opcion = LeerEntero("Seleccione una opción: ");

            switch (opcion)
            {
                case 1: RegistrarLibro(); break;
                case 2: BuscarLibro();    break;
                case 3: ListarLibros();   break;
                case 4: EliminarLibro();  break;
                case 0: break;
                default:
                    Console.WriteLine("\n  [!] Opción inválida.\n");
                    Pausa();
                    break;
            }
        } while (opcion != 0);
    }

    static void RegistrarLibro()
    {
        Console.Clear();
        Console.WriteLine("=== REGISTRAR NUEVO LIBRO ===\n");

        // Verificar capacidad máxima del arreglo 
        if (totalLibros >= MAX_LIBROS)
        {
            Console.WriteLine("[!] Se alcanzó el límite máximo de libros (" + MAX_LIBROS + ").");
            Pausa();
            return;
        }

        Libro l = new Libro();

        // Validar código: 8 caracteres alfanuméricos 
        do
        {
            Console.Write("Código (ej. LIB00001): ");
            l.Codigo = Console.ReadLine().Trim().ToUpper();

            if (!ValidarCodigoLibro(l.Codigo))
                Console.WriteLine("[!] Código inválido. Debe ser alfanumérico de exactamente 8 caracteres.\n");
            else if (ExisteCodigoLibro(l.Codigo))
                Console.WriteLine("[!] Ese código ya está registrado.\n");
            else
                break;
        } while (true);

        // Campos de texto obligatorios 
        l.Titulo    = LeerTextoObligatorio("Título: ");
        l.Autor     = LeerTextoObligatorio("Autor: ");
        l.Editorial = LeerTextoObligatorio("Editorial: ");

        // Validar año con if-else 
        do
        {
            l.AnioPublicacion = LeerEntero("Año de publicación: ");
            if (l.AnioPublicacion < 1900 || l.AnioPublicacion > DateTime.Now.Year)
                Console.WriteLine("[!] El año debe estar entre 1900 y " + DateTime.Now.Year + ".");
            else
                break;
        } while (true);

        l.Categoria = LeerTextoObligatorio("Categoría: ");

        // Validar ejemplares: no negativos 
        do
        {
            l.EjemplaresDisponibles = LeerEntero("Cantidad de ejemplares disponibles: ");
            if (l.EjemplaresDisponibles < 0)
                Console.WriteLine("[!] La cantidad no puede ser negativa.");
            else
                break;
        } while (true);

        libros[totalLibros] = l;
        totalLibros++;

        Console.WriteLine("\n[✓] Libro registrado exitosamente.");
        Pausa();
    }

    static void BuscarLibro()
    {
        Console.Clear();
        Console.WriteLine("=== BUSCAR LIBRO POR CÓDIGO ===\n");
        Console.Write("Ingrese el código del libro: ");
        string codigo = Console.ReadLine().Trim().ToUpper();

        int indice = -1;

        // Búsqueda lineal con for 
        for (int i = 0; i < totalLibros; i++)
        {
            if (libros[i].Codigo == codigo)
            {
                indice = i;
                break;
            }
        }

        if (indice == -1)
            Console.WriteLine("[!] No se encontró ningún libro con ese código.");
        else
        {
            Console.WriteLine("\n--- Datos del Libro ---");
            MostrarLibro(libros[indice]);
        }

        Pausa();
    }

    static void ListarLibros()
    {
        Console.Clear();
        Console.WriteLine("=== LISTADO DE LIBROS REGISTRADOS ===\n");

        if (totalLibros == 0)
        {
            Console.WriteLine("  No hay libros registrados.");
        }
        else
        {
            for (int i = 0; i < totalLibros; i++)
            {
                Console.WriteLine("--- Libro " + (i + 1) + " ---");
                MostrarLibro(libros[i]);
                Console.WriteLine();
            }
        }

        Pausa();
    }

    static void EliminarLibro()
    {
        Console.Clear();
        Console.WriteLine("=== ELIMINAR LIBRO ===\n");
        Console.Write("Ingrese el código del libro a eliminar: ");
        string codigo = Console.ReadLine().Trim().ToUpper();

        int indice = -1;
        for (int i = 0; i < totalLibros; i++)
        {
            if (libros[i].Codigo == codigo)
            {
                indice = i;
                break;
            }
        }

        if (indice == -1)
        {
            Console.WriteLine("[!] No se encontró ningún libro con ese código.");
        }
        else
        {
            // Verificar que no tenga préstamos activos antes de eliminar
            bool tienePrestamo = false;
            for (int i = 0; i < totalPrestamos; i++)
            {
                if (prestamos[i].CodigoLibro == codigo && prestamos[i].Estado == "activo")
                {
                    tienePrestamo = true;
                    break;
                }
            }

            if (tienePrestamo)
            {
                Console.WriteLine("[!] No se puede eliminar: el libro tiene préstamos activos.");
            }
            else
            {
                Console.WriteLine("\nLibro encontrado:");
                MostrarLibro(libros[indice]);
                Console.Write("\n¿Confirma la eliminación? (s/n): ");
                string confirmacion = Console.ReadLine().Trim().ToLower();

                if (confirmacion == "s")
                {
                    // Desplazar elementos con for (Guía 5)
                    for (int i = indice; i < totalLibros - 1; i++)
                        libros[i] = libros[i + 1];

                    libros[totalLibros - 1] = new Libro();
                    totalLibros--;
                    Console.WriteLine("[✓] Libro eliminado exitosamente.");
                }
                else
                {
                    Console.WriteLine("Operación cancelada.");
                }
            }
        }

        Pausa();
    }

    //validaciones

    // Valida que el código tenga exactamente 8 caracteres alfanuméricos
    static bool ValidarCodigoLibro(string codigo)
    {
        if (codigo.Length != 8) return false;
        for (int i = 0; i < codigo.Length; i++)
            if (!char.IsLetterOrDigit(codigo[i])) return false;
        return true;
    }

    // Verifica si ya existe un libro con ese código
    static bool ExisteCodigoLibro(string codigo)
    {
        for (int i = 0; i < totalLibros; i++)
            if (libros[i].Codigo == codigo) return true;
        return false;
    }

    // Retorna el índice del libro o -1 si no existe
    static int BuscarIndiceLibro(string codigo)
    {
        for (int i = 0; i < totalLibros; i++)
            if (libros[i].Codigo == codigo) return i;
        return -1;
    }

   //metodos basicos

    static void MostrarLibro(Libro l)
    {
        Console.WriteLine("  Código       : " + l.Codigo);
        Console.WriteLine("  Título       : " + l.Titulo);
        Console.WriteLine("  Autor        : " + l.Autor);
        Console.WriteLine("  Editorial    : " + l.Editorial);
        Console.WriteLine("  Año          : " + l.AnioPublicacion);
        Console.WriteLine("  Categoría    : " + l.Categoria);
        Console.WriteLine("  Disponibles  : " + l.EjemplaresDisponibles);
    }

    static void MostrarUsuario(Usuario u)
    {
        Console.WriteLine("  Carné        : " + u.Carne);
        Console.WriteLine("  Nombre       : " + u.NombreCompleto);
        Console.WriteLine("  Carrera      : " + u.Carrera);
        Console.WriteLine("  Correo       : " + u.CorreoElectronico);
        Console.WriteLine("  Teléfono     : " + u.Telefono);
        Console.WriteLine("  Estado       : " + u.Estado);
    }

    static void MostrarPrestamo(Prestamo p)
    {
        Console.WriteLine("  ID           : " + p.Id);
        Console.WriteLine("  Carné        : " + p.CarneUsuario);
        Console.WriteLine("  Código libro : " + p.CodigoLibro);
        Console.WriteLine("  Préstamo     : " + p.FechaPrestamo);
        Console.WriteLine("  Devolución   : " + p.FechaDevolucion);
        Console.WriteLine("  Estado       : " + p.Estado);
    }


    static int LeerEntero(string mensaje)
    {
        int valor;
        while (true)
        {
            if (!string.IsNullOrEmpty(mensaje))
                Console.Write(mensaje);
            try
            {
                valor = int.Parse(Console.ReadLine());
                return valor;
            }
            catch (FormatException)
            {
                Console.WriteLine("[!] Ingrese un número entero válido.");
            }
        }
    }

    static string LeerTextoObligatorio(string etiqueta)
    {
        string valor;
        do
        {
            Console.Write(etiqueta);
            valor = Console.ReadLine().Trim();
            if (string.IsNullOrEmpty(valor))
                Console.WriteLine("[!] Este campo es obligatorio.\n");
        } while (string.IsNullOrEmpty(valor));
        return valor;
    }

    static void Pausa()
    {
        Console.WriteLine("\nPresione cualquier tecla para continuar...");
        Console.ReadKey();
    }
}