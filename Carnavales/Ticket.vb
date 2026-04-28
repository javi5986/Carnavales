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
            ' Reducir el tamaño de la fuente en 6 puntos
            ctrl.Font = New Font(ctrl.Font.FontFamily, ctrl.Font.Size - 6)
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

                    ' impresión del ticket
                    Impresion(venta)

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


                ' Borrar el objeto venta
                venta = Nothing

            Catch ex As Exception
                ' Si ocurre un error al ejecutar la consulta SQL, mostrar un mensaje de error
                MessageBox.Show("Error al guardar la venta: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
                ButtonMas1.Focus() ' Poner el foco en el primer botón para agilizar la venta
            End Try

        End If

    End Sub

    Private Sub Impresion(venta As Ventas)
        Try

            ' ==========================
            ' TICKETS DE COMIDA X UNIDAD
            ' ==========================

            ' RECORRER LOS PRIMEROS 4 PRODUCTOS QUE SON COMIDAS PARA IMPRESION POR SEPARADO
            For i = 1 To DatosGlobales.ListaProductos.Count

                ' Obtener el producto correspondiente al índice actual (restamos 1 porque la lista es 0-indexada)
                Dim producto As Producto = DatosGlobales.ListaProductos(i - 1)

                ' Verificar si el producto se imprime por unidad
                If producto.ImprimirPorUnidad Then

                    '   Obtener la propiedad "CantidadX" del objeto venta usando reflexión
                    Dim prop As System.Reflection.PropertyInfo = venta.GetType().GetProperty("Cantidad" & i)

                    '   Obtener el valor de la propiedad (cantidad vendida)
                    Dim cantidad As Integer = prop.GetValue(venta)

                    '   Si la cantidad vendida es mayor a 0, imprimir un ticket por cada unidad vendida
                    If cantidad > 0 Then

                        ' Obtener el nombre y precio del producto desde la lista global
                        Dim nombre As String = DatosGlobales.ListaProductos(i - 1).Nombre
                        Dim precio As Double = DatosGlobales.ListaProductos(i - 1).Precio

                        ' 1 ticket por unidad
                        For x = 1 To cantidad

                            ' Crear un objeto de tipo String para almacenar el texto a imprimir
                            Dim texto As String = ""
                            ' Reset de la impresora
                            texto &= Chr(&H1B) & "@"
                            ' Fuente A (12pt), con negrita
                            texto &= Chr(&H1B) & "!" & Chr(16)

                            ' Texto a imprimir (asegurarse de que cada línea tenga hasta 48 caracteres)            
                            ' LOS TICKET DE VENTA PUEDEN TENER HASTA 9999 UNIDADES DE CADA PRODUCTO
                            ' EL LARGO DE LOS NOMBRES DE LOS PRODUCTOS NO DEBE EXCEDER LOS 32 CARACTERES
                            ' EL PRECIO SE MUESTRA COMO DOUBLE SIN DECIMALES HASTA  $9.999.999

                            texto = texto & "================================================" & vbCrLf

                            ' Centrar
                            texto &= Chr(&H1B) & "a" & Chr(1)

                            ' Fuente grande: doble ancho + doble alto + negrita (8+16+32=56)
                            texto &= Chr(&H1B) & "!" & Chr(56)

                            ' El número con espacios de padding para que el bloque negro sea ancho
                            texto = texto & "A.D.J.C" & vbCrLf

                            ' Volver a fuente normal y alineación izquierda
                            texto &= Chr(&H1B) & "!" & Chr(16)
                            texto &= Chr(&H1B) & "a" & Chr(0)

                            ' ─────────────────────────────────────────────────────────────────

                            texto = texto & "================================================" & vbCrLf

                            texto &= "Fecha: " & venta.Fecha & vbCrLf
                            'texto &= "Ticket Nº: " & LabelNumTicket.Text & vbCrLf


                            ' ── Número de ticket grande — fondo negro, número blanco ─────────

                            ' Centrar
                            texto &= Chr(&H1B) & "a" & Chr(1)

                            ' Fuente grande: doble ancho + doble alto + negrita (8+16+32=56)
                            texto &= Chr(&H1B) & "!" & Chr(56)

                            ' Activar impresión invertida (fondo negro, texto blanco)
                            texto &= Chr(&H1D) & "B" & Chr(1)

                            ' El número con espacios de padding para que el bloque negro sea ancho
                            texto &= "Ticket Nº: " & LabelNumTicket.Text & vbCrLf

                            ' Desactivar inversión
                            texto &= Chr(&H1D) & "B" & Chr(0)

                            ' Volver a fuente normal y alineación izquierda
                            texto &= Chr(&H1B) & "!" & Chr(16)
                            texto &= Chr(&H1B) & "a" & Chr(0)

                            ' ─────────────────────────────────────────────────────────────────


                            texto &= "Evento/Cajeros: " & DatosGlobales.cajeros.Apellidos & vbCrLf
                            texto &= "Cant    Detalle                          Monto  " & vbCrLf
                            texto &= "------------------------------------------------" & vbCrLf

                            texto &= "   1 " & nombre.PadRight(33) &
                             "$" & precio.ToString("N0").PadLeft(9) & vbCrLf

                            texto &= "================================================" & vbCrLf
                            texto &= Chr(&H1D) & "V" & Chr(66) & Chr(0)

                            RawPrinterHelper.SendStringToPrinter(
                        Configuraciones.nombreImpresora, texto)

                        Next

                    End If

                End If

            Next

            ' =========================
            ' TICKET RESTO 
            ' =========================

            ' contador para ver si solo tenemos ITEMS y no imprima ticket vacio 
            Dim cantBebidas As Integer = 0
            ' Crear un objeto de tipo String para almacenar el texto a imprimir
            Dim textoFinal As String = ""
            ' Reset de la impresora
            textoFinal &= Chr(&H1B) & "@"
            ' Fuente A (12pt), con negrita
            textoFinal &= Chr(&H1B) & "!" & Chr(16)

            textoFinal = textoFinal & "================================================" & vbCrLf

            ' Centrar
            textoFinal &= Chr(&H1B) & "a" & Chr(1)

            ' Fuente grande: doble ancho + doble alto + negrita (8+16+32=56)
            textoFinal &= Chr(&H1B) & "!" & Chr(56)

            ' El número con espacios de padding para que el bloque negro sea ancho
            textoFinal = textoFinal & "A.D.J.C" & vbCrLf

            ' Volver a fuente normal y alineación izquierda
            textoFinal &= Chr(&H1B) & "!" & Chr(16)
            textoFinal &= Chr(&H1B) & "a" & Chr(0)

            ' ─────────────────────────────────────────────────────────────────

            textoFinal = textoFinal & "================================================" & vbCrLf
            textoFinal &= "Fecha: " & venta.Fecha & vbCrLf
            'textoFinal &= "Ticket Nº: " & LabelNumTicket.Text & vbCrLf

            ' ── Número de ticket grande — fondo negro, número blanco ─────────

            ' Centrar
            textoFinal &= Chr(&H1B) & "a" & Chr(1)

            ' Fuente grande: doble ancho + doble alto + negrita (8+16+32=56)
            textoFinal &= Chr(&H1B) & "!" & Chr(56)

            ' Activar impresión invertida (fondo negro, texto blanco)
            textoFinal &= Chr(&H1D) & "B" & Chr(1)

            ' El número con espacios de padding para que el bloque negro sea ancho
            textoFinal &= "Ticket Nº: " & LabelNumTicket.Text & vbCrLf

            ' Desactivar inversión
            textoFinal &= Chr(&H1D) & "B" & Chr(0)

            ' Volver a fuente normal y alineación izquierda
            textoFinal &= Chr(&H1B) & "!" & Chr(16)
            textoFinal &= Chr(&H1B) & "a" & Chr(0)

            ' ─────────────────────────────────────────────────────────────────
            textoFinal &= "Evento/Cajeros: " & DatosGlobales.cajeros.Apellidos & vbCrLf
            textoFinal &= "Cant    Detalle                          Monto  " & vbCrLf
            textoFinal &= "------------------------------------------------" & vbCrLf

            ' Recorremos los productos y sus cantidades para armar el ticket
            For i = 1 To DatosGlobales.ListaProductos.Count

                ' Obtener el producto correspondiente al índice actual (restamos 1 porque la lista es 0-indexada)
                Dim producto As Producto = DatosGlobales.ListaProductos(i - 1)

                ' Verificar si el producto se imprime por unidad, si es así, ya se imprimió en el ciclo anterior, no lo incluimos en el ticket final
                If Not producto.ImprimirPorUnidad Then

                    ' Obtenemos la propiedad de cantidad del objeto venta
                    Dim prop As System.Reflection.PropertyInfo = venta.GetType().GetProperty("Cantidad" & i)
                    ' sumamos la cantidad total de bebidas
                    cantBebidas += prop.GetValue(venta)
                    ' Verificamos si la propiedad tiene un valor mayor a 0
                    If prop.GetValue(venta) > 0 Then

                        Dim nombre As String = DatosGlobales.ListaProductos(i - 1).Nombre
                        Dim precio As Double = DatosGlobales.ListaProductos(i - 1).Precio
                        Dim subtotal As Double = precio * prop.GetValue(venta)

                        textoFinal &= prop.GetValue(venta).ToString.PadLeft(4) & " " &
                              nombre.PadRight(33) & "$" &
                              subtotal.ToString("N0").PadLeft(9) & vbCrLf

                        textoFinal &= "------------------------------------------------" & vbCrLf

                    End If

                End If

            Next

            textoFinal &= "================================================" & vbCrLf
            ' Agregar el total del ticket
            Dim totalTicket As Double = venta.TotalVentas
            textoFinal &= "TOTAL DEL TICKET             $" &
            totalTicket.ToString("N0") & vbCrLf
            textoFinal &= "================================================" & vbCrLf
            textoFinal &= Chr(&H1D) & "V" & Chr(66) & Chr(0)
            ' Si no hay bebidas, no imprimir el ticket final
            If cantBebidas = 0 Then
                Return
            End If

            ' Enviar a la impresora
            RawPrinterHelper.SendStringToPrinter(
            Configuraciones.nombreImpresora, textoFinal)

        Catch ex As Exception
            'MessageBox.Show("Error Impresion: " & ex.Message)

            MessageBox.Show("Error Impresion: Revise en el menu principal la impresora predeterminada " & ex.Message)
            MessageBox.Show("Revise la tabla principal para confirmar si el ticket fue guardado y reimprimirlo")
            Me.Close()
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

    Private Sub Ticket_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        ' Detecta I o i (no distingue mayúsculas)
        If e.KeyCode = Keys.I Then
            Call Imprimir_Click(sender, e)
            e.Handled = True
        End If
        ' Detecta S o s (no distingue mayúsculas)
        If e.KeyCode = Keys.S Then
            ' Cerrar el formulario y mostrar el menú principal
            Menu.Show()
            Me.Close()
        End If
    End Sub

End Class