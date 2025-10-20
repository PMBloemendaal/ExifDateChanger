Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Text

Module Module1
    Public Function OutputExifData(imagePath As String) As String

        Dim ret As String = ""

        ' Controleer of het bestand bestaat
        If Not IO.File.Exists(imagePath) Then
            Debug.WriteLine("Bestand niet gevonden: " & imagePath)
            Return ret
        End If

        ' Laad de afbeelding
        Using img As Image = Image.FromFile(imagePath)
            ' Controleer of er EXIF-properties zijn
            If img.PropertyItems.Length = 0 Then
                Debug.WriteLine("Geen EXIF-data gevonden.")


            Else
                Debug.WriteLine("EXIF-data van " & imagePath & ":" & vbCrLf)

                ' Doorloop alle PropertyItems
                For Each prop As PropertyItem In img.PropertyItems
                    ' Converteer de tag-ID naar een bekende naam (indien beschikbaar)
                    Dim tagName As String = GetTagName(prop.Id)

                    ' Converteer de waarde naar tekst
                    Dim value As String = GetValueAsString(prop)

                    If tagName <> "Unknown" Then
                        ret &= "TagName: " & tagName & " = " & value & vbCrLf
                    Else
                        ret &= "TagName: " & prop.Id & " = " & value & vbCrLf

                    End If
                    Debug.WriteLine($"{tagName} ({prop.Id:X4}) = {value}")
                Next
            End If
        End Using
        Return ret

    End Function

    ' Helper-functie om EXIF-tag naam te bepalen
    Private Function GetTagName(tagId As Integer) As String
        Select Case tagId
            Case &H10E : Return "ImageDescription"
            Case &H10F : Return "Make"
            Case &H110 : Return "Model"
            Case &H112 : Return "Orientation (1 = Horizontal (normal), 2 = Mirror horizontal, 3 = Rotate 180, 4 = Mirror vertical, 5 = Mirror horizontal and rotate 270 CW, 6 = Rotate 90 CW, 7 = Mirror horizontal and rotate 90 CW, 8 = Rotate 270 CW)"
            Case &H11A : Return "XResolution"
            Case &H11B : Return "YResolution"
            Case &H128 : Return "Resolution Unit (1 = None, 2 = inches, 3 = cm)"
            Case &H131 : Return "Software"
            Case &H132 : Return "Modifydate (0 = Not Defined, 1 = Manual, 2 = Program AE, 3 = Aperture-priority AE, 4 = Shutter speed priority AE, 5 = Creative (Slow speed), 6 = Action (High speed), 7 = Portrait, 8 = Landscape, 9 = Bulb)"
            Case &H9003 : Return "DateTimeOriginal"
            Case &H829A : Return "ExposureTime"
            Case &H829D : Return "FNumber"
            Case &H8827 : Return "ISO"
            Case &H920A : Return "FocalLength"
            Case &H9209 : Return "Flash"
            Case &H9207 : Return "MeteringMode"
            Case &H9201 : Return "ShutterSpeedValue"
            Case &H9202 : Return "ApertureValue"
            Case &H9204 : Return "ExposureBiasValue"
            Case &HA002 : Return "ImageWidth"
            Case &HA003 : Return "ImageHeight"
            Case &H8822 : Return "Exposureprogram"
            Case Else
                Return "Unknown"
        End Select
    End Function

    ' Helper-functie om EXIF-waarde naar tekst te converteren
    Private Function GetValueAsString(prop As PropertyItem) As String
        Try
            Select Case prop.Type
                Case 1 ' Byte
                    Return BitConverter.ToString(prop.Value)
                Case 2 ' ASCII string
                    Return Encoding.ASCII.GetString(prop.Value).Trim(Chr(0))
                Case 3 ' Short (2 bytes)
                    Return BitConverter.ToUInt16(prop.Value, 0).ToString()
                Case 4 ' Long (4 bytes)
                    Return BitConverter.ToUInt32(prop.Value, 0).ToString()
                Case 5 ' Rational (2 Longs)
                    Dim num As UInteger = BitConverter.ToUInt32(prop.Value, 0)
                    Dim denom As UInteger = BitConverter.ToUInt32(prop.Value, 4)
                    If denom <> 0 Then
                        Return $"{num / denom} ({num}/{denom})"
                    Else
                        Return "0"
                    End If
                Case Else
                    Return BitConverter.ToString(prop.Value)
            End Select
        Catch ex As Exception
            Return "Kon waarde niet lezen"
        End Try
    End Function
End Module
