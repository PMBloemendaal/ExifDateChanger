Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports System.Text.RegularExpressions
Imports ExifLibrary 'Install-Package ExifLibrary (Package Manager Console)

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
            Case &H132 : Return "Modifydate"
            Case &H501B : Return "Thumbnaildata"
            Case &H9003 : Return "DateTimeOriginal"
            Case &H829A : Return "ExposureTime"
            Case &H829D : Return "FNumber"
            Case &H8822 : Return "Exposureprogram (0 = Not Defined, 1 = Manual, 2 = Program AE, 3 = Aperture-priority AE, 4 = Shutter speed priority AE, 5 = Creative (Slow speed), 6 = Action (High speed), 7 = Portrait, 8 = Landscape, 9 = Bulb)"
            Case &H8827 : Return "ISO"
            Case &H9000 : Return "Exifversion"
            Case &H9004 : Return "CreateDate"
            Case &H9205 : Return "MaxApertureValue"
            Case &H9010 : Return "OffsetDate"
            Case &H9010 : Return "OffsetDateOriginal"
            Case &H9203 : Return "BrightnessValue"
            Case &H920A : Return "FocalLength"
            Case &H9209 : Return "Flash"
            Case &H9207 : Return "MeteringMode"
            Case &H9201 : Return "ShutterSpeedValue"
            Case &H9202 : Return "ApertureValue"
            Case &H9204 : Return "ExposureBiasValue"
            Case &H9291 : Return "SubSecTimeOriginal"
            Case &H9292 : Return "SubSecTimeDigitized"
            Case &HA001 : Return "ColorSpace (1 = None, 2 = inches, 3 = cm, 4 = mm, 5 = um)"
            Case &HA002 : Return "ImageWidth"
            Case &HA003 : Return "ImageHeight"
            Case &HA402 : Return "ExposureMode (0 = Auto, 1 = Manual, 2 = Auto bracket)"
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

    ' EXIF Tag ID voor DateTimeOriginal: 0x9003
    Private Const TAG_DATE_TIME_ORIGINAL As Integer = &H9003

    ' De vereiste datumformaat string in de EXIF-standaard: "YYYY:MM:DD HH:MM:SS"
    Private Const EXIF_DATE_FORMAT As String = "yyyy:MM:dd HH:mm:ss"

    ''' <summary>
    ''' Wijzigt de EXIF DateTimeOriginal (CreationDate) tag in een JPG-bestand naar de huidige datum en tijd.
    ''' </summary>
    ''' <param name="filePath">Het volledige pad naar het JPG-bestand.</param>
    ''' <returns>True als de bewerking succesvol was, anders False.</returns>
    Public Function UpdateExifDate(ByVal filePath As String, Optional newDate As DateTime = Nothing) As Boolean
        If IsNothing(newDate) Then
            newDate = DateAndTime.Now.ToString(EXIF_DATE_FORMAT)
        End If

        If Not File.Exists(filePath) Then
            Return False
        End If

        Try
            ' 1. Bepaal de nieuwe datumwaarde (huidige datum en tijd)
            Dim newDateTimeString As String = DateTime.Now.ToString(EXIF_DATE_FORMAT)

            ' EXIF-waarden worden opgeslagen als een ASCII byte-array, afgesloten met een null-byte (0)
            Dim newDateBytes As Byte() = Encoding.ASCII.GetBytes(newDateTimeString & Chr(0))

            ' 2. Laad de afbeelding vanuit een stream om het bestand te deblokkeren
            ' (Dit is cruciaal om het bestand te kunnen overschrijven)
            Using fs As New FileStream(filePath, FileMode.Open, FileAccess.ReadWrite)
                Using img As Image = Image.FromStream(fs)

                    ' 3. Maak een nieuw PropertyItem aan voor de datum
                    Dim pi As PropertyItem = img.PropertyItems(0) ' Gebruik een bestaand PropertyItem als template

                    ' Vul het PropertyItem met de nieuwe waarden
                    pi.Id = TAG_DATE_TIME_ORIGINAL
                    pi.Type = 2 ' Type 2 = ASCII (String)
                    pi.Len = newDateBytes.Length
                    pi.Value = newDateBytes

                    ' 4. Schrijf de nieuwe PropertyItem terug naar de afbeelding
                    img.SetPropertyItem(pi)

                    ' 5. De afbeelding opslaan
                    ' Om de metadata echt te updaten, moeten we de afbeelding opslaan.
                    ' We slaan deze tijdelijk op in een MemoryStream om de bewerking te voltooien
                    ' en schrijven dan de volledige data terug naar het originele bestand.

                    Using ms As New MemoryStream()
                        img.Save(ms, ImageFormat.Jpeg)

                        ' Schrijf de bijgewerkte afbeeldingsgegevens terug naar het originele bestand
                        File.WriteAllBytes(filePath, ms.ToArray())
                    End Using
                End Using ' Sluit de Image
            End Using ' Sluit de FileStream en maakt het bestand weer vrij

            Return True

        Catch ex As Exception
            Return False
        End Try
    End Function

    Function UpdateExifDates(fileName As String) As String
        ' Pad naar het JPG-bestand

        ' Controleer of bestand bestaat
        If Not File.Exists(fileName) Then
            Return ("File not found:" & fileName)
        End If

        Try
            ' Probeer het JPG-bestand te laden (met of zonder bestaande EXIF)
            Dim afbeelding As ImageFile = Nothing
            afbeelding = ImageFile.FromFile(fileName)

            ' Huidige datum/tijd (nu)
            Dim nu As DateTime = ExtractDateTimeFromFilename(fileName)
            If fileName IsNot Nothing Then
                Dim blnDateTimeChanged As Boolean

                ' EXIF datums instellen of bijwerken
                If AddOrUpdateExifDate(afbeelding, ExifTag.DateTimeOriginal, nu) Then blnDateTimeChanged = True
                If AddOrUpdateExifDate(afbeelding, ExifTag.DateTimeDigitized, nu) Then blnDateTimeChanged = True
                If AddOrUpdateExifDate(afbeelding, ExifTag.DateTime, nu) Then blnDateTimeChanged = True


                ' Bestand opslaan (overschrijft het origineel, alleen als er wat veranderd is.)
                If blnDateTimeChanged Then
                    afbeelding.Save(fileName)
                    Return "EXIF-date of file: '" & fileName & "' updated to " & nu.ToString("yyyy:MM:dd HH:mm:ss")
                Else
                    Return "EXIF-date of file: '" & fileName & "' already updated."
                End If


            Else
                Return "No date/time found in filename: " & fileName
            End If
        Catch ex As Exception
            Return "Error: " & ex.Message
        End Try
    End Function
    Private Function ContainsTag(imgProperties As ExifPropertyCollection, myTag As ExifTag) As Boolean
        For Each tag In imgProperties
            If tag.Name = myTag.ToString Then
                Return True
            End If
        Next
        Return False
    End Function

    Private Function AddOrUpdateExifDate(img As ImageFile, tag As ExifTag, waarde As DateTime) As Boolean
        If ContainsTag(img.Properties, tag) Then
            Return False
        Else
            img.Properties.Add(New ExifDateTime(tag, waarde))
            Return True
        End If

    End Function
    Function ExtractDateTimeFromFilename(pad As String) As DateTime
        Dim bestandsnaam As String = Path.GetFileNameWithoutExtension(pad)

        ' Lijst met regex-patronen
        Dim patronen As String() = {
            "(?<jaar>\d{4})(?<maand>\d{2})(?<dag>\d{2})[_-](?<uur>\d{2})(?<min>\d{2})(?<sec>\d{2})",             ' 20251022_100054 of SV_20250704_103014
            "(?<jaar>\d{4})[-_](?<maand>\d{2})[-_](?<dag>\d{2})[ T]?(?<uur>\d{2})[.:](?<min>\d{2})[.:](?<sec>\d{2})", ' 2025-01-05 09.04.19
            "(?<jaar>\d{4})[-_](?<maand>\d{2})[-_](?<dag>\d{2}).*?at[ _]?(?<uur>\d{2})[.:](?<min>\d{2})[.:](?<sec>\d{2})" ' WhatsApp Image ...
        }

        'Alternatief van Andries
        'Dim patroon As String = "(^|\D)(?<jaar>[12]\d{3})[\/-]?(?<maand>[01]\d)[\/-]?(?<dag>[0-3]\d)\D*(?<uur>[0-2]\d)[:\.]?(?<min>[0-5]\d)[:\.]?(?<sec>[0-5]\d)(\D|$)"
        'TODO implement als 1 patroon

        For Each patroon In patronen
            Dim m As Match = Regex.Match(bestandsnaam, patroon, RegexOptions.IgnoreCase)
            If m.Success Then
                Try
                    Dim jaar = Integer.Parse(m.Groups("jaar").Value)
                    Dim maand = Integer.Parse(m.Groups("maand").Value)
                    Dim dag = Integer.Parse(m.Groups("dag").Value)
                    Dim uur = Integer.Parse(m.Groups("uur").Value)
                    Dim min = Integer.Parse(m.Groups("min").Value)
                    Dim sec = Integer.Parse(m.Groups("sec").Value)
                    Return New DateTime(jaar, maand, dag, uur, min, sec)
                Catch
                    ' Ga door naar volgende patroon
                End Try
            End If
        Next

        ' Geen match gevonden
        Return Nothing
    End Function
End Module
