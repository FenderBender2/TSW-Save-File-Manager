' $Id: Classes.vb 1466 2026-08-22 09:43:38Z Pete $
Imports System.Globalization

Module Classes

    ' -----------------------------------------------------------------------------------------------------------
    ' Class for the listview sort function
    ' -----------------------------------------------------------------------------------------------------------
    Public Class ListViewItemComparer
        Implements IComparer

        Private ReadOnly col As Integer
        Private ReadOnly order As SortOrder
        Private ReadOnly dateFormat As String = "dd/MM/yyyy HH:mm"
        Private ReadOnly culture As CultureInfo = CultureInfo.InvariantCulture
        ' ----------------------------------------------------
        Public Sub New(column As Integer, sortOrder As SortOrder)
            col = column
            order = sortOrder
        End Sub
        ' ----------------------------------------------------
        Public Function Compare(x As Object, y As Object) As Integer Implements IComparer.Compare
            Dim itemX As ListViewItem = CType(x, ListViewItem)
            Dim itemY As ListViewItem = CType(y, ListViewItem)

            Dim valueX As String = itemX.SubItems(col).Text
            Dim valueY As String = itemY.SubItems(col).Text

            Dim result As Integer

            If col = 2 Then
                Dim dx, dy As DateTime

                If DateTime.TryParseExact(valueX, dateFormat, culture, DateTimeStyles.None, dx) AndAlso
                   DateTime.TryParseExact(valueY, dateFormat, culture, DateTimeStyles.None, dy) Then

                    result = DateTime.Compare(dx, dy)
                Else
                    result = String.Compare(valueX, valueY)
                End If
            Else
                result = String.Compare(valueX, valueY)
            End If

            If order = SortOrder.Descending Then result = -result

            Return result
        End Function
    End Class

    ' -----------------------------------------------------------------------------------------------------------
End Module
