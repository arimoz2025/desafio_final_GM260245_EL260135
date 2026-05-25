// =========================================================================================
// 🤖 Código Generado por Claude (Anthropic)
// 💡 Módulo: Sistema Integral de Gestión de Biblioteca Universitaria
// 🛠️ Arquitectura: Arreglos estáticos, Estructuras (Structs), Persistencia en TXT/CSV
// ✅ Versión corregida — cumple todos los requisitos del Desafío Final Ciclo I-2026
// =========================================================================================

using System;
using System.IO;

namespace BibliotecaUniversitariaAI
{
    // ====================================================================
    // 📦 DEFINICIÓN DE ESTRUCTURAS DE DATOS
    // ====================================================================
    public struct Libro
    {
        public string Codigo;
        public string Titulo;
        public string Autor;
        public string Editorial;
        public int    AnioPublicacion;
        public string Categoria;
        public int    EjemplaresDisponibles;
    }

    public struct Usuario
    {
        public string Carne;
        public string NombreCompleto;
        public string Carrera;
        public string CorreoElectronico;
        public string Telefono;
        public string Estado;               // "Activo" o "Inactivo"
    }

    public struct Prestamo
    {
        public int    Id;
        public string CarneUsuario;
        public string CodigoLibro;
        public string FechaPrestamo;        // Formato: dd/mm/yyyy
        public string FechaDevolucion;      // Formato: dd/mm/yyyy
        public string Estado;               // "Activo" o "Devuelto"
    }

    class Program
    {
        // ====================================================================
        // ⚙️ CONSTANTES Y MEMORIA ESTÁTICA
        // Límites según sección 4.2 del desafío: 10 libros, 5 usuarios, 10 préstamos
        // ====================================================================
        const int MAX_LIBROS    = 10;
        const int MAX_USUARIOS  = 5;
        const int MAX_PRESTAMOS = 10;

        static Libro[]    dbLibros    = new Libro[MAX_LIBROS];
        static Usuario[]  dbUsuarios  = new Usuario[MAX_USUARIOS];
        static Prestamo[] dbPrestamos = new Prestamo[MAX_PRESTAMOS];

        static int totalLibros    = 0;
        static int totalUsuarios  = 0;
        static int totalPrestamos = 0;

        // 📁 Rutas de Archivos — carpeta Data_AI/ según sección 4.4
        const string CARPETA_DATA      = "Data_AI/";
        const string RUTA_LIBROS       = "Data_AI/libros.csv";
        const string RUTA_USUARIOS     = "Data_AI/usuarios.txt";
        const string RUTA_PRESTAMOS    = "Data_AI/prestamos.txt";
        const string RUTA_REPORTE      = "Data_AI/reporte_prestamos.txt";

        // ====================================================================
        // 🚀 PUNTO DE ENTRADA PRINCIPAL
        // ====================================================================
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Title = "🤖 Claude LibManager Pro v2.0";

            if (!Directory.Exists(CARPETA_DATA))
                Directory.CreateDirectory(CARPETA_DATA);

            InicializarSistema();

            int opcion;
            do
            {
                MostrarEncabezado("🏛️  SISTEMA DE BIBLIOTECA UNIVERSITARIA  🏛️");
                Console.WriteLine(" 1️⃣  📚 Gestión de Libros");
                Console.WriteLine(" 2️⃣  👥 Gestión de Usuarios");
                Console.WriteLine(" 3️⃣  🔁 Gestión de Préstamos");
                Console.WriteLine(" 4️⃣  💾 Guardar Datos y Salir");
                Console.WriteLine("==================================================");

                opcion = LeerEnteroSeguro("👉 Seleccione una opción: ");

                switch (opcion)
                {
                    case 1: MenuLibros();    break;
                    case 2: MenuUsuarios();  break;
                    case 3: MenuPrestamos(); break;
                    case 4:
                        GuardarSistema();
                        ImprimirExito("👋 ¡Gracias por usar el sistema! Hasta pronto.");
                        break;
                    default:
                        ImprimirError("❌ Opción inválida. Intente nuevamente.");
                        break;
                }
            } while (opcion != 4);
        }

