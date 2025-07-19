Module Configuraciones

    ' Configuraciones generales del sistema
    ' Nombre de la impresora por defecto
    Public nombreImpresora As String = "cobro"
    ' Ruta de la base de datos
    Public ReadOnly rutaEscritorio As String = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
    ' Ruta completa de la base de datos
    Public ReadOnly rutaDB As String = System.IO.Path.Combine(Application.StartupPath, "Carnavales.accdb")

End Module
