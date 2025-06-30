Module Configuraciones

    Public nombreImpresora As String = "cobro"
    Public ReadOnly rutaEscritorio As String = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
    Public ReadOnly rutaDB As String = System.IO.Path.Combine(rutaEscritorio, "Carnavales.accdb")
    ''
End Module
