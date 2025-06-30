Imports System.ComponentModel
Imports System.Drawing.Printing
Imports System.Text

Public Class Ticket


    Private Sub Ticket_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing

        Menu.Show()

    End Sub

    Private Sub Salir_Click(sender As Object, e As EventArgs) Handles Salir.Click

        Menu.Show()
        Me.Close()

    End Sub




    Private Sub Ticket_Load(sender As Object, e As EventArgs) Handles Me.Load

        CheckBoxEfectivo.ForeColor = Color.Green

        LabelNumTicket.Text = DatosGlobales.ListaVentas.Count + 1

        ' Agregar evento a los botones dinámicamente
        For i As Integer = 1 To DatosGlobales.ListaProductos.Count

            Dim btnMas As Button = Me.Controls("ButtonMas" & i)
            btnMas.Text = DatosGlobales.ListaProductos(i - 1).Nombre
            Dim btnMenos As Button = Me.Controls("ButtonMenos" & i)

            If btnMas IsNot Nothing Then

                btnMas.Tag = i
                AddHandler btnMas.Click, AddressOf BotonCantidad_Click

            End If

            If btnMenos IsNot Nothing Then

                btnMenos.Tag = i
                AddHandler btnMenos.Click, AddressOf BotonCantidad_Click

            End If
        Next



    End Sub


    Private Sub BotonCantidad_Click(sender As Object, e As EventArgs)

        ' Obtener el botón que generó el evento
        Dim boton As Button = DirectCast(sender, Button)
        Dim index As Integer = CInt(boton.Tag) ' Identifica el producto

        ' Buscar los controles asociados al producto
        Dim txtCantidad As TextBox = Me.Controls("TextBoxCantidad" & index)
        Dim txtTotal As TextBox = Me.Controls("TextBoxSubTotal" & index)


        '        Dim lblNombre As Label = Me.Controls("lblNombre" & index)

        Dim cantidad As Integer = Integer.Parse(txtCantidad.Text)

        If boton.Text = "-" And cantidad > 0 Then

            cantidad -= 1
            If cantidad = 0 Then

                txtCantidad.BackColor = Color.WhiteSmoke
                txtTotal.BackColor = Color.WhiteSmoke

            End If
        End If
        If Not boton.Text = "-" Then

            cantidad += 1
            txtCantidad.BackColor = Color.LightGreen
            txtTotal.BackColor = Color.LightGreen
        End If

        txtCantidad.Text = cantidad.ToString()

        ' Calcular el total del producto        
        txtTotal.Text = ((CLng(DatosGlobales.ListaProductos(index - 1).Precio)) * cantidad).ToString

        ' Actualizar el total general
        ActualizarTotalGeneral()

    End Sub

    Private Sub ActualizarTotalGeneral()

        Dim total As Double = 0
        For i As Integer = 1 To DatosGlobales.ListaProductos.Count

            Dim txtTotal As TextBox = TryCast(Me.Controls("TextBoxSubTotal" & i), TextBox)

            If txtTotal IsNot Nothing Then

                total += Double.Parse(txtTotal.Text)

            End If
        Next

        TextBoxTotal.Text = total.ToString()

    End Sub


    Private Sub Imprimir_Click(sender As Object, e As EventArgs) Handles Imprimir.Click

        If TextBoxTotal.Text = "0" Then

            MessageBox.Show("No hay productos seleccionados", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Else

            'Dim respuesta As DialogResult


            'respuesta = MessageBox.Show("¿Deseas continuar?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
            'If respuesta = DialogResult.Yes Then


            'Else
            'Exit Sub
            'End If

            Dim venta As New Ventas()

            venta.TotalVentas = Convert.ToDouble(TextBoxTotal.Text)
            If CheckBoxEfectivo.Checked = True Then
                venta.MetodoPago = True
            Else
                venta.MetodoPago = False
            End If
            For i As Integer = 1 To DatosGlobales.ListaProductos.Count

                Dim txt As TextBox = TryCast(Me.Controls("TextBoxCantidad" & i), TextBox)

                If txt IsNot Nothing Then

                    Dim propiedad As System.Reflection.PropertyInfo = venta.GetType().GetProperty("Cantidad" & i)

                    If propiedad IsNot Nothing Then

                        propiedad.SetValue(venta, Convert.ToInt32(txt.Text))

                    End If

                End If

            Next

            'ACA VA EL INSERT
            Try

                Dim tabla As String = "Ventas"
                Dim sql As String = "INSERT INTO Ventas (Cantidad1,Cantidad2,Cantidad3,Cantidad4,Cantidad5,Cantidad6,Cantidad7,Cantidad8,Cantidad9,Cantidad10,Cantidad11,Cantidad12,Cantidad13,Cantidad14,Cantidad15,Cantidad16,Cantidad17,Cantidad18,Cantidad19,Cantidad20,Cantidad21,Cantidad22,TotalVentas,Efectivo) VALUES (" & venta.Cantidad1 & "," & venta.Cantidad2 & "," & venta.Cantidad3 & "," & venta.Cantidad4 & "," & venta.Cantidad5 & "," & venta.Cantidad6 & "," & venta.Cantidad7 & "," & venta.Cantidad8 & "," & venta.Cantidad9 & "," & venta.Cantidad10 & "," & venta.Cantidad11 & "," & venta.Cantidad12 & "," & venta.Cantidad13 & "," & venta.Cantidad14 & "," & venta.Cantidad15 & "," & venta.Cantidad16 & "," & venta.Cantidad17 & "," & venta.Cantidad18 & "," & venta.Cantidad19 & "," & venta.Cantidad20 & "," & venta.Cantidad21 & "," & venta.Cantidad22 & "," & venta.TotalVentas & "," & venta.MetodoPago & ")"
                ' Llamar a la función e insertar el registro
                If InsertarRegistro(tabla, sql) Then

                    ' Limpiar los controles
                    For i As Integer = 1 To DatosGlobales.ListaProductos.Count

                        Dim txtCantidad As TextBox = TryCast(Me.Controls("TextBoxCantidad" & i), TextBox)
                        Dim txtTotal As TextBox = TryCast(Me.Controls("TextBoxSubTotal" & i), TextBox)
                        Dim labelElementos As Label = TryCast(Me.Controls("Label" & i), Label)

                        If txtCantidad IsNot Nothing Then

                            txtTotal.BackColor = Color.WhiteSmoke
                            txtCantidad.BackColor = Color.WhiteSmoke
                            txtCantidad.Text = "0"
                            txtTotal.Text = "0"

                        End If

                    Next

                    TextBoxTotal.Text = "0"
                    LabelNumTicket.Text = CInt(LabelNumTicket.Text) + 1

                End If

                Impresion(venta)
                venta = Nothing

            Catch ex As Exception

                MessageBox.Show("Error al ejecutar SQL: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Application.Exit()
            End Try

        End If

    End Sub



    Private Sub Impresion(venta As Ventas)
        Try

            Dim texto As String
            Dim FechaHora As Date = Now
            ' Texto a imprimir (asegurarse de que cada línea tenga hasta 42/27 caracteres)
            texto = "===========================" & vbCrLf
            texto = texto & "            ADJC           " & vbCrLf
            texto = texto & "===========================" & vbCrLf
            texto = texto & "Fecha: " & FechaHora.ToString & " " & vbCrLf
            texto = texto & "Ticket Nº: " & LabelNumTicket.Text & "  " & vbCrLf
            texto = texto & "Evento/Cajeros: " & vbCrLf
            texto = texto & DatosGlobales.cajeros.Apellidos.ToString & "  " & vbCrLf
            texto = texto & "Cant Detalle          Monto" & vbCrLf
            For i = 1 To DatosGlobales.ListaProductos.Count

                Dim propiedad As System.Reflection.PropertyInfo = venta.GetType().GetProperty("Cantidad" & i)
                Dim nombre As String = DatosGlobales.ListaProductos(i - 1).Nombre.ToString
                Dim precio As Double = DatosGlobales.ListaProductos(i - 1).Precio
                If propiedad.GetValue(venta) > 0 Then

                    texto = texto & " " & propiedad.GetValue(venta) & "  " & nombre & vbCrLf
                    texto = texto & "                  $" & precio * propiedad.GetValue(venta) & vbCrLf
                    texto = texto & "---------------------------" & vbCrLf
                End If
            Next
            texto = texto & "===========================" & vbCrLf
            Dim propiedad2 As System.Reflection.PropertyInfo = venta.GetType().GetProperty("TotalVentas")
            texto = texto & "TOTAL DEL TICKET   $" & propiedad2.GetValue(venta) & vbCrLf
            texto = texto & "===========================" & vbCrLf
            texto = texto & vbCrLf & vbCrLf & vbCrLf & vbCrLf ' Espacios para el corte

            ' ESC/POS: Comando para tamaño de fuente doble en ancho y alto
            Dim esc As String = Chr(&H1B) ' Código ESC
            Dim dobleTamaño As String = esc & "!" & Chr(56) ' Doble ancho y alto
            Dim reset As String = esc & "@" ' Reset de la impresora

            ' Texto a enviar con formato
            Dim comando As String = reset & dobleTamaño & texto & vbCrLf & vbCrLf

            ' Enviar a la impresora
            Dim p As New PrintDocument()
            p.PrinterSettings.PrinterName = Configuraciones.nombreImpresora

            AddHandler p.PrintPage, Sub(sender As Object, e As PrintPageEventArgs)
                                        Dim font As New Font("Courier New", 12, FontStyle.Bold)
                                        e.Graphics.DrawString(texto, font, Brushes.Black, 0, 0)
                                    End Sub
            p.Print()
        Catch ex As Exception
            MessageBox.Show("Error Impresion: Revise en el menu principal la impresora predeterminada " & ex.Message)
            MessageBox.Show("Revise la tabla principal para confirmar si el ticket fue guardado y reimprimirlo")
            Me.Dispose()
            Menu.Show()
        End Try
    End Sub

    Private Sub Ticket_Resize(sender As Object, e As EventArgs) Handles Me.Resize


        For Each ctrl As Control In Me.Controls

            ctrl.AutoSize = True
        Next
    End Sub

    Private Sub CheckBoxtTransferencia_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxtTransferencia.CheckedChanged

        If CheckBoxtTransferencia.Checked = True Then
            CheckBoxEfectivo.Checked = False
            CheckBoxtTransferencia.ForeColor = Color.Blue
            CheckBoxEfectivo.ForeColor = Color.Black
        Else
            CheckBoxEfectivo.Checked = True
            CheckBoxEfectivo.ForeColor = Color.Green
            CheckBoxtTransferencia.ForeColor = Color.Black
        End If
    End Sub

    Private Sub CheckBoxEfectivo_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxEfectivo.CheckedChanged

        If CheckBoxEfectivo.Checked = True Then
            CheckBoxtTransferencia.Checked = False
            CheckBoxtTransferencia.ForeColor = Color.Black
            CheckBoxEfectivo.ForeColor = Color.Green
        Else
            CheckBoxtTransferencia.Checked = True
            CheckBoxtTransferencia.ForeColor = Color.Blue
            CheckBoxEfectivo.ForeColor = Color.Black
        End If
    End Sub
End Class