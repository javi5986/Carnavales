Imports System.Management
Imports System.Security.Cryptography
Imports System.Text
Module Configuraciones

    ' Configuraciones generales del sistema
    ' Nombre de la impresora por defecto
    Public nombreImpresora As String = "cobro"
    ' Ruta de la base de datos
    Public ReadOnly rutaEscritorio As String = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
    ' Ruta completa de la base de datos
    Public ReadOnly rutaDB As String = System.IO.Path.Combine(Application.StartupPath, "Carnavales.accdb")

    ' Obtiene la huella única de la PC
    Public Function ObtenerHuella() As String
        ' Combina la MAC y el número de serie del disco para generar una huella única
        Dim mac As String = ObtenerMAC()
        Dim disco As String = ObtenerSerieDisco()
        Dim combinado As String = mac & "|" & disco
        ' Hash SHA256 para obtener una cadena fija de longitud
        Return HashSHA256(combinado)
    End Function

    ' Obtiene la dirección MAC de la primera tarjeta de red física encontrada
    Private Function ObtenerMAC() As String

        Try
            ' Usa WMI para obtener la dirección MAC de la tarjeta de red física
            Dim searcher As New ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter WHERE PhysicalAdapter=True")

            ' Recorre los resultados y devuelve la primera MAC válida encontrada
            For Each obj As ManagementObject In searcher.Get()
                ' Algunas tarjetas de red pueden no tener la propiedad MACAddress, por eso se usa el operador de acceso seguro (?.)
                Dim mac As String = obj("MACAddress")?.ToString()
                ' Elimina los guiones de la dirección MAC para obtener una cadena más limpia
                If Not String.IsNullOrEmpty(mac) Then Return mac
            Next

        Catch
            '   Si ocurre algún error (por ejemplo, falta de permisos), se captura la excepción y se devuelve un valor por defecto
            MsgBox("Error al obtener la dirección MAC. Asegúrese de ejecutar el programa con permisos adecuados.", MsgBoxStyle.Critical)

        End Try
        ' Si no se encuentra ninguna MAC válida, devuelve un valor por defecto
        Return "SIN-MAC"

    End Function

    ' Obtiene el número de serie del primer disco duro encontrado
    Private Function ObtenerSerieDisco() As String
        Try
            ' Usa WMI para obtener el número de serie del disco duro
            Dim searcher As New ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive")

            ' Recorre los resultados y devuelve el número de serie del primer disco encontrado
            For Each obj As ManagementObject In searcher.Get()
                ' Algunas unidades de disco pueden no tener la propiedad SerialNumber, por eso se usa el operador de acceso seguro (?.)
                Dim serie As String = obj("SerialNumber")?.ToString().Trim()
                ' Elimina espacios en blanco y caracteres no alfanuméricos para obtener una cadena más limpia
                If Not String.IsNullOrEmpty(serie) Then Return serie
            Next
        Catch
            ' Si ocurre algún error (por ejemplo, falta de permisos), se captura la excepción y se devuelve un valor por defecto
            MsgBox("Error al obtener el número de serie del disco. Asegúrese de ejecutar el programa con permisos adecuados.", MsgBoxStyle.Critical)
        End Try

        Return "SIN-DISCO"

    End Function

    ' Aplica un hash SHA256 a la cadena combinada para obtener una huella única de longitud fija
    Private Function HashSHA256(texto As String) As String

        ' Usa SHA256 para generar un hash de la cadena combinada (MAC + número de serie)
        Using sha As SHA256 = SHA256.Create()

            ' Convierte la cadena a bytes, calcula el hash y luego lo convierte a una representación hexadecimal sin guiones
            Dim bytes() As Byte = Encoding.UTF8.GetBytes(texto)
            Dim hash() As Byte = sha.ComputeHash(bytes)
            Return BitConverter.ToString(hash).Replace("-", "").ToLower()
        End Using

    End Function

    ' Guarda la huella autorizada en el registro
    Public Sub RegistrarLicencia()

        ' Obtiene la huella actual de la PC y la guarda en el registro para autorizar esta máquina a ejecutar la aplicación
        Dim huella As String = ObtenerHuella()
        ' Abre la clave de registro en HKEY_LOCAL_MACHINE\SOFTWARE\ADJC\Carnavales y guarda el valor "LIC" con la huella obtenida
        Dim baseKey = Microsoft.Win32.RegistryKey.OpenBaseKey(
        Microsoft.Win32.RegistryHive.LocalMachine,
        Microsoft.Win32.RegistryView.Registry64)
        ' Crea o abre la subclave "SOFTWARE\ADJC\Carnavales" con permisos de lectura y escritura, guarda la huella en el valor "LIC" y luego cierra las claves de registro
        Dim subKey = baseKey.CreateSubKey("SOFTWARE\ADJC\Carnavales",
        Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree)
        ' Guarda la huella en el valor "LIC" de la clave de registro
        subKey.SetValue("LIC", huella)
        ' Cierra las claves de registro para liberar recursos
        subKey.Close()
        baseKey.Close()

    End Sub

    ' Verifica si la PC actual es la autorizada
    Public Function EsLicenciaValida() As Boolean
        Try
            ' Lee la huella autorizada del registro y la compara con la huella actual de la PC para determinar si la licencia es válida
            Dim baseKey = Microsoft.Win32.RegistryKey.OpenBaseKey(
            Microsoft.Win32.RegistryHive.LocalMachine,
            Microsoft.Win32.RegistryView.Registry64)
            ' Abre la subclave "SOFTWARE\ADJC\Carnavales" para leer el valor "LIC", que contiene la huella autorizada, y luego compara esa huella con la huella actual obtenida de la PC. Si coinciden, la licencia es válida; de lo contrario, no lo es. Si ocurre algún error al acceder al registro, se captura la excepción y se considera que la licencia no es válida.
            Dim clave = baseKey.OpenSubKey("SOFTWARE\ADJC\Carnavales")
            ' Si la clave no existe, significa que no se ha registrado una licencia, por lo que se devuelve False
            If clave Is Nothing Then Return False
            ' Lee el valor "LIC" de la clave de registro, que contiene la huella autorizada, y lo compara con la huella actual obtenida de la PC. Si coinciden, la licencia es válida; de lo contrario, no lo es. Si ocurre algún error al acceder al registro, se captura la excepción y se considera que la licencia no es válida.
            Dim licGuardada As String = clave.GetValue("LIC")?.ToString()
            ' Cierra las claves de registro para liberar recursos
            Return licGuardada = ObtenerHuella()
        Catch
            Return False
        End Try
    End Function
End Module
