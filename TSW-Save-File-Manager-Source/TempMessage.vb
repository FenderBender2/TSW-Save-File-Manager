Module TempMessage

    Public CurrentColour As Color
    Public StartColour As Color = SystemColors.HotTrack ' Light blue
    Public EndColour As Color = SystemColors.Control ' Light grey
    Public FadeStep As Integer = 15

    ' -----------------------------------------------------------------------------------------------------------
    ' Show status message until MessageTimer times out
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub ShowTempMessage(msg As String)

        With TSWSFM
            Dim flags As TextFormatFlags = TextFormatFlags.NoPadding Or TextFormatFlags.NoClipping Or TextFormatFlags.TextBoxControl
            Dim textSize As Size = TextRenderer.MeasureText(msg, .StatusMessage.Font, New Size(Integer.MaxValue, Integer.MaxValue), flags)
            Dim usableWidth As Integer = .StatusMessage.ClientSize.Width
            Dim wraps As Boolean = (textSize.Width > usableWidth - 2)

            ' Add padding only if the text fits on one line
            Dim rtf As String = $"{{\rtf1\ansi{{\pard\sb{If(wraps, 0, 100)} {msg}\par}}}}"

            .StatusMessage.Rtf = rtf
            .StatusMessage.Visible = True
            CurrentColour = StartColour
            .StatusMessage.ForeColor = CurrentColour

            .FadeTimer.Stop()
            .MessageTimer.Stop()
            .MessageTimer.Start()
        End With

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
    ' Calculate colour fade for each channel R G B
    ' -----------------------------------------------------------------------------------------------------------
    Private Function MoveChannel(current As Integer, target As Integer) As Integer

        Dim stepSize As Integer = Math.Max(1, Math.Abs(current - target) \ 3)

        If current < target Then
            Return Math.Min(target, current + stepSize)
        ElseIf current > target Then
            Return Math.Max(target, current - stepSize)
        Else
            Return current
        End If

    End Function

    ' -----------------------------------------------------------------------------------------------------------
    ' Fade colour on each tick of the fade timer
    ' -----------------------------------------------------------------------------------------------------------
    Public Sub FadeColours(tm As Timer)

        Dim r = MoveChannel(CurrentColour.R, EndColour.R)
        Dim g = MoveChannel(CurrentColour.G, EndColour.G)
        Dim b = MoveChannel(CurrentColour.B, EndColour.B)

        CurrentColour = Color.FromArgb(r, g, b)
        TSWSFM.StatusMessage.ForeColor = CurrentColour

        ' Stop when close enough to the target colour
        If r = EndColour.R AndAlso g = EndColour.G AndAlso b = EndColour.B Then
            tm.Stop()
            TSWSFM.StatusMessage.Visible = False
            TSWSFM.StatusMessage.ForeColor = StartColour
        End If

    End Sub

    ' -----------------------------------------------------------------------------------------------------------
End Module
