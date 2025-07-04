<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Menu
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
        Dim DataGridViewCellStyle1 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Cobrar = New Button()
        CerrarCaja = New Button()
        Salir = New Button()
        DataGridView1 = New DataGridView()
        Reimprimir = New Button()
        ConfigurarImpresora = New Button()
        ResetTabla = New Button()
        Label1 = New Label()
        TxtTotalVentas = New TextBox()
        ConfigurarProductos = New Button()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Cobrar
        ' 
        Cobrar.Font = New Font("Segoe UI", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Cobrar.Location = New Point(85, 41)
        Cobrar.Name = "Cobrar"
        Cobrar.Size = New Size(270, 95)
        Cobrar.TabIndex = 0
        Cobrar.Text = "COBRAR"
        Cobrar.UseVisualStyleBackColor = True
        ' 
        ' CerrarCaja
        ' 
        CerrarCaja.Font = New Font("Segoe UI", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        CerrarCaja.Location = New Point(85, 151)
        CerrarCaja.Name = "CerrarCaja"
        CerrarCaja.Size = New Size(270, 95)
        CerrarCaja.TabIndex = 1
        CerrarCaja.Text = "CERRAR CAJA"
        CerrarCaja.UseVisualStyleBackColor = True
        ' 
        ' Salir
        ' 
        Salir.Font = New Font("Segoe UI", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Salir.Location = New Point(85, 630)
        Salir.Name = "Salir"
        Salir.Size = New Size(270, 95)
        Salir.TabIndex = 2
        Salir.Text = "SALIR"
        Salir.UseVisualStyleBackColor = True
        ' 
        ' DataGridView1
        ' 
        DataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = SystemColors.Control
        DataGridViewCellStyle1.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        DataGridViewCellStyle1.ForeColor = SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = DataGridViewTriState.True
        DataGridView1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = SystemColors.Window
        DataGridViewCellStyle2.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        DataGridViewCellStyle2.ForeColor = SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = DataGridViewTriState.False
        DataGridView1.DefaultCellStyle = DataGridViewCellStyle2
        DataGridView1.Location = New Point(443, 41)
        DataGridView1.Name = "DataGridView1"
        DataGridView1.RowHeadersWidth = 70
        DataGridView1.ScrollBars = ScrollBars.Vertical
        DataGridView1.Size = New Size(780, 549)
        DataGridView1.TabIndex = 3
        ' 
        ' Reimprimir
        ' 
        Reimprimir.Enabled = False
        Reimprimir.Font = New Font("Segoe UI", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Reimprimir.Location = New Point(537, 630)
        Reimprimir.Name = "Reimprimir"
        Reimprimir.Size = New Size(270, 95)
        Reimprimir.TabIndex = 4
        Reimprimir.Text = "IMPRIMIR"
        Reimprimir.UseVisualStyleBackColor = True
        ' 
        ' ConfigurarImpresora
        ' 
        ConfigurarImpresora.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        ConfigurarImpresora.Location = New Point(85, 393)
        ConfigurarImpresora.Name = "ConfigurarImpresora"
        ConfigurarImpresora.Size = New Size(270, 95)
        ConfigurarImpresora.TabIndex = 5
        ConfigurarImpresora.Text = "CONFIGURAR IMPRESORA"
        ConfigurarImpresora.UseVisualStyleBackColor = True
        ' 
        ' ResetTabla
        ' 
        ResetTabla.Font = New Font("Segoe UI", 16F, FontStyle.Bold)
        ResetTabla.Location = New Point(85, 511)
        ResetTabla.Name = "ResetTabla"
        ResetTabla.Size = New Size(270, 95)
        ResetTabla.TabIndex = 6
        ResetTabla.Text = "ELIMINAR  TODAS LAS VENTAS"
        ResetTabla.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label1.Location = New Point(917, 617)
        Label1.Name = "Label1"
        Label1.Size = New Size(207, 31)
        Label1.TabIndex = 7
        Label1.Text = "TOTAL DE VENTAS"
        ' 
        ' TxtTotalVentas
        ' 
        TxtTotalVentas.Enabled = False
        TxtTotalVentas.Font = New Font("Segoe UI", 24.8000011F, FontStyle.Bold)
        TxtTotalVentas.Location = New Point(890, 663)
        TxtTotalVentas.Name = "TxtTotalVentas"
        TxtTotalVentas.Size = New Size(266, 62)
        TxtTotalVentas.TabIndex = 8
        TxtTotalVentas.TextAlign = HorizontalAlignment.Center
        ' 
        ' ConfigurarProductos
        ' 
        ConfigurarProductos.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        ConfigurarProductos.Location = New Point(85, 268)
        ConfigurarProductos.Name = "ConfigurarProductos"
        ConfigurarProductos.Size = New Size(270, 107)
        ConfigurarProductos.TabIndex = 9
        ConfigurarProductos.Text = "CONFIGURAR" & vbCrLf & "PRECIOS"
        ConfigurarProductos.UseVisualStyleBackColor = True
        ' 
        ' Menu
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1262, 800)
        Controls.Add(ConfigurarProductos)
        Controls.Add(TxtTotalVentas)
        Controls.Add(Label1)
        Controls.Add(ResetTabla)
        Controls.Add(ConfigurarImpresora)
        Controls.Add(Reimprimir)
        Controls.Add(DataGridView1)
        Controls.Add(Salir)
        Controls.Add(CerrarCaja)
        Controls.Add(Cobrar)
        FormBorderStyle = FormBorderStyle.FixedSingle
        MaximizeBox = False
        Name = "Menu"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Menu"
        CType(DataGridView1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Cobrar As Button
    Friend WithEvents CerrarCaja As Button
    Friend WithEvents Salir As Button
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents Reimprimir As Button
    Friend WithEvents ConfigurarImpresora As Button
    Friend WithEvents ResetTabla As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents TxtTotalVentas As TextBox
    Friend WithEvents ConfigurarProductos As Button
End Class
