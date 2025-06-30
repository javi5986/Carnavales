Module DatosGlobales

    Public ListaProductos As List(Of Producto) = ObtenerProductos()
    Public cajeros As Cajeros = ObtenerCajeros()
    Public ListaVentas As List(Of Ventas) = ObtenerVentas()

    Public Function ObtenerVentas() As List(Of Ventas)

        Dim ListaVentas As New List(Of Ventas)

        ' Conexión a Access

        Dim conexion As New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Configuraciones.rutaDB & ";")
        Dim comando As New OleDb.OleDbCommand("SELECT * FROM Ventas ORDER BY ID ASC", conexion)

        Try
            conexion.Open()
            Dim lector As OleDb.OleDbDataReader = comando.ExecuteReader()

            While lector.Read()
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
                    .TotalVentas = Convert.ToDouble(lector("TotalVentas")),
                    .Anulado = Convert.ToBoolean(lector("Anulado")),
                    .MetodoPago = Convert.ToBoolean(lector("Efectivo"))
                })
            End While

            lector.Close()

        Catch ex As Exception
            MessageBox.Show("Error al ejecutar SQL: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            conexion.Close()
        End Try

        Return ListaVentas

    End Function

    Public Function ObtenerCajeros() As Cajeros

        Dim cajeros2 As New Cajeros

        ' Conexión a Access        
        Dim conexion As New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Configuraciones.rutaDB & ";")
        Dim comando As New OleDb.OleDbCommand("SELECT Id, Apellidos FROM Cajeros", conexion)

        Try
            conexion.Open()
            Dim lector As OleDb.OleDbDataReader = comando.ExecuteReader()

            While lector.Read()
                cajeros2.ID = Convert.ToInt32(lector("Id"))
                cajeros2.Apellidos = lector("Apellidos").ToString()
            End While
            lector.Close()
        Catch ex As Exception
            MessageBox.Show("Error al ejecutar SQL: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Exit()
        Finally
            conexion.Close()
        End Try

        Return cajeros2

    End Function

    Public Function ObtenerProductos() As List(Of Producto)

        Dim productos As New List(Of Producto)

        ' Conexión a Access        
        Dim conexion As New OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Configuraciones.rutaDB & ";")
        Dim comando As New OleDb.OleDbCommand("SELECT Id, Nombre, Precio FROM Listado", conexion)

        Try
            conexion.Open()
            Dim lector As OleDb.OleDbDataReader = comando.ExecuteReader()

            While lector.Read()
                productos.Add(New Producto With {
                    .ID = Convert.ToInt32(lector("Id")),
                    .Nombre = lector("Nombre").ToString(),
                    .Precio = Convert.ToDecimal(lector("Precio"))
                })
            End While
            lector.Close()
        Catch ex As Exception
            MessageBox.Show("Error al ejecutar SQL: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            conexion.Close()
        End Try

        Return productos

    End Function

End Module
