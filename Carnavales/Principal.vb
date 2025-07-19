Public Class Principal

    Private Sub Principal_Shown(sender As Object, e As EventArgs) Handles Me.Shown

        Try

            ' Inicializar la lista de cajeros
            Dim cajero As New Cajeros

            ' Mostrar el formulario Cajeros
            cajero = ObtenerCajeros()

            ' Verificar si la lista de cajeros tiene elementos
            If cajero.Apellidos IsNot Nothing Then

                ' Acceder a los valores de las columnas por nombre
                Apellidos.Text = cajero.Apellidos
                Apellidos.Enabled = False

            End If

        Catch ex As Exception

            MessageBox.Show("Error al cargar CAJEROS: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Exit()
        End Try

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Salir.Click

        ' salir del programa
        Application.Exit()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Aceptar.Click

        ' Verificar si el campo Apellidos tiene contenido
        If TieneContenido(Apellidos) Then

            ' Verificar si el campo Apellidos está habilitado
            If Apellidos.Enabled Then

                Try

                    ' Definir la tabla y la consulta SQL para insertar el registro
                    Dim tabla As String = "Cajeros"
                    Dim sql As String = "INSERT INTO Cajeros (Apellidos) VALUES ('" & Apellidos.Text & "')"

                    ' Llamar a la función e insertar el registro
                    If InsertarRegistro(tabla, sql) Then

                        ' cerrar el formulario actual y mostrar el menú
                        Menu.Show()
                        Me.Hide()

                    End If

                Catch ex As Exception
                    ' Manejar cualquier error que ocurra durante la ejecución del SQL
                    MessageBox.Show("Error al guardar Evento Cajero: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Application.Exit()
                End Try

            Else
                ' Cerrar el formulario actual y mostrar el menú
                Menu.Show()
                Me.Hide()

            End If
        Else

            ' Si el campo Apellidos no tiene contenido, mostrar un mensaje de error
            MsgBox("Ingrese los datos Solicitados")

        End If

    End Sub

    Private Sub Apellidos_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Apellidos.KeyPress

        ' Pasar a mayúsculas la letra ingresada en el campo Apellidos
        e.KeyChar = Char.ToUpper(e.KeyChar)

    End Sub

End Class
