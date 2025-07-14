Imports System.Data.OleDb

Public Class Precios

    Private copiaProductos As List(Of Producto)

    Private Sub Precios_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Cargar una copia profunda de los productos
        copiaProductos = DatosGlobales.ObtenerProductos().Select(Function(p) New Producto With {
        .ID = p.ID,
        .Nombre = p.Nombre,
        .Precio = p.Precio
    }).ToList()

        DataGridView1.DataSource = Nothing
        DataGridView1.DataSource = copiaProductos

        ConfigurarDataGridView()


    End Sub


    Private Sub ConfigurarDataGridView()
        With DataGridView1
            .Columns("ID").ReadOnly = True
            .Columns("ID").Width = 80
            .Columns("ID").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("Nombre").ReadOnly = False
            .Columns("Nombre").Width = 750
            .Columns("Nombre").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .Columns("Precio").ReadOnly = False
            .Columns("Precio").Width = 200
            .Columns("Precio").DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
        End With
    End Sub





    ' Solo permite 33 caracteres en la columna "Nombre"
    Private Sub DataGridView1_CellValidating(sender As Object, e As DataGridViewCellValidatingEventArgs) Handles DataGridView1.CellValidating

        ' Verificamos si es la columna "Nombre"
        If DataGridView1.Columns(e.ColumnIndex).Name = "Nombre" Then
            Dim textoIngresado As String = e.FormattedValue.ToString()
            If textoIngresado.Length > 33 Then
                MessageBox.Show("El nombre no puede tener más de 33 caracteres.", "Límite excedido", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                e.Cancel = True ' Cancela el cambio si supera el límite
            End If
        End If




    End Sub


    Private Sub ConvertirAMayusculas_KeyPress(sender As Object, e As KeyPressEventArgs)
        If Char.IsLetter(e.KeyChar) Then
            e.KeyChar = Char.ToUpper(e.KeyChar)
        End If
        ' No toques los números ni otros caracteres
    End Sub


    Private Sub Salir_Click(sender As Object, e As EventArgs) Handles Salir.Click
        Dim resultado As DialogResult = MessageBox.Show("¿Desea salir sin guardar los cambios?", "Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If resultado = DialogResult.Yes Then
            Me.Close()
        End If
    End Sub


    Private Sub Guardar_Click(sender As Object, e As EventArgs) Handles Guardar.Click
        If ValidarDatosAntesDeGuardar() Then
            DataBase.ActualizarListadoProductos(DataGridView1, Me)
            ' También podés copiar los cambios a DatosGlobales si querés
            DatosGlobales.ListaProductos = copiaProductos.Select(Function(p) New Producto With {
            .ID = p.ID,
            .Nombre = p.Nombre,
            .Precio = p.Precio
        }).ToList()
        Else
            MessageBox.Show("Hay errores en la tabla. Verifique que los nombres tengan precios y los precios tengan nombres.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub




    Private Sub DataGridView1_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles DataGridView1.EditingControlShowing
        Dim txt As TextBox = TryCast(e.Control, TextBox)
        If txt Is Nothing Then Exit Sub

        ' Siempre eliminamos handlers previos para evitar conflictos
        RemoveHandler txt.KeyPress, AddressOf SoloNumeros_KeyPress
        RemoveHandler txt.KeyPress, AddressOf ConvertirAMayusculas_KeyPress

        ' Si está editando la columna Precio => solo números
        If DataGridView1.CurrentCell.ColumnIndex = DataGridView1.Columns("Precio").Index Then
            AddHandler txt.KeyPress, AddressOf SoloNumeros_KeyPress
        End If

        ' Si está editando la columna Nombre => mayúsculas solo para letras
        If DataGridView1.CurrentCell.ColumnIndex = DataGridView1.Columns("Nombre").Index Then
            AddHandler txt.KeyPress, AddressOf ConvertirAMayusculas_KeyPress
        End If
    End Sub


    Private Sub SoloNumeros_KeyPress(sender As Object, e As KeyPressEventArgs)

        ' Permitir solo números, coma, retroceso
        If Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsDigit(e.KeyChar) AndAlso e.KeyChar <> ","c Then
            e.Handled = True
        End If

        ' Solo una coma permitida
        Dim txt As TextBox = CType(sender, TextBox)
        If e.KeyChar = ","c AndAlso txt.Text.Contains(",") Then
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

                Dim nombreCelda As Object = fila.Cells("Nombre").Value
                Dim precioCelda As Object = fila.Cells("Precio").Value

                Dim tieneNombre As Boolean = nombreCelda IsNot Nothing AndAlso Not IsDBNull(nombreCelda) AndAlso Not String.IsNullOrWhiteSpace(nombreCelda.ToString())
                Dim tienePrecio As Boolean = precioCelda IsNot Nothing AndAlso Not IsDBNull(precioCelda) AndAlso IsNumeric(precioCelda) AndAlso Convert.ToDecimal(precioCelda) > 0

                ' ❌ Si tiene nombre pero no tiene precio válido
                If tieneNombre AndAlso Not tienePrecio Then
                    Return False
                End If

                ' ❌ Si tiene precio pero no tiene nombre
                If tienePrecio AndAlso Not tieneNombre Then
                    Return False
                End If
            End If
        Next

        Return True ' ✅ Todo válido
    End Function

    Private Sub LimpiarTabla_Click(sender As Object, e As EventArgs) Handles LimpiarTabla.Click
        ' Recorre cada fila del DataGridView
        For Each fila As DataGridViewRow In DataGridView1.Rows
            ' Evita procesar la fila nueva (vacía para entrada)
            If Not fila.IsNewRow Then
                fila.Cells("Nombre").Value = ""     ' Borra el nombre
                fila.Cells("Precio").Value = ""      ' Borra el precio (pone 0)
            End If
        Next
    End Sub

End Class