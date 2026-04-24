Imports System.Data.OleDb

Public Class Precios

    ' Esta clase maneja la edición de precios de productos en el DataGridView
    ' Se carga una copia de los productos para evitar modificar la lista original directamente
    Private copiaProductos As List(Of Producto)

    Private Sub Precios_Shown(sender As Object, e As EventArgs) Handles Me.Shown

        ' Cargar una copia profunda de los productos
        copiaProductos = DatosGlobales.ObtenerProductos().Select(Function(p) New Producto With {
        .ID = p.ID,
        .Nombre = p.Nombre,
        .Precio = p.Precio
    }).ToList()

        ' Reiniciar completamente el DataGridView
        DataGridView1.DataSource = Nothing
        ' Limpiar las filas y columnas del DataGridView
        DataGridView1.Rows.Clear()
        ' Limpiar las columnas del DataGridView
        DataGridView1.Columns.Clear()
        ' Asignar la copia a DataGridView
        DataGridView1.DataSource = copiaProductos
        ' Configurar las columnas del DataGridView
        ConfigurarDataGridView()

        ' Verificar si hay ventas en DatosGlobales
        If DatosGlobales.ListaVentas.Count > 0 Then
            ' Habilitar el botón LimpiarTabla si hay ventas
            LimpiarTabla.Enabled = False
        End If

    End Sub

    Private Sub ConfigurarDataGridView()

        ' Primero, obtenemos la lista de productos desde DatosGlobales
        Dim ventas As New List(Of Ventas)

        ' Si ya tenemos una lista de ventas en DatosGlobales, la usamos
        If DatosGlobales.ListaVentas IsNot Nothing Then
            ' Usamos la lista de ventas existente
            ventas = DatosGlobales.ListaVentas
        Else
            ' Si no hay lista de ventas, obtenemos las ventas desde la base de datos
            ventas = DatosGlobales.ObtenerVentas()

        End If

        ' Sumamos las cantidades vendidas de cada elemento
        ' copiaproductos esta inicializado con los productos de DatosGlobales cuando se muestra el formulario shown
        Dim cantidades(copiaProductos.Count - 1) As Integer

        ' Inicializamos el array de cantidades a 0
        For Each venta In ventas

            ' Recorremos las cantidades de cada venta y las sumamos a nuestro array
            For i = 0 To copiaProductos.Count - 1

                ' Usamos CallByName para acceder a las propiedades de Cantidad1, Cantidad2, etc.
                cantidades(i) += CInt(CallByName(venta, "Cantidad" & i + 1, CallType.Get))

            Next

        Next

        ' Recorremos los productos y bloqueamos las filas que tienen ventas
        For i = 0 To copiaProductos.Count - 1

            ' Obtenemos el producto actual
            Dim producto As Producto = copiaProductos(i)

            ' Si  hay ventas para este producto, bloqueamos la fila
            If cantidades(i) > 0 Then

                ' Si hay ventas, SI bloqueamos la fila
                DataGridView1.Rows(i).ReadOnly = True
                ' Cambiamos el color de fondo para indicar que está bloqueado
                DataGridView1.Rows(i).DefaultCellStyle.BackColor = Color.LightGray

            End If
        Next

        ' Mensaje de tooltip para las celdas bloqueadas
        For i = 0 To DataGridView1.Rows.Count - 1
            ' Si la fila está bloqueada, asignamos un tooltip a cada celda
            If DataGridView1.Rows(i).ReadOnly Then
                ' Asignamos un tooltip a cada celda de la fila bloqueada
                For Each celda As DataGridViewCell In DataGridView1.Rows(i).Cells
                    ' Asignamos el tooltip a la celda
                    celda.ToolTipText = "No se puede editar porque registra ventas."
                Next
            End If
        Next

        ' Configurar las columnas del DataGridView
        With DataGridView1

            ' Configuramos las columnas del DataGridView
            .Columns("ID").ReadOnly = True
            .Columns("ID").Width = 60
            .Columns("ID").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("Nombre").ReadOnly = False
            .Columns("Nombre").Width = 600
            .Columns("Nombre").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("Precio").ReadOnly = False
            .Columns("Precio").Width = 180
            .Columns("Precio").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("Precio").DefaultCellStyle.Format = "N0"
        End With


    End Sub


    Private Sub DataGridView1_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) Handles DataGridView1.CellValidating

        ' Solo permite 33 caracteres en la columna "Nombre"

        ' Verificamos si es la columna "Nombre"
        If DataGridView1.Columns(e.ColumnIndex).Name = "Nombre" Then

            ' Verificamos si el valor ingresado es nulo o vacío
            Dim textoIngresado As String = e.FormattedValue.ToString()

            ' Si el texto ingresado es nulo o vacío, no hacemos nada
            If textoIngresado.Length > 32 Then

                ' Si el texto ingresado supera los 32 caracteres, mostramos un mensaje y cancelamos el cambio
                MessageBox.Show("El nombre no puede tener más de 32 caracteres.", "Límite excedido", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                ' Cancela el cambio si supera el límite
                e.Cancel = True

            End If

        End If

    End Sub

    Private Sub ConvertirAMayusculas_KeyPress(sender As Object, e As KeyPressEventArgs)

        ' Bloquear comilla simple '
        If e.KeyChar = "'"c Then
            e.Handled = True
            Exit Sub
        End If

        ' Permitir solo letras y convertirlas a mayúsculas
        If Char.IsLetter(e.KeyChar) Then

            ' Convertir la letra a mayúscula
            e.KeyChar = Char.ToUpper(e.KeyChar)

        End If

    End Sub

    Private Sub Salir_Click(sender As Object, e As EventArgs) Handles Salir.Click

        ' Preguntar al usuario si desea salir sin guardar los cambios
        Dim resultado As DialogResult = MessageBox.Show("¿Desea salir sin guardar los cambios?", "Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        ' Si el usuario elige "Sí", cerramos el formulario
        If resultado = DialogResult.Yes Then
            ' cerrar el formulario
            Me.Close()
        End If

    End Sub

    Private Sub Guardar_Click(sender As Object, e As EventArgs) Handles Guardar.Click

        ' Validar los datos antes de guardar
        If ValidarDatosAntesDeGuardar() Then

            ' Guardar los cambios en la copia de productos
            DataBase.ActualizarListadoProductos(DataGridView1, Me)

            ' Atuilizar la lista de productos en DatosGlobales
            DatosGlobales.ListaProductos = copiaProductos.Select(Function(p) New Producto With {
            .ID = p.ID,
            .Nombre = p.Nombre,
            .Precio = p.Precio
        }).ToList()
        Else
            ' Si hay errores en la tabla, mostrar un mensaje de advertencia
            ' No se guardan los cambios, solo se muestra el mensaje
            MessageBox.Show("Hay errores en la tabla. Verifique que los nombres tengan precios y los precios tengan nombres.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub DataGridView1_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles DataGridView1.EditingControlShowing

        ' Este evento se dispara cuando se está editando una celda del DataGridView
        Dim txt As TextBox = TryCast(e.Control, TextBox)

        ' Si no es un TextBox, salimos del sub
        If txt Is Nothing Then Exit Sub

        ' Siempre eliminamos handlers previos para evitar conflictos
        RemoveHandler txt.KeyPress, AddressOf SoloNumeros_KeyPress
        RemoveHandler txt.KeyPress, AddressOf ConvertirAMayusculas_KeyPress

        ' Si está editando la columna Precio => solo números
        If DataGridView1.CurrentCell.ColumnIndex = DataGridView1.Columns("Precio").Index Then

            ' Agregar el handler para permitir solo números y coma
            AddHandler txt.KeyPress, AddressOf SoloNumeros_KeyPress
        End If

        ' Si está editando la columna Nombre => mayúsculas solo para letras
        If DataGridView1.CurrentCell.ColumnIndex = DataGridView1.Columns("Nombre").Index Then
            ' Agregar el handler para convertir a mayúsculas
            AddHandler txt.KeyPress, AddressOf ConvertirAMayusculas_KeyPress
        End If

    End Sub

    Private Sub SoloNumeros_KeyPress(sender As Object, e As KeyPressEventArgs)

        ' Permitir solo números, coma, retroceso
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> ","c Then
            ' Si no es un número, una coma o un retroceso, ignorar la entrada
            e.Handled = True
        End If

        ' Solo una coma permitida
        Dim txt As TextBox = CType(sender, TextBox)
        ' Si se presiona una coma y ya hay una, ignorar la entrada
        If e.KeyChar = ","c AndAlso txt.Text.Contains(",") Then
            ' Si ya hay una coma en el texto, ignorar la entrada
            e.Handled = True
        End If

    End Sub

    Private Function ValidarDatosAntesDeGuardar() As Boolean

        ' Recorre cada fila del DataGridView para validar los datos
        For Each fila As DataGridViewRow In DataGridView1.Rows

            ' Verifica que la fila no sea una nueva fila (es decir, no es una fila de entrada de datos)
            If Not fila.IsNewRow Then

                ' Asegúrate de que la celda ID no sea nula o vacía
                Dim nombre As String = ""
                Dim precio As Decimal = 0

                ' Obtenemos los valores de las celdas "Nombre" y "Precio"
                Dim nombreCelda As Object = fila.Cells("Nombre").Value
                Dim precioCelda As Object = fila.Cells("Precio").Value

                ' Verificamos si las celdas tienen valores válidos
                Dim tieneNombre As Boolean = nombreCelda IsNot Nothing AndAlso Not IsDBNull(nombreCelda) AndAlso Not String.IsNullOrWhiteSpace(nombreCelda.ToString())
                Dim tienePrecio As Boolean = precioCelda IsNot Nothing AndAlso Not IsDBNull(precioCelda) AndAlso IsNumeric(precioCelda) AndAlso Convert.ToDecimal(precioCelda) > 0

                ' Si tiene nombre pero no tiene precio válido
                If tieneNombre AndAlso Not tienePrecio Then
                    Return False
                End If

                ' Si tiene precio pero no tiene nombre
                If tienePrecio AndAlso Not tieneNombre Then
                    Return False
                End If

            End If

        Next

        ' Si llegamos aquí, significa que todos los datos son válidos
        Return True

    End Function

    Private Sub LimpiarTabla_Click(sender As Object, e As EventArgs) Handles LimpiarTabla.Click

        ' Recorre cada fila del DataGridView
        For Each fila As DataGridViewRow In DataGridView1.Rows

            ' Evita procesar la fila nueva (vacía para entrada)
            If Not fila.IsNewRow Then
                ' Limpiar los valores de las celdas "Nombre" y "Precio"
                fila.Cells("Nombre").Value = ""
                fila.Cells("Precio").Value = ""
            End If
        Next
    End Sub

End Class