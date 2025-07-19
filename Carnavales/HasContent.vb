Module HasContent

    Public Function TieneContenido(text_box As TextBox) As Boolean

        ' Esta función verifica si un TextBox tiene contenido.        
        TieneContenido = (Len(Trim(text_box.Text)) > 0)

    End Function

End Module
