Public Class Principal

    Private Sub Principal_Shown(sender As Object, e As EventArgs) Handles Me.Shown

        Try

            ' Llamar a la función Obtener Cajeros
            DatosGlobales.ObtenerCajeros()

            If DatosGlobales.cajeros.Apellidos IsNot Nothing Then

                ' Acceder a los valores de las columnas por nombre
                Apellidos.Text = DatosGlobales.cajeros.Apellidos
                Apellidos.Enabled = False

            End If

        Catch ex As Exception

            MessageBox.Show("Error al cargar CAJEROS: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Exit()
        End Try

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Salir.Click

        Application.Exit()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Aceptar.Click

        If TieneContenido(Apellidos) Then

            If Apellidos.Enabled Then

                Try

                    Dim tabla As String = "Cajeros"
                    Dim sql As String = "INSERT INTO Cajeros (Apellidos) VALUES ('" & Apellidos.Text & "')"

                    ' Llamar a la función e insertar el registro
                    If InsertarRegistro(tabla, sql) Then

                        Me.Hide()
                        Menu.Show()

                    End If

                Catch ex As Exception

                    MessageBox.Show("Error al ejecutar SQL: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Application.Exit()
                End Try

            Else

                Me.Hide()
                Menu.Show()

            End If
        Else

            MsgBox("Ingrese los datos Solicitados")

        End If

    End Sub

    Private Sub Apellidos_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Apellidos.KeyPress

        e.KeyChar = Char.ToUpper(e.KeyChar)

    End Sub

End Class
