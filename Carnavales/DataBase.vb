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

End Module
