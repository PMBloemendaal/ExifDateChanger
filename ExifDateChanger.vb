

Option Strict On
Option Explicit On

Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Windows.Forms
Imports System.Text

Public Class ExifDateChanger

    Dim blnPictureLoaded As Boolean = False
    Dim lastFileName As String = ""
    Private Sub ExifDateChanger_Load(sender As Object, e As EventArgs) Handles Me.Load
        '' Application.EnableVisualStyles() zorgt ervoor dat Windows Forms (WinForms)-applicaties de moderne
        '' Windows-look gebruiken (met thema's, afgeronde knoppen, gekleurde progressbars, etc.),
        '' in plaats van de oude Windows 95-achtige stijl.
        Application.EnableVisualStyles()
    End Sub
    Private Sub ChoosAndLoadJPG()
        ' We gebruiken de Using-statement om ervoor te zorgen dat de OpenFileDialog wordt vrijgegeven
        Using ofd As New OpenFileDialog()

            ' Configureer de dialoogbox
            ofd.Filter = "JPEG Bestanden (*.jpg;*.jpeg)|*.jpg;*.jpeg|Alle Bestanden (*.*)|*.*"
            ofd.Title = "Selecteer een JPG-bestand"

            ' Haal het laatst gebruikte pad op uit My.Settings
            Dim lastFilePath As String = My.Settings.LastFilePath

            ' Controleer of er een geldig pad is opgeslagen en het bestand nog bestaat
            If Not String.IsNullOrWhiteSpace(lastFilePath) AndAlso File.Exists(lastFilePath) Then
                Try
                    ' Stel de initiële map en bestandsnaam in
                    ofd.InitialDirectory = Path.GetDirectoryName(lastFilePath)
                    ofd.FileName = Path.GetFileName(lastFilePath)
                Catch ex As Exception
                    ' Indien Path.GetDirectoryName een fout geeft (bijv. ongeldig pad),
                    ' gebruik een veilige standaardmap
                    ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
                End Try
            Else
                ' Indien er geen vorig pad is, gebruik dan de standaard Afbeeldingen-map
                ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            End If

            ' Toon het dialoogvenster en controleer of de gebruiker op OK heeft geklikt
            If ofd.ShowDialog() = DialogResult.OK Then
                Dim selectedFilePath As String = ofd.FileName

                Try
                    ' Laad het bestand
                    LoadJPG(selectedFilePath)
                    TextBoxOutput.Text = OutputExifData(selectedFilePath)
                    blnPictureLoaded = True
                    ' **Sla het nieuwe, succesvol geladen pad op**
                    My.Settings.LastFilePath = selectedFilePath
                    My.Settings.Save() ' Sla de wijzigingen op

                Catch ex As Exception
                    MessageBox.Show($"Fout bij het laden van de afbeelding: {ex.Message}", "Fout", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Using
    End Sub

    ' Functie om de afbeelding daadwerkelijk te laden en weer te geven
    Private Sub LoadJPG(ByVal filePath As String)
        ' Belangrijk: Gebruik New Bitmap(Image.FromFile(path)) om het bronbestand te ontgrendelen.
        ' Dit voorkomt dat het JPG-bestand "in gebruik" blijft na het laden.
        Using tempImage As Image = Image.FromFile(filePath)
            PictureBox1.Image = New Bitmap(tempImage)
        End Using
        lastFileName = filePath
        ' Pas PictureBox-eigenschappen aan
        PictureBox1.SizeMode = PictureBoxSizeMode.Zoom
        Me.Text = $"Afbeelding geladen: {Path.GetFileName(filePath)}"
        Application.DoEvents()
    End Sub

    Private Sub ButtonLoadPicture_Click(sender As Object, e As EventArgs) Handles butLoadPicture.Click
        lastFileName = ""

        ' Eerst de oude afbeelding disposen om geheugen vrij te geven en het bestand te ontgrendelen
        If PictureBox1.Image IsNot Nothing Then
            PictureBox1.Image.Dispose()
            PictureBox1.Image = Nothing
        End If

        TextBoxOutput.Text = ""

        ChoosAndLoadJPG()
        If lastFileName <> "" Then
            butSaveCreationDate.Enabled = True
        End If
    End Sub

    Private Sub butSaveCreationDate_Click(sender As Object, e As EventArgs) Handles butSaveCreationDate.Click
        If lastFileName <> "" Then
            TextBoxOutput.Text = ""
            Dim myDate = Now

            TextBoxOutput.Text = UpdateExifDates(lastFileName)
            TextBoxOutput.Text &= vbCrLf & vbCrLf
            TextBoxOutput.Text &= OutputExifData(lastFileName)
            butSaveCreationDate.Enabled = False

        End If
    End Sub

    Private Sub butSaveDateFolder_Click(sender As Object, e As EventArgs) Handles butSaveDateFolder.Click


        Dim gekozenMap As String = ChooseFolder()

        If String.IsNullOrEmpty(gekozenMap) Then
            TextBoxOutput.Text = "No folder selected"
            ' Eerst de oude afbeelding disposen om geheugen vrij te geven en het bestand te ontgrendelen

            Return
        End If

        If PictureBox1.Image IsNot Nothing Then
            PictureBox1.Image.Dispose()
            PictureBox1.Image = Nothing
        End If

        ' Vanaf hier is een folder geselecteerd.
        ' Set state of form
        TextBoxOutput.Text = ""
        lastFileName = ""
        butSaveCreationDate.Enabled = False

        ' 2️⃣  Haal alle .jpg bestanden op (case-insensitive)
        Dim myFiles As List(Of String) = Directory.GetFiles(gekozenMap, "*.jpg", SearchOption.TopDirectoryOnly).ToList()

        TextBoxOutput.Text = "Selected folder: " & gekozenMap & vbCrLf
        TextBoxOutput.Text &= "Aantal JPG-bestanden gevonden: " & myFiles.Count & vbCrLf & vbCrLf




        ' Toon enkele resultaten
        For Each myFile In myFiles
            LoadJPG(myFile)
            TextBoxOutput.Text &= UpdateExifDates(myFile) & vbCrLf & vbCrLf
        Next
        TextBoxOutput.Text &= vbCrLf & "Done..."
    End Sub



    Function ChooseFolder() As String
        Dim dialoog As New FolderBrowserDialog()

        ' Laad laatste map (als die bestaat)
        Dim vorigeMap As String = My.Settings.LastFolder
        If Not String.IsNullOrEmpty(vorigeMap) AndAlso Directory.Exists(vorigeMap) Then
            dialoog.SelectedPath = vorigeMap
        End If

        dialoog.Description = "Selecteer de map met JPG-bestanden:"

        If dialoog.ShowDialog() = DialogResult.OK Then
            Dim pad As String = dialoog.SelectedPath
            ' Sla keuze op voor volgende keer
            My.Settings.LastFolder = pad
            My.Settings.Save()
            Return pad
        Else
            Return Nothing
        End If
    End Function
End Class
