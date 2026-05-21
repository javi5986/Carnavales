Imports System.IO.Ports
Imports System.Threading

' ╔══════════════════════════════════════════════════════════════╗
' ║              MÓDULO VISOR DE CAJA                            ║
' ║  Agregar en el proyecto como nuevo módulo: Visor.vb          ║
' ╚══════════════════════════════════════════════════════════════╝

Public Module Visor

    Private _puerto As SerialPort = Nothing
    Private _puertoNombre As String = ""
    Private _conectado As Boolean = False

    ' ── Propiedad pública ─────────────────────────────────────────
    Public ReadOnly Property Conectado As Boolean
        Get
            Return _conectado
        End Get
    End Property

    Public ReadOnly Property PuertoNombre As String
        Get
            Return _puertoNombre
        End Get
    End Property

    ' ════════════════════════════════════════════════════════════
    ' Detecta automáticamente el puerto COM del visor
    ' Prueba cada puerto disponible enviando "PING" y esperando "PONG"
    ' ════════════════════════════════════════════════════════════
    Public Function DetectarYConectar() As Boolean
        Desconectar()

        Dim puertos() As String = SerialPort.GetPortNames()

        For Each nombrePuerto As String In puertos
            Try
                Dim sp As New SerialPort(nombrePuerto, 9600)
                sp.ReadTimeout = 700
                sp.WriteTimeout = 300
                sp.NewLine = vbLf ' Arduino usa \n

                sp.Open()
                Thread.Sleep(1500) ' esperar reset del Arduino Nano al conectarse

                sp.DiscardInBuffer()
                sp.WriteLine("PING")
                Thread.Sleep(300)

                Dim respuesta As String = ""
                Try
                    respuesta = sp.ReadLine().Trim()
                Catch
                End Try

                If respuesta = "PONG" Then
                    _puerto = sp
                    _puertoNombre = nombrePuerto
                    _conectado = True

                    ' Guardar en configuraciones para próxima vez
                    Configuraciones.puertoVisor = nombrePuerto

                    Return True
                Else
                    sp.Close()
                    sp.Dispose()
                End If

            Catch
                ' Puerto ocupado o no responde — continuar
            End Try
        Next

        _conectado = False
        Return False
    End Function

    ' ════════════════════════════════════════════════════════════
    ' Intentar conectar al puerto guardado en configuraciones
    ' Si falla, hace detección automática
    ' ════════════════════════════════════════════════════════════
    Public Function Conectar() As Boolean
        ' Si ya hay un puerto guardado, intentar primero ese
        If Not String.IsNullOrEmpty(Configuraciones.puertoVisor) Then
            Try
                Dim sp As New SerialPort(Configuraciones.puertoVisor, 9600)
                sp.ReadTimeout = 700
                sp.NewLine = vbLf
                sp.Open()
                Thread.Sleep(1500)
                sp.DiscardInBuffer()
                sp.WriteLine("PING")
                Thread.Sleep(300)

                Dim respuesta As String = ""
                Try
                    respuesta = sp.ReadLine().Trim()
                Catch
                End Try

                If respuesta = "PONG" Then
                    _puerto = sp
                    _puertoNombre = Configuraciones.puertoVisor
                    _conectado = True
                    Return True
                Else
                    sp.Close()
                    sp.Dispose()
                End If
            Catch
            End Try
        End If

        ' Si no funcionó, hacer detección automática
        Return DetectarYConectar()
    End Function

    ' ════════════════════════════════════════════════════════════
    ' Enviar total en tiempo real (mientras se cobra)
    ' ════════════════════════════════════════════════════════════
    Public Sub EnviarTotal(total As Double)
        EnviarComando("T:" & CInt(total).ToString())
    End Sub

    ' ════════════════════════════════════════════════════════════
    ' Notificar que se imprimió — sostiene 2 minutos en el display
    ' ════════════════════════════════════════════════════════════
    Public Sub EnviarImpresion(total As Double)
        EnviarComando("P:" & CInt(total).ToString())
    End Sub

    ' ════════════════════════════════════════════════════════════
    ' Volver a marquesina inmediatamente
    ' ════════════════════════════════════════════════════════════
    Public Sub EnviarIdle()
        EnviarComando("I")
    End Sub

    ' ════════════════════════════════════════════════════════════
    ' Cerrar la conexión
    ' ════════════════════════════════════════════════════════════
    Public Sub Desconectar()
        Try
            If _puerto IsNot Nothing Then
                If _puerto.IsOpen Then
                    EnviarComando("I")
                    Thread.Sleep(100)
                    _puerto.Close()
                End If
                _puerto.Dispose()
                _puerto = Nothing
            End If
        Catch
        End Try
        _conectado = False
        _puertoNombre = ""
    End Sub

    ' ── Envío interno (hilo separado para no bloquear la UI) ──────
    Private Sub EnviarComando(cmd As String)
        If Not _conectado OrElse _puerto Is Nothing Then Return

        Try
            ' Enviar en thread separado para no bloquear la UI ni la impresora
            Dim t As New Thread(Sub()
                                    Try
                                        If _puerto.IsOpen Then
                                            _puerto.WriteLine(cmd)
                                        End If
                                    Catch
                                        _conectado = False
                                    End Try
                                End Sub)
            t.IsBackground = True
            t.Start()

        Catch
            _conectado = False
        End Try
    End Sub

End Module
