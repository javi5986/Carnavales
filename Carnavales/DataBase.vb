Imports System.Data.OleDb
Imports System.Data.SqlClient

Module DataBase

    Public Function EjecutarSQL(ByVal tabla As String, ByVal consultaSQL As String) As DataTable

        ' Ruta de la base de datos Access 2016 (.accdb) -> Modificar según ubicación
        Dim cadenaConexion As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Configuraciones.rutaDB & ";"

        ' DataTable para almacenar los resultados
        Dim dt As New DataTable()

        Try
            ' Establecer la conexión con la base de datos
            Using conexion As New OleDbConnection(cadenaConexion)

                ' Abrir la conexión
                conexion.Open()

                ' Crear comando SQL
                Using comando As New OleDbCommand(consultaSQL, conexion)

                    ' Adaptador para llenar el DataTable
                    Using adaptador As New OleDbDataAdapter(comando)

                        ' Llenar el DataTable con los resultados de la consulta
                        adaptador.Fill(dt)

                    End Using

                End Using

            End Using

        Catch ex As Exception

            ' Manejo de errores: mostrar mensaje y cerrar la aplicación
            MessageBox.Show("Error al ejecutar CONSULTA SQL: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Exit()
        End Try

        ' Retornar los datos al formulario que la llamó
        Return dt

    End Function

    Public Function InsertarRegistro(ByVal tabla As String, ByVal consultaSQL As String) As Boolean

        ' Ruta de la base de datos Access 2016 (.accdb) -> Modificar según ubicación
        Dim cadenaConexion As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Configuraciones.rutaDB & ";"

        Try
            ' Establecer conexión con la base de datos
            Using conexion As New OleDbConnection(cadenaConexion)

                ' Abrir la conexión
                conexion.Open()

                ' Crear y ejecutar el comando SQL
                Using comando As New OleDbCommand(consultaSQL, conexion)

                    ' Agregar parámetros si es necesario
                    Dim filasAfectadas As Integer = comando.ExecuteNonQuery()

                    ' Si se insertó al menos una fila, retornar True
                    If filasAfectadas > 0 Then
                        Return True
                    Else
                        Return False
                    End If
                End Using

                ' Cerrar la conexión
                conexion.Close()

            End Using

        Catch ex As Exception
            ' Manejo de errores: mostrar mensaje y cerrar la aplicación
            MessageBox.Show("Error al insertar datos: " & ex.Message, "REVISE NO PUEDE UTILIZAR COMILLAS SIMPLES ' O DOBLES ''", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False

        End Try

    End Function

    Public Function EditarRegistro(ByVal tabla As String, ByVal sqlUpdate As String) As Boolean

        ' Ruta de la base de datos Access 2016 (.accdb) -> Modificar según ubicación
        Dim cadenaConexion As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Configuraciones.rutaDB & ";"

        ' Establecer conexión con la base de datos
        Using conexion As New OleDbConnection(cadenaConexion)

            Try
                ' Verificar si la cadena de conexión es válida
                conexion.Open()

                ' Crear comando SQL para actualizar el registro
                Using comando As New OleDbCommand(sqlUpdate, conexion)

                    ' Ejecutar el comando SQL
                    Dim filasAfectadas As Integer = comando.ExecuteNonQuery()

                    ' Si se insertó al menos una fila, retornar True
                    If filasAfectadas > 0 Then
                        Return True
                    Else
                        Return False
                    End If

                End Using

                ' Cerrar la conexión
                conexion.Close()

            Catch ex As Exception
                ' Manejo de errores: mostrar mensaje y cerrar la aplicación
                MessageBox.Show("Error al actualizar REGISTRO en " & tabla & ": " & ex.Message)
                Application.Exit()
                Return False

            End Try
        End Using
    End Function

    Public Sub ActualizarListadoProductos(grilla As DataGridView, Optional formulario As Form = Nothing)

        ' Ruta de la base de datos Access 2016 (.accdb) -> Modificar según ubicación
        Dim cadenaConexion As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Configuraciones.rutaDB & ";"

        ' Establecer conexión con la base de datos
        Using conexion As New OleDb.OleDbConnection(cadenaConexion)

            Try
                ' Abrir la conexión
                conexion.Open()

                ' Recorrer las filas del DataGridView y actualizar la base de datos
                For Each fila As DataGridViewRow In grilla.Rows

                    ' Verificar que la fila no sea una nueva fila (NewRow)
                    If Not fila.IsNewRow Then

                        ' Obtener los valores de las celdas
                        Dim id As Integer = Convert.ToInt32(fila.Cells("ID").Value)
                        Dim nombre As String = ""
                        Dim precio As Decimal = 0
                        Dim imprimirPorUnidad As Boolean = Convert.ToBoolean(fila.Cells("ImprimirPorUnidad").Value)

                        ' Obtener el valor de la celda "Nombre" y "Precio"
                        Dim nombreCelda As Object = fila.Cells("Nombre").Value
                        Dim precioCelda As Object = fila.Cells("Precio").Value
                        Dim imprimirPorUnidadCelda As Object = fila.Cells("ImprimirPorUnidad").Value

                        ' Validar que los valores no sean Nothing o DBNull
                        If nombreCelda IsNot Nothing AndAlso Not IsDBNull(nombreCelda) AndAlso Not String.IsNullOrWhiteSpace(nombreCelda.ToString()) Then

                            ' Convertir el valor a cadena y eliminar espacios en blanco
                            nombre = nombreCelda.ToString().Trim()

                            ' Validar que el nombre no esté vacío   
                            If precioCelda IsNot Nothing AndAlso Not IsDBNull(precioCelda) AndAlso IsNumeric(precioCelda) Then

                                ' Convertir el valor a Decimal
                                precio = Convert.ToDecimal(precioCelda)
                            End If

                        End If

                        ' Construir la consulta SQL de actualización
                        Dim sqlUpdate As String = "UPDATE Listado SET Nombre = @Nombre, Precio = @Precio, ImprimirPorUnidad = @ImprimirPorUnidad WHERE ID = @ID"

                        ' Crear el comando SQL con parámetros
                        Using comando As New OleDb.OleDbCommand(sqlUpdate, conexion)

                            ' Agregar los parámetros al comando
                            comando.Parameters.AddWithValue("@Nombre", nombre)

                            ' Validar si el precio es 0, en cuyo caso se usa DBNull
                            If precio = 0 Then
                                comando.Parameters.AddWithValue("@Precio", DBNull.Value)
                            Else
                                comando.Parameters.AddWithValue("@Precio", precio)
                            End If

                            ' Agregar el valor de ImprimirPorUnidad, que se obtiene del checkbox en la fila
                            comando.Parameters.AddWithValue("@ImprimirPorUnidad", imprimirPorUnidad)  ' ← nueva línea

                            ' Agregar el parámetro ID
                            comando.Parameters.AddWithValue("@ID", id)
                            ' Ejecutar el comando SQL
                            comando.ExecuteNonQuery()

                        End Using
                    End If
                Next
                ' Cerrar la conexión
                conexion.Close()

                ' Mostrar mensaje de éxito
                MessageBox.Show("Actualización completada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)

                ' Si se pasó un formulario, cerrarlo
                If formulario IsNot Nothing Then formulario.Close()

            Catch ex As Exception

                ' Manejo de errores: mostrar mensaje de error
                MessageBox.Show("Error al actualizar la tabla Listado de Productos: " & ex.Message)
                Return

            End Try

        End Using

    End Sub

End Module
