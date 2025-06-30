Imports System.CodeDom
Imports System.ComponentModel
Imports System.Data.OleDb
Imports System.Drawing.Printing
Imports System.IO

Public Class Menu

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
            .Columns("ID").Width = 120
            .Columns("ID").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("TotalVentas").HeaderText = "Monto Total"
            .Columns("TotalVentas").ReadOnly = True
            .Columns("TotalVentas").Width = 230
            .Columns("TotalVentas").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("Anulado").HeaderText = "Anulado"
            .Columns("Anulado").Width = 160
            .Columns("Anulado").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("MetodoPago").HeaderText = "Efectivo"
            .Columns("MetodoPago").Width = 160
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
        If DataGridView1.Rows.Count > 0 Then

            DataGridView1.FirstDisplayedScrollingRowIndex = DataGridView1.Rows.Count - 1

        End If

        ' ACTUALIZAMOS EL TOTAL DE VENTAS

        ' Filtramos solo las ventas no anuladas
        Dim ventasValidas As IEnumerable(Of Ventas) = DatosGlobales.ListaVentas.Where(Function(v) Not v.Anulado)

        ' Sumamos el total vendido
        Dim montoTotal As Double = ventasValidas.Sum(Function(v) v.TotalVentas)
        TxtTotalVentas.Text = montoTotal.ToString()

    End Sub

    Private Sub Menu_Load(sender As Object, e As EventArgs) Handles Me.Load

        DatosGlobales.ObtenerProductos()
        ActualizarDataViewGrid()

    End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        ' Verificar que la fila seleccionada no sea la fila de encabezado

        If e.ColumnIndex = -1 Or e.ColumnIndex = 0 Or e.ColumnIndex = 23 And e.RowIndex >= 0 Then
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
                    TxtTotalVentas.Text = montoTotal.ToString()
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
                    TxtTotalVentas.Text = montoTotal.ToString()
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
        Dim totalVenta As Decimal = DataGridView1.CurrentRow.Cells("TotalVentas").Value
        Dim anulado As Boolean = DataGridView1.CurrentRow.Cells("Anulado").Value

        Dim venta As Ventas = DatosGlobales.ListaVentas.Find(Function(v) v.ID = idVenta)

        If anulado Then
            MessageBox.Show("No se puede reimprimir un ticket anulado.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Try

            Dim texto As String
            Dim FechaHora As Date = Now
            ' Texto a imprimir (asegurarse de que cada línea tenga hasta 27 caracteres)
            texto = "===========================" & vbCrLf
            texto = texto & "            ADJC           " & vbCrLf
            texto = texto & "===========================" & vbCrLf
            texto = texto & "Fecha: " & FechaHora.ToString & " " & vbCrLf
            texto = texto & "Ticket Nº: " & venta.ID.ToString & "  " & vbCrLf
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
            ImprimirTexto(texto, p)
            p.Print()
        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message)
        End Try
    End Sub

    Private Shared Sub ImprimirTexto(texto As String, p As PrintDocument)
        AddHandler p.PrintPage, Sub(sender As Object, e As PrintPageEventArgs)
                                    Dim font As New Font("Courier New", 12, FontStyle.Bold)
                                    e.Graphics.DrawString(texto, font, Brushes.Black, 0, 0)
                                End Sub
    End Sub

    Private Sub CerrarCaja_Click(sender As Object, e As EventArgs) Handles CerrarCaja.Click


        ' Preguntar al usuario si está seguro de cerrar la caja
        Dim respuesta As DialogResult = MessageBox.Show("¿Está seguro de que desea CERRAR LA CAJA ?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        ' Verificamos si el usuario confirma el cierre de caja
        If respuesta = DialogResult.Yes Then


            Dim ventas As List(Of Ventas) = DatosGlobales.ObtenerVentas()

            ' Filtramos solo las ventas no anuladas
            Dim ventasValidas As IEnumerable(Of Ventas) = ventas.Where(Function(v) Not v.Anulado)

            ' Contamos los tickets (IDs únicos)
            Dim cantidadTickets As Integer = ventasValidas.Count()

            ' Sumamos las cantidades vendidas de cada elemento
            Dim cantidades(DatosGlobales.ListaProductos.Count - 1) As Integer

            For Each venta In ventasValidas

                For i As Integer = 0 To DatosGlobales.ListaProductos.Count - 1

                    cantidades(i) += CInt(CallByName(venta, "Cantidad" & (i + 1), CallType.Get))

                Next

            Next

            ' Filtramos solo las ventas Efectivo
            Dim ventasValidasEfectivo As IEnumerable(Of Ventas) = ventasValidas.Where(Function(v) v.MetodoPago)
            Dim ventasValidasTransferencia As IEnumerable(Of Ventas) = ventasValidas.Where(Function(v) Not v.MetodoPago)
            ' Sumamos el total vendido
            Dim VentasEfectivo As Double = ventasValidasEfectivo.Sum(Function(v) v.TotalVentas)
            Dim VentasTransferencia As Double = ventasValidasTransferencia.Sum(Function(v) v.TotalVentas)
            Dim montoTotal As Double = Double.Parse(VentasEfectivo) + Double.Parse(VentasTransferencia)

            Try

                Dim texto As String
                Dim FechaHora As Date = Now
                ' Texto a imprimir (asegurarse de que cada línea tenga hasta 42 caracteres)
                texto = "===========================" & vbCrLf
                texto = texto & "     ADJC CARNAVALES       " & vbCrLf
                texto = texto & "===========================" & vbCrLf
                texto = texto & "Fecha: " & FechaHora.ToString & " " & vbCrLf
                texto = texto & "CIERRE DE CAJA " & vbCrLf
                texto = texto & "Cajero: " & DatosGlobales.cajeros.Apellidos.ToString & "  " & vbCrLf
                texto = texto & "Cant de ticket: " & cantidadTickets & "  " & vbCrLf
                texto = texto & "Cant Detalle          Monto" & vbCrLf
                For i = 1 To DatosGlobales.ListaProductos.Count

                    If cantidades(i - 1) = 0 Then
                        Continue For
                    End If
                    ' MONTO TOTAL POR PRODUCTO
                    Dim montoTotalCant As Double = cantidades(i - 1) * DatosGlobales.ListaProductos(i - 1).Precio


                    texto = texto & " " & cantidades(i - 1).ToString & "  " & DatosGlobales.ListaProductos(i - 1).Nombre & vbCrLf
                    texto = texto & "                  $" & montoTotalCant & vbCrLf
                    texto = texto & "---------------------------" & vbCrLf

                Next
                texto = texto & "===========================" & vbCrLf
                texto = texto & "TOTAL EFECTIVO $" & VentasEfectivo & vbCrLf
                texto = texto & "TOTAL TRANSFER $" & VentasTransferencia & vbCrLf
                texto = texto & "TOTAL VENTAS $" & montoTotal & vbCrLf
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
                ' Configurar la impresora
                p.PrinterSettings.PrinterName = Configuraciones.nombreImpresora

                ' Asegurarse de que la impresora esté configurada correctamente
                ImprimirTexto(texto, p)

                ' Asignar el evento PrintPage para imprimir el texto
                p.Print()


            Catch ex As Exception
                MessageBox.Show("Error: " & ex.Message)
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
            MessageBox.Show("Error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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


        Catch ex As Exception
            MessageBox.Show("Error al compactar: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try

        MessageBox.Show("Base de datos compactada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information)
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
End Class