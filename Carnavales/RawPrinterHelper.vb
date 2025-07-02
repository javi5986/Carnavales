Imports System.Runtime.InteropServices
Imports System.Drawing.Printing
Imports System.IO

Public Class RawPrinterHelper
    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Ansi)>
    Public Structure DOCINFOA
        <MarshalAs(UnmanagedType.LPStr)> Public pDocName As String
        <MarshalAs(UnmanagedType.LPStr)> Public pOutputFile As String
        <MarshalAs(UnmanagedType.LPStr)> Public pDataType As String
    End Structure

    <DllImport("winspool.Drv", EntryPoint:="OpenPrinterA", SetLastError:=True, CharSet:=CharSet.Ansi)>
    Public Shared Function OpenPrinter(pPrinterName As String, ByRef phPrinter As IntPtr, pDefault As IntPtr) As Boolean
    End Function

    <DllImport("winspool.Drv", SetLastError:=True)>
    Public Shared Function ClosePrinter(hPrinter As IntPtr) As Boolean
    End Function

    <DllImport("winspool.Drv", EntryPoint:="StartDocPrinterA", SetLastError:=True, CharSet:=CharSet.Ansi)>
    Public Shared Function StartDocPrinter(hPrinter As IntPtr, level As Integer, ByRef di As DOCINFOA) As Boolean
    End Function

    <DllImport("winspool.Drv", SetLastError:=True)>
    Public Shared Function EndDocPrinter(hPrinter As IntPtr) As Boolean
    End Function

    <DllImport("winspool.Drv", SetLastError:=True)>
    Public Shared Function StartPagePrinter(hPrinter As IntPtr) As Boolean
    End Function

    <DllImport("winspool.Drv", SetLastError:=True)>
    Public Shared Function EndPagePrinter(hPrinter As IntPtr) As Boolean
    End Function

    <DllImport("winspool.Drv", SetLastError:=True)>
    Public Shared Function WritePrinter(hPrinter As IntPtr, pBytes As IntPtr, dwCount As Integer, ByRef dwWritten As Integer) As Boolean
    End Function

    Public Shared Function SendStringToPrinter(szPrinterName As String, szString As String) As Boolean
        Dim pBytes As IntPtr
        Dim dwCount As Integer = szString.Length
        pBytes = Marshal.StringToCoTaskMemAnsi(szString)
        Dim bSuccess As Boolean = SendBytesToPrinter(szPrinterName, pBytes, dwCount)
        Marshal.FreeCoTaskMem(pBytes)
        Return bSuccess
    End Function

    Public Shared Function SendBytesToPrinter(szPrinterName As String, pBytes As IntPtr, dwCount As Integer) As Boolean
        Dim hPrinter As IntPtr
        Dim di As New DOCINFOA()
        Dim dwWritten As Integer = 0
        Dim bSuccess As Boolean = False
        di.pDocName = "RAW Ticket"
        di.pDataType = "RAW"

        If OpenPrinter(szPrinterName, hPrinter, IntPtr.Zero) Then
            If StartDocPrinter(hPrinter, 1, di) Then
                If StartPagePrinter(hPrinter) Then
                    bSuccess = WritePrinter(hPrinter, pBytes, dwCount, dwWritten)
                    EndPagePrinter(hPrinter)
                End If
                EndDocPrinter(hPrinter)
            End If
            ClosePrinter(hPrinter)
        End If
        Return bSuccess
    End Function
End Class
