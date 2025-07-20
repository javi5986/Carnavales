Imports System.CodeDom
Imports System.ComponentModel
Imports System.Data.OleDb
Imports System.Drawing.Printing
Imports System.IO
Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class Menu

    Private Sub Menu_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        ' Confirmación de cierre de la aplicación
        Application.Exit()

    End Sub

    Private Sub Salir_Click(sender As Object, e As EventArgs) Handles Salir.Click
        ' Confirmación de cierre de la aplicación
        Application.Exit()

    End Sub

    Private Sub Cobrar_Click(sender As Object, e As EventArgs) Handles Cobrar.Click

        ' Mostrar el formulario de Cobro
        Me.Dispose()
        Ticket.Show()

    End Sub

    Private Sub Menu_Shown(sender As Object, e As EventArgs) Handles Me.Load

        ' Obtener la lista de productos al cargar el formulario
        DatosGlobales.ListaProductos = DatosGlobales.ObtenerProductos()
        ' Actualizar la lista de ventas al cargar el formulario
        ActualizarDataViewGrid()

    End Sub

    Private Sub ActualizarDataViewGrid()

        ' Actualizar el DataGridView con la lista de ventas
        DatosGlobales.ListaVentas = DatosGlobales.ObtenerVentas()
        Dim listaVentas = DatosGlobales.ListaVentas

        ' Limpiar el DataGridView antes de asignar la nueva fuente de datos
        DataGridView1.DataSource = Nothing

        ' Asignar la fuente de datos al DataGridView
        DataGridView1.DataSource = ListaVentas

        ' Ocultar columnas innecesarias
        With DataGridView1

            ' Configurar propiedades del DataGridView
            .AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
            .Columns("ID").HeaderText = "ID"
            .Columns("ID").ReadOnly = True
            .Columns("ID").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("TotalVentas").HeaderText = "Monto Total"
            .Columns("TotalVentas").ReadOnly = True
            .Columns("TotalVentas").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            .Columns("TotalVentas").DefaultCellStyle.Format = "N0"
            .Columns("Anulado").HeaderText = "Anulado"
            .Columns("Anulado").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("MetodoPago").HeaderText = "Efectivo"
            .Columns("MetodoPago").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            ' Asegurar que solo se muestren las columnas necesarias
            For Each col As DataGridViewColumn In .Columns

                ' Mostrar solo las columnas necesarias
                If col.Name <> "ID" AndAlso col.Name <> "TotalVentas" AndAlso col.Name <> "Anulado" AndAlso col.Name <> "MetodoPago" Then
                    ' Ocultar las columnas que no son necesarias
                    col.Visible = False
                End If
            Next

        End With

        ' Cambiar el color de fondo de las filas anuladas a rojo
        For i = 1 To DataGridView1.Rows.Count
            ' Verificar si la fila está anulada
            If DataGridView1.Rows(DataGridView1.Rows.Count - i).Cells("Anulado").Value Then
                ' Cambiar el color de fondo de la fila a rojo
                DataGridView1.Rows(DataGridView1.Rows.Count - i).DefaultCellStyle.BackColor = Color.Red
            End If
        Next

        ' Scroll al final del DataGridView
        If DataGridView1.Rows.Count > 0 Then
            DataGridView1.FirstDisplayedScrollingRowIndex = DataGridView1.Rows.Count - 1
        End If

        ' ACTUALIZAMOS EL TOTAL DE VENTAS

        ' Filtramos solo las ventas no anuladas
        Dim ventasValidas As IEnumerable(Of Ventas) = ListaVentas.Where(Function(v) Not v.Anulado)

        ' Sumamos el total vendido
        Dim montoTotal As Double = ventasValidas.Sum(Function(v) v.TotalVentas)
        TxtTotalVentas.Text = montoTotal.ToString("N0")

    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick

        ' Verificar que la fila seleccionada no sea la fila de encabezado
        If e.ColumnIndex = -1 Or e.ColumnIndex = 0 Or e.ColumnIndex = 28 And e.RowIndex >= 0 Then
            ' Si se hace clic en la columna de Anulado o Efectivo, no hacer nada
            Reimprimir.Enabled = True
        Else
            ' Si se hace clic en cualquier otra celda, deshabilitar el botón de Reimprimir
            Reimprimir.Enabled = False
        End If

    End Sub

    Private Sub DataGridView1_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellValueChanged

        ' Verificar que la celda modificada pertenece a la columna "Anulado"
        If e.RowIndex >= 0 AndAlso DataGridView1.Columns(e.ColumnIndex).Name = "Anulado" Then

            ' Obtener el estado actual de la celda "Anulado"
            Dim estadoActual As Boolean = DataGridView1.Rows(e.RowIndex).Cells("Anulado").Value

            ' Verificar si el estado actual es verdadero o falso
            If estadoActual Then

                ' Si el estado es verdadero, significa que se está anulando el ticket
                Dim idVenta As Integer = DataGridView1.CurrentRow.Cells("ID").Value
                ' Definimos el nombre de la tabla y la consulta SQL para actualizar el estado del ticket
                Dim tabla As String = "Ventas"
                Dim sql As String = "UPDATE Ventas SET Anulado = True WHERE ID = " & idVenta

                ' Ejecutamos la consulta para actualizar el estado del ticket a anulado
                If DataBase.EditarRegistro(tabla, sql) Then

                    ' Mostramos un mensaje de confirmación al usuario
                    MessageBox.Show("El ticket ha sido ANULADO.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    ' Cambiamos el color de fondo de la fila a rojo para indicar que está anulado
                    DataGridView1.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.Red

                    ' Actualizamos data grid view
                    ActualizarDataViewGrid()

                End If
            Else

                ' Si el estado es falso, significa que se está restaurando el ticket
                Dim idVenta As Integer = DataGridView1.CurrentRow.Cells("ID").Value
                ' Definimos el nombre de la tabla y la consulta SQL para actualizar el estado del ticket
                Dim tabla As String = "Ventas"
                ' Consulta SQL para restaurar el ticket
                Dim sql As String = "UPDATE Ventas SET Anulado = False WHERE ID = " & idVenta

                ' Ejecutamos la consulta para actualizar el estado del ticket a no anulado
                If DataBase.EditarRegistro(tabla, sql) Then

                    ' Mostramos un mensaje de confirmación al usuario
                    MessageBox.Show("El ticket ha sido RESTAURADO.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)

                    ' Cambiamos el color de fondo de la fila a blanco para indicar que está restaurado
                    DataGridView1.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.White

                    ' Actualizamos data grid view
                    ActualizarDataViewGrid()

                End If
            End If
        End If

        ' Verificar que la celda modificada pertenece a la columna "Efectivo"
        If e.RowIndex >= 0 AndAlso DataGridView1.Columns(e.ColumnIndex).Name = "MetodoPago" Then

            ' Obtener el estado actual de la celda "MetodoPago"
            Dim estadoActual As Boolean = DataGridView1.Rows(e.RowIndex).Cells("MetodoPago").Value

            ' Verificar si el estado actual, Verdadero es fectivo y Falso es transferencia
            ' Si el estado es verdadero, significa que se está cambiando a transferencia
            If estadoActual Then

                ' Guardamos el ID de la venta actual
                Dim idVenta As Integer = DataGridView1.CurrentRow.Cells("ID").Value
                ' Definimos el nombre de la tabla y la consulta SQL para actualizar el estado del ticket
                Dim tabla As String = "Ventas"
                Dim sql As String = "UPDATE Ventas SET Efectivo = True WHERE ID = " & idVenta

                ' Ejecutamos la consulta para actualizar el estado del ticket a efectivo
                If DataBase.EditarRegistro(tabla, sql) Then
                    ' Mostramos un mensaje de confirmación al usuario
                    MessageBox.Show("Forma de PAGO MODIFICADA", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)

                End If
            Else
                ' Si el estado es falso, significa que se está cambiando a efectivo
                ' Guardamos el ID de la venta actual
                Dim idVenta As Integer = DataGridView1.CurrentRow.Cells("ID").Value
                ' Definimos el nombre de la tabla y la consulta SQL para actualizar el estado del ticket
                Dim tabla As String = "Ventas"
                ' Consulta SQL para cambiar el estado del ticket a efectivo
                Dim sql As String = "UPDATE Ventas SET Efectivo = False WHERE ID = " & idVenta
                ' Ejecutamos la consulta para actualizar el estado del ticket a efectivo
                If DataBase.EditarRegistro(tabla, sql) Then
                    ' Mostramos un mensaje de confirmación al usuario
                    MessageBox.Show("Forma de PAGO MODIFICADA.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If
        End If
    End Sub

    Private Sub DataGridView1_CurrentCellDirtyStateChanged(sender As Object, e As EventArgs) Handles DataGridView1.CurrentCellDirtyStateChanged

        ' Verificar si la celda actual es una celda de casilla de verificación Forma de Pago
        If DataGridView1.CurrentCell IsNot Nothing AndAlso TypeOf DataGridView1.CurrentCell Is DataGridViewCheckBoxCell Then

            ' Si la celda está marcada, significa que se ha cambiado el valor de la casilla de verificación
            DataGridView1.EndEdit()
        End If
    End Sub

    Private Sub Menu_Click(sender As Object, e As EventArgs) Handles Me.Click

        ' Limpiar la selección del DataGridView y deshabilitar el botón de Reimprimir
        DataGridView1.ClearSelection()
        Reimprimir.Enabled = False

    End Sub

    Private Sub Reimprimir_Click(sender As Object, e As EventArgs) Handles Reimprimir.Click

        ' Funcion para reimprimir el ticket seleccionado
        Dim idVenta As Integer = DataGridView1.CurrentRow.Cells("ID").Value

        ' Encontrar la venta correspondiente en la lista de ventas
        Dim venta As Ventas = DatosGlobales.ListaVentas.Find(Function(v) v.ID = idVenta)

        ' Verificar si la venta no esta anulada
        If venta.Anulado Then

            ' Si la venta está anulada, mostrar un mensaje y no permitir reimpresión
            MessageBox.Show("No se puede reimprimir un ticket anulado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Try
            ' Crear cadena de texto para la impresión
            Dim texto As String = ""
            ' Reset de la impresora
            texto &= Chr(&H1B) & "@"
            ' Fuente A (12pt), con negrita
            texto &= Chr(&H1B) & "!" & Chr(16)

            ' Obtener la fecha y hora actual
            Dim FechaHora As Date = Now

            ' Texto a imprimir (asegurarse de que cada línea tenga hasta 48 caracteres)


            texto = texto & "================================================" & vbCrLf
            texto = texto & "                     ADJC                       " & vbCrLf
            texto = texto & "================================================" & vbCrLf

            texto = texto & "Fecha: " & venta.Fecha.ToString & " " & vbCrLf
            texto = texto & "Ticket Nº: " & venta.ID.ToString & "  " & vbCrLf
            texto = texto & "Evento/Cajeros: " & DatosGlobales.cajeros.Apellidos.ToString & "  " & vbCrLf
            texto = texto & "Cant Detalle                             Monto  " & vbCrLf
            texto = texto & "------------------------------------------------" & vbCrLf

            ' Iterar sobre los productos vendidos en la venta
            For i = 1 To DatosGlobales.ListaProductos.Count

                ' Obtener la propiedad de cantidad del producto
                Dim propiedad As System.Reflection.PropertyInfo = venta.GetType().GetProperty("Cantidad" & i)
                Dim nombre As String = DatosGlobales.ListaProductos(i - 1).Nombre.ToString
                Dim precio As Double = DatosGlobales.ListaProductos(i - 1).Precio

                ' Verificar si la cantidad del producto es mayor que 0
                If propiedad.GetValue(venta) > 0 Then

                    ' Agregar la línea al texto de impresión
                    texto = texto & " " & propiedad.GetValue(venta).ToString.PadLeft(4) & "  " & nombre.ToString.PadRight(34) & "$" & precio * propiedad.GetValue(venta).ToString.PadLeft(6) & vbCrLf
                    texto = texto & "------------------------------------------------" & vbCrLf
                End If
            Next
            texto = texto & "================================================" & vbCrLf
            ' Agregar el total del ticket
            Dim propiedad2 As System.Reflection.PropertyInfo = venta.GetType().GetProperty("TotalVentas")
            ' Obtener el total del ticket
            Dim totalTicket As Double = Convert.ToDouble(propiedad2.GetValue(venta))
            ' Agregar el total del ticket al texto de impresión
            texto = texto & "TOTAL DEL TICKET             $" & totalTicket.ToString("N0") & vbCrLf
            texto = texto & "================================================" & vbCrLf

            texto &= Chr(&H1D) & "V" & Chr(66) & Chr(0) ' Full cut con espera
            ' Enviar a la impresora
            RawPrinterHelper.SendStringToPrinter(Configuraciones.nombreImpresora, texto)

        Catch ex As Exception
            ' Si ocurre un error al enviar a la impresora, mostrar un mensaje de error
            MessageBox.Show("Error Impresion: Revise en el menu principal la impresora predeterminada " & ex.Message)

        End Try
    End Sub

    Private Sub CerrarCaja_Click(sender As Object, e As EventArgs) Handles CerrarCaja.Click

        ' Verificar si hay una impresora configurada
        Dim nombreImpresora As String = Configuraciones.nombreImpresora

        ' Si no hay impresora configurada, mostrar un mensaje y salir del procedimiento
        If String.IsNullOrEmpty(nombreImpresora) Then
            MessageBox.Show("Debe configurar una impresora antes de cerrar la caja.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        End If

        ' Verificar si hay ventas registradas
        If DatosGlobales.ListaVentas.Count = 0 Then
            ' Si no hay ventas registradas, mostrar un mensaje y salir del procedimiento
            MessageBox.Show("No hay ventas registradas para cerrar la caja.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        ' Preguntar al usuario si está seguro de cerrar la caja
        Dim respuesta = MessageBox.Show("¿Está seguro de que desea CERRAR LA CAJA ?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        ' Verificamos si el usuario confirma el cierre de caja
        If respuesta = DialogResult.Yes Then

            ' Obtenemos la lista de ventas
            'Dim ventas As List(Of Ventas) = ListaVentas()

            ' Filtramos solo las ventas no anuladas
            Dim ventasValidas = DatosGlobales.ListaVentas.Where(Function(v) Not v.Anulado)

            ' Contamos los tickets (IDs únicos)
            Dim cantidadTickets = ventasValidas.Count

            ' Sumamos las cantidades vendidas de cada elemento
            Dim cantidades(DatosGlobales.ListaProductos.Count - 1) As Integer

            ' Inicializamos las cantidades a 0
            For Each venta In ventasValidas
                ' Iteramos sobre cada venta y sumamos las cantidades de cada producto
                For i = 0 To DatosGlobales.ListaProductos.Count - 1
                    ' Obtenemos la cantidad de cada producto de la venta
                    cantidades(i) += CInt(CallByName(venta, "Cantidad" & i + 1, CallType.Get))

                Next

            Next

            ' Filtramos solo las ventas Efectivo
            Dim ventasValidasEfectivo = ventasValidas.Where(Function(v) v.MetodoPago)
            ' Filtramos solo las ventas Transferencia
            Dim ventasValidasTransferencia = ventasValidas.Where(Function(v) Not v.MetodoPago)

            ' Sumamos el total de ventas por método de pago
            Dim VentasEfectivo = ventasValidasEfectivo.Sum(Function(v) v.TotalVentas)
            ' Sumamos el total de ventas por transferencia
            Dim VentasTransferencia = ventasValidasTransferencia.Sum(Function(v) v.TotalVentas)
            ' Calculamos el monto total de ventas
            Dim montoTotal = Double.Parse(VentasEfectivo) + Double.Parse(VentasTransferencia)

            Try
                ' Crear cadena de texto para la impresión
                Dim texto = ""
                ' Reset de la impresora
                texto &= Chr(&H1B) & "@"
                ' Fuente A (12pt), con negrita
                texto &= Chr(&H1B) & "!" & Chr(16)

                ' Obtener la fecha y hora actual
                Dim FechaHora = Now

                ' Texto a imprimir (asegurarse de que cada línea tenga hasta 48 caracteres)
                texto = texto & "================================================" & vbCrLf
                texto = texto & "                     ADJC                       " & vbCrLf
                texto = texto & "================================================" & vbCrLf
                texto = texto & "Fecha: " & FechaHora.ToString & " " & vbCrLf
                texto = texto & "CIERRE DE CAJA " & vbCrLf
                texto = texto & "Evento/Cajeros: " & DatosGlobales.cajeros.Apellidos.ToString & "  " & vbCrLf
                texto = texto & "Cant de ticket: " & cantidadTickets & "  " & vbCrLf
                texto = texto & "Cant Detalle                             Monto  " & vbCrLf
                texto = texto & "------------------------------------------------" & vbCrLf

                ' Iterar sobre los productos vendidos
                For i = 1 To DatosGlobales.ListaProductos.Count

                    ' Verificar si la cantidad del producto es mayor que 0
                    If cantidades(i - 1) = 0 Then
                        ' Si la cantidad es 0, no imprimir nada y continuar con el siguiente producto
                        Continue For
                    End If

                    ' Calcular el monto total por producto
                    Dim montoTotalCant = cantidades(i - 1) * DatosGlobales.ListaProductos(i - 1).Precio
                    ' Agregar la línea al texto de impresión
                    texto = texto & cantidades(i - 1).ToString.PadLeft(5) & " " & DatosGlobales.ListaProductos(i - 1).Nombre.ToString.PadRight(33) & "$" & montoTotalCant.ToString.PadLeft(8) & vbCrLf
                    texto = texto & "------------------------------------------------" & vbCrLf

                Next i
                texto = texto & "================================================" & vbCrLf
                texto = texto & "TOTAL EFECTIVO      $" & VentasEfectivo.ToString("N0") & vbCrLf
                texto = texto & "TOTAL TRANSFERENCIA $" & VentasTransferencia.ToString("N0") & vbCrLf
                texto = texto & "TOTAL VENTAS        $" & montoTotal.ToString("N0") & vbCrLf
                texto = texto & "================================================" & vbCrLf

                texto &= Chr(&H1D) & "V" & Chr(66) & Chr(0) ' Full cut con espera
                ' Enviar a la impresora
                RawPrinterHelper.SendStringToPrinter(nombreImpresora, texto)

            Catch ex As Exception
                ' Si ocurre un error al enviar a la impresora, mostrar un mensaje de error
                MessageBox.Show("Error Impresion: Revise en el menu principal la impresora predeterminada " & ex.Message)
            Finally

                ' Al cerrar caja respaldamos la base de datos actual en una carpeta específica

                ' 1. Ruta del directorio donde está la app
                Dim rutaApp As String = AppDomain.CurrentDomain.BaseDirectory

                ' 2. Ruta de la carpeta de cierres de caja
                Dim rutaCierres As String = System.IO.Path.Combine(rutaApp, "CierresDeCaja")

                ' 3. Nombre único de subcarpeta según fecha y hora
                Dim nombreSubcarpeta As String = "Cierre_" & Now.ToString("yyyyMMdd_HHmmss")

                ' 4. Ruta completa de la subcarpeta de este cierre
                Dim rutaCierreActual As String = System.IO.Path.Combine(rutaCierres, nombreSubcarpeta)

                ' 5. Crear la carpeta si no existe
                If Not System.IO.Directory.Exists(rutaCierres) Then
                    System.IO.Directory.CreateDirectory(rutaCierres)
                End If

                If Not System.IO.Directory.Exists(rutaCierreActual) Then
                    System.IO.Directory.CreateDirectory(rutaCierreActual)
                End If

                ' 6. Ruta de la base de datos actual (ajustala si tu variable es distinta)
                Dim rutaBaseDatos As String = System.IO.Path.Combine(rutaApp, "Carnavales.accdb")
                ' Si tu variable es diferente, usá esa: Ej: Dim rutaBaseDatos As String = Configuraciones.rutaDB

                ' 7. Ruta de la copia que vas a guardar
                Dim rutaCopia As String = System.IO.Path.Combine(rutaCierreActual, "Carnavales.accdb")

                ' 8. Copiar la base de datos
                System.IO.File.Copy(rutaBaseDatos, rutaCopia, True) ' True para sobreescribir si existe

                ' 9. (Opcional) Podés guardar un log, resumen, o lo que quieras también en esa carpeta

                MessageBox.Show("Cierre de caja guardado en: " & rutaCopia, "Cierre de caja exitoso")

            End Try

        End If


    End Sub

    Private Sub ConfigurarImpresora_Click(sender As Object, e As EventArgs) Handles ConfigurarImpresora.Click

        ' Configurar la impresora predeterminada
        Dim dialogo As New PrintDialog()

        ' Establecer la impresora predeterminada
        dialogo.ShowDialog()

        ' Verificar si se seleccionó una impresora
        Configuraciones.nombreImpresora = dialogo.PrinterSettings.PrinterName

    End Sub


    Sub ResetearTabla()

        ' Función para resetear la tabla Ventas y reiniciar el autonumérico
        Dim tabla As String = "Ventas" ' Nombre de la tabla
        Dim tablaCajeros As String = "Cajeros" ' Nombre de la tabla de cajeros
        ' Cadena de conexión a la base de datos
        Dim connStr As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Configuraciones.rutaDB
        ' Crear una conexión a la base de datos
        Dim conn As New OleDbConnection(connStr)

        Try
            ' Abrir la conexión a la base de datos
            conn.Open()

            ' 1. Eliminar todos los registros de la tabla
            Dim cmdDelete As New OleDbCommand("DELETE FROM " & tabla, conn)

            ' Eliminar todos los registros de la tabla Ventas
            cmdDelete.ExecuteNonQuery()

            ' 2. Reiniciar el autonumérico de la tabla Ventas
            cmdDelete = New OleDbCommand("DELETE FROM " & tablaCajeros, conn)

            ' Eliminar todos los registros de la tabla Cajeros
            cmdDelete.ExecuteNonQuery()

            ' Cerrar la conexión
            conn.Close()

            ' 3. Reiniciar el autonumérico de la tabla Ventas
            CompactarBaseDatos(Configuraciones.rutaDB)

            ' Mensaje de éxito
            MessageBox.Show("Tabla reseteada y autonumérico reiniciado." & vbCrLf & "DEBE VOLVER A REGISTRARSE", "ATENCIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ' Reiniciar la aplicación para que los cambios surtan efecto
            Application.Restart() ' Reiniciar la aplicación para que los cambios surtan efecto

        Catch ex As Exception

            ' Si ocurre un error, mostrar un mensaje de error
            MessageBox.Show("Error al resetear Base de Datos: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Exit()
        Finally

            ' Asegurarse de cerrar la conexión si está abierta
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Sub CompactarBaseDatos(dbPath As String)

        ' Función para compactar la base de datos Access
        Dim tempPath As String = Path.ChangeExtension(dbPath, ".temp.accdb")

        Try
            ' Crear una instancia de DAO.DBEngine
            Dim dbe As Object = Activator.CreateInstance(Type.GetTypeFromProgID("DAO.DBEngine.120"))

            ' Compactar la base de datos a un archivo temporal
            dbe.CompactDatabase(dbPath, tempPath)

            ' Eliminar la base de datos original y reemplazarla con la compactada
            File.Delete(dbPath)
            ' Mover el archivo temporal al nombre original de la base de datos
            File.Move(tempPath, dbPath)
            ' Mensaje de éxito
            MessageBox.Show("Base de datos compactada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception

            ' Si ocurre un error, mostrar un mensaje de error
            MessageBox.Show("Error al compactar: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Exit()
        End Try

    End Sub

    Private Sub ResetTabla_Click(sender As Object, e As EventArgs) Handles ResetTabla.Click

        ' Confirmación antes de resetear la tabla Ventas
        Dim respuesta As DialogResult = MessageBox.Show("¿Está seguro de que desea resetear la tabla Ventas?" & vbCrLf & "Este proceso no puede revertirse y elimina todos los registros", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        ' Verificar si el usuario confirma el reset de la tabla
        If respuesta = DialogResult.Yes Then

            ' Eliminar todos los datos de la tabla Ventas
            ResetearTabla()

        End If
    End Sub

    Private Sub ConfigurarProductos_Click(sender As Object, e As EventArgs) Handles ConfigurarProductos.Click

        ' Mostrar el formulario de configuración de productos
        Precios.ShowDialog()

    End Sub
End Class