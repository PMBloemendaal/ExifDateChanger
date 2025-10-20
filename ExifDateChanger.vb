

Option Strict On
Option Explicit On

Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Windows.Forms
Imports System.Text

Public Class ExifDateChanger

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
        ' Eerst de oude afbeelding disposen om geheugen vrij te geven en het bestand te ontgrendelen
        If PictureBox1.Image IsNot Nothing Then
            PictureBox1.Image.Dispose()
            PictureBox1.Image = Nothing
        End If

        ' Belangrijk: Gebruik New Bitmap(Image.FromFile(path)) om het bronbestand te ontgrendelen.
        ' Dit voorkomt dat het JPG-bestand "in gebruik" blijft na het laden.
        Using tempImage As Image = Image.FromFile(filePath)
            PictureBox1.Image = New Bitmap(tempImage)
        End Using

        ' Pas PictureBox-eigenschappen aan
        PictureBox1.SizeMode = PictureBoxSizeMode.Zoom
        Me.Text = $"Afbeelding geladen: {Path.GetFileName(filePath)}"

        TextBoxOutput.Text = OutputExifData(filePath)

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ChoosAndLoadJPG()
    End Sub
End Class
