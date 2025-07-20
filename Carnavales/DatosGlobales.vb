Module DatosGlobales

    ' Este módulo contiene las variables globales y funciones para obtener datos de la base de datos.
    ' Se utiliza para almacenar la lista de productos, cajeros y ventas.

    Public ListaProductos As List(Of Producto) = ObtenerProductos()
    Public cajeros As Cajeros = ObtenerCajeros()
    Public ListaVentas As List(Of Ventas) = ObtenerVentas()

    Public Function ObtenerVentas() As List(Of Ventas)

        ' Esta función obtiene la lista de ventas desde la base de datos Access y las devuelve como una lista de objetos Ventas.
        Dim ListaVentas As New List(Of Ventas)

        ' Conexión a Access
        Dim conexion As New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Configuraciones.rutaDB & ";")

        ' Comando SQL para seleccionar todas las ventas ordenadas por ID
        Dim comando As New OleDb.OleDbCommand("SELECT * FROM Ventas ORDER BY ID ASC", conexion)

        Try
            ' Abrir la conexión y ejecutar el comando
            conexion.Open()

            ' Ejecutar el comando y obtener un lector de datos
            Dim lector As OleDb.OleDbDataReader = comando.ExecuteReader()

            ' Leer los datos y agregarlos a la lista de ventas
            While lector.Read()

                ' Crear un nuevo objeto Ventas y asignar los valores de las columnas
                ListaVentas.Add(New Ventas With {
                    .ID = Convert.ToInt32(lector("ID")),
                    .Cantidad1 = Convert.ToInt32(lector("Cantidad1")),
                    .Cantidad2 = Convert.ToInt32(lector("Cantidad2")),
                    .Cantidad3 = Convert.ToInt32(lector("Cantidad3")),
                    .Cantidad4 = Convert.ToInt32(lector("Cantidad4")),
                    .Cantidad5 = Convert.ToInt32(lector("Cantidad5")),
                    .Cantidad6 = Convert.ToInt32(lector("Cantidad6")),
                    .Cantidad7 = Convert.ToInt32(lector("Cantidad7")),
                    .Cantidad8 = Convert.ToInt32(lector("Cantidad8")),
                    .Cantidad9 = Convert.ToInt32(lector("Cantidad9")),
                    .Cantidad10 = Convert.ToInt32(lector("Cantidad10")),
                    .Cantidad11 = Convert.ToInt32(lector("Cantidad11")),
                    .Cantidad12 = Convert.ToInt32(lector("Cantidad12")),
                    .Cantidad13 = Convert.ToInt32(lector("Cantidad13")),
                    .Cantidad14 = Convert.ToInt32(lector("Cantidad14")),
                    .Cantidad15 = Convert.ToInt32(lector("Cantidad15")),
                    .Cantidad16 = Convert.ToInt32(lector("Cantidad16")),
                    .Cantidad17 = Convert.ToInt32(lector("Cantidad17")),
                    .Cantidad18 = Convert.ToInt32(lector("Cantidad18")),
                    .Cantidad19 = Convert.ToInt32(lector("Cantidad19")),
                    .Cantidad20 = Convert.ToInt32(lector("Cantidad20")),
                    .Cantidad21 = Convert.ToInt32(lector("Cantidad21")),
                    .Cantidad22 = Convert.ToInt32(lector("Cantidad22")),
                    .Cantidad23 = Convert.ToInt32(lector("Cantidad23")),
                    .Cantidad24 = Convert.ToInt32(lector("Cantidad24")),
                    .Cantidad25 = Convert.ToInt32(lector("Cantidad25")),
                    .Cantidad26 = Convert.ToInt32(lector("Cantidad26")),
                    .Cantidad27 = Convert.ToInt32(lector("Cantidad27")),
                    .TotalVentas = Convert.ToDouble(lector("TotalVentas")),
                    .Anulado = Convert.ToBoolean(lector("Anulado")),
                    .MetodoPago = Convert.ToBoolean(lector("Efectivo")),
                    .Fecha = Convert.ToDateTime(lector("Fecha"))
                    })
            End While

            ' Cerrar el lector de datos
            lector.Close()

        Catch ex As Exception
            ' Manejar cualquier error que ocurra durante la ejecución del SQL
            MessageBox.Show("Error al ejecutar SQL: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Exit()
        Finally
            ' Asegurarse de cerrar la conexión
            conexion.Close()
        End Try

        ' Devolver la lista de ventas obtenida
        Return ListaVentas

    End Function

    Public Function ObtenerCajeros() As Cajeros

        ' Esta función obtiene los datos del cajero desde la base de datos Access y devuelve un objeto Cajeros.
        Dim cajeros2 As New Cajeros

        ' Conexión a Access        
        Dim conexion As New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Configuraciones.rutaDB & ";")
        Dim comando As New OleDb.OleDbCommand("SELECT Id, Apellidos FROM Cajeros", conexion)

        Try
            ' Abrir la conexión y ejecutar el comando
            conexion.Open()

            ' Ejecutar el comando y obtener un lector de datos
            Dim lector As OleDb.OleDbDataReader = comando.ExecuteReader()

            ' Leer los datos y asignarlos al objeto Cajeros
            While lector.Read()

                ' Verificar si el campo ID es DBNull antes de convertirlo
                ' Asignar los valores de las columnas al objeto Cajeros
                cajeros2.ID = Convert.ToInt32(lector("Id"))
                cajeros2.Apellidos = lector("Apellidos").ToString()

            End While

            ' Cerrar el lector de datos
            lector.Close()

        Catch ex As Exception

            ' Manejar cualquier error que ocurra durante la ejecución del SQL
            MessageBox.Show("Error al ejecutar SQL: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Exit()
        Finally
            ' Asegurarse de cerrar la conexión
            conexion.Close()
        End Try

        ' Devolver el objeto Cajeros obtenido
        Return cajeros2

    End Function

    Public Function ObtenerProductos() As List(Of Producto)

        ' Esta función obtiene la lista de productos desde la base de datos Access y los devuelve como una lista de objetos Producto.
        Dim productos As New List(Of Producto)
        ' Conexión a Access
        Dim conexion As New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Configuraciones.rutaDB & ";")
        ' Comando SQL para seleccionar los productos
        Dim comando As New OleDb.OleDbCommand("SELECT Id, Nombre, Precio FROM Listado", conexion)

        Try
            ' Abrir la conexión y ejecutar el comando                
            conexion.Open()
            ' Ejecutar el comando y obtener un lector de datos
            Dim lector As OleDb.OleDbDataReader = comando.ExecuteReader()

            ' Leer los datos y agregarlos a la lista de productos
            While lector.Read()

                ' Crear un nuevo objeto Producto y asignar los valores de las columnas
                Dim producto As New Producto

                ' Verificar si el campo ID es DBNull antes de convertirlo
                If Not IsDBNull(lector("Id")) Then

                    ' Asignar el ID del producto
                    producto.ID = Convert.ToInt32(lector("Id"))
                Else
                    ' Si el ID es DBNull, asignar 0
                    producto.ID = 0
                End If

                ' Asignar el nombre del producto, verificando si es DBNull
                If Not IsDBNull(lector("Nombre")) Then

                    ' Asignar el nombre del producto
                    producto.Nombre = lector("Nombre").ToString()
                Else
                    ' Si el nombre es DBNull, asignar una cadena vacía
                    producto.Nombre = ""
                End If

                ' Asignar el precio del producto, verificando si es DBNull
                If IsDBNull(lector("Precio")) Then

                    ' Si el precio es DBNull, asignar Nothing
                    producto.Precio = Nothing
                Else
                    ' Asignar el precio del producto
                    producto.Precio = Convert.ToDecimal(lector("Precio"))
                End If

                ' Agregar el producto a la lista de productos
                productos.Add(producto)
            End While

            ' Cerrar el lector de datos
            lector.Close()
        Catch ex As Exception

            ' Manejar cualquier error que ocurra durante la ejecución del SQL
            MessageBox.Show("Error al ejecutar SQL: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Exit()
        Finally
            ' Asegurarse de cerrar la conexión
            conexion.Close()
        End Try

        ' Devolver la lista de productos obtenida
        Return productos

    End Function

End Module
