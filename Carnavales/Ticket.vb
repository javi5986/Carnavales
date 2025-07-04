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

        ' Inicializar los controles de la tabla de botones
        CheckBoxEfectivo.ForeColor = Color.Green
        LabelNumTicket.Text = DatosGlobales.ListaVentas.Count + 1

        ' Cargar los productos en los botones y textboxes
        For i As Integer = 1 To DatosGlobales.ListaProductos.Count

            ' Crear los controles dinámicamente si no existen
            Dim btnMas As Button = tblBotones.Controls("ButtonMas" & i)
            Dim btnMenos As Button = tblBotones.Controls("ButtonMenos" & i)

            If btnMas IsNot Nothing Then
                btnMas.Text = DatosGlobales.ListaProductos(i - 1).Nombre
                btnMas.Dock = DockStyle.Fill
                btnMas.Tag = i
                btnMenos.Tag = i

                ' Verifica si el texto está vacío o solo espacios
                If String.IsNullOrWhiteSpace(btnMas.Text) Then
                    btnMas.Enabled = False
                    btnMenos.Enabled = False
                Else
                    btnMas.Enabled = True
                    btnMenos.Enabled = True
                    AddHandler btnMas.Click, AddressOf BotonCantidad_Click
                    AddHandler btnMenos.Click, AddressOf BotonCantidad_Click
                End If
            End If

        Next

    End Sub


    Private Sub BotonCantidad_Click(sender As Object, e As EventArgs)

        Try
            Dim boton As Button = DirectCast(sender, Button)
            Dim index As Integer = CInt(boton.Tag)

            Dim txtCantidad As TextBox = TryCast(tblBotones.Controls("TextBoxCantidad" & index), TextBox)
            Dim txtTotal As TextBox = TryCast(tblBotones.Controls("TextBoxSubTotal" & index), TextBox)

            'If txtCantidad Is Nothing OrElse txtTotal Is Nothing Then Exit Sub

            ' Intentamos leer la cantidad, si falla, la ponemos en 0
            Dim cantidad As Integer = 0
            If Not Integer.TryParse(txtCantidad.Text, cantidad) Then cantidad = 0

            If boton.Text = "-" Then
                If cantidad > 0 Then
                    cantidad -= 1
                End If

                If cantidad = 0 Then
                    txtCantidad.BackColor = Color.WhiteSmoke
                    txtTotal.BackColor = Color.WhiteSmoke
                End If
            Else
                cantidad += 1
                txtCantidad.BackColor = Color.LightGreen
                txtTotal.BackColor = Color.LightGreen
            End If

            txtCantidad.Text = cantidad.ToString()

            ' Validar que el índice sea correcto
            If index > 0 AndAlso index <= DatosGlobales.ListaProductos.Count Then
                Dim precio As Decimal = DatosGlobales.ListaProductos(index - 1).Precio
                txtTotal.Text = (precio * cantidad).ToString("N0")
            End If

            ActualizarTotalGeneral()

        Catch ex As Exception
            MessageBox.Show("Error al actualizar cantidad: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub ActualizarTotalGeneral()

        Dim total As Double = 0
        For i As Integer = 1 To DatosGlobales.ListaProductos.Count

            Dim txtTotal As TextBox = TryCast(tblBotones.Controls("TextBoxSubTotal" & i), TextBox)

            If txtTotal IsNot Nothing Then

                total += Double.Parse(txtTotal.Text)

            End If
        Next

        TextBoxTotal.Text = total.ToString("N0")

    End Sub


    Private Sub Imprimir_Click(sender As Object, e As EventArgs) Handles Imprimir.Click

        If TextBoxTotal.Text = "0" Then

            MessageBox.Show("No hay productos seleccionados", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Else

            Dim venta As New Ventas()

            venta.TotalVentas = Convert.ToDouble(TextBoxTotal.Text)
            If CheckBoxEfectivo.Checked = True Then
                venta.MetodoPago = True
            Else
                venta.MetodoPago = False
            End If
            For i As Integer = 1 To DatosGlobales.ListaProductos.Count

                Dim txt As TextBox = TryCast(tblBotones.Controls("TextBoxCantidad" & i), TextBox)

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
                Dim sql As String = "INSERT INTO Ventas (Cantidad1,Cantidad2,Cantidad3,Cantidad4,Cantidad5,Cantidad6,Cantidad7,Cantidad8,Cantidad9,Cantidad10,Cantidad11,Cantidad12,Cantidad13,Cantidad14,Cantidad15,Cantidad16,Cantidad17,Cantidad18,Cantidad19,Cantidad20,Cantidad21,Cantidad22,Cantidad23,Cantidad24,Cantidad25,Cantidad26,Cantidad27,TotalVentas,Efectivo) VALUES (" & venta.Cantidad1 & "," & venta.Cantidad2 & "," & venta.Cantidad3 & "," & venta.Cantidad4 & "," & venta.Cantidad5 & "," & venta.Cantidad6 & "," & venta.Cantidad7 & "," & venta.Cantidad8 & "," & venta.Cantidad9 & "," & venta.Cantidad10 & "," & venta.Cantidad11 & "," & venta.Cantidad12 & "," & venta.Cantidad13 & "," & venta.Cantidad14 & "," & venta.Cantidad15 & "," & venta.Cantidad16 & "," & venta.Cantidad17 & "," & venta.Cantidad18 & "," & venta.Cantidad19 & "," & venta.Cantidad20 & "," & venta.Cantidad21 & "," & venta.Cantidad22 & "," & venta.Cantidad23 & "," & venta.Cantidad24 & "," & venta.Cantidad25 & "," & venta.Cantidad26 & "," & venta.Cantidad27 & "," & venta.TotalVentas & "," & venta.MetodoPago & ")"
                ' Llamar a la función e insertar el registro
                If InsertarRegistro(tabla, sql) Then

                    ' Limpiar los controles
                    For i As Integer = 1 To DatosGlobales.ListaProductos.Count

                        Dim txtCantidad As TextBox = TryCast(tblBotones.Controls("TextBoxCantidad" & i), TextBox)
                        Dim txtTotal As TextBox = TryCast(tblBotones.Controls("TextBoxSubTotal" & i), TextBox)
                        Dim labelElementos As Label = TryCast(tblBotones.Controls("Label" & i), Label)

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

            Dim texto As String = ""
            ' Reset de la impresora
            texto &= Chr(&H1B) & "@"
            ' Fuente A (12pt), con negrita
            texto &= Chr(&H1B) & "!" & Chr(16)


            Dim FechaHora As Date = Now

            ' Texto a imprimir (asegurarse de que cada línea tenga hasta 48 caracteres)

            texto = texto & "================================================" & vbCrLf
            texto = texto & "                     ADJC                       " & vbCrLf
            texto = texto & "================================================" & vbCrLf
            texto = texto & "Fecha: " & FechaHora.ToString & " " & vbCrLf
            texto = texto & "Ticket Nº: " & LabelNumTicket.Text - 1 & "  " & vbCrLf
            texto = texto & "Evento/Cajeros: " & DatosGlobales.cajeros.Apellidos.ToString & "  " & vbCrLf
            texto = texto & "Cant Detalle                             Monto  " & vbCrLf
            texto = texto & "------------------------------------------------" & vbCrLf
            For i = 1 To DatosGlobales.ListaProductos.Count

                Dim propiedad As System.Reflection.PropertyInfo = venta.GetType().GetProperty("Cantidad" & i)
                Dim nombre As String = DatosGlobales.ListaProductos(i - 1).Nombre.ToString
                Dim precio As Double = DatosGlobales.ListaProductos(i - 1).Precio
                If propiedad.GetValue(venta) > 0 Then

                    texto = texto & " " & propiedad.GetValue(venta).ToString.PadLeft(4) & "  " & nombre.ToString.PadRight(34) & "$" & precio * propiedad.GetValue(venta).ToString.PadLeft(6) & vbCrLf

                    texto = texto & "------------------------------------------------" & vbCrLf
                End If
            Next
            texto = texto & "================================================" & vbCrLf
            Dim propiedad2 As System.Reflection.PropertyInfo = venta.GetType().GetProperty("TotalVentas")
            Dim totalTicket As Double = Convert.ToDouble(propiedad2.GetValue(venta))
            texto = texto & "TOTAL DEL TICKET             $" & totalTicket.ToString("N0") & vbCrLf
            texto = texto & "================================================" & vbCrLf

            texto &= Chr(&H1D) & "V" & Chr(66) & Chr(0) ' Full cut con espera

            ' Enviar a la impresora
            RawPrinterHelper.SendStringToPrinter(Configuraciones.nombreImpresora, texto)


        Catch ex As Exception
            MessageBox.Show("Error Impresion: Revise en el menu principal la impresora predeterminada " & ex.Message)
            MessageBox.Show("Revise la tabla principal para confirmar si el ticket fue guardado y reimprimirlo")
            Me.Dispose()
            Menu.Show()
        End Try
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