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
        Dim DataGridViewCellStyle3 As DataGridViewCellStyle = New DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As DataGridViewCellStyle = New DataGridViewCellStyle()
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
        PanelCargando = New Panel()
        ProgressBarCarga = New ProgressBar()
        LabelCargando = New Label()
        CType(DataGridView1, ComponentModel.ISupportInitialize).BeginInit()
        PanelCargando.SuspendLayout()
        SuspendLayout()
        ' 
        ' Cobrar
        ' 
        Cobrar.Font = New Font("Microsoft Sans Serif", 12F, FontStyle.Bold)
        Cobrar.Location = New Point(12, 12)
        Cobrar.Name = "Cobrar"
        Cobrar.Size = New Size(182, 58)
        Cobrar.TabIndex = 0
        Cobrar.Text = "COBRAR"
        Cobrar.UseVisualStyleBackColor = True
        ' 
        ' CerrarCaja
        ' 
        CerrarCaja.Font = New Font("Microsoft Sans Serif", 12F, FontStyle.Bold)
        CerrarCaja.Location = New Point(12, 87)
        CerrarCaja.Name = "CerrarCaja"
        CerrarCaja.Size = New Size(182, 58)
        CerrarCaja.TabIndex = 1
        CerrarCaja.Text = "CERRAR CAJA"
        CerrarCaja.UseVisualStyleBackColor = True
        ' 
        ' Salir
        ' 
        Salir.Font = New Font("Microsoft Sans Serif", 12F, FontStyle.Bold)
        Salir.Location = New Point(12, 401)
        Salir.Name = "Salir"
        Salir.Size = New Size(182, 58)
        Salir.TabIndex = 5
        Salir.Text = "SALIR"
        Salir.UseVisualStyleBackColor = True
        ' 
        ' DataGridView1
        ' 
        DataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = SystemColors.Control
        DataGridViewCellStyle3.Font = New Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        DataGridViewCellStyle3.ForeColor = SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = DataGridViewTriState.True
        DataGridView1.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle3
        DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = SystemColors.Window
        DataGridViewCellStyle4.Font = New Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        DataGridViewCellStyle4.ForeColor = SystemColors.ControlText
        DataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = DataGridViewTriState.False
        DataGridView1.DefaultCellStyle = DataGridViewCellStyle4
        DataGridView1.Location = New Point(213, 15)
        DataGridView1.Name = "DataGridView1"
        DataGridView1.RowHeadersWidth = 70
        DataGridView1.ScrollBars = ScrollBars.Vertical
        DataGridView1.Size = New Size(763, 444)
        DataGridView1.TabIndex = 7
        ' 
        ' Reimprimir
        ' 
        Reimprimir.Enabled = False
        Reimprimir.Font = New Font("Segoe UI", 12F, FontStyle.Bold)
        Reimprimir.Location = New Point(256, 475)
        Reimprimir.Name = "Reimprimir"
        Reimprimir.Size = New Size(182, 66)
        Reimprimir.TabIndex = 6
        Reimprimir.Text = "IMPRIMIR"
        Reimprimir.UseVisualStyleBackColor = True
        ' 
        ' ConfigurarImpresora
        ' 
        ConfigurarImpresora.Font = New Font("Microsoft Sans Serif", 12F, FontStyle.Bold)
        ConfigurarImpresora.Location = New Point(12, 253)
        ConfigurarImpresora.Name = "ConfigurarImpresora"
        ConfigurarImpresora.Size = New Size(182, 58)
        ConfigurarImpresora.TabIndex = 3
        ConfigurarImpresora.Text = "CONFIGURAR IMPRESORA"
        ConfigurarImpresora.UseVisualStyleBackColor = True
        ' 
        ' ResetTabla
        ' 
        ResetTabla.Font = New Font("Microsoft Sans Serif", 10F, FontStyle.Bold)
        ResetTabla.Location = New Point(12, 328)
        ResetTabla.Name = "ResetTabla"
        ResetTabla.Size = New Size(182, 58)
        ResetTabla.TabIndex = 4
        ResetTabla.Text = "ELIMINAR  TODAS LAS VENTAS"
        ResetTabla.UseVisualStyleBackColor = True
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label1.Location = New Point(558, 462)
        Label1.Name = "Label1"
        Label1.Size = New Size(207, 31)
        Label1.TabIndex = 8
        Label1.Text = "TOTAL DE VENTAS"
        ' 
        ' TxtTotalVentas
        ' 
        TxtTotalVentas.Enabled = False
        TxtTotalVentas.Font = New Font("Segoe UI", 18F, FontStyle.Bold)
        TxtTotalVentas.Location = New Point(524, 496)
        TxtTotalVentas.Name = "TxtTotalVentas"
        TxtTotalVentas.Size = New Size(266, 47)
        TxtTotalVentas.TabIndex = 9
        TxtTotalVentas.TextAlign = HorizontalAlignment.Center
        ' 
        ' ConfigurarProductos
        ' 
        ConfigurarProductos.Font = New Font("Microsoft Sans Serif", 12F, FontStyle.Bold)
        ConfigurarProductos.Location = New Point(12, 165)
        ConfigurarProductos.Name = "ConfigurarProductos"
        ConfigurarProductos.Size = New Size(182, 70)
        ConfigurarProductos.TabIndex = 2
        ConfigurarProductos.Text = "CONFIGURAR" & vbCrLf & "PRECIOS"
        ConfigurarProductos.UseVisualStyleBackColor = True
        ' 
        ' PanelCargando
        ' 
        PanelCargando.Controls.Add(ProgressBarCarga)
        PanelCargando.Controls.Add(LabelCargando)
        PanelCargando.Dock = DockStyle.Fill
        PanelCargando.Location = New Point(0, 0)
        PanelCargando.Name = "PanelCargando"
        PanelCargando.Size = New Size(1006, 553)
        PanelCargando.TabIndex = 10
        PanelCargando.Visible = False
        ' 
        ' ProgressBarCarga
        ' 
        ProgressBarCarga.Location = New Point(191, 264)
        ProgressBarCarga.MarqueeAnimationSpeed = 30
        ProgressBarCarga.Name = "ProgressBarCarga"
        ProgressBarCarga.Size = New Size(634, 58)
        ProgressBarCarga.Step = 30
        ProgressBarCarga.Style = ProgressBarStyle.Marquee
        ProgressBarCarga.TabIndex = 1
        ' 
        ' LabelCargando
        ' 
        LabelCargando.AutoSize = True
        LabelCargando.Font = New Font("Microsoft Sans Serif", 16.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        LabelCargando.Location = New Point(255, 203)
        LabelCargando.Name = "LabelCargando"
        LabelCargando.Size = New Size(510, 32)
        LabelCargando.TabIndex = 0
        LabelCargando.Text = "Cargando ventas, por favor espere..."
        LabelCargando.TextAlign = ContentAlignment.MiddleCenter
        ' 
        ' Menu
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1006, 553)
        Controls.Add(PanelCargando)
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
        PanelCargando.ResumeLayout(False)
        PanelCargando.PerformLayout()
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
    Friend WithEvents PanelCargando As Panel
    Friend WithEvents LabelCargando As Label
    Friend WithEvents ProgressBarCarga As ProgressBar
End Class
