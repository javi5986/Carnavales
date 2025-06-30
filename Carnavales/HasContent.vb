Module HasContent

    Public Function TieneContenido(text_box As TextBox) As Boolean

        ' FUNCION PARA VERIFICAR CAMPOS VACIOS
        TieneContenido = (Len(Trim(text_box.Text)) > 0)

    End Function

End Module
