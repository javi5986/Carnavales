<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Principal
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Label1 = New Label()
        Label2 = New Label()
        PictureBox1 = New PictureBox()
        Apellidos = New TextBox()
        Aceptar = New Button()
        Salir = New Button()
        CType(PictureBox1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Segoe UI", 28.2F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Label1.Location = New Point(154, 89)
        Label1.Name = "Label1"
        Label1.Size = New Size(434, 62)
        Label1.TabIndex = 0
        Label1.Text = "Sistema de Cobros"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Font = New Font("Segoe UI", 19.8000011F)
        Label2.Location = New Point(154, 171)
        Label2.Name = "Label2"
        Label2.Size = New Size(459, 92)
        Label2.TabIndex = 1
        Label2.Text = "Ingrese el nombre del evento" & vbCrLf & "o los apellidos de los cajeros"
        ' 
        ' PictureBox1
        ' 
        PictureBox1.Image = My.Resources.Resources.Escudo
        PictureBox1.Location = New Point(619, 12)
        PictureBox1.Name = "PictureBox1"
        PictureBox1.Size = New Size(169, 162)
        PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage
        PictureBox1.TabIndex = 3
        PictureBox1.TabStop = False
        ' 
        ' Apellidos
        ' 
        Apellidos.Font = New Font("Segoe UI", 19.8000011F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Apellidos.Location = New Point(62, 288)
        Apellidos.MaxLength = 32
        Apellidos.Name = "Apellidos"
        Apellidos.Size = New Size(670, 51)
        Apellidos.TabIndex = 1
        Apellidos.TextAlign = HorizontalAlignment.Center
        ' 
        ' Aceptar
        ' 
        Aceptar.Font = New Font("Segoe UI", 19.8000011F, FontStyle.Bold)
        Aceptar.Location = New Point(165, 374)
        Aceptar.Name = "Aceptar"
        Aceptar.Size = New Size(189, 56)
        Aceptar.TabIndex = 2
        Aceptar.Text = "ACEPTAR"
        Aceptar.UseVisualStyleBackColor = True
        ' 
        ' Salir
        ' 
        Salir.Font = New Font("Segoe UI", 19.8000011F, FontStyle.Bold)
        Salir.Location = New Point(410, 374)
        Salir.Name = "Salir"
        Salir.Size = New Size(189, 56)
        Salir.TabIndex = 3
        Salir.Text = "SALIR"
        Salir.UseVisualStyleBackColor = True
        ' 
        ' Principal
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(800, 475)
        Controls.Add(Salir)
        Controls.Add(Aceptar)
        Controls.Add(Apellidos)
        Controls.Add(PictureBox1)
        Controls.Add(Label2)
        Controls.Add(Label1)
        FormBorderStyle = FormBorderStyle.FixedSingle
        MaximizeBox = False
        Name = "Principal"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Principal"
        CType(PictureBox1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents Apellidos As TextBox
    Friend WithEvents Aceptar As Button
    Friend WithEvents Salir As Button

End Class