        // ====================================================================
        // 📚 MÓDULO A: GESTIÓN DE LIBROS
        // ====================================================================
        static void MenuLibros()
        {
            int op;
            do
            {
                MostrarEncabezado("📚 MÓDULO DE LIBROS");
                Console.WriteLine(" 1. ➕ Registrar nuevo libro");
                Console.WriteLine(" 2. 🔍 Buscar libro por código");
                Console.WriteLine(" 3. 📋 Listar inventario de libros");
                Console.WriteLine(" 4. 🗑️  Eliminar libro del sistema");
                Console.WriteLine(" 0. 🔙 Regresar al menú principal");

                op = LeerEnteroSeguro("\n👉 Opción: ");
                switch (op)
                {
                    case 1: RegistrarLibro(); break;
                    case 2: BuscarLibro();    break;
                    case 3: ListarLibros();   break;
                    case 4: EliminarLibro();  break;
                    case 0: break;
                    default:
                        ImprimirError("❌ Opción inválida.");
                        break;
                }
            } while (op != 0);
        }

        static void RegistrarLibro()
        {
            MostrarEncabezado("➕ REGISTRO DE LIBRO");
            if (totalLibros >= MAX_LIBROS)
            {
                ImprimirError("⚠️ Capacidad máxima de libros alcanzada (" + MAX_LIBROS + ").");
                return;
            }

            Libro l = new Libro();

            // Validación: código alfanumérico de exactamente 8 caracteres
            do
            {
                l.Codigo = LeerCadenaSegura("🔖 Código (ej. LIB00001): ").ToUpper();
                if (!ValidarCodigoLibro(l.Codigo))
                    ImprimirError("❌ Código inválido. Debe ser alfanumérico de exactamente 8 caracteres.");
                else if (BuscarIndiceLibro(l.Codigo) != -1)
                    ImprimirError("❌ Este código de libro ya está registrado.");
                else
                    break;
            } while (true);

            l.Titulo    = LeerCadenaSegura("📖 Título: ");
            l.Autor     = LeerCadenaSegura("✍️  Autor: ");
            l.Editorial = LeerCadenaSegura("🏢 Editorial: ");

            // Validación: año entre 1900 y año actual
            do
            {
                l.AnioPublicacion = LeerEnteroSeguro("📅 Año de publicación: ");
                if (l.AnioPublicacion < 1900 || l.AnioPublicacion > DateTime.Now.Year)
                    ImprimirError("❌ El año debe estar entre 1900 y " + DateTime.Now.Year + ".");
                else
                    break;
            } while (true);

            l.Categoria = LeerCadenaSegura("🏷️  Categoría: ");

            // Validación: ejemplares no negativos
            do
            {
                l.EjemplaresDisponibles = LeerEnteroSeguro("📦 Ejemplares disponibles: ");
                if (l.EjemplaresDisponibles < 0)
                    ImprimirError("❌ La cantidad no puede ser negativa.");
                else
                    break;
            } while (true);

            dbLibros[totalLibros++] = l;
            ImprimirExito("✅ Libro registrado con éxito.");
        }

