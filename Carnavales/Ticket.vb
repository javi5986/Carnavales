Imports System.ComponentModel
Imports System.Drawing.Printing
Imports System.Text
Imports Windows.Media.Casting

Public Class Ticket


    Private Sub Ticket_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing

        ' Al cerrar el formulario, mostrar el menú principal
        Menu.Show()

    End Sub

    Private Sub Salir_Click(sender As Object, e As EventArgs) Handles Salir.Click

        ' Cerrar el formulario y mostrar el menú principal
        Menu.Show()
        Me.Close()

    End Sub


    Private Sub Ticket_Shown(sender As Object, e As EventArgs) Handles Me.Load

        ' Cargar los datos globales y la lista de productos
        DatosGlobales.cajeros = DatosGlobales.ObtenerCajeros()

        ' Detectar tamaño del monitor
        Dim screenWidth As Integer = Screen.PrimaryScreen.Bounds.Width

        ' Si es menor a 1300px de ancho (monitor chico)
        If screenWidth < 1300 Then
            ' Reducir fuentes de los controles del formulario
            ReducirFuentes()
        End If

        ' Inicializar los controles de la tabla de botones
        CheckBoxEfectivo.ForeColor = Color.Green
        LabelNumTicket.Text = DatosGlobales.ListaVentas.Count + 1

        ' Cargar los productos en los botones y textboxes
        For i As Integer = 1 To DatosGlobales.ListaProductos.Count

            ' Crear los controles dinámicamente si no existen
            Dim btnMas As Button = tblBotones.Controls("ButtonMas" & i)
            Dim btnMenos As Button = tblBotones.Controls("ButtonMenos" & i)
            Dim textCantidad As TextBox = tblBotones.Controls("TextBoxCantidad" & i)
            Dim textSubTotal As TextBox = tblBotones.Controls("TextBoxSubTotal" & i)

            ' Crear los TextBox para Cantidad y SubTotal si no existen
            If btnMas IsNot Nothing Then

                btnMas.Text = DatosGlobales.ListaProductos(i - 1).Nombre
                btnMas.Dock = DockStyle.Fill
                btnMas.Tag = i
                btnMenos.Tag = i

                ' Verifica si el texto está vacío o solo espacios
                If String.IsNullOrWhiteSpace(btnMas.Text) Then
                    ' Si el texto está vacío, deshabilita los botones
                    btnMas.Enabled = False
                    btnMenos.Enabled = False
                Else
                    ' Si el texto no está vacío, habilita los botones
                    btnMas.Enabled = True
                    btnMenos.Enabled = True
                    ' Crear los TextBox para Cantidad y SubTotal si no existen
                    AddHandler btnMas.Click, AddressOf BotonCantidad_Click
                    AddHandler btnMenos.Click, AddressOf BotonCantidad_Click
                End If
            End If

        Next

    End Sub

    Private Sub ReducirFuentes()

        ' Cambiar fuente de todos los controles del formulario a menor tamaño
        For Each ctrl As Control In Me.Controls
            ' Ajustar fuente de controles individuales
            AjustarFuente(ctrl)
        Next
    End Sub

    Private Sub AjustarFuente(ctrl As Control)

        ' Recursivo para contenedores como GroupBox, Panel, TableLayoutPanel
        If TypeOf ctrl Is ContainerControl OrElse TypeOf ctrl Is Panel OrElse TypeOf ctrl Is TableLayoutPanel Then
            ' Ajustar fuente de los controles dentro del contenedor
            For Each subCtrl As Control In ctrl.Controls
                ' Llamar recursivamente para ajustar fuente de subcontroles
                AjustarFuente(subCtrl)
            Next
        End If

        ' Cambiar fuente si es Label, Button o TextBox
        If TypeOf ctrl Is Label OrElse TypeOf ctrl Is Button OrElse TypeOf ctrl Is TextBox Then
            ' Reducir el tamaño de la fuente en 4 puntos
            ctrl.Font = New Font(ctrl.Font.FontFamily, ctrl.Font.Size - 4)
        End If

    End Sub



    Private Sub BotonCantidad_Click(sender As Object, e As EventArgs)

        Try
            ' Obtener el botón que disparó el evento
            Dim boton As Button = DirectCast(sender, Button)
            Dim index As Integer = CInt(boton.Tag)

            ' Validar que el índice sea correcto
            Dim txtCantidad As TextBox = TryCast(tblBotones.Controls("TextBoxCantidad" & index), TextBox)
            Dim txtTotal As TextBox = TryCast(tblBotones.Controls("TextBoxSubTotal" & index), TextBox)

            ' Intentamos leer la cantidad, si falla, la ponemos en 0
            Dim cantidad As Integer = 0

            ' Si txtCantidad es Nothing, significa que no se encontró el TextBox correspondiente
            If Not Integer.TryParse(txtCantidad.Text, cantidad) Then cantidad = 0
            ' si text es - , restamos 1 unidad, si ya es cero , no hacemos nada
            If boton.Text = "-" Then
                If cantidad > 0 Then
                    cantidad -= 1
                End If
                ' Actualizar el color de fondo del TextBox según la cantidad
                If cantidad = 0 Then
                    txtCantidad.BackColor = Color.WhiteSmoke
                    txtTotal.BackColor = Color.WhiteSmoke
                End If
            Else
                ' si text tiene texto, sumamos 1 unidad
                cantidad += 1
                ' Actualizar el color de fondo del TextBox según la cantidad
                txtCantidad.BackColor = Color.LightGreen
                txtTotal.BackColor = Color.LightGreen
            End If

            ' Actualizar el TextBox de cantidad
            txtCantidad.Text = cantidad.ToString()

            ' Validar que el índice sea correcto
            If index > 0 AndAlso index <= DatosGlobales.ListaProductos.Count Then
                ' Actualizar el TextBox de SubTotal
                Dim precio As Decimal = DatosGlobales.ListaProductos(index - 1).Precio
                txtTotal.Text = (precio * cantidad).ToString("N0")
            End If

            ' Función para actualizar el total general
            ActualizarTotalGeneral()

        Catch ex As Exception
            ' Mostrar un mensaje de error si ocurre una excepción
            MessageBox.Show("Error al actualizar cantidad: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub ActualizarTotalGeneral()

        ' Calcular el total general sumando los subtotales de cada producto
        Dim total As Double = 0

        ' Recorremos los TextBox de SubTotal para sumar sus valores
        For i As Integer = 1 To DatosGlobales.ListaProductos.Count

            ' Intentamos obtener el TextBox de SubTotal correspondiente
            Dim txtTotal As TextBox = TryCast(tblBotones.Controls("TextBoxSubTotal" & i), TextBox)

            ' Si el TextBox no es Nothing, intentamos sumar su valor
            If txtTotal IsNot Nothing Then

                ' Intentamos convertir el texto a Double, si falla, lo ignoramos
                total += Double.Parse(txtTotal.Text)

            End If
        Next

        ' Actualizar el TextBox de Total con el valor calculado
        TextBoxTotal.Text = total.ToString("N0")

    End Sub


    Private Sub Imprimir_Click(sender As Object, e As EventArgs) Handles Imprimir.Click

        ' Validar que haya productos seleccionados antes de proceder a imprimir
        If TextBoxTotal.Text = "0" Then

            ' Si el total es 0, significa que no hay productos seleccionados
            MessageBox.Show("No hay productos seleccionados", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

        Else

            ' Si hay productos seleccionados, proceder a crear el objeto de venta
            Dim venta As New Ventas()

            ' Asignar las propiedades del objeto venta
            venta.TotalVentas = Convert.ToDouble(TextBoxTotal.Text)

            ' Asignar el método de pago según la selección del usuario
            If CheckBoxEfectivo.Checked = True Then
                ' Si se selecciona Efectivo
                venta.MetodoPago = True
            Else
                ' Si se selecciona Transferencia
                venta.MetodoPago = False
            End If

            ' Asignar las cantidades de cada producto al objeto venta
            For i As Integer = 1 To DatosGlobales.ListaProductos.Count

                ' Intentamos obtener el TextBox de Cantidad correspondiente
                Dim txt As TextBox = TryCast(tblBotones.Controls("TextBoxCantidad" & i), TextBox)

                ' Si el TextBox no es Nothing, intentamos asignar su valor a la propiedad del objeto venta
                If txt IsNot Nothing Then

                    ' Intentamos obtener la propiedad "Cantidad" correspondiente al índice
                    Dim propiedad As System.Reflection.PropertyInfo = venta.GetType().GetProperty("Cantidad" & i)

                    ' Si la propiedad existe, asignamos el valor del TextBox convertido a entero
                    If propiedad IsNot Nothing Then
                        Dim valor = If(String.IsNullOrWhiteSpace(txt.Text) OrElse Not IsNumeric(txt.Text), 0, Convert.ToInt32(txt.Text))
                        propiedad.SetValue(venta, valor)
                    End If


                End If

            Next

            ' Luego de tener la venta armada, realizamos la inserción en la base de datos
            Try

                ' Definir la tabla y la consulta SQL para insertar el registro
                Dim tabla As String = "Ventas"

                ' Definimos la fecha actual
                venta.Fecha = Now
                Dim fechaHoraActual As String = venta.Fecha.ToString("MM/dd/yyyy HH:mm:ss")

                Dim sql As String = "INSERT INTO Ventas (Cantidad1,Cantidad2,Cantidad3,Cantidad4,Cantidad5,Cantidad6,Cantidad7,Cantidad8,Cantidad9,Cantidad10,Cantidad11,Cantidad12,Cantidad13,Cantidad14,Cantidad15,Cantidad16,Cantidad17,Cantidad18,Cantidad19,Cantidad20,Cantidad21,Cantidad22,Cantidad23,Cantidad24,Cantidad25,Cantidad26,Cantidad27,TotalVentas,Efectivo,Fecha) VALUES (" & venta.Cantidad1 & "," & venta.Cantidad2 & "," & venta.Cantidad3 & "," & venta.Cantidad4 & "," & venta.Cantidad5 & "," & venta.Cantidad6 & "," & venta.Cantidad7 & "," & venta.Cantidad8 & "," & venta.Cantidad9 & "," & venta.Cantidad10 & "," & venta.Cantidad11 & "," & venta.Cantidad12 & "," & venta.Cantidad13 & "," & venta.Cantidad14 & "," & venta.Cantidad15 & "," & venta.Cantidad16 & "," & venta.Cantidad17 & "," & venta.Cantidad18 & "," & venta.Cantidad19 & "," & venta.Cantidad20 & "," & venta.Cantidad21 & "," & venta.Cantidad22 & "," & venta.Cantidad23 & "," & venta.Cantidad24 & "," & venta.Cantidad25 & "," & venta.Cantidad26 & "," & venta.Cantidad27 & "," & venta.TotalVentas & "," & venta.MetodoPago & ",#" & fechaHoraActual & "#)"
                ' Llamar a la función e insertar el registro
                If InsertarRegistro(tabla, sql) Then

                    ' Limpiar los controles
                    For i As Integer = 1 To DatosGlobales.ListaProductos.Count

                        ' setear los TextBox de Cantidad y SubTotal a 0
                        Dim txtCantidad As TextBox = TryCast(tblBotones.Controls("TextBoxCantidad" & i), TextBox)
                        Dim txtTotal As TextBox = TryCast(tblBotones.Controls("TextBoxSubTotal" & i), TextBox)
                        Dim labelElementos As Label = TryCast(tblBotones.Controls("Label" & i), Label)

                        ' Si el TextBox no es Nothing, lo limpiamos
                        If txtCantidad IsNot Nothing Then

                            ' Limpiar el TextBox de Cantidad y SubTotal
                            txtTotal.BackColor = Color.WhiteSmoke
                            txtCantidad.BackColor = Color.WhiteSmoke
                            txtCantidad.Text = "0"
                            txtTotal.Text = "0"

                        End If

                    Next

                    ' Limpiar el TextBox de Total
                    TextBoxTotal.Text = "0"

                End If

                ' impresión del ticket
                Impresion(venta)

                ' Borrar el objeto venta
                venta = Nothing

            Catch ex As Exception
                ' Si ocurre un error al ejecutar la consulta SQL, mostrar un mensaje de error
                MessageBox.Show("Error al ejecutar SQL: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Application.Exit()

            Finally

                ' Aumentar el número de ticket en el Label
                If LabelNumTicket.Text IsNot Nothing Then
                    ' Verificar si el LabelNumTicket tiene un valor numérico válido
                    Dim valor = If(String.IsNullOrWhiteSpace(LabelNumTicket.Text) OrElse Not IsNumeric(LabelNumTicket.Text), 0, Convert.ToInt32(LabelNumTicket.Text))
                    ' Aumentar el valor en 1
                    LabelNumTicket.Text = (valor + 1).ToString
                End If

                ' Volvemos a setear el check efectivo como seleccionado
                CheckBoxEfectivo.Checked = True
                CheckBoxtTransferencia.Checked = False

            End Try

        End If

    End Sub



    Private Sub Impresion(venta As Ventas)
        Try
            ' Crear un objeto de tipo String para almacenar el texto a imprimir
            Dim texto As String = ""
            ' Reset de la impresora
            texto &= Chr(&H1B) & "@"
            ' Fuente A (12pt), con negrita
            texto &= Chr(&H1B) & "!" & Chr(16)


            Dim FechaHora As Date = Now

            ' Texto a imprimir (asegurarse de que cada línea tenga hasta 48 caracteres)            
            ' LOS TICKET DE VENTA PUEDEN TENER HASTA 9999 UNIDADES DE CADA PRODUCTO
            ' EL LARGO DE LOS NOMBRES DE LOS PRODUCTOS NO DEBE EXCEDER LOS 32 CARACTERES
            ' EL PRECIO SE MUESTRA COMO DOUBLE SIN DECIMALES HASTA  $9.999.999

            texto = texto & "================================================" & vbCrLf
            texto = texto & "                     ADJC                       " & vbCrLf
            texto = texto & "================================================" & vbCrLf
            texto = texto & "Fecha: " & venta.Fecha & " " & vbCrLf
            texto = texto & "Ticket Nº: " & LabelNumTicket.Text & "  " & vbCrLf
            texto = texto & "Evento/Cajeros: " & DatosGlobales.cajeros.Apellidos.ToString & "  " & vbCrLf
            texto = texto & "Cant    Detalle                          Monto  " & vbCrLf
            texto = texto & "------------------------------------------------" & vbCrLf
            ' Recorremos los productos y sus cantidades para armar el ticket
            For i = 1 To DatosGlobales.ListaProductos.Count

                ' Obtenemos la propiedad de cantidad del objeto venta
                Dim propiedad As System.Reflection.PropertyInfo = venta.GetType().GetProperty("Cantidad" & i)

                ' Verificamos si la propiedad tiene un valor mayor a 0
                If propiedad.GetValue(venta) > 0 Then

                    Dim nombre As String = DatosGlobales.ListaProductos(i - 1).Nombre.ToString
                    Dim precio As Double = DatosGlobales.ListaProductos(i - 1).Precio
                    Dim subTotal As Double = precio * propiedad.GetValue(venta)

                    texto = texto & propiedad.GetValue(venta).ToString.PadLeft(4) & " " & nombre.ToString.PadRight(33) & "$" & subTotal.ToString("N0").PadLeft(9) & vbCrLf

                    texto = texto & "------------------------------------------------" & vbCrLf
                End If
            Next
            texto = texto & "================================================" & vbCrLf
            ' Agregar el total del ticket
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

        ' Cambiar el color del texto del CheckBox según su estado
        If CheckBoxtTransferencia.Checked = True Then
            ' Si se selecciona Transferencia, desmarcar Efectivo
            CheckBoxEfectivo.Checked = False
            CheckBoxtTransferencia.ForeColor = Color.Blue
            CheckBoxEfectivo.ForeColor = Color.Black
        Else
            ' Si se desmarca Transferencia, marcar Efectivo
            CheckBoxEfectivo.Checked = True
            CheckBoxEfectivo.ForeColor = Color.Green
            CheckBoxtTransferencia.ForeColor = Color.Black
        End If
    End Sub

    Private Sub CheckBoxEfectivo_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBoxEfectivo.CheckedChanged

        ' Cambiar el color del texto del CheckBox según su estado
        If CheckBoxEfectivo.Checked = True Then
            ' Si se selecciona Efectivo, desmarcar Transferencia
            CheckBoxtTransferencia.Checked = False
            CheckBoxtTransferencia.ForeColor = Color.Black
            CheckBoxEfectivo.ForeColor = Color.Green
        Else
            ' Si se desmarca Efectivo, marcar Transferencia
            CheckBoxtTransferencia.Checked = True
            CheckBoxtTransferencia.ForeColor = Color.Blue
            CheckBoxEfectivo.ForeColor = Color.Black
        End If
    End Sub

End Class