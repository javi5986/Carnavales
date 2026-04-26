Public Class Principal

    Private Sub Principal_Shown(sender As Object, e As EventArgs) Handles Me.Shown

        ' Verificar licencia antes de cualquier otra cosa
        If Not Configuraciones.EsLicenciaValida() Then
            MessageBox.Show(
            "Esta aplicación no está autorizada para ejecutarse en esta PC." & vbCrLf &
            "Contacte al administrador del sistema.",
            "Licencia inválida", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Application.Exit()
            Return
        End If
        Try

            ' Inicializar la lista de cajeros
            Dim cajero As New Cajeros

            ' Consultamos la lista de cajeros desde la base de datos y guardamos el resultado en DatosGlobales
            DatosGlobales.cajeros = ObtenerCajeros()

            ' Asignamos la lista de cajeros a la variable cajero
            cajero = DatosGlobales.cajeros

            ' Verificar si la lista de cajeros tiene elementos
            If cajero.Apellidos IsNot Nothing Then

                ' Acceder a los valores de las columnas por nombre
                Apellidos.Text = cajero.Apellidos
                Apellidos.Enabled = False

            End If

        Catch ex As Exception
            ' Manejar cualquier error que ocurra al cargar los cajeros
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

        ' Bloquear comilla simple '
        If e.KeyChar = "'"c Then
            e.Handled = True
            Exit Sub
        End If

        ' Convertir a mayúsculas respetando Unicode
        e.KeyChar = Char.ToUpper(e.KeyChar)

        ' Si se presiona Enter, pasar al siguiente control
        If e.KeyChar = ChrW(Keys.Enter) Then
            e.Handled = True
            Me.SelectNextControl(CType(sender, Control), True, True, True, True)
        End If

    End Sub
End Class
