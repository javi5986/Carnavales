Imports System.CodeDom
Imports System.ComponentModel
Imports System.Data.OleDb
Imports System.Drawing.Printing
Imports System.IO

Public Class Menu

    Private ToolTipConfigurar As New ToolTip()

    Private Sub Menu_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing

        Application.Exit()

    End Sub

    Private Sub Salir_Click(sender As Object, e As EventArgs) Handles Salir.Click

        Application.Exit()

    End Sub

    Private Sub Cobrar_Click(sender As Object, e As EventArgs) Handles Cobrar.Click

        Me.Hide()
        Ticket.Show()

    End Sub
    Private Sub Menu_VisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged

        ActualizarDataViewGrid()

    End Sub

    Private Sub ActualizarDataViewGrid()

        DatosGlobales.ListaVentas = DatosGlobales.ObtenerVentas()   ' Método que consulta la BD y actualiza el DataGridView
        DataGridView1.DataSource = Nothing
        DataGridView1.DataSource = DatosGlobales.ListaVentas

        ' Ocultar columnas innecesarias
        With DataGridView1
            .Columns("ID").HeaderText = "ID"
            .Columns("ID").ReadOnly = True
            .Columns("ID").Width = 100
            .Columns("ID").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("TotalVentas").HeaderText = "Monto Total"
            .Columns("TotalVentas").ReadOnly = True
            .Columns("TotalVentas").Width = 200
            .Columns("TotalVentas").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight

            .Columns("TotalVentas").DefaultCellStyle.Format = "N0"

            .Columns("Anulado").HeaderText = "Anulado"
            .Columns("Anulado").Width = 140
            .Columns("Anulado").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("MetodoPago").HeaderText = "Efectivo"
            .Columns("MetodoPago").Width = 140
            .Columns("MetodoPago").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

            ' Asegurar que solo se muestren las columnas necesarias
            For Each col As DataGridViewColumn In .Columns
                If col.Name <> "ID" AndAlso col.Name <> "TotalVentas" AndAlso col.Name <> "Anulado" AndAlso col.Name <> "MetodoPago" Then
                    col.Visible = False
                End If
            Next
        End With

        For i = 1 To DataGridView1.Rows.Count
            If DataGridView1.Rows(DataGridView1.Rows.Count - i).Cells("Anulado").Value Then
                DataGridView1.Rows(DataGridView1.Rows.Count - i).DefaultCellStyle.BackColor = Color.Red
            End If
        Next
        ' Ajustar el ancho de las columnas automáticamente
        If DataGridView1.Rows.Count > 0 Then

            DataGridView1.FirstDisplayedScrollingRowIndex = DataGridView1.Rows.Count - 1

        End If

        ' ACTUALIZAMOS EL TOTAL DE VENTAS

        ' Filtramos solo las ventas no anuladas
        Dim ventasValidas As IEnumerable(Of Ventas) = DatosGlobales.ListaVentas.Where(Function(v) Not v.Anulado)

        ' Sumamos el total vendido
        Dim montoTotal As Double = ventasValidas.Sum(Function(v) v.TotalVentas)
        TxtTotalVentas.Text = montoTotal.ToString("N0")

        If DatosGlobales.ListaVentas IsNot Nothing AndAlso DatosGlobales.ListaVentas.Count > 0 Then
            ' Simula desactivación
            ConfigurarProductos.Enabled = True
            ConfigurarProductos.ForeColor = Color.Gray
            ConfigurarProductos.BackColor = Color.LightGray
            ConfigurarProductos.Cursor = Cursors.No
            ConfigurarProductos.Tag = "Bloqueado"

            ToolTipConfigurar.SetToolTip(ConfigurarProductos, "No se pueden cambiar los precios si ya tiene ventas realizadas." & vbCrLf & "Para modificar precios debe Cerrar Caja y luego eliminar todas las Ventas.")
        Else
            ConfigurarProductos.Enabled = True
            ConfigurarProductos.ForeColor = SystemColors.ControlText
            ConfigurarProductos.BackColor = SystemColors.Control
            ConfigurarProductos.Cursor = Cursors.Default
            ConfigurarProductos.Tag = ""

            ToolTipConfigurar.SetToolTip(ConfigurarProductos, "")
        End If




    End Sub

    Private Sub Menu_Load(sender As Object, e As EventArgs) Handles Me.Load

        DatosGlobales.ObtenerProductos()
        ActualizarDataViewGrid()

    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick

        ' Verificar que la fila seleccionada no sea la fila de encabezado

        If e.ColumnIndex = -1 Or e.ColumnIndex = 0 Or e.ColumnIndex = 28 And e.RowIndex >= 0 Then
            Reimprimir.Enabled = True

        Else
            Reimprimir.Enabled = False

        End If

    End Sub

    Private Sub DataGridView1_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellValueChanged

        ' Verificar que la celda modificada pertenece a la columna "Anulado"
        If e.RowIndex >= 0 AndAlso DataGridView1.Columns(e.ColumnIndex).Name = "Anulado" Then

            Dim estadoActual As Boolean = DataGridView1.Rows(e.RowIndex).Cells("Anulado").Value

            If estadoActual Then

                Dim idVenta As Integer = DataGridView1.CurrentRow.Cells("ID").Value
                Dim tabla As String = "Ventas"
                Dim sql As String = "UPDATE Ventas SET Anulado = True WHERE ID = " & idVenta
                If DataBase.EditarRegistro(tabla, sql) Then
                    MessageBox.Show("El ticket ha sido ANULADO.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    DataGridView1.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.Red

                    ' ACTUALIZAMOS EL TOTAL DE VENTAS
                    ' actualizamos lista de ventas
                    DatosGlobales.ListaVentas = DatosGlobales.ObtenerVentas()
                    ' Filtramos solo las ventas no anuladas
                    Dim ventasValidas As IEnumerable(Of Ventas) = DatosGlobales.ListaVentas.Where(Function(v) Not v.Anulado)

                    ' Sumamos el total vendido
                    Dim montoTotal As Double = ventasValidas.Sum(Function(v) v.TotalVentas)
                    TxtTotalVentas.Text = montoTotal.ToString("N0")
                End If
            Else
                Dim idVenta As Integer = DataGridView1.CurrentRow.Cells("ID").Value
                Dim tabla As String = "Ventas"
                Dim sql As String = "UPDATE Ventas SET Anulado = False WHERE ID = " & idVenta
                If DataBase.EditarRegistro(tabla, sql) Then
                    MessageBox.Show("El ticket ha sido RESTAURADO.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    DataGridView1.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.White
                    ' ACTUALIZAMOS EL TOTAL DE VENTAS
                    ' actualizamos lista de ventas
                    DatosGlobales.ListaVentas = DatosGlobales.ObtenerVentas()
                    ' Filtramos solo las ventas no anuladas
                    Dim ventasValidas As IEnumerable(Of Ventas) = DatosGlobales.ListaVentas.Where(Function(v) Not v.Anulado)

                    ' Sumamos el total vendido
                    Dim montoTotal As Double = ventasValidas.Sum(Function(v) v.TotalVentas)
                    TxtTotalVentas.Text = montoTotal.ToString("N0")
                End If
            End If
        End If

        ' Verificar que la celda modificada pertenece a la columna "Efectivo"
        If e.RowIndex >= 0 AndAlso DataGridView1.Columns(e.ColumnIndex).Name = "MetodoPago" Then

            Dim estadoActual As Boolean = DataGridView1.Rows(e.RowIndex).Cells("MetodoPago").Value

            If estadoActual Then

                Dim idVenta As Integer = DataGridView1.CurrentRow.Cells("ID").Value
                Dim tabla As String = "Ventas"
                Dim sql As String = "UPDATE Ventas SET Efectivo = True WHERE ID = " & idVenta
                If DataBase.EditarRegistro(tabla, sql) Then
                    MessageBox.Show("Forma de PAGO MODIFICADA", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    ActualizarDataViewGrid()

                End If
            Else
                Dim idVenta As Integer = DataGridView1.CurrentRow.Cells("ID").Value
                Dim tabla As String = "Ventas"
                Dim sql As String = "UPDATE Ventas SET Efectivo = False WHERE ID = " & idVenta
                If DataBase.EditarRegistro(tabla, sql) Then
                    MessageBox.Show("Forma de PAGO MODIFICADA.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    ActualizarDataViewGrid()
                End If
            End If
        End If
    End Sub

    Private Sub DataGridView1_CurrentCellDirtyStateChanged(sender As Object, e As EventArgs) Handles DataGridView1.CurrentCellDirtyStateChanged
        If DataGridView1.CurrentCell IsNot Nothing AndAlso TypeOf DataGridView1.CurrentCell Is DataGridViewCheckBoxCell Then
            DataGridView1.EndEdit() ' Confirma el cambio y dispara CellValueChanged
        End If
    End Sub

    Private Sub Menu_Click(sender As Object, e As EventArgs) Handles Me.Click
        DataGridView1.ClearSelection()
        Reimprimir.Enabled = False
    End Sub

    Private Sub Reimprimir_Click(sender As Object, e As EventArgs) Handles Reimprimir.Click

        Dim idVenta As Integer = DataGridView1.CurrentRow.Cells("ID").Value
        Dim totalVenta As Double = DataGridView1.CurrentRow.Cells("TotalVentas").Value
        Dim anulado As Boolean = DataGridView1.CurrentRow.Cells("Anulado").Value

        Dim venta As Ventas = DatosGlobales.ListaVentas.Find(Function(v) v.ID = idVenta)

        If anulado Then
            MessageBox.Show("No se puede reimprimir un ticket anulado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

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
            texto = texto & "Ticket Nº: " & venta.ID.ToString & "  " & vbCrLf
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

        End Try
    End Sub

    Private Shared Sub ImprimirTexto(texto As String, p As PrintDocument)
        AddHandler p.PrintPage, Sub(sender As Object, e As PrintPageEventArgs)
                                    Dim font As New Font("Courier New", 12, FontStyle.Bold)
                                    e.Graphics.DrawString(texto, font, Brushes.Black, 0, 0)
                                End Sub
    End Sub

    Private Sub CerrarCaja_Click(sender As Object, e As EventArgs) Handles CerrarCaja.Click

        ' Verificar si hay ventas registradas
        If DatosGlobales.ListaVentas.Count = 0 Then
            MessageBox.Show("No hay ventas registradas para cerrar la caja.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        ' Preguntar al usuario si está seguro de cerrar la caja
        Dim respuesta = MessageBox.Show("¿Está seguro de que desea CERRAR LA CAJA ?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        ' Verificamos si el usuario confirma el cierre de caja
        If respuesta = DialogResult.Yes Then


            Dim ventas = ObtenerVentas()

            ' Filtramos solo las ventas no anuladas
            Dim ventasValidas = ventas.Where(Function(v) Not v.Anulado)

            ' Contamos los tickets (IDs únicos)
            Dim cantidadTickets = ventasValidas.Count

            ' Sumamos las cantidades vendidas de cada elemento
            Dim cantidades(ListaProductos.Count - 1) As Integer

            For Each venta In ventasValidas

                For i = 0 To ListaProductos.Count - 1

                    cantidades(i) += CInt(CallByName(venta, "Cantidad" & i + 1, CallType.Get))

                Next

            Next

            ' Filtramos solo las ventas Efectivo
            Dim ventasValidasEfectivo = ventasValidas.Where(Function(v) v.MetodoPago)
            Dim ventasValidasTransferencia = ventasValidas.Where(Function(v) Not v.MetodoPago)
            ' Sumamos el total vendido
            Dim VentasEfectivo = ventasValidasEfectivo.Sum(Function(v) v.TotalVentas)
            Dim VentasTransferencia = ventasValidasTransferencia.Sum(Function(v) v.TotalVentas)
            Dim montoTotal = Double.Parse(VentasEfectivo) + Double.Parse(VentasTransferencia)

            Try

                Dim texto = ""
                ' Reset de la impresora
                texto &= Chr(&H1B) & "@"
                ' Fuente A (12pt), con negrita
                texto &= Chr(&H1B) & "!" & Chr(16)

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

                For i = 1 To ListaProductos.Count

                    If cantidades(i - 1) = 0 Then
                        Continue For
                    End If
                    ' MONTO TOTAL POR PRODUCTO
                    Dim montoTotalCant = cantidades(i - 1) * ListaProductos(i - 1).Precio

                    texto = texto & cantidades(i - 1).ToString.PadLeft(5) & " " & ListaProductos(i - 1).Nombre.ToString.PadRight(33) & "$" & montoTotalCant.ToString.PadLeft(8) & vbCrLf

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
                MessageBox.Show("Error Impresion: Revise en el menu principal la impresora predeterminada " & ex.Message)
            End Try

        End If


    End Sub

    Private Sub ConfigurarImpresora_Click(sender As Object, e As EventArgs) Handles ConfigurarImpresora.Click

        Dim dialogo As New PrintDialog()
        dialogo.ShowDialog()
        Configuraciones.nombreImpresora = dialogo.PrinterSettings.PrinterName

    End Sub


    Sub ResetearTabla()

        Dim tabla As String = "Ventas" ' Nombre de la tabla
        Dim tablaCajeros As String = "Cajeros" ' Nombre de la tabla de cajeros
        Dim connStr As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Configuraciones.rutaDB
        Dim conn As New OleDbConnection(connStr)

        Try
            conn.Open()

            ' 1. Eliminar todos los registros de la tabla
            Dim cmdDelete As New OleDbCommand("DELETE FROM " & tabla, conn)
            ' Eliminar todos los registros de la tabla Ventas
            cmdDelete.ExecuteNonQuery()

            cmdDelete = New OleDbCommand("DELETE FROM " & tablaCajeros, conn)
            ' Eliminar todos los registros de la tabla Cajeros
            cmdDelete.ExecuteNonQuery()

            ' 2. Compactar la base de datos para resetear el autonumérico
            conn.Close()
            CompactarBaseDatos(Configuraciones.rutaDB)

            MessageBox.Show("Tabla reseteada y autonumérico reiniciado." & vbCrLf & "DEBE VOLVER A REGISTRARSE", "ATENCIÓN", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Application.Restart() ' Reiniciar la aplicación para que los cambios surtan efecto

        Catch ex As Exception
            MessageBox.Show("Error al resetear Base de Datos: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Exit()
        Finally
            If conn.State = ConnectionState.Open Then conn.Close()
        End Try
    End Sub

    Sub CompactarBaseDatos(dbPath As String)

        Dim tempPath As String = Path.ChangeExtension(dbPath, ".temp.accdb")

        Try
            ' Crear una instancia de DAO.DBEngine
            Dim dbe As Object = Activator.CreateInstance(Type.GetTypeFromProgID("DAO.DBEngine.120"))

            ' Compactar la base de datos a un archivo temporal
            dbe.CompactDatabase(dbPath, tempPath)

            ' Eliminar la base de datos original y reemplazarla con la compactada
            File.Delete(dbPath)
            File.Move(tempPath, dbPath)

            MessageBox.Show("Base de datos compactada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception
            MessageBox.Show("Error al compactar: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Application.Exit()
        End Try

    End Sub

    Private Sub ResetTabla_Click(sender As Object, e As EventArgs) Handles ResetTabla.Click

        Dim respuesta As DialogResult = MessageBox.Show("¿Está seguro de que desea resetear la tabla Ventas?" & vbCrLf & "Este proceso no puede revertirse y elimina todos los registros", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If respuesta = DialogResult.Yes Then

            ' Eliminar todos los datos de la tabla Ventas
            ResetearTabla()
            ' Actualizar el DataGridView después de resetear la tabla

            ActualizarDataViewGrid()
        End If
    End Sub

    Private Sub ConfigurarProductos_Click(sender As Object, e As EventArgs) Handles ConfigurarProductos.Click


        If ConfigurarProductos.Tag = "Bloqueado" Then
            ' No hacer nada
            Return
        End If

        ' Abrís el formulario solo si está permitido
        Precios.ShowDialog()

    End Sub
End Class