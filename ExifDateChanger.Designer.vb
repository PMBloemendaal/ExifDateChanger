<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ExifDateChanger
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ExifDateChanger))
        butLoadPicture = New Button()
        PictureBox1 = New PictureBox()
        TextBoxOutput = New TextBox()
        butSaveCreationDate = New Button()
        butSaveDateFolder = New Button()
        CType(PictureBox1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' butLoadPicture
        ' 
        butLoadPicture.Location = New Point(12, 12)
        butLoadPicture.Name = "butLoadPicture"
        butLoadPicture.Size = New Size(140, 23)
        butLoadPicture.TabIndex = 0
        butLoadPicture.Text = "Load Picture"
        butLoadPicture.UseVisualStyleBackColor = True
        ' 
        ' PictureBox1
        ' 
        PictureBox1.Location = New Point(12, 41)
        PictureBox1.Name = "PictureBox1"
        PictureBox1.Size = New Size(713, 472)
        PictureBox1.TabIndex = 1
        PictureBox1.TabStop = False
        ' 
        ' TextBoxOutput
        ' 
        TextBoxOutput.Location = New Point(12, 519)
        TextBoxOutput.Multiline = True
        TextBoxOutput.Name = "TextBoxOutput"
        TextBoxOutput.ReadOnly = True
        TextBoxOutput.ScrollBars = ScrollBars.Vertical
        TextBoxOutput.Size = New Size(713, 268)
        TextBoxOutput.TabIndex = 2
        ' 
        ' butSaveCreationDate
        ' 
        butSaveCreationDate.Enabled = False
        butSaveCreationDate.Location = New Point(158, 12)
        butSaveCreationDate.Name = "butSaveCreationDate"
        butSaveCreationDate.Size = New Size(140, 23)
        butSaveCreationDate.TabIndex = 3
        butSaveCreationDate.Text = "Save Date"
        butSaveCreationDate.UseVisualStyleBackColor = True
        ' 
        ' butSaveDateFolder
        ' 
        butSaveDateFolder.Location = New Point(586, 12)
        butSaveDateFolder.Name = "butSaveDateFolder"
        butSaveDateFolder.Size = New Size(140, 23)
        butSaveDateFolder.TabIndex = 4
        butSaveDateFolder.Text = "Save Date Folder"
        butSaveDateFolder.UseVisualStyleBackColor = True
        ' 
        ' ExifDateChanger
        ' 
        AutoScaleDimensions = New SizeF(7F, 15F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(737, 799)
        Controls.Add(butSaveDateFolder)
        Controls.Add(butSaveCreationDate)
        Controls.Add(TextBoxOutput)
        Controls.Add(PictureBox1)
        Controls.Add(butLoadPicture)
        FormBorderStyle = FormBorderStyle.FixedToolWindow
        Icon = CType(resources.GetObject("$this.Icon"), Icon)
        MaximizeBox = False
        Name = "ExifDateChanger"
        SizeGripStyle = SizeGripStyle.Hide
        Text = "Exif Date Changer"
        CType(PictureBox1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents butLoadPicture As Button
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents TextBoxOutput As TextBox
    Friend WithEvents butSaveCreationDate As Button
    Friend WithEvents butSaveDateFolder As Button

End Class
