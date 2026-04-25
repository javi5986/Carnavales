<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Precios
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Titulo = New Label()
        DataGridView1 = New DataGridView()
        Guardar = New Button()
        Salir = New Button()
        LimpiarTabla = New Button()
        ToolTip1 = New ToolTip(components)
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Titulo
        ' 
        Titulo.AutoSize = True
        Titulo.Font = New Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Titulo.Location = New Point(199, 9)
        Titulo.Name = "Titulo"
        Titulo.Size = New Size(874, 54)
        Titulo.TabIndex = 0
        Titulo.Text = "CONFIGURACIÓN DE PRODUCTOS Y PRECIOS"
        ' 
        ' DataGridView1
        ' 
        DataGridView1.AllowUserToAddRows = False
        DataGridView1.AllowUserToDeleteRows = False
        DataGridView1.AllowUserToResizeRows = False
        DataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = SystemColors.Control
        DataGridViewCellStyle1.Font = New Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        DataGridViewCellStyle1.ForeColor = SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = DataGridViewTriState.True
        DataGridView1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = SystemColors.Window
        DataGridViewCellStyle2.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        DataGridViewCellStyle2.ForeColor = SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = DataGridViewTriState.False
        DataGridView1.DefaultCellStyle = DataGridViewCellStyle2
        DataGridView1.Location = New Point(43, 118)
        DataGridView1.MultiSelect = False
        DataGridView1.Name = "DataGridView1"
        DataGridView1.RowHeadersWidth = 51
        DataGridView1.RowTemplate.Height = 45
        DataGridView1.Size = New Size(1143, 532)
        DataGridView1.TabIndex = 1
        ' 
        ' Guardar
        ' 
        Guardar.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Guardar.Location = New Point(248, 670)
        Guardar.Name = "Guardar"
        Guardar.Size = New Size(252, 57)
        Guardar.TabIndex = 2
        Guardar.Text = "GUARDAR"
        Guardar.UseVisualStyleBackColor = True
        ' 
        ' Salir
        ' 
        Salir.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Salir.Location = New Point(821, 670)
        Salir.Name = "Salir"
        Salir.Size = New Size(252, 57)
        Salir.TabIndex = 3
        Salir.Text = "SALIR"
        Salir.UseVisualStyleBackColor = True
        ' 
        ' LimpiarTabla
        ' 
        LimpiarTabla.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        LimpiarTabla.Location = New Point(530, 670)
        LimpiarTabla.Name = "LimpiarTabla"
        LimpiarTabla.Size = New Size(252, 57)
        LimpiarTabla.TabIndex = 4
        LimpiarTabla.Text = "LIMPIAR TABLA"
        LimpiarTabla.UseVisualStyleBackColor = True
        ' 
        ' Precios
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1260, 739)
        Controls.Add(LimpiarTabla)
        Controls.Add(Salir)
        Controls.Add(Guardar)
        Controls.Add(DataGridView1)
        Controls.Add(Titulo)
        MaximizeBox = False
        Name = "Precios"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Precios"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Titulo As Label
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents Guardar As Button
    Friend WithEvents Salir As Button
    Friend WithEvents LimpiarTabla As Button
    Friend WithEvents ToolTip1 As ToolTip
End Class
