using System;
using System.IO;



struct Libro
{
    public string Codigo;
    public string Titulo;
    public string Autor;
    public string Editorial;
    public int    AnioPublicacion;
    public string Categoria;
    public int    EjemplaresDisponibles;
}

struct Usuario
{
    public string Carne;
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
                case 1: MenuGestionLibros();    break;
                case 2: MenuGestionUsuarios();  break;
                case 3: MenuGestionPrestamos(); break;
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

        if (totalLibros >= MAX_LIBROS)
        {
            Console.WriteLine("[!] Se alcanzó el límite máximo de libros (" + MAX_LIBROS + ").");
            Pausa();
            return;
        }

        Libro l = new Libro();

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

        l.Titulo    = LeerTextoObligatorio("Título: ");
        l.Autor     = LeerTextoObligatorio("Autor: ");
        l.Editorial = LeerTextoObligatorio("Editorial: ");

        do
        {
            l.AnioPublicacion = LeerEntero("Año de publicación: ");
            if (l.AnioPublicacion < 1900 || l.AnioPublicacion > DateTime.Now.Year)
                Console.WriteLine("[!] El año debe estar entre 1900 y " + DateTime.Now.Year + ".");
            else
                break;
        } while (true);

        l.Categoria = LeerTextoObligatorio("Categoría: ");

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
        for (int i = 0; i < totalLibros; i++)
            if (libros[i].Codigo == codigo) { indice = i; break; }

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
            Console.WriteLine("  No hay libros registrados.");
        else
            for (int i = 0; i < totalLibros; i++)
            {
                Console.WriteLine("--- Libro " + (i + 1) + " ---");
                MostrarLibro(libros[i]);
                Console.WriteLine();
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
            if (libros[i].Codigo == codigo) { indice = i; break; }

        if (indice == -1)
        {
            Console.WriteLine("[!] No se encontró ningún libro con ese código.");
        }
        else
        {
            bool tienePrestamo = false;
            for (int i = 0; i < totalPrestamos; i++)
                if (prestamos[i].CodigoLibro == codigo && prestamos[i].Estado == "activo")
                { tienePrestamo = true; break; }

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
                    for (int i = indice; i < totalLibros - 1; i++)
                        libros[i] = libros[i + 1];
                    libros[totalLibros - 1] = new Libro();
                    totalLibros--;
                    Console.WriteLine("[✓] Libro eliminado exitosamente.");
                }
                else
                    Console.WriteLine("Operación cancelada.");
            }
        }
        Pausa();
    }

    static void MenuGestionUsuarios()
    {
        int opcion;
        do
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine("║        MÓDULO B – GESTIÓN DE USUARIOS        ║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");
            Console.WriteLine("║  1. Registrar nuevo usuario                  ║");
            Console.WriteLine("║  2. Buscar usuario por carné                 ║");
            Console.WriteLine("║  3. Buscar usuario por nombre                ║");
            Console.WriteLine("║  4. Listar todos los usuarios                ║");
            Console.WriteLine("║  0. Volver al menú principal                 ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");

            opcion = LeerEntero("Seleccione una opción: ");

            switch (opcion)
            {
                case 1: RegistrarUsuario();       break;
                case 2: BuscarUsuarioPorCarne();  break;
                case 3: BuscarUsuarioPorNombre(); break;
                case 4: ListarUsuarios();         break;
                case 0: break;
                default:
                    Console.WriteLine("\n  [!] Opción inválida.\n");
                    Pausa();
                    break;
            }
        } while (opcion != 0);
    }

    static void RegistrarUsuario()
    {
        Console.Clear();
        Console.WriteLine("=== REGISTRAR NUEVO USUARIO ===\n");

        if (totalUsuarios >= MAX_USUARIOS)
        {
            Console.WriteLine("[!] Se alcanzó el límite máximo de usuarios (" + MAX_USUARIOS + ").");
            Pausa();
            return;
        }

        Usuario u = new Usuario();

        // Validar carné: exactamente 8 dígitos numéricos 

        do
        {
            Console.Write("Carné (8 dígitos): ");
            u.Carne = Console.ReadLine().Trim();
            if (!ValidarCarne(u.Carne))
                Console.WriteLine("[!] El carné debe tener exactamente 8 dígitos numéricos.\n");
            else if (ExisteCarneUsuario(u.Carne))
                Console.WriteLine("[!] Ese carné ya está registrado.\n");
            else
                break;
        } while (true);

        u.NombreCompleto = LeerTextoObligatorio("Nombre completo: ");
        u.Carrera        = LeerTextoObligatorio("Carrera: ");

                // Validar correo: debe contener '@' y un punto después 

        do
        {
            Console.Write("Correo electrónico: ");
            u.CorreoElectronico = Console.ReadLine().Trim();
            if (!ValidarCorreo(u.CorreoElectronico))
                Console.WriteLine("[!] Correo inválido. Debe contener '@' y un punto después.\n");
            else
                break;
        } while (true);

        u.Telefono = LeerTextoObligatorio("Teléfono: ");
        u.Estado   = "activo";

        usuarios[totalUsuarios] = u;
        totalUsuarios++;
        Console.WriteLine("\n[✓] Usuario registrado exitosamente.");
        Pausa();
    }

    static void BuscarUsuarioPorCarne()
    {
        Console.Clear();
        Console.WriteLine("=== BUSCAR USUARIO POR CARNÉ ===\n");
        Console.Write("Ingrese el carné: ");
        string carne = Console.ReadLine().Trim();

        int indice = -1;
        for (int i = 0; i < totalUsuarios; i++)
            if (usuarios[i].Carne == carne) { indice = i; break; }

        if (indice == -1)
            Console.WriteLine("[!] No se encontró usuario con ese carné.");
        else
        {
            Console.WriteLine("\n--- Datos del Usuario ---");
            MostrarUsuario(usuarios[indice]);
        }
        Pausa();
    }

    static void BuscarUsuarioPorNombre()
    {
        Console.Clear();
        Console.WriteLine("=== BUSCAR USUARIO POR NOMBRE ===\n");
        Console.Write("Ingrese el nombre (o parte de él): ");
        string nombre = Console.ReadLine().Trim().ToLower();

        bool encontrado = false;
        for (int i = 0; i < totalUsuarios; i++)
        {
            if (usuarios[i].NombreCompleto.ToLower().Contains(nombre))
            {
                if (!encontrado) { Console.WriteLine("\n--- Resultados ---"); encontrado = true; }
                MostrarUsuario(usuarios[i]);
                Console.WriteLine();
            }
        }

        if (!encontrado)
            Console.WriteLine("[!] No se encontraron usuarios con ese nombre.");
        Pausa();
    }

    static void ListarUsuarios()
    {
        Console.Clear();
        Console.WriteLine("=== LISTADO DE USUARIOS REGISTRADOS ===\n");

        if (totalUsuarios == 0)
            Console.WriteLine("  No hay usuarios registrados.");
        else
            for (int i = 0; i < totalUsuarios; i++)
            {
                Console.WriteLine("--- Usuario " + (i + 1) + " ---");
                MostrarUsuario(usuarios[i]);
                Console.WriteLine();
            }
        Pausa();
    }

    static void MenuGestionPrestamos()
    {
        int opcion;
        do
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine("║       MÓDULO C – GESTIÓN DE PRÉSTAMOS        ║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");
            Console.WriteLine("║  1. Registrar nuevo préstamo                 ║");
            Console.WriteLine("║  2. Registrar devolución                     ║");
            Console.WriteLine("║  3. Historial de préstamos activos           ║");
            Console.WriteLine("║  4. Actualizar estado de préstamo            ║");
            Console.WriteLine("║  5. Reporte general de préstamos             ║");
            Console.WriteLine("║  6. Exportar reporte a archivo .txt          ║");
            Console.WriteLine("║  0. Volver al menú principal                 ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");

            opcion = LeerEntero("Seleccione una opción: ");

            switch (opcion)
            {
                case 1: RegistrarPrestamo();         break;
                case 2: RegistrarDevolucion();       break;
                case 3: HistorialPrestamosActivos(); break;
                case 4: ActualizarEstadoPrestamo();  break;
                case 5: ReporteGeneralPrestamos();   break;
                case 6: ExportarReporte();           break;
                case 0: break;
                default:
                    Console.WriteLine("\n  [!] Opción inválida.\n");
                    Pausa();
                    break;
            }
        } while (opcion != 0);
    }

    static void RegistrarPrestamo()
    {
        Console.Clear();
        Console.WriteLine("=== REGISTRAR NUEVO PRÉSTAMO ===\n");

        if (totalPrestamos >= MAX_PRESTAMOS)
        {
            Console.WriteLine("[!] Se alcanzó el límite máximo de préstamos (" + MAX_PRESTAMOS + ").");
            Pausa();
            return;
        }

        Prestamo p = new Prestamo();

        Console.Write("Carné del usuario: ");
        p.CarneUsuario = Console.ReadLine().Trim();

        Console.Write("Código del libro: ");
        p.CodigoLibro = Console.ReadLine().Trim().ToUpper();

        Console.Write("Fecha de préstamo (dd/mm/yyyy): ");
        p.FechaPrestamo = Console.ReadLine().Trim();

        Console.Write("Fecha estimada de devolución (dd/mm/yyyy): ");
        p.FechaDevolucion = Console.ReadLine().Trim();

        p.Id     = totalPrestamos + 1;
        p.Estado = "activo";


        int indiceLibro = BuscarIndiceLibro(p.CodigoLibro);
        if (indiceLibro != -1)
            libros[indiceLibro].EjemplaresDisponibles--;

        prestamos[totalPrestamos] = p;
        totalPrestamos++;

        Console.WriteLine("\n[✓] Préstamo registrado. ID: " + p.Id);
        Pausa();
    }

    static void RegistrarDevolucion()
    {
        Console.Clear();
        Console.WriteLine("=== REGISTRAR DEVOLUCIÓN ===\n");
        int id = LeerEntero("Ingrese el ID del préstamo: ");

        int indice = -1;
        for (int i = 0; i < totalPrestamos; i++)
            if (prestamos[i].Id == id) { indice = i; break; }

        if (indice == -1)
        {
            Console.WriteLine("[!] No se encontró préstamo con ese ID.");
        }
        else if (prestamos[indice].Estado == "devuelto")
        {
            Console.WriteLine("[!] Ese préstamo ya fue devuelto.");
        }
        else
        {
            Console.WriteLine("\nPréstamo encontrado:");
            MostrarPrestamo(prestamos[indice]);
            Console.Write("\n¿Confirmar devolución? (s/n): ");
            string conf = Console.ReadLine().Trim().ToLower();

            if (conf == "s")
            {
                prestamos[indice].Estado = "devuelto";

                int idxLibro = BuscarIndiceLibro(prestamos[indice].CodigoLibro);
                if (idxLibro != -1)
                    libros[idxLibro].EjemplaresDisponibles++;

                Console.WriteLine("[✓] Devolución registrada. Inventario actualizado.");
            }
            else
                Console.WriteLine("Operación cancelada.");
        }
        Pausa();
    }

    static void HistorialPrestamosActivos()
    {
        Console.Clear();
        Console.WriteLine("=== HISTORIAL DE PRÉSTAMOS ACTIVOS ===\n");
        Console.Write("Ingrese el carné del usuario: ");
        string carne = Console.ReadLine().Trim();

        bool encontrado = false;
        for (int i = 0; i < totalPrestamos; i++)
        {
            if (prestamos[i].CarneUsuario == carne && prestamos[i].Estado == "activo")
            {
                if (!encontrado)
                {
                    Console.WriteLine("Préstamos activos para carné " + carne + ":\n");
                    encontrado = true;
                }
                MostrarPrestamo(prestamos[i]);
                Console.WriteLine();
            }
        }

        if (!encontrado)
            Console.WriteLine("[!] No se encontraron préstamos activos para ese carné.");
        Pausa();
    }

    static void ActualizarEstadoPrestamo()
    {
        Console.Clear();
        Console.WriteLine("=== ACTUALIZAR ESTADO DE PRÉSTAMO ===\n");
        int id = LeerEntero("ID del préstamo: ");

        int indice = -1;
        for (int i = 0; i < totalPrestamos; i++)
            if (prestamos[i].Id == id) { indice = i; break; }

        if (indice == -1)
        {
            Console.WriteLine("[!] No se encontró préstamo con ese ID.");
        }
        else
        {
            Console.WriteLine("Estado actual: " + prestamos[indice].Estado);
            Console.Write("Nuevo estado (activo/devuelto): ");
            string nuevoEstado = Console.ReadLine().Trim().ToLower();

            if (nuevoEstado == "activo" || nuevoEstado == "devuelto")
            {
                prestamos[indice].Estado = nuevoEstado;
                Console.WriteLine("[✓] Estado actualizado.");
            }
            else
                Console.WriteLine("[!] Estado inválido. Use 'activo' o 'devuelto'.");
        }
        Pausa();
    }

    static void ReporteGeneralPrestamos()
    {
        Console.Clear();
        Console.WriteLine("=== REPORTE GENERAL DE PRÉSTAMOS ===\n");


        int[,] resumen = new int[MAX_LIBROS, 2];

        int activos   = 0;
        int devueltos = 0;

        for (int i = 0; i < totalPrestamos; i++)
        {
            if (prestamos[i].Estado == "activo")
                activos++;
            else
                devueltos++;

            int idxL = BuscarIndiceLibro(prestamos[i].CodigoLibro);
            if (idxL != -1)
            {
                if (prestamos[i].Estado == "activo")
                    resumen[idxL, 0]++;
                else
                    resumen[idxL, 1]++;
            }
        }

        Console.WriteLine("Total préstamos registrados : " + totalPrestamos);
        Console.WriteLine("Préstamos activos           : " + activos);
        Console.WriteLine("Préstamos devueltos         : " + devueltos);

        Console.WriteLine("\n--- Resumen por libro ---");
        Console.WriteLine("{0,-10} {1,-30} {2,-10} {3,-10}", "Código", "Título", "Activos", "Devueltos");
        Console.WriteLine(new string('-', 65));

        for (int i = 0; i < totalLibros; i++)
        {
            string titulo = libros[i].Titulo.Length > 28
                ? libros[i].Titulo.Substring(0, 28) + ".."
                : libros[i].Titulo;

            Console.WriteLine("{0,-10} {1,-30} {2,-10} {3,-10}",
                libros[i].Codigo, titulo, resumen[i, 0], resumen[i, 1]);
        }

        Pausa();
    }

    static void ExportarReporte()
    {
        Console.Clear();
        Console.WriteLine("=== EXPORTAR REPORTE A ARCHIVO ===\n");

        string nombreArchivo = "Data/reporte_prestamos.txt";


        using (StreamWriter sw = new StreamWriter(nombreArchivo, false))
        {
            sw.WriteLine("REPORTE DE PRÉSTAMOS – BIBLIOTECA UDB");
            sw.WriteLine("Fecha de exportación: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            sw.WriteLine(new string('=', 60));

            for (int i = 0; i < totalPrestamos; i++)
            {
                sw.WriteLine("ID          : " + prestamos[i].Id);
                sw.WriteLine("Carné       : " + prestamos[i].CarneUsuario);
                sw.WriteLine("Código libro: " + prestamos[i].CodigoLibro);
                sw.WriteLine("Préstamo    : " + prestamos[i].FechaPrestamo);
                sw.WriteLine("Devolución  : " + prestamos[i].FechaDevolucion);
                sw.WriteLine("Estado      : " + prestamos[i].Estado);
                sw.WriteLine(new string('-', 40));
            }

            sw.WriteLine("Total registros: " + totalPrestamos);
        }

        Console.WriteLine("[✓] Reporte exportado a: " + nombreArchivo);
        Pausa();
    }

       //validaciones

     // Código de libro: 8 caracteres alfanuméricos

    static bool ValidarCodigoLibro(string codigo)
    {
        if (codigo.Length != 8) return false;
        for (int i = 0; i < codigo.Length; i++)
            if (!char.IsLetterOrDigit(codigo[i])) return false;
        return true;
    }

    // Carné: exactamente 8 dígitos numéricos
    static bool ValidarCarne(string carne)
    {
        if (carne.Length != 8) return false;
        for (int i = 0; i < carne.Length; i++)
            if (!char.IsDigit(carne[i])) return false;
        return true;
    }

    // Correo: debe contener '@' y un punto después de el
    static bool ValidarCorreo(string correo)
    {
        int posArroba = correo.IndexOf('@');
        if (posArroba < 0) return false;
        int posPunto = correo.IndexOf('.', posArroba);
        return posPunto > posArroba;
    }

    static bool ExisteCodigoLibro(string codigo)
    {
        for (int i = 0; i < totalLibros; i++)
            if (libros[i].Codigo == codigo) return true;
        return false;
    }

    static bool ExisteCarneUsuario(string carne)
    {
        for (int i = 0; i < totalUsuarios; i++)
            if (usuarios[i].Carne == carne) return true;
        return false;
    }

    static int BuscarIndiceLibro(string codigo)
    {
        for (int i = 0; i < totalLibros; i++)
            if (libros[i].Codigo == codigo) return i;
        return -1;
    }

    static int BuscarIndiceUsuario(string carne)
    {
        for (int i = 0; i < totalUsuarios; i++)
            if (usuarios[i].Carne == carne) return i;
        return -1;
    }


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
