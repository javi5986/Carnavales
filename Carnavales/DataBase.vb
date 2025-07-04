Imports System.Data.OleDb
Imports System.Data.SqlClient

Module DataBase

    Public Function EjecutarSQL(ByVal tabla As String, ByVal consultaSQL As String) As DataTable

        ' Ruta de la base de datos Access 2016 (.accdb) -> Modificar según ubicación
        Dim rutaEscritorio As String = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        Dim rutaDB As String = System.IO.Path.Combine(rutaEscritorio, "Carnavales.accdb")

        Dim cadenaConexion As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & rutaDB & ";"

        ' DataTable para almacenar los resultados
        Dim dt As New DataTable()

        Try
            ' Establecer la conexión con la base de datos
            Using conexion As New OleDbConnection(cadenaConexion)
                conexion.Open()

                ' Crear comando SQL
                Using comando As New OleDbCommand(consultaSQL, conexion)

                    ' Adaptador para llenar el DataTable
                    Using adaptador As New OleDbDataAdapter(comando)
                        adaptador.Fill(dt)
                    End Using
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error al ejecutar SQL: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Exit()
        End Try

        ' Retornar los datos al formulario que la llamó
        Return dt

    End Function

    Public Function InsertarRegistro(ByVal tabla As String, ByVal consultaSQL As String) As Boolean

        ' Ruta de la base de datos Access 2016 (.accdb) -> Modificar según ubicación
        Dim rutaEscritorio As String = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        Dim rutaDB As String = System.IO.Path.Combine(rutaEscritorio, "Carnavales.accdb")
        Dim cadenaConexion As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & rutaDB & ";"

        Try
            ' Establecer conexión con la base de datos
            Using conexion As New OleDbConnection(cadenaConexion)
                conexion.Open()

                ' Crear y ejecutar el comando SQL
                Using comando As New OleDbCommand(consultaSQL, conexion)
                    Dim filasAfectadas As Integer = comando.ExecuteNonQuery()

                    ' Si se insertó al menos una fila, retornar True
                    If filasAfectadas > 0 Then
                        Return True
                    Else
                        Return False
                    End If
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error al insertar datos: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Exit()
            Return False
        End Try

    End Function

    Public Function EditarRegistro(ByVal tabla As String, ByVal sqlUpdate As String) As Boolean

        Dim rutaEscritorio As String = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        Dim rutaDB As String = System.IO.Path.Combine(rutaEscritorio, "Carnavales.accdb")
        Dim cadenaConexion As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & rutaDB & ";"


        Using conexion As New OleDbConnection(cadenaConexion)
            Try
                conexion.Open()


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

            Catch ex As Exception
                MessageBox.Show("Error al actualizar en " & tabla & ": " & ex.Message)
                Application.Exit()
                Return False
            End Try
        End Using
    End Function

    Public Sub ActualizarListadoProductos(grilla As DataGridView, Optional formulario As Form = Nothing)
        Dim rutaEscritorio As String = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
        Dim rutaDB As String = System.IO.Path.Combine(rutaEscritorio, "Carnavales.accdb")
        Dim cadenaConexion As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & rutaDB & ";"

        Using conexion As New OleDb.OleDbConnection(cadenaConexion)
            Try
                conexion.Open()

                For Each fila As DataGridViewRow In grilla.Rows
                    If Not fila.IsNewRow Then
                        Dim id As Integer = Convert.ToInt32(fila.Cells("ID").Value)
                        Dim nombre As String = ""
                        Dim precio As Decimal = 0

                        Dim nombreCelda As Object = fila.Cells("Nombre").Value
                        Dim precioCelda As Object = fila.Cells("Precio").Value

                        If nombreCelda IsNot Nothing AndAlso Not IsDBNull(nombreCelda) AndAlso Not String.IsNullOrWhiteSpace(nombreCelda.ToString()) Then
                            nombre = nombreCelda.ToString().Trim()

                            If precioCelda IsNot Nothing AndAlso Not IsDBNull(precioCelda) AndAlso IsNumeric(precioCelda) Then
                                precio = Convert.ToDecimal(precioCelda)
                            End If
                        End If

                        Dim sqlUpdate As String = "UPDATE Listado SET Nombre = @Nombre, Precio = @Precio WHERE ID = @ID"
                        Using comando As New OleDb.OleDbCommand(sqlUpdate, conexion)
                            comando.Parameters.AddWithValue("@Nombre", nombre)
                            If precio = 0 Then
                                comando.Parameters.AddWithValue("@Precio", DBNull.Value)
                            Else
                                comando.Parameters.AddWithValue("@Precio", precio)
                            End If
                            comando.Parameters.AddWithValue("@ID", id)
                            comando.ExecuteNonQuery()
                        End Using
                    End If
                Next

                MessageBox.Show("Actualización completada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
                If formulario IsNot Nothing Then formulario.Close()

            Catch ex As Exception
                MessageBox.Show("Error al actualizar la tabla Listado de Productos: " & ex.Message)
            End Try
        End Using
    End Sub



End Module