        static void BuscarLibro()
        {
            MostrarEncabezado("🔍 BÚSQUEDA DE LIBRO");
            string cod = LeerCadenaSegura("🔖 Ingrese el código a buscar: ").ToUpper();

            int idx = BuscarIndiceLibro(cod);
            if (idx != -1)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n📖 " + dbLibros[idx].Titulo.ToUpper());
                Console.ResetColor();
                Console.WriteLine("✍️  Autor     : " + dbLibros[idx].Autor);
                Console.WriteLine("🏢 Editorial  : " + dbLibros[idx].Editorial + " (" + dbLibros[idx].AnioPublicacion + ")");
                Console.WriteLine("🏷️  Categoría  : " + dbLibros[idx].Categoria);
                Console.WriteLine("📦 Disponibles: " + dbLibros[idx].EjemplaresDisponibles);
                Pausar();
            }
            else
            {
                ImprimirError("❌ Libro no encontrado en el sistema.");
            }
        }

        static void ListarLibros()
        {
            MostrarEncabezado("📋 INVENTARIO GLOBAL DE LIBROS");
            if (totalLibros == 0)
            {
                ImprimirError("⚠️ El inventario está vacío.");
                return;
            }

            Console.WriteLine(string.Format("{0,-10} | {1,-30} | {2,-20} | {3,-10}", "CÓDIGO", "TÍTULO", "AUTOR", "STOCK"));
            Console.WriteLine(new string('-', 78));

            for (int i = 0; i < totalLibros; i++)
            {
                string t = dbLibros[i].Titulo.Length > 27 ? dbLibros[i].Titulo.Substring(0, 27) + "..." : dbLibros[i].Titulo;
                string a = dbLibros[i].Autor.Length  > 17 ? dbLibros[i].Autor.Substring(0, 17)  + "..." : dbLibros[i].Autor;

                Console.WriteLine(string.Format("{0,-10} | {1,-30} | {2,-20} | {3,-10}",
                    dbLibros[i].Codigo, t, a, dbLibros[i].EjemplaresDisponibles));
            }
            Pausar();
        }

        static void EliminarLibro()
        {
            MostrarEncabezado("🗑️  ELIMINACIÓN DE LIBRO");
            string cod = LeerCadenaSegura("🔖 Ingrese código del libro a eliminar: ").ToUpper();

            int idx = BuscarIndiceLibro(cod);
            if (idx == -1)
            {
                ImprimirError("❌ Libro no encontrado.");
                return;
            }

            // Verificar que no tenga préstamos activos
            for (int p = 0; p < totalPrestamos; p++)
            {
                if (dbPrestamos[p].CodigoLibro == cod && dbPrestamos[p].Estado == "Activo")
                {
                    ImprimirError("❌ No se puede eliminar: el libro tiene préstamos activos.");
                    return;
                }
            }

            Console.WriteLine("\nLibro encontrado:");
            MostrarLibro(dbLibros[idx]);
            Console.Write("\n¿Confirmar eliminación? (s/n): ");
            string conf = Console.ReadLine().Trim().ToLower();

            if (conf == "s")
            {
                for (int i = idx; i < totalLibros - 1; i++)
                    dbLibros[i] = dbLibros[i + 1];
                dbLibros[totalLibros - 1] = new Libro();
                totalLibros--;
                ImprimirExito("✅ Libro eliminado permanentemente del sistema.");
            }
            else
            {
                Console.WriteLine("Operación cancelada.");
                Pausar();
            }
        }

        // ====================================================================
        // 👥 MÓDULO B: GESTIÓN DE USUARIOS
        // ====================================================================
        static void MenuUsuarios()
        {
            int op;
            do
            {
                MostrarEncabezado("👥 MÓDULO DE USUARIOS");
                Console.WriteLine(" 1. ➕ Registrar usuario");
                Console.WriteLine(" 2. 🔍 Buscar usuario por carné");
                Console.WriteLine(" 3. 🔎 Buscar usuario por nombre");
                Console.WriteLine(" 4. 📋 Listar todos los usuarios");
                Console.WriteLine(" 0. 🔙 Regresar al menú principal");

                op = LeerEnteroSeguro("\n👉 Opción: ");
                switch (op)
                {
                    case 1: RegistrarUsuario();       break;
                    case 2: BuscarUsuarioPorCarne();  break;
                    case 3: BuscarUsuarioPorNombre(); break;
                    case 4: ListarUsuarios();         break;
                    case 0: break;
                    default:
                        ImprimirError("❌ Opción inválida.");
                        break;
                }
            } while (op != 0);
        }

        static void RegistrarUsuario()
        {
            MostrarEncabezado("➕ REGISTRO DE USUARIO");
            if (totalUsuarios >= MAX_USUARIOS)
            {
                ImprimirError("⚠️ Límite de usuarios alcanzado (" + MAX_USUARIOS + ").");
                return;
            }

            Usuario u = new Usuario();

            // Validación: carné de exactamente 8 dígitos numéricos
            do
            {
                u.Carne = LeerCadenaSegura("🪪 Carné Universitario (8 dígitos numéricos): ");
                if (!ValidarCarne(u.Carne))
                    ImprimirError("❌ El carné debe tener exactamente 8 dígitos numéricos.");
                else if (BuscarIndiceUsuario(u.Carne) != -1)
                    ImprimirError("❌ Ese carné ya está registrado.");
                else
                    break;
            } while (true);

            u.NombreCompleto = LeerCadenaSegura("👤 Nombre Completo: ");
            u.Carrera        = LeerCadenaSegura("🎓 Carrera Académica: ");

            // Validación: correo con '@' y punto posterior
            do
            {
                u.CorreoElectronico = LeerCadenaSegura("📧 Correo Electrónico: ");
                if (!ValidarCorreo(u.CorreoElectronico))
                    ImprimirError("❌ El correo debe contener '@' y un punto después del mismo.");
                else
                    break;
            } while (true);

            u.Telefono = LeerCadenaSegura("📱 Teléfono: ");
            u.Estado   = "Activo";

            dbUsuarios[totalUsuarios++] = u;
            ImprimirExito("✅ Usuario registrado exitosamente.");
        }

        static void BuscarUsuarioPorCarne()
        {
            MostrarEncabezado("🔍 BÚSQUEDA DE USUARIO POR CARNÉ");
            string carne = LeerCadenaSegura("🪪 Ingrese el carné: ");

            int idx = BuscarIndiceUsuario(carne);
            if (idx != -1)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n👤 " + dbUsuarios[idx].NombreCompleto);
                Console.ResetColor();
                MostrarUsuario(dbUsuarios[idx]);
                Pausar();
            }
            else
            {
                ImprimirError("❌ Usuario no encontrado en el padrón.");
            }
        }

        static void BuscarUsuarioPorNombre()
        {
            MostrarEncabezado("🔎 BÚSQUEDA DE USUARIO POR NOMBRE");
            string nombre = LeerCadenaSegura("👤 Ingrese el nombre (o parte de él): ").ToLower();

            bool encontrado = false;
            for (int i = 0; i < totalUsuarios; i++)
            {
                if (dbUsuarios[i].NombreCompleto.ToLower().Contains(nombre))
                {
                    if (!encontrado)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("\n--- Resultados encontrados ---");
                        Console.ResetColor();
                        encontrado = true;
                    }
                    MostrarUsuario(dbUsuarios[i]);
                    Console.WriteLine();
                }
            }

            if (!encontrado)
                ImprimirError("❌ No se encontraron usuarios con ese nombre.");
            else
                Pausar();
        }

        static void ListarUsuarios()
        {
            MostrarEncabezado("📋 LISTADO DE USUARIOS REGISTRADOS");
            if (totalUsuarios == 0)
            {
                ImprimirError("⚠️ No hay usuarios registrados.");
                return;
            }

            for (int i = 0; i < totalUsuarios; i++)
            {
                Console.WriteLine("--- Usuario " + (i + 1) + " ---");
                MostrarUsuario(dbUsuarios[i]);
                Console.WriteLine();
            }
            Pausar();
        }

        // ====================================================================
        // 🔁 MÓDULO C: GESTIÓN DE PRÉSTAMOS
        // ====================================================================
        static void MenuPrestamos()
        {
            int op;
            do
            {
                MostrarEncabezado("🔁 MÓDULO DE PRÉSTAMOS");
                Console.WriteLine(" 1. 📤 Registrar nuevo préstamo");
                Console.WriteLine(" 2. 📥 Registrar devolución");
                Console.WriteLine(" 3. 📜 Historial de préstamos activos por usuario");
                Console.WriteLine(" 4. 🔄 Actualizar estado de préstamo");
                Console.WriteLine(" 5. 📈 Reporte estadístico (uso de matriz)");
                Console.WriteLine(" 6. 💾 Exportar reporte a archivo .txt");
                Console.WriteLine(" 0. 🔙 Regresar al menú principal");

                op = LeerEnteroSeguro("\n👉 Opción: ");
                switch (op)
                {
                    case 1: RegistrarPrestamo();         break;
                    case 2: ProcesarDevolucion();        break;
                    case 3: HistorialPrestamosActivos(); break;
                    case 4: ActualizarEstadoPrestamo();  break;
                    case 5: GenerarReporteEstadistico(); break;
                    case 6: ExportarReporte();           break;
                    case 0: break;
                    default:
                        ImprimirError("❌ Opción inválida.");
                        break;
                }
            } while (op != 0);
        }

        static void RegistrarPrestamo()
        {
            MostrarEncabezado("📤 REGISTRAR PRÉSTAMO");
            if (totalPrestamos >= MAX_PRESTAMOS)
            {
                ImprimirError("⚠️ Base de préstamos llena (" + MAX_PRESTAMOS + ").");
                return;
            }

            Prestamo p = new Prestamo();
            p.Id = totalPrestamos + 1;

            // Validar que el usuario exista y esté activo
            do
            {
                p.CarneUsuario = LeerCadenaSegura("🪪 Carné del usuario: ");
                int idxU = BuscarIndiceUsuario(p.CarneUsuario);
                if (idxU == -1)
                    ImprimirError("❌ No existe usuario con ese carné.");
                else if (dbUsuarios[idxU].Estado != "Activo")
                    ImprimirError("❌ El usuario está inactivo.");
                else
                    break;
            } while (true);

            // Validar que el libro exista y tenga disponibilidad
            do
            {
                p.CodigoLibro = LeerCadenaSegura("🔖 Código del libro: ").ToUpper();
                int idxL = BuscarIndiceLibro(p.CodigoLibro);
                if (idxL == -1)
                    ImprimirError("❌ El libro no existe en el catálogo.");
                else if (dbLibros[idxL].EjemplaresDisponibles <= 0)
                    ImprimirError("❌ No hay ejemplares disponibles para préstamo.");
                else
                    break;
            } while (true);

            // Validar formato de fecha de préstamo
            do
            {
                p.FechaPrestamo = LeerCadenaSegura("📅 Fecha de préstamo (dd/mm/yyyy): ");
                if (!ValidarFecha(p.FechaPrestamo))
                    ImprimirError("❌ Formato inválido. Use dd/mm/yyyy.");
                else
                    break;
            } while (true);

            // Validar formato de fecha de devolución
            do
            {
                p.FechaDevolucion = LeerCadenaSegura("📅 Fecha estimada de devolución (dd/mm/yyyy): ");
                if (!ValidarFecha(p.FechaDevolucion))
                    ImprimirError("❌ Formato inválido. Use dd/mm/yyyy.");
                else
                    break;
            } while (true);

            p.Estado = "Activo";

            int indiceLibro = BuscarIndiceLibro(p.CodigoLibro);
            dbLibros[indiceLibro].EjemplaresDisponibles--;
            dbPrestamos[totalPrestamos++] = p;

            ImprimirExito("✅ Préstamo procesado con éxito. ID de transacción: #" + p.Id);
        }

        static void ProcesarDevolucion()
        {
            MostrarEncabezado("📥 REGISTRO DE DEVOLUCIÓN");
            int id = LeerEnteroSeguro("🔢 ID del préstamo a devolver: ");

            int indice = -1;
            for (int i = 0; i < totalPrestamos; i++)
                if (dbPrestamos[i].Id == id) { indice = i; break; }

            if (indice == -1)
            {
                ImprimirError("❌ ID de préstamo no encontrado.");
                return;
            }

            if (dbPrestamos[indice].Estado == "Devuelto")
            {
                ImprimirError("⚠️ Este préstamo ya fue devuelto previamente.");
                return;
            }

            Console.WriteLine("\nPréstamo encontrado:");
            MostrarPrestamo(dbPrestamos[indice]);
            Console.Write("\n¿Confirmar devolución? (s/n): ");
            string conf = Console.ReadLine().Trim().ToLower();

            if (conf == "s")
            {
                dbPrestamos[indice].Estado = "Devuelto";
                int idxLibro = BuscarIndiceLibro(dbPrestamos[indice].CodigoLibro);
                if (idxLibro != -1)
                    dbLibros[idxLibro].EjemplaresDisponibles++;
                ImprimirExito("✅ Libro devuelto. Inventario restaurado correctamente.");
            }
            else
            {
                Console.WriteLine("Operación cancelada.");
                Pausar();
            }
        }

        static void HistorialPrestamosActivos()
        {
            MostrarEncabezado("📜 HISTORIAL DE PRÉSTAMOS ACTIVOS");
            string carne = LeerCadenaSegura("🪪 Ingrese el carné del usuario: ");

            bool encontrado = false;
            for (int i = 0; i < totalPrestamos; i++)
            {
                if (dbPrestamos[i].CarneUsuario == carne && dbPrestamos[i].Estado == "Activo")
                {
                    if (!encontrado)
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("\nPréstamos activos para carné " + carne + ":\n");
                        Console.ResetColor();
                        encontrado = true;
                    }
                    MostrarPrestamo(dbPrestamos[i]);
                    Console.WriteLine();
                }
            }

            if (!encontrado)
                ImprimirError("❌ No se encontraron préstamos activos para ese carné.");
            else
                Pausar();
        }

        static void ActualizarEstadoPrestamo()
        {
            MostrarEncabezado("🔄 ACTUALIZAR ESTADO DE PRÉSTAMO");
            int id = LeerEnteroSeguro("🔢 ID del préstamo: ");

            int indice = -1;
            for (int i = 0; i < totalPrestamos; i++)
                if (dbPrestamos[i].Id == id) { indice = i; break; }

            if (indice == -1)
            {
                ImprimirError("❌ No se encontró préstamo con ese ID.");
                return;
            }

            Console.WriteLine("Estado actual: " + dbPrestamos[indice].Estado);
            Console.Write("Nuevo estado (Activo/Devuelto): ");
            string nuevoEstado = Console.ReadLine().Trim();

            if (nuevoEstado.ToLower() == "activo")
            {
                dbPrestamos[indice].Estado = "Activo";
                ImprimirExito("✅ Estado actualizado a 'Activo'.");
            }
            else if (nuevoEstado.ToLower() == "devuelto")
            {
                dbPrestamos[indice].Estado = "Devuelto";
                ImprimirExito("✅ Estado actualizado a 'Devuelto'.");
            }
            else
            {
                ImprimirError("❌ Estado inválido. Use 'Activo' o 'Devuelto'.");
            }
        }

        static void GenerarReporteEstadistico()
        {
            MostrarEncabezado("📈 REPORTE ESTADÍSTICO (uso de matriz 2D)");

            // Matriz[Libro, Estado]: columna 0 = Activos, columna 1 = Devueltos
            int[,] estadisticas = new int[MAX_LIBROS, 2];
            int totalActivos = 0, totalDevueltos = 0;

            for (int i = 0; i < totalPrestamos; i++)
            {
                int idxLibro = BuscarIndiceLibro(dbPrestamos[i].CodigoLibro);
                if (idxLibro != -1)
                {
                    if (dbPrestamos[i].Estado == "Activo")
                    {
                        estadisticas[idxLibro, 0]++;
                        totalActivos++;
                    }
                    else
                    {
                        estadisticas[idxLibro, 1]++;
                        totalDevueltos++;
                    }
                }
            }

            Console.WriteLine("Total préstamos  : " + totalPrestamos);
            Console.WriteLine("Activos          : " + totalActivos);
            Console.WriteLine("Devueltos        : " + totalDevueltos);
            Console.WriteLine();
            Console.WriteLine(string.Format("{0,-10} {1,-28} {2,-10} {3,-10}", "CÓDIGO", "TÍTULO", "ACTIVOS", "DEVUELTOS"));
            Console.WriteLine(new string('-', 62));

            for (int i = 0; i < totalLibros; i++)
            {
                string t = dbLibros[i].Titulo.Length > 25
                    ? dbLibros[i].Titulo.Substring(0, 25) + "..." : dbLibros[i].Titulo;
                Console.WriteLine(string.Format("{0,-10} {1,-28} {2,-10} {3,-10}",
                    dbLibros[i].Codigo, t, estadisticas[i, 0], estadisticas[i, 1]));
            }
            Pausar();
        }

        static void ExportarReporte()
        {
            MostrarEncabezado("💾 EXPORTAR REPORTE A ARCHIVO .TXT");

            using (StreamWriter sw = new StreamWriter(RUTA_REPORTE, false))
            {
                sw.WriteLine("REPORTE DE PRÉSTAMOS — BIBLIOTECA UNIVERSITARIA");
                sw.WriteLine("Fecha de exportación: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                sw.WriteLine(new string('=', 60));

                for (int i = 0; i < totalPrestamos; i++)
                {
                    sw.WriteLine("ID           : " + dbPrestamos[i].Id);
                    sw.WriteLine("Carné        : " + dbPrestamos[i].CarneUsuario);
                    sw.WriteLine("Código libro : " + dbPrestamos[i].CodigoLibro);
                    sw.WriteLine("Préstamo     : " + dbPrestamos[i].FechaPrestamo);
                    sw.WriteLine("Devolución   : " + dbPrestamos[i].FechaDevolucion);
                    sw.WriteLine("Estado       : " + dbPrestamos[i].Estado);
                    sw.WriteLine(new string('-', 40));
                }
                sw.WriteLine("Total registros: " + totalPrestamos);
            }

            ImprimirExito("✅ Reporte exportado a: " + RUTA_REPORTE);
        }

        // ====================================================================
        // 💾 PERSISTENCIA DE DATOS (ARCHIVOS TXT / CSV)
        // ====================================================================
        static void InicializarSistema()
        {
            Console.WriteLine("⏳ Cargando datos del sistema...");
            try
            {
                // Cargar Libros (CSV)
                if (File.Exists(RUTA_LIBROS))
                {
                    using (StreamReader sr = new StreamReader(RUTA_LIBROS))
                    {
                        string linea;
                        while ((linea = sr.ReadLine()) != null && totalLibros < MAX_LIBROS)
                        {
                            if (string.IsNullOrWhiteSpace(linea)) continue;
                            string[] d = linea.Split(',');
                            if (d.Length == 7)
                            {
                                dbLibros[totalLibros].Codigo                = d[0].Trim();
                                dbLibros[totalLibros].Titulo                = d[1].Trim();
                                dbLibros[totalLibros].Autor                 = d[2].Trim();
                                dbLibros[totalLibros].Editorial             = d[3].Trim();
                                dbLibros[totalLibros].AnioPublicacion       = int.Parse(d[4].Trim());
                                dbLibros[totalLibros].Categoria             = d[5].Trim();
                                dbLibros[totalLibros].EjemplaresDisponibles = int.Parse(d[6].Trim());
                                totalLibros++;
                            }
                        }
                    }
                }

                // Cargar Usuarios (TXT)
                if (File.Exists(RUTA_USUARIOS))
                {
                    using (StreamReader sr = new StreamReader(RUTA_USUARIOS))
                    {
                        string linea;
                        while ((linea = sr.ReadLine()) != null && totalUsuarios < MAX_USUARIOS)
                        {
                            if (string.IsNullOrWhiteSpace(linea)) continue;
                            string[] d = linea.Split('|');
                            if (d.Length == 6)
                            {
                                dbUsuarios[totalUsuarios].Carne             = d[0].Trim();
                                dbUsuarios[totalUsuarios].NombreCompleto    = d[1].Trim();
                                dbUsuarios[totalUsuarios].Carrera           = d[2].Trim();
                                dbUsuarios[totalUsuarios].CorreoElectronico = d[3].Trim();
                                dbUsuarios[totalUsuarios].Telefono          = d[4].Trim();
                                dbUsuarios[totalUsuarios].Estado            = d[5].Trim();
                                totalUsuarios++;
                            }
                        }
                    }
                }

                // Cargar Préstamos (TXT)
                if (File.Exists(RUTA_PRESTAMOS))
                {
                    using (StreamReader sr = new StreamReader(RUTA_PRESTAMOS))
                    {
                        string linea;
                        while ((linea = sr.ReadLine()) != null && totalPrestamos < MAX_PRESTAMOS)
                        {
                            if (string.IsNullOrWhiteSpace(linea)) continue;
                            string[] d = linea.Split('|');
                            if (d.Length == 6)
                            {
                                dbPrestamos[totalPrestamos].Id              = int.Parse(d[0].Trim());
                                dbPrestamos[totalPrestamos].CarneUsuario    = d[1].Trim();
                                dbPrestamos[totalPrestamos].CodigoLibro     = d[2].Trim();
                                dbPrestamos[totalPrestamos].FechaPrestamo   = d[3].Trim();
                                dbPrestamos[totalPrestamos].FechaDevolucion = d[4].Trim();
                                dbPrestamos[totalPrestamos].Estado          = d[5].Trim();
                                totalPrestamos++;
                            }
                        }
                    }
                }

                Console.WriteLine("✅ Sistema inicializado. Presione una tecla para continuar...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                ImprimirError("❌ Error al cargar datos: " + ex.Message);
            }
        }

        static void GuardarSistema()
        {
            try
            {
                // Guardar Libros (CSV)
                using (StreamWriter sw = new StreamWriter(RUTA_LIBROS, false))
                {
                    for (int i = 0; i < totalLibros; i++)
                        sw.WriteLine(
                            dbLibros[i].Codigo + "," +
                            dbLibros[i].Titulo + "," +
                            dbLibros[i].Autor  + "," +
                            dbLibros[i].Editorial + "," +
                            dbLibros[i].AnioPublicacion + "," +
                            dbLibros[i].Categoria + "," +
                            dbLibros[i].EjemplaresDisponibles);
                }

                // Guardar Usuarios (TXT)
                using (StreamWriter sw = new StreamWriter(RUTA_USUARIOS, false))
                {
                    for (int i = 0; i < totalUsuarios; i++)
                        sw.WriteLine(
                            dbUsuarios[i].Carne + "|" +
                            dbUsuarios[i].NombreCompleto + "|" +
                            dbUsuarios[i].Carrera + "|" +
                            dbUsuarios[i].CorreoElectronico + "|" +
                            dbUsuarios[i].Telefono + "|" +
                            dbUsuarios[i].Estado);
                }

                // Guardar Préstamos (TXT)
                using (StreamWriter sw = new StreamWriter(RUTA_PRESTAMOS, false))
                {
                    for (int i = 0; i < totalPrestamos; i++)
                        sw.WriteLine(
                            dbPrestamos[i].Id + "|" +
                            dbPrestamos[i].CarneUsuario + "|" +
                            dbPrestamos[i].CodigoLibro + "|" +
                            dbPrestamos[i].FechaPrestamo + "|" +
                            dbPrestamos[i].FechaDevolucion + "|" +
                            dbPrestamos[i].Estado);
                }
            }
            catch (Exception ex)
            {
                ImprimirError("❌ Error crítico al guardar datos: " + ex.Message);
            }
        }

        // ====================================================================
        // ✅ VALIDACIONES
        // ====================================================================

        // Código libro: 8 caracteres alfanuméricos
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

        // Correo: contiene '@' y un punto posterior al mismo
        static bool ValidarCorreo(string correo)
        {
            int posArroba = correo.IndexOf('@');
            if (posArroba < 0) return false;
            int posPunto = correo.IndexOf('.', posArroba);
            return posPunto > posArroba;
        }

        // Fecha: formato dd/mm/yyyy validado por longitud y separadores
        static bool ValidarFecha(string fecha)
        {
            if (fecha.Length != 10) return false;
            if (fecha[2] != '/' || fecha[5] != '/') return false;
            string dd   = fecha.Substring(0, 2);
            string mm   = fecha.Substring(3, 2);
            string yyyy = fecha.Substring(6, 4);
            foreach (char c in dd)   if (!char.IsDigit(c)) return false;
            foreach (char c in mm)   if (!char.IsDigit(c)) return false;
            foreach (char c in yyyy) if (!char.IsDigit(c)) return false;
            return true;
        }

        // ====================================================================
        // 🛠️ MÉTODOS DE APOYO Y UTILIDADES
        // ====================================================================
        static int BuscarIndiceLibro(string codigo)
        {
            for (int i = 0; i < totalLibros; i++)
                if (dbLibros[i].Codigo == codigo) return i;
            return -1;
        }

        static int BuscarIndiceUsuario(string carne)
        {
            for (int i = 0; i < totalUsuarios; i++)
                if (dbUsuarios[i].Carne == carne) return i;
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

        static int LeerEnteroSeguro(string mensaje)
        {
            int numero = 0;
            bool valido = false;
            while (!valido)
            {
                Console.Write(mensaje);
                try
                {
                    numero = int.Parse(Console.ReadLine());
                    valido = true;
                }
                catch (FormatException)
                {
                    ImprimirError("⚠️ Formato incorrecto. Ingrese un valor numérico entero.");
                }
            }
            return numero;
        }

        static string LeerCadenaSegura(string mensaje)
        {
            string input = "";
            while (string.IsNullOrWhiteSpace(input))
            {
                Console.Write(mensaje);
                input = Console.ReadLine()?.Trim();
                if (string.IsNullOrWhiteSpace(input))
                    ImprimirError("⚠️ El campo no puede estar vacío.");
            }
            return input;
        }

        static void MostrarEncabezado(string titulo)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("==================================================");
            Console.WriteLine("  " + titulo);
            Console.WriteLine("==================================================\n");
            Console.ResetColor();
        }

        static void ImprimirExito(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n" + mensaje);
            Console.ResetColor();
            Pausar();
        }

        static void ImprimirError(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n" + mensaje);
            Console.ResetColor();
        }

        static void Pausar()
        {
            Console.WriteLine("\nPresione cualquier tecla para continuar...");
            Console.ReadKey();
        }
    }
}
